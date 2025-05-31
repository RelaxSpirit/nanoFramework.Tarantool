// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
#else
using System.Collections;
using System.Text;
#endif
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Dto;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Headers;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Client.Stream
{
    internal class ResponseReader : IResponseReader
    {
        private static readonly string ErrorReadingPacket = "Error reading the response packet from the network stream";

        private readonly byte[] _packetSizeBuffer = new byte[Constants.PacketSizeBufferSize];
        private readonly IPhysicalConnection _physicalConnection;
        private readonly Hashtable _pendingRequests = new Hashtable();
        private readonly object _pendingRequestsLock = new object();
        private readonly Thread _readingThread;
        private readonly Thread _processPackageThread;
        private readonly ManualResetEvent _exitEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _readNewResponseEvent = new ManualResetEvent(false);
        private readonly AutoResetEvent _allResponsesProcessedEvent = new AutoResetEvent(false);
        private readonly ManualResetEvent _processResponsesEvent = new ManualResetEvent(false);
        private readonly Queue _readingParts = new Queue();
        private readonly object _processingLock = new object();
        private readonly WaitHandle[] _readingPartsQueueFreeWaitHandles;

        private readonly byte[] _buffer;
        private int _readingOffset = 0;

        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseReader"/> class.
        /// </summary>
        /// <param name="clientOptions">Client options.</param>
        /// <param name="physicalConnection">Physical connection <see cref="IPhysicalConnection"/> interface.</param>
        internal ResponseReader(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            _physicalConnection = physicalConnection;
            _buffer = new byte[clientOptions.ConnectionOptions.ReadStreamBufferSize];
            _readingThread = new Thread(ReadFunction);
            _processPackageThread = new Thread(ProcessPackageFunction);
            _readingPartsQueueFreeWaitHandles = new WaitHandle[] { _exitEvent, _allResponsesProcessedEvent };
        }

        bool IResponseReader.IsConnected => !_disposed;

        private static void LogUnMatchedResponse(byte[] result)
        {
            var builder = new StringBuilder("Warning: can't match request via requestId from response. Response:");
            var length = 80 / 3;
            for (var i = 0; i < result.Length; i++)
            {
                if (i % length == 0)
                {
                    builder.AppendLine().Append("   ");
                }
                else
                {
                    builder.Append(' ');
                }

                builder.Append(result[i].ToString("X2"));
            }

            Console.WriteLine(builder.ToString());
        }

#nullable enable
        private static ArraySegment GetErrorResponsePacket(string message, ResponseHeader? responseHeader)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //// CommandCode.ErrorMask is a Unknown error
                if (responseHeader != null)
                {
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.ErrorMask, responseHeader.RequestId, responseHeader.SchemaId), ms);
                }
                else
                {
                    MessagePackSerializer.Serialize(new ResponseHeader(CommandCode.ErrorMask, new RequestId(0), 0), ms);
                }

                MessagePackSerializer.Serialize(new ErrorResponse(message), ms);

                var result = ms.ToArray();
                return new ArraySegment(result, 0, result.Length);
            }
        }

        private static ResponseHeader? GetResponseHeader(ArraySegment buffer)
        {
            ResponseHeader? responseHeader = null;
            try
            {
                responseHeader = ResponseHeaderConverter.Read(buffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ErrorReadingPacket}. Error read response header:\n{ex}");
            }

            return responseHeader;
        }

        public void Dispose()
        {
            if (!_exitEvent.Set() || _disposed)
            {
                return;
            }

            _disposed = true;

            _physicalConnection.Dispose();

            lock (_pendingRequestsLock)
            {
                _pendingRequests.Clear();
            }
        }

        CompletionSource IResponseReader.GetResponseCompletionSource(RequestId requestId)
        {
            if (_disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(ResponseReader));
#endif
            }

            lock (_pendingRequestsLock)
            {
                if (_pendingRequests.Contains(requestId.Value))
                {
                    throw ExceptionHelper.RequestWithSuchIdAlreadySent(requestId);
                }
            }

            var cs = new CompletionSource();

            lock (_pendingRequestsLock)
            {
                _pendingRequests.Add(requestId.Value, cs);
            }

            _readNewResponseEvent.Set();

            return cs;
        }

        void IResponseReader.BeginReading()
        {
            if (_disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(ResponseReader));
#endif
            }

            _readingThread.Start();
            _processPackageThread.Start();
        }

        CompletionSource? IResponseReader.PopResponseCompletionSource(RequestId requestId)
        {
            if (_disposed)
            {
                return null;
            }

            lock (_pendingRequestsLock)
            {
                CompletionSource? resultCompletionSource = (CompletionSource?)_pendingRequests[requestId.Value];

                if (resultCompletionSource != null)
                {
                    _pendingRequests.Remove(requestId.Value);
                }

                return resultCompletionSource;
            }
        }

        private void MatchResult(byte[] result)
        {
            ArraySegment arraySegment = new ArraySegment(result, 0, result.Length);
            var header = GetResponseHeader(arraySegment);
            if (header != null)
            {
                var cs = ((IResponseReader)this).PopResponseCompletionSource(header.RequestId);

                if (cs?.CompleteResultCallback == null)
                {
                    if (header.Code != CommandCode.Ping)
                    {
                        LogUnMatchedResponse(result);
                    }
                }
                else
                {
                    if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
                    {
                        var errorResponse = ErrorResponsePacketConverter.Read(arraySegment);
                        cs.CompleteResultCallback.Invoke(ExceptionHelper.TarantoolError(header, errorResponse));
                    }
                    else
                    {
                        cs.CompleteResultCallback.Invoke(arraySegment);
                    }
                }
            }
            else
            {
                LogUnMatchedResponse(result);
            }
        }

        private void ReadFunction()
        {
            var handles = new WaitHandle[] { _exitEvent, _readNewResponseEvent };

            while (true)
            {
                switch (WaitHandle.WaitAny(handles))
                {
                    case 0:
                        return;
                    case 1:
                        try
                        {
                            ReadNetworkStream();
                        }
                        catch (Exception e)
                        {
                            if (_disposed)
                            {
                                return;
                            }
                            else
                            {
                                Console.WriteLine($"Error reading network stream:\n{e}");
                            }
                        }

                        break;
                }
            }
        }

        private void ProcessPackageFunction()
        {
            var handles = new WaitHandle[] { _exitEvent, _processResponsesEvent };
            while (true)
            {
                switch (WaitHandle.WaitAny(handles))
                {
                    case 0:
                        return;
                    case 1:
                        while (_readingParts.Count > 0)
                        { 
                            ArraySegment? arraySegment = null;

                            lock (_processingLock)
                            {
                                if (_readingParts.Count > 0)
                                {
                                    arraySegment = (ArraySegment?)_readingParts.Dequeue();
                                }
                            }

                            if (arraySegment != null)
                            {
                                MatchResult((byte[])arraySegment);
                            }
                        }

                        _processResponsesEvent.Reset();
                        _allResponsesProcessedEvent.Set();
                        break;
                }
            }
        }

        private void ReadNetworkStream()
        {
            var packetSize = GetPacketSize();
            if (packetSize != null)
            {
                //// DOTO The response packet may be too large (maximum 2 Gb).
                //// For a microcontroller, this may lead to the exhaustion of all memory.
                if (packetSize.Value > _buffer.Length)
                {
                    if (IsReadingPartsQueueFree())
                    {
                        int reader = _physicalConnection.Read(_buffer, _readingOffset, _buffer.Length);
                        ResponseHeader? responseHeader = GetResponseHeader(_buffer);

                        var errorPacketSegment = GetErrorResponsePacket($"The package size {packetSize.Value} bytes is too large, maximum packet size for reception {_buffer.Length} bytes", responseHeader);

                        //// Clear network stream
                        while ((reader += _physicalConnection.Read(_buffer, _readingOffset, _buffer.Length)) < packetSize.Value)
                        {
                            if (_disposed)
                            {
                                return;
                            }
                        }

                        lock (_processingLock)
                        {
                            _readingParts.Enqueue(errorPacketSegment);
                        }

                        _processResponsesEvent.Set();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    EnqueuePacket((int)packetSize.Value);
                }
            }
            else
            {
                _processResponsesEvent.Reset();
            }
        }

        private PacketSize? GetPacketSize()
        {
            var readByteCount = _physicalConnection.Read(_packetSizeBuffer, 0, _packetSizeBuffer.Length);
            
            if (_disposed)
            {
                return null;
            }

            while (readByteCount < _packetSizeBuffer.Length && readByteCount > 0)
            {
                var reading = _physicalConnection.Read(_packetSizeBuffer, readByteCount, _packetSizeBuffer.Length - readByteCount);
                if (reading < 1 || _disposed)
                {
                    return null;
                }

                readByteCount += reading;
            }

            return (PacketSize?)MessagePackSerializer.Deserialize(typeof(PacketSize), _packetSizeBuffer);
        }

        private void EnqueuePacket(int packetSize)
        {
            if (_buffer.Length - _readingOffset < packetSize)
            {
                while (!IsReadingPartsQueueFree())
                {
                    if (_disposed)
                    {
                        return;
                    }
                }
            }

            ArraySegment? packetSegmentArray = null;
            while (packetSegmentArray == null)
            {
                int readingPacketSize = ReadAllPacketBytes(packetSize, out packetSegmentArray);
                if (packetSegmentArray != null)
                {
                    _readingOffset += readingPacketSize;
                    lock (_processingLock)
                    {
                        _readingParts.Enqueue(packetSegmentArray);
                    }

                    _processResponsesEvent.Set();
                    break;
                }
            }
        }

        private int ReadAllPacketBytes(int packetLength, out ArraySegment? result)
        {
            var freeBufferSpace = _buffer.Length - _readingOffset;
            result = null;
            int reading = 0;
            if (freeBufferSpace > packetLength)
            {
                while (reading < packetLength)
                {
                    var readBytesCount = _physicalConnection.Read(_buffer, _readingOffset + reading, packetLength - reading);
                    if (readBytesCount < 1)
                    {
                        break;
                    }
                    else
                    {
                        reading += readBytesCount;
                    }
                }

                if (reading >= packetLength)
                {
                    result = new ArraySegment(_buffer, _readingOffset, reading);
                }
            }

            return reading;
        }

        private bool IsReadingPartsQueueFree()
        {
            lock (_processingLock)
            {
                if (_readingParts.Count < 1)
                {
                    _readingOffset = 0;
                    return true;
                }
            }

            int waitResult = WaitHandle.WaitAny(_readingPartsQueueFreeWaitHandles);
            if (waitResult > 0)
            {
                _readingOffset = 0;
                return true;
            }

            return false;
        }
    }
}
