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

        private readonly byte[] packetSizeBuffer = new byte[Constants.PacketSizeBufferSize];
        private readonly IPhysicalConnection physicalConnection;
        private readonly Hashtable pendingRequests = new Hashtable();
        private readonly object pendingRequestsLock = new object();
        private readonly Thread readingThread;
        private readonly Thread processPackageThread;
        private readonly ManualResetEvent exitEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent readNewResponseEvent = new ManualResetEvent(false);
        private readonly AutoResetEvent allResponsesProcessedEvent = new AutoResetEvent(false);
        private readonly ManualResetEvent processResponsesEvent = new ManualResetEvent(false);
        private readonly Queue readingParts = new Queue();
        private readonly object processingLock = new object();
        private readonly WaitHandle[] readingPartsQueueFreeWaitHandles;

        private readonly byte[] buffer;
        private int readingOffset = 0;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseReader"/> class.
        /// </summary>
        /// <param name="clientOptions">Client options.</param>
        /// <param name="physicalConnection">Physical connection <see cref="IPhysicalConnection"/> interface.</param>
        internal ResponseReader(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            this.physicalConnection = physicalConnection;
            this.buffer = new byte[clientOptions.ConnectionOptions.ReadStreamBufferSize];
            this.readingThread = new Thread(this.ReadFunction);
            this.processPackageThread = new Thread(this.ProcessPackageFunction);
            this.readingPartsQueueFreeWaitHandles = new WaitHandle[] { this.exitEvent, this.allResponsesProcessedEvent };
        }

        bool IResponseReader.IsConnected => !this.disposed;

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
            if (!this.exitEvent.Set() || this.disposed)
            {
                return;
            }

            this.disposed = true;

            this.physicalConnection.Dispose();
            this.readingThread.Join(100);
            this.processPackageThread.Join(100);

            lock (this.pendingRequestsLock)
            {
                this.pendingRequests.Clear();
            }
        }

        CompletionSource IResponseReader.GetResponseCompletionSource(RequestId requestId)
        {
            if (this.disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(ResponseReader));
#endif
            }

            lock (this.pendingRequestsLock)
            {
                if (this.pendingRequests.Contains(requestId))
                {
                    throw ExceptionHelper.RequestWithSuchIdAlreadySent(requestId);
                }
            }

            var cs = new CompletionSource();

            lock (this.pendingRequestsLock)
            {
                this.pendingRequests.Add(requestId, cs);
            }

            this.readNewResponseEvent.Set();

            return cs;
        }

        void IResponseReader.BeginReading()
        {
            if (this.disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(ResponseReader));
#endif
            }

            this.readingThread.Start();
            this.processPackageThread.Start();
        }

        CompletionSource? IResponseReader.PopResponseCompletionSource(RequestId requestId)
        {
            if (this.disposed)
            {
                return null;
            }

            lock (this.pendingRequestsLock)
            {
                CompletionSource? resultCompletionSource = (CompletionSource?)this.pendingRequests[requestId];

                if (resultCompletionSource != null)
                {
                    this.pendingRequests.Remove(requestId);
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
                    LogUnMatchedResponse(result);
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
            var handles = new WaitHandle[] { this.exitEvent, this.readNewResponseEvent };

            while (true)
            {
                switch (WaitHandle.WaitAny(handles))
                {
                    case 0:
                        return;
                    case 1:
                        try
                        {
                            this.ReadNetworkStream();
                        }
                        catch (Exception e)
                        {
                            if (this.disposed)
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
            var handles = new WaitHandle[] { exitEvent, processResponsesEvent };
            while (true)
            {
                switch (WaitHandle.WaitAny(handles))
                {
                    case 0:
                        return;
                    case 1:
                        while (readingParts.Count > 0)
                        { 
                            ArraySegment? arraySegment = null;

                            lock (processingLock)
                            {
                                arraySegment = (ArraySegment?)readingParts.Dequeue();
                            }

                            if (arraySegment != null)
                            {
                                MatchResult((byte[])arraySegment);
                            }
                        }

                        processResponsesEvent.Reset();
                        allResponsesProcessedEvent.Set();
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
                if (packetSize.Value > this.buffer.Length)
                {
                    if (IsReadingPartsQueueFree())
                    {
                        physicalConnection.Read(this.buffer, this.readingOffset, this.buffer.Length);
                        ResponseHeader? responseHeader = GetResponseHeader(buffer);

                        var errorPacketSegment = GetErrorResponsePacket($"The package size {packetSize.Value} bytes is too large, maximum packet size for reception {buffer.Length} bytes", responseHeader);

                        //// Clear network stream
                        while (physicalConnection.Read(this.buffer, this.readingOffset, this.buffer.Length) > 0)
                        {
                            if (disposed)
                            {
                                return;
                            }
                        }

                        lock (this.processingLock)
                        {
                            this.readingParts.Enqueue(errorPacketSegment);
                        }

                        this.processResponsesEvent.Set();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    this.EnqueuePacket((int)packetSize.Value);
                }
            }
            else
            {
                this.processResponsesEvent.Reset();
            }
        }

        private PacketSize? GetPacketSize()
        {
            var readByteCount = this.physicalConnection.Read(this.packetSizeBuffer, 0, this.packetSizeBuffer.Length);
            
            if (this.disposed)
            {
                return null;
            }

            while (readByteCount < this.packetSizeBuffer.Length && readByteCount > 0)
            {
                var reading = this.physicalConnection.Read(packetSizeBuffer, readByteCount, this.packetSizeBuffer.Length - readByteCount);
                if (reading < 1 || this.disposed)
                {
                    return null;
                }

                readByteCount += reading;
            }

            return (PacketSize?)MessagePackSerializer.Deserialize(typeof(PacketSize), this.packetSizeBuffer);
        }

        private void EnqueuePacket(int packetSize)
        {
            if (this.buffer.Length - this.readingOffset < packetSize)
            {
                while (!this.IsReadingPartsQueueFree())
                {
                    if (this.disposed)
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
                    this.readingOffset += readingPacketSize;
                    lock (this.processingLock)
                    {
                        this.readingParts.Enqueue(packetSegmentArray);
                    }

                    this.processResponsesEvent.Set();
                    break;
                }
            }
        }

        private int ReadAllPacketBytes(int packetLength, out ArraySegment? result)
        {
            var freeBufferSpace = this.buffer.Length - this.readingOffset;
            result = null;
            int reading = 0;
            if (freeBufferSpace > packetLength)
            {
                while (reading < packetLength)
                {
                    var readBytesCount = this.physicalConnection.Read(this.buffer, this.readingOffset + reading, packetLength - reading);
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
                    result = new ArraySegment(this.buffer, this.readingOffset, reading);
                }
            }

            return reading;
        }

        private bool IsReadingPartsQueueFree()
        {
            lock (this.processingLock)
            {
                if (this.readingParts.Count < 1)
                {
                    this.readingOffset = 0;
                    return true;
                }
            }

            int waitResult = WaitHandle.WaitAny(this.readingPartsQueueFreeWaitHandles);
            if (waitResult > 0)
            {
                this.readingOffset = 0;
                return true;
            }

            return false;
        }
    }
}
