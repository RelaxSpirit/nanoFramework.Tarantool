// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using System.IO;
using System.Threading;
#endif
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
        private static readonly ResponsePacketConverter ResponseConverter = (ResponsePacketConverter)ConverterContext.GetConverter(typeof(DataResponse));

        private readonly ClientOptions clientOptions;
        private readonly RequestIdCounter requestIdCounter;
        private readonly NetworkStreamPhysicalConnection physicalConnection;
        private readonly IResponseReader responseReader;
        private readonly IRequestWriter requestWriter;
        private readonly ManualResetEvent exitEvent = new ManualResetEvent(false);

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalConnection"/> class.
        /// </summary>
        /// <param name="options">Client options.</param>
        /// <param name="requestIdCounter"><see cref="Tarantool"/> request id counter.</param>
        public LogicalConnection(ClientOptions options, RequestIdCounter requestIdCounter)
        {
            this.clientOptions = options;
            this.requestIdCounter = requestIdCounter;

            this.physicalConnection = new NetworkStreamPhysicalConnection();
            this.responseReader = new ResponseReader(this.clientOptions, this.physicalConnection);
            this.requestWriter = new RequestWriter(this.clientOptions, this.physicalConnection);
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
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            this.exitEvent.Set();
            this.responseReader.Dispose();
            this.requestWriter.Dispose();
            this.physicalConnection.Dispose();
        }

        public void Connect()
        {
            this.physicalConnection.Connect(clientOptions);

            var greetingsResponseBytes = new byte[128];
            var readCount = this.physicalConnection.Read(greetingsResponseBytes, 0, greetingsResponseBytes.Length);
            if (readCount != greetingsResponseBytes.Length)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(readCount);
            }

            var greetings = new GreetingsResponse(greetingsResponseBytes);

            this.PingsFailedByTimeoutCount = 0;

            this.responseReader.BeginReading();
            this.requestWriter.BeginWriting();

            this.LoginIfNotGuest(greetings);
        }

        public bool IsConnected()
        {
            if (this.disposed)
            {
                return false;
            }

            return this.responseReader.IsConnected && this.requestWriter.IsConnected && this.physicalConnection.IsConnected;
        }

        public void SendRequestWithEmptyResponse(IRequest request, TimeSpan timeout)
        {
            var _ = this.SendRequestImpl(request, timeout);
        }

#nullable enable
        public DataResponse? SendRequest(IRequest request, TimeSpan timeout, Type? responseDataType)
        {
            var arraySegment = this.SendRequestImpl(request, timeout);

            if (arraySegment != null)
            {
                if (responseDataType == null)
                {
                    return ResponseConverter.Read(arraySegment);
                }

                var converter = TarantoolContext.GetDataResponseDataTypeConverter(responseDataType);

                return (DataResponse?)converter.Read(arraySegment);
            }
            else
            {
                return null;
            }    
        }

        public ArraySegment? SendRawRequest(IRequest request, TimeSpan timeout)
        {
            return this.SendRequestImpl(request, timeout);
        }

        private void LoginIfNotGuest(GreetingsResponse greetings)
        {
            if (this.clientOptions.ConnectionOptions.Nodes.Length < 1)
            {
                throw new ClientSetupException("There are zero configured nodes, you should provide one");
            }

            var singleNode = this.clientOptions.ConnectionOptions.Nodes[0];

            if (string.IsNullOrEmpty(singleNode.Uri.UserName))
            {
                //// Debug.WriteLine("Guest mode, no authentication attempt.");

                return;
            }

            var authenticateRequest = AuthenticationRequest.Create(greetings, singleNode.Uri);

            //// Debug.WriteLine($"Authentication request send: {authenticateRequest}");

            this.SendRequestWithEmptyResponse(authenticateRequest, TimeSpan.Zero);
        }

        private ArraySegment? SendRequestImpl(IRequest request, TimeSpan timeout)
        {
            if (this.disposed)
            {
                return null;
            }

            var requestId = this.requestIdCounter.GetRequestId();

            var responseCompletionSource = this.responseReader.GetResponseCompletionSource(requestId);

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

                this.requestWriter.Write(buffer);

                //// System.Diagnostics.Debug.WriteLine($"Request with Id {requestId} is sent.");
            }

            try
            {
                WaitHandle[] waitHandles = new[] { this.exitEvent, responseCompletionSource.ResponseEvent };

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
                    var waitResult = WaitHandle.WaitAny(waitHandles, this.clientOptions.RequestTimeout, false);
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
                this.responseReader.PopResponseCompletionSource(requestId);

                //// Debug.WriteLine($"Response with requestId {requestId} failed, content:\n{buffer.ToReadableString()} ");
                
                throw;
            }
            catch (TimeoutException)
            {
                this.responseReader.PopResponseCompletionSource(requestId);
                this.PingsFailedByTimeoutCount++;
                throw;
            }
        }
    }
}
