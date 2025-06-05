// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Threading;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Dto;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Client.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Exceptions;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Headers;
using nanoFramework.Tarantool.Model.Requests;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Client.Connections
{
    /// <summary>
    /// The logical connection class.
    /// </summary>
    internal class LogicalConnection : ILogicalConnection
    {
        private readonly ResponsePacketConverter _responseConverter = (ResponsePacketConverter)ConverterContext.GetConverter(typeof(DataResponse));
        private readonly ClientOptions _clientOptions;
        private readonly RequestIdCounter _requestIdCounter;
        private readonly NetworkStreamPhysicalConnection _physicalConnection;
        private readonly IResponseReader _responseReader;
        private readonly IRequestWriter _requestWriter;
        private readonly ManualResetEvent _exitEvent = new ManualResetEvent(false);

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalConnection"/> class.
        /// </summary>
        /// <param name="options">Client options.</param>
        /// <param name="requestIdCounter"><see cref="Tarantool"/> request id counter.</param>
        public LogicalConnection(ClientOptions options, RequestIdCounter requestIdCounter)
        {
            _clientOptions = options;
            _requestIdCounter = requestIdCounter;

            _physicalConnection = new NetworkStreamPhysicalConnection();
            _responseReader = new ResponseReader(_clientOptions, _physicalConnection);
            _requestWriter = new RequestWriter(_clientOptions, _physicalConnection);
        }

        public uint PingsFailedByTimeoutCount { get; private set; } = 0;

        private static MemoryStream CreateAndSerializeHeader(IRequest request, RequestId requestId)
        {
            var stream = new MemoryStream();

            var requestHeader = new RequestHeader(request.Code, requestId);
            stream.Seek(Constants.PacketSizeBufferSize, SeekOrigin.Begin);
            MessagePackSerializer.Serialize(requestHeader, stream);

            return stream;
        }

        private static void AddPacketSize(MemoryStream stream, PacketSize packetLength)
        {
            stream.Seek(0, SeekOrigin.Begin);
            MessagePackSerializer.Serialize(packetLength, stream);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;

            _exitEvent.Set();
            _responseReader.Dispose();
            _requestWriter.Dispose();
            _physicalConnection.Dispose();
        }

        public void Connect()
        {
            _physicalConnection.Connect(_clientOptions);

            var greetingsResponseBytes = new byte[128];
            var readCount = _physicalConnection.Read(greetingsResponseBytes, 0, greetingsResponseBytes.Length);
            if (readCount != greetingsResponseBytes.Length)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(readCount);
            }

            var greetings = new GreetingsResponse(greetingsResponseBytes);

            PingsFailedByTimeoutCount = 0;

            _responseReader.BeginReading();
            _requestWriter.BeginWriting();

            LoginIfNotGuest(greetings);
        }

        public bool IsConnected()
        {
            if (disposed)
            {
                return false;
            }

            return _responseReader.IsConnected && _requestWriter.IsConnected && _physicalConnection.IsConnected;
        }

        public void SendRequestWithEmptyResponse(IRequest request, TimeSpan timeout)
        {
            var _ = SendRequestImpl(request, timeout);
        }

#nullable enable
        public DataResponse? SendRequest(IRequest request, TimeSpan timeout, Type? responseDataType)
        {
            var arraySegment = SendRequestImpl(request, timeout);

            if (arraySegment != null)
            {
                if (responseDataType == null)
                {
                    return _responseConverter.Read(arraySegment);
                }

                var converter = TarantoolContext.Instance.GetDataResponseDataTypeConverter(responseDataType);

                return (DataResponse?)converter.Read(arraySegment);
            }
            else
            {
                return null;
            }    
        }

        public ArraySegment? SendRawRequest(IRequest request, TimeSpan timeout)
        {
            return SendRequestImpl(request, timeout);
        }

        private void LoginIfNotGuest(GreetingsResponse greetings)
        {
            if (_clientOptions.ConnectionOptions.Nodes.Length < 1)
            {
                throw new ClientSetupException("There are zero configured nodes, you should provide one");
            }

            var singleNode = _clientOptions.ConnectionOptions.Nodes[0];

            if (string.IsNullOrEmpty(singleNode.Uri.UserName))
            {
                //// Debug.WriteLine("Guest mode, no authentication attempt.");

                return;
            }

            var authenticateRequest = AuthenticationRequest.Create(greetings, singleNode.Uri);

            //// Debug.WriteLine($"Authentication request send: {authenticateRequest}");

            SendRequestWithEmptyResponse(authenticateRequest, TimeSpan.Zero);
        }

        private ArraySegment? SendRequestImpl(IRequest request, TimeSpan timeout)
        {
            if (disposed)
            {
                return null;
            }

            var requestId = _requestIdCounter.GetRequestId();

            var responseCompletionSource = _responseReader.GetResponseCompletionSource(requestId);

            void CompleteResult(object result)
            {
                responseCompletionSource.Result = result;
                responseCompletionSource.ResponseEvent.Set();
            }

            responseCompletionSource.CompleteResultCallback = CompleteResult;

            using (var stream = CreateAndSerializeHeader(request, requestId))
            {
                MessagePackSerializer.Serialize(request, stream);
                var totalLength = stream.Position - Constants.PacketSizeBufferSize;
                var packetLength = new PacketSize((uint)totalLength);
                AddPacketSize(stream, packetLength);

                byte[] buffer = stream.ToArray();

                if (buffer == null || buffer.Length < 1)
                {
                    throw new InvalidOperationException("broken buffer");
                }

                //// keep API for the sake of backward comp.
                //// merged header and body

                _requestWriter.Write(buffer);

                //// System.Diagnostics.Debug.WriteLine($"Request with Id {requestId} is sent.");
            }

            try
            {
                WaitHandle[] waitHandles = new[] { _exitEvent, responseCompletionSource.ResponseEvent };

                if (timeout > TimeSpan.Zero)
                {
                    var waitResult = WaitHandle.WaitAny(waitHandles, (int)(timeout.Ticks / TimeSpan.TicksPerMillisecond), false);
                    if (waitResult < 0)
                    {
                        throw new TimeoutException();
                    }

                    if (waitResult == 0)
                    {
                        return null;
                    }
                }
                else
                {
                    var waitResult = WaitHandle.WaitAny(waitHandles, _clientOptions.RequestTimeout, false);
                    if (waitResult == 0)
                    {
                        return null;
                    }
                }

                if (responseCompletionSource.Result is Exception exception)
                {
                    throw exception;
                }

                var arraySegment = (ArraySegment?)responseCompletionSource.Result;

                //// System.Diagnostics.Debug.WriteLine($"Response with requestId {requestId} is received.");

                return arraySegment;
            }
            catch (ArgumentException)
            {
                _responseReader.PopResponseCompletionSource(requestId);

                //// Debug.WriteLine($"Response with requestId {requestId} failed, content:\n{buffer.ToReadableString()} ");
                
                throw;
            }
            catch (TimeoutException)
            {
                _responseReader.PopResponseCompletionSource(requestId);
                PingsFailedByTimeoutCount++;
                throw;
            }
        }
    }
}
