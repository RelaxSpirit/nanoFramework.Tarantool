// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses _writer file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
#else
using System;
using System.Collections;
using System.Text;
#endif
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Dto;
using nanoFramework.MessagePack.Exceptions;
using nanoFramework.Tarantool.Client;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Headers;
using nanoFramework.Tarantool.Model.Requests;
using nanoFramework.Tarantool.Model.Responses;
using nanoFramework.Tarantool.Tests.Mocks.Data;

namespace nanoFramework.Tarantool.Tests.Mocks
{
    internal class TarantoolStreamMock : MemoryStream
    {
        private readonly byte[] _tarantoolHello = Encoding.UTF8.GetBytes(TarantoolMockContext.TarantoolHelloString);
        private readonly object _processingLock = new object();
        private readonly object _responseLock = new object();
        private readonly Thread _processPackageThread;
        private readonly ManualResetEvent _exitEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _newRequestEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _newResponseEvent = new ManualResetEvent(false);
        private readonly byte[] _adminPasswordScramble;
        private readonly Queue _requestQueue = new Queue();
        private readonly Queue _responseQueue = new Queue();
        private readonly WaitHandle[] _readHandles;
        private readonly ClientOptions _clientOptions;
        private readonly TarantoolMockContext _context = TarantoolMockContext.Instanse;

        private bool _isAdminSession = false;
        private bool _isSessionOpen = false;
        private bool _isStreamClose = false;

        internal TarantoolStreamMock(ClientOptions clientOptions) : base()
        {
            _clientOptions = clientOptions;

            if (!string.IsNullOrEmpty(_clientOptions.ConnectionOptions.Nodes[0].Uri.Password))
            {
                var greetings = new GreetingsResponse(_tarantoolHello);
                _adminPasswordScramble = AuthenticationRequest.GetScrable(greetings, TarantoolMockContext.AdminPassword);
            }
            else
            {
                _adminPasswordScramble = new byte[0];
            }

            _readHandles = new WaitHandle[] { _exitEvent, _newResponseEvent };

            _processPackageThread = new Thread(ProcessPackages);
            _processPackageThread.Start();
        }

        private static bool CompareArray(byte[] source, byte[] destination)
        {
            if (source.Length != destination.Length)
            {
                return false;
            }

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != destination[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static void WritePingResponseToStream(MemoryStream writer, RequestId requestId, PingRequest pingRequest)
        {
            MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
            MessagePackSerializer.Serialize(new DataResponseMock(pingRequest), writer);
            AddPacketSize(writer, new PacketSize((uint)(writer.Position - Constants.PacketSizeBufferSize)));
        }

        private static void WriteCallResponseToStream(MemoryStream writer, RequestId requestId, CallRequest callRequest)
        {
            switch (callRequest.FunctionName)
            {
                case "math.sqrt":
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
#nullable enable
                    object? tupleValue = callRequest.Tuple[0];

                    double argument = double.Parse(tupleValue?.ToString() ?? throw new ArgumentNullException());
#nullable disable
                    if (argument != 1.3d)
                    {
                        throw new ArgumentException($"Unknown tuple argument {argument} for {callRequest.FunctionName} function");
                    }

                    ArrayList sqrtResult = new ArrayList();
                    sqrtResult.Add(1.1401754250991381);
                    MessagePackSerializer.Serialize(new DataResponseMock(sqrtResult), writer);
                    break;
                case "math.pi":
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.ErrorMask, requestId, 1), writer);
                    MessagePackSerializer.Serialize(new DataResponseMock(new ErrorResponse("Not supported.")), writer);
                    break;
                case "string.reverse":
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
                    ArrayList reverseResult = new ArrayList();
                    reverseResult.Add("nanoFramework");
                    MessagePackSerializer.Serialize(new DataResponseMock(reverseResult), writer);
                    break;
                case "os.tmpname":
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
                    ArrayList tmpnameResult = new ArrayList();
                    tmpnameResult.Add("/tmp/lua_test");
                    MessagePackSerializer.Serialize(new DataResponseMock(tmpnameResult), writer);
                    break;
                default:
                    throw new NotImplementedException($"Unknown Expression = '{callRequest.FunctionName}'");
            }

            AddPacketSize(writer, new PacketSize((uint)(writer.Position - Constants.PacketSizeBufferSize)));
        }

