// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using System.Collections;
using System.Threading;
#else
using System.Collections;
#endif
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Client.Stream
{
    /// <summary>
    /// The request writer class.
    /// </summary>
    internal class RequestWriter : IRequestWriter
    {
        private readonly ClientOptions clientOptions;
        private readonly IPhysicalConnection physicalConnection;
        private readonly Queue requestQueue = new Queue();
        private readonly object writeLock = new object();
        private readonly Thread thread;
        private readonly ManualResetEvent exitEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent newRequestsAvailable = new ManualResetEvent(false);
        private readonly ConnectionOptions connectionOptions;
        private bool disposed = false;
        private long remaining = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestWriter"/> class.
        /// </summary>
        /// <param name="clientOptions">Client options.</param>
        /// <param name="physicalConnection">Physical connection <see cref="IPhysicalConnection"/> interface.</param>
        internal RequestWriter(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            this.clientOptions = clientOptions;
            this.physicalConnection = physicalConnection;
            this.thread = new Thread(this.WriteFunction);
            this.connectionOptions = this.clientOptions.ConnectionOptions;
        }

        void IRequestWriter.BeginWriting()
        {
            if (this.disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(RequestWriter));
#endif
            }

            this.thread.Start();
        }

        bool IRequestWriter.IsConnected => !this.disposed;

        void IRequestWriter.Write(byte[] request)
        {
            if (this.disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(ResponseReader));
#endif
            }

            bool shouldSignal = false;
            lock (this.writeLock)
            {
                this.requestQueue.Enqueue(request);
                shouldSignal = this.requestQueue.Count == 1;
            }

            if (shouldSignal)
            {
                this.newRequestsAvailable.Set();
            }
        }

        private void WriteFunction()
        {
            var handles = new[] { this.exitEvent, this.newRequestsAvailable };
            var throttle = this.connectionOptions.WriteThrottlePeriodInMs;

            while (true)
            {
                switch (WaitHandle.WaitAny(handles))
                {
                    case 0:
                        return;
                    case 1:
                        this.WriteRequests(this.connectionOptions.WriteStreamBufferSize, this.connectionOptions.MaxRequestsInBatch);

                        if (throttle > 0 && this.remaining < this.connectionOptions.MinRequestsWithThrottle)
                        {
                            Thread.Sleep(throttle);
                        }

                        break;
                }
            }
        }

#nullable enable
        private bool GetRequest(out object? result)
        {
            if (this.requestQueue.Count > 0)
            {
                lock (this.writeLock)
                {
                    if (this.requestQueue.Count > 0)
                    {
                        this.remaining = this.requestQueue.Count + 1;
                        result = this.requestQueue.Dequeue();
                        return result != null;
                    }
                }
            }

            result = null;
            return false;
        }

        private void WriteRequests(int bufferLength, int limit)
        {
            var count = 0;
            ulong length = 0;
            ArrayList list = new ArrayList();

            while (this.GetRequest(out object? requestObject))
            {
                if (requestObject != null)
                {
                    var request = (byte[])requestObject;

                    length += (uint)(request != null ? request.Length : 0);

                    list.Add(request);

                    count++;

                    if ((limit > 0 && count > limit) || length > (ulong)bufferLength)
                    {
                        break;
                    }
                }
            }

            if (list.Count > 0)
            {
                // merge requests into one buffer
                var result = new byte[length];
                int position = 0;
                foreach (byte[] r in list)
                {
                    Array.Copy(r, 0, result, position, r.Length);
                    position += r.Length;
                }

                this.physicalConnection.Write(result, 0, result.Length);
            }

            lock (this.writeLock)
            {
                if (this.requestQueue.Count == 0)
                {
                    this.newRequestsAvailable.Reset();
                }
            }

            this.physicalConnection.Flush();
        }
        
        public void Dispose()
        {
            if (!this.exitEvent.Set() || this.disposed)
            {
                return;
            }

            this.disposed = true;

            this.thread.Join(100);
        }
    }
}