        private static void AddPacketSize(MemoryStream writer, PacketSize packetLength)
        {
            writer.Seek(0, SeekOrigin.Begin);
            MessagePackSerializer.Serialize(packetLength, writer);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_isStreamClose)
            {
                throw new ObjectDisposedException(nameof(TarantoolStreamMock));
            }

            if (!_isSessionOpen)
            {
                if (count >= _tarantoolHello.Length)
                {
                    Array.Copy(_tarantoolHello, 0, buffer, offset, _tarantoolHello.Length);
                    _isSessionOpen = true;
                    return _tarantoolHello.Length;
                }
                else
                {
                    throw new ArgumentException($"Actual: {count} but expected {_tarantoolHello.Length}", nameof(count));
                }
            }
            else
            {
                var eventRead = WaitHandle.WaitAny(_readHandles);

                switch (eventRead)
                {
                    case 1:
                        if (_responseQueue.Count > 0)
                        {
                            int i = 0;
                            for (; i < count; i++)
                            {
#nullable enable
                                object? responseByte = null;
                                lock (_responseLock)
                                {
                                    if (_responseQueue.Count > 0)
                                    {
                                        responseByte = _responseQueue.Dequeue();
                                    }
                                    else
                                    {
                                        responseByte = null;
                                    }
                                }
#nullable disable
                                if (responseByte != null)
                                {
                                    buffer[offset + i] = (byte)responseByte;
                                }
                                else
                                {
                                    return i;
                                }
                            }

                            lock (_responseLock)
                            {
                                if (_responseQueue.Count < 1)
                                {
                                    _newResponseEvent.Reset();
                                }
                            }

                            return i;
                        }
                        else
                        {
                            _newResponseEvent.Reset();
                            return 0;
                        }

                    default:
                        return 0;
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_isStreamClose)
            {
                throw new ObjectDisposedException(nameof(TarantoolStreamMock));
            }

            if (!_isSessionOpen)
            {
                throw new NotSupportedException("Session is not open");
            }

            int finishOffset = count - 1;
            while (finishOffset > offset)
            {
                var firstRead = MessagePackSerializer.Deserialize(typeof(PacketSize), buffer) ?? throw new OutOfMemoryException("Invalid packet size");

                var packetSize = (PacketSize)firstRead;
                offset += Constants.PacketSizeBufferSize;

                var arraySegment = new ArraySegment(buffer, offset, packetSize.Value);
                RequestHeader requestHeader = (RequestHeader)(TarantoolMockContext.RequestHeaderConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize RequestHeader"));

                switch (requestHeader.Code)
                {
                    case CommandCode.Auth:
                        EnqueueAuthRequest(requestHeader.RequestId, arraySegment);
                        break;
                    case CommandCode.Select:
                        EnqueueSelectRequest(requestHeader.RequestId, arraySegment);
                        break;
                    case CommandCode.Call:
                    case CommandCode.OldCall:
                        EnqueueCallRequest(requestHeader.RequestId, arraySegment);
                        break;
                    case CommandCode.Eval:
                        EnqueueEvalRequest(requestHeader.RequestId, arraySegment);
                        break;
                    case CommandCode.Execute:
                        EnqueueExecuteSqlRequest(requestHeader.RequestId, arraySegment);
                        break;
                    case CommandCode.Ping:
                        EnqueuePingRequest(requestHeader.RequestId, arraySegment);
                        break;
                    case CommandCode.Insert:
                        EnqueueInsertRequest(requestHeader.RequestId, arraySegment);
                        break;
                    case CommandCode.Replace:
                        EnqueueReplaceRequest(requestHeader.RequestId, arraySegment);
                        break;
                    case CommandCode.Delete:
                        EnqueueDeleteRequest(requestHeader.RequestId, arraySegment);
                        break;
                    default:
                        throw new NotImplementedException($"Process request command code {requestHeader.Code} not implemented");
                }

                offset += (int)packetSize.Value;

                _newRequestEvent.Set();
            }
        }

        public override void Flush()
        {
            if (_isStreamClose)
            {
                throw new ObjectDisposedException(nameof(TarantoolStreamMock));
            }

            base.Flush();
        }

        public override void Close()
        {
            if (!_isStreamClose)
            {
                _exitEvent.Set();
                _isSessionOpen = false;
                _requestQueue.Clear();
                _responseQueue.Clear();

                base.Close();
            }

            _isStreamClose = true;
        }

#nullable enable
        private void ProcessPackages()
        {
            var handles = new WaitHandle[] { _exitEvent, _newRequestEvent };

            while (!_isStreamClose)
            {
                switch (WaitHandle.WaitAny(handles))
                {
                    case 0:
                        return;
                    case 1:

                        while (_requestQueue.Count > 0)
                        {
                            object? request = null;
                            lock (_processingLock)
                            {
                                if (_requestQueue.Count > 0)
                                {
                                    request = _requestQueue.Dequeue();
                                }
                            }

                            if (request != null)
                            {
                                EnqueueMockResponse((DictionaryEntry)request);
                            }
                        }

                        _newRequestEvent.Reset();

                        break;
                }
            }
        }
#nullable disable

        private void EnqueueAuthRequest(RequestId requestId, ArraySegment arraySegment)
        {
            AuthenticationRequest request = (AuthenticationRequest)(TarantoolMockContext.AuthenticationPacketConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize AuthenticationRequest"));

            lock (_processingLock)
            {
                _requestQueue.Enqueue(new DictionaryEntry(requestId, request));
            }
        }

        private void EnqueueSelectRequest(RequestId requestId, ArraySegment arraySegment)
        {
            SelectRequest request = (SelectRequest)(TarantoolMockContext.SelectPacketConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize SelectRequest"));

            lock (_processingLock)
            {
                _requestQueue.Enqueue(new DictionaryEntry(requestId, request));
            }
        }

        private void EnqueueCallRequest(RequestId requestId, ArraySegment arraySegment)
        {
            CallRequest request = (CallRequest)(TarantoolMockContext.CallPacketConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize CallRequest"));

            lock (_processingLock)
            {
                _requestQueue.Enqueue(new DictionaryEntry(requestId, request));
            }
        }

        private void EnqueueEvalRequest(RequestId requestId, ArraySegment arraySegment)
        {
            EvalRequest request = (EvalRequest)(TarantoolMockContext.EvalPacketConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize EvalRequest"));

            lock (_processingLock)
            {
                _requestQueue.Enqueue(new DictionaryEntry(requestId, request));
            }
        }

        private void EnqueueExecuteSqlRequest(RequestId requestId, ArraySegment arraySegment)
        {
            ExecuteSqlRequest request = (ExecuteSqlRequest)(TarantoolMockContext.ExecuteSqlRequestConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize ExecuteSqlRequest"));

            lock (_processingLock)
            {
                _requestQueue.Enqueue(new DictionaryEntry(requestId, request));
            }
        }

        private void EnqueuePingRequest(RequestId requestId, ArraySegment arraySegment)
        {
            PingRequest request = (PingRequest)(TarantoolMockContext.PingPacketConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize PingRequest"));

            lock (_processingLock)
            {
                _requestQueue.Enqueue(new DictionaryEntry(requestId, request));
            }
        }

        private void EnqueueInsertRequest(RequestId requestId, ArraySegment arraySegment)
        {
            InsertRequest request = (InsertRequest)(TarantoolMockContext.InsertPacketConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize InsertRequest"));

            lock (_processingLock)
            {
                _requestQueue.Enqueue(new DictionaryEntry(requestId, request));
            }
        }

        private void EnqueueReplaceRequest(RequestId requestId, ArraySegment arraySegment)
        {
            ReplaceRequest request = (ReplaceRequest)(TarantoolMockContext.ReplacePacketConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize InsertRequest"));

            lock (_processingLock)
            {
                _requestQueue.Enqueue(new DictionaryEntry(requestId, request));
            }
        }

        private void EnqueueDeleteRequest(RequestId requestId, ArraySegment arraySegment)
        {
            DeleteRequest request = (DeleteRequest)(TarantoolMockContext.DeletePacketConverter.Read(arraySegment) ?? throw new SerializationException("Error serialize DeleteRequest"));

            lock (_processingLock)
            {
                _requestQueue.Enqueue(new DictionaryEntry(requestId, request));
            }
        }

        private void EnqueueMockResponse(DictionaryEntry request)
        {
            using (MemoryStream writer = new MemoryStream())
            {
                writer.Seek(Constants.PacketSizeBufferSize, SeekOrigin.Begin);
                var requestId = (RequestId)request.Key;
                try
                {
                    if (request.Value is AuthenticationRequest authentication)
                    {
                        WriteAuthenticationResponseToStream(writer, requestId, authentication);
                    }
                    else
                    {
                        if (request.Value is SelectRequest select)
                        {
                            WriteSelectResponseToStream(writer, requestId, select);
                        }
                        else
                        {
                            if (request.Value is CallRequest call)
                            {
                                WriteCallResponseToStream(writer, requestId, call);
                            }
                            else
                            {
                                if (request.Value is EvalRequest eval)
                                {
                                    WriteEvalResponseToStream(writer, requestId, eval);
                                }
                                else
                                {
                                    if (request.Value is ExecuteSqlRequest execute)
                                    {
                                        WriteExecuteSqlResponseToStream(writer, requestId, execute);
                                    }
                                    else
                                    {
                                        if (request.Value is PingRequest ping)
                                        {
                                            WritePingResponseToStream(writer, requestId, ping);
                                        }
                                        else
                                        {
                                            if (request.Value is InsertReplaceRequest insertReplace)
                                            {
                                                WriteInsertReplaceResponseToStream(writer, requestId, insertReplace);
                                            }
                                            else
                                            {
                                                if (request.Value is DeleteRequest delete)
                                                {
                                                    WriteDeleteResponseToStream(writer, requestId, delete);
                                                }
                                                else
                                                {
                                                    throw new ArgumentException("Unknown request type");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    writer.SetLength(0);
                    writer.Seek(Constants.PacketSizeBufferSize, SeekOrigin.Begin);
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.ErrorMask, requestId, 1), writer);
                    MessagePackSerializer.Serialize(new ErrorResponse($"{e}"), writer);
                    AddPacketSize(writer, new PacketSize((uint)(writer.Position - Constants.PacketSizeBufferSize)));
                }
                finally
                {
                    writer.Seek(0, SeekOrigin.Begin);

                    lock (_responseLock)
                    {
                        int b = -1;
                        while ((b = writer.ReadByte()) > -1)
                        {
                            _responseQueue.Enqueue((byte)b);
                        }
                    }

                    if (_responseQueue.Count > 0)
                    {
                        _newResponseEvent.Set();
                    }
                }
            }
        }

        private void WriteAuthenticationResponseToStream(MemoryStream writer, RequestId requestId, AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest.Username == TarantoolMockContext.AdminUserName && CompareArray(_adminPasswordScramble, authenticationRequest.Scramble))
            {
                MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
                MessagePackSerializer.Serialize(new EmptyResponseMock(), writer);
                _isAdminSession = true;
            }
            else
            {
                MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.ErrorMask, requestId, 1), writer);
                MessagePackSerializer.Serialize(new ErrorResponse("User not found or supplied credentials are invalid."), writer);
            }

            AddPacketSize(writer, new PacketSize((uint)(writer.Position - Constants.PacketSizeBufferSize)));
        }

        private void WriteExecuteSqlResponseToStream(MemoryStream writer, RequestId requestId, ExecuteSqlRequest executeSqlRequest)
        {
            switch (executeSqlRequest.Query)
            {
                case "SELECT 1 as ABC, 'z', 3":
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
                    var responseTuple = TarantoolTuple.Create(1, "z", 3);
                    var response = new DataResponseMock(new TarantoolTuple[] { responseTuple }, new SqlInfo(1));
                    MessagePackSerializer.Serialize(response, writer);
                    break;
                case "create table sql_test(id int primary key, name text)":
                case "drop table sql_test":
                    if (!_isAdminSession)
                    {
                        MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.ErrorMask, requestId, 1), writer);
                        MessagePackSerializer.Serialize(new DataResponseMock(new ErrorResponse($"{(executeSqlRequest.Query.StartsWith("create") ? "Create" : "Drop")} access to space 'sql_test' is denied for user 'guest'.")), writer);
                    }
                    else
                    {
                        MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
                        MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[] { TarantoolTuple.Empty }, new SqlInfo(1)), writer);
                    }

                    break;
                case "insert into sql_test values (1, 'asdf'), (2, 'zxcv'), (3, 'qwer')":
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
                    MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[] { TarantoolTuple.Empty }, new SqlInfo(3)), writer);
                    break;
                case "update sql_test set name = '1234' where id = 2":
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
                    MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[] { TarantoolTuple.Create(2, "1234") }, new SqlInfo(1)), writer);
                    break;
                case "SELECT * FROM sql_test WHERE id = $1":
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);
                    var responseSelect = TarantoolTuple.Create(2, "1234");
                    var dataResponse = new DataResponseMock(new TarantoolTuple[] { responseSelect }, new SqlInfo(1));
                    MessagePackSerializer.Serialize(dataResponse, writer);
                    break;
                default:
                    throw new NotImplementedException($"Unknown Expression = '{executeSqlRequest.Query}'");
            }

            AddPacketSize(writer, new PacketSize((uint)(writer.Position - Constants.PacketSizeBufferSize)));
        }

        private void WriteSelectResponseToStream(MemoryStream writer, RequestId requestId, SelectRequest selectRequest)
        {
            MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);

            switch (selectRequest.SpaceId)
            {
                case Schema.VSpace:
                    MessagePackSerializer.Serialize(new DataResponseMock(_context.Spaces), writer);
                    break;
                case Schema.VIndex:
                    MessagePackSerializer.Serialize(new DataResponseMock(_context.Indices), writer);
                    break;
                case 2:
                    uint key = uint.Parse(selectRequest.SelectKey[0].ToString());
                    if (key > _context.TestTable.Length)
                    {
                        if (!_context.ModifyTable.Contains(key + selectRequest.Offset))
                        {
                            MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[0]), writer);
                        }
                        else
                        {
                            if (selectRequest.Limit == 1)
                            {
                                MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[] { (TarantoolTuple)_context.ModifyTable[(uint)selectRequest.Offset + key] }), writer);
                            }
                            else
                            {
                                var length = (int)(key + selectRequest.Offset) - _context.TestTable.Length;

                                if (length > _context.ModifyTable.Count)
                                {
                                    MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[0]), writer);
                                }
                                else
                                {
                                    length = length + (int)selectRequest.Limit;

                                    length = length > _context.TestTable.Length ? _context.TestTable.Length : length;

                                    var responseTuples = new TarantoolTuple[length];
                                    int index = 0;
                                    for (int i = 0; i < length; i++)
                                    {
                                        var selectKey = (uint)(selectRequest.Offset + i + key);
                                        if (_context.ModifyTable.Contains(selectKey))
                                        {
                                            responseTuples[index] = (TarantoolTuple)_context.ModifyTable[selectKey];
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    MessagePackSerializer.Serialize(new DataResponseMock(responseTuples), writer);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (selectRequest.Limit == 1)
                        {
                            MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[] { _context.TestTable[selectRequest.Offset + key - 1] }), writer);
                        }
                        else
                        {
                            var length = selectRequest.Limit > _context.TestTable.Length + _context.ModifyTable.Count ? (uint)_context.TestTable.Length + _context.ModifyTable.Count : selectRequest.Limit;

                            length = length - key + 1;

                            TarantoolTuple[] responseTuples = new TarantoolTuple[length];
                            for (int i = 0; i < length; i++)
                            {
                                var tableIndex = selectRequest.Offset + i + key - 1;
                                if (tableIndex < _context.TestTable.Length)
                                {
                                    responseTuples[i] = _context.TestTable[tableIndex];
                                }
                                else
                                {
                                    break;
                                }
                            }

                            MessagePackSerializer.Serialize(new DataResponseMock(responseTuples), writer);
                        }
                    }

                    break;
                default:
                    throw new NotImplementedException($"Unknown SpaceId value = '{selectRequest.SpaceId}'");
            }

            AddPacketSize(writer, new PacketSize((uint)(writer.Position - Constants.PacketSizeBufferSize)));
        }

        private void WriteEvalResponseToStream(MemoryStream writer, RequestId requestId, EvalRequest evalRequest)
        {
            MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, 1), writer);

            switch (evalRequest.Expression)
            {
                case "return box.info":
                    MessagePackSerializer.Serialize(new DataResponseMock(_context.TestBoxInfo), writer);
                    break;
                case "return ...":
                    MessagePackSerializer.Serialize(new DataResponseMock(evalRequest.Tuple), writer);
                    break;
                case "return 1":
                    ArrayList arrayList = new ArrayList();
                    arrayList.Add(1);
                    MessagePackSerializer.Serialize(new DataResponseMock(arrayList), writer);
                    break;
                case "return box.space.bands:select{...}":
                    MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[][] { _context.TestTable }), writer);
                    break;
                case "return box.space.bands:select({3}, {iterator='GT', limit = 3})":
                    var result = new TarantoolTuple[][]
                    {
                        new TarantoolTuple[]
                        {
                            _context.TestTable[3],
                            _context.TestTable[4],
                            _context.TestTable[5]
                        }
                    };

                    MessagePackSerializer.Serialize(new DataResponseMock(result), writer);
                    break;
                default:
                    throw new NotImplementedException($"Unknown Expression = '{evalRequest.Expression}'");
            }

            AddPacketSize(writer, new PacketSize((uint)(writer.Position - Constants.PacketSizeBufferSize)));
        }

        private void WriteInsertReplaceResponseToStream(MemoryStream writer, RequestId requestId, InsertReplaceRequest insertReplacRequest)
        {
            MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, insertReplacRequest.SpaceId), writer);

            uint key = uint.Parse(insertReplacRequest.Tuple[0].ToString());
            if (!_context.ModifyTable.Contains(key))
            {
                _context.ModifyTable.Add(key, insertReplacRequest.Tuple);
            }
            else
            {
                _context.ModifyTable[key] = insertReplacRequest.Tuple;
            }

            MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[] { (TarantoolTuple)_context.ModifyTable[key] }), writer);

            AddPacketSize(writer, new PacketSize((uint)(writer.Position - Constants.PacketSizeBufferSize)));
        }

        private void WriteDeleteResponseToStream(MemoryStream writer, RequestId requestId, DeleteRequest deleteRequest)
        {
            MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.Ok, requestId, deleteRequest.SpaceId), writer);

            uint key = uint.Parse(deleteRequest.Key[0].ToString());

            if (_context.ModifyTable.Contains(key))
            {
                var tuple = _context.ModifyTable[key];
                _context.ModifyTable.Remove(key);
                MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[] { (TarantoolTuple)tuple }), writer);
            }
            else
            {
                MessagePackSerializer.Serialize(new DataResponseMock(new TarantoolTuple[] { TarantoolTuple.Empty }), writer);
            }

            AddPacketSize(writer, new PacketSize((uint)(writer.Position - Constants.PacketSizeBufferSize)));
        }
    }
}
