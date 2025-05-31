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
        private readonly ClientOptions _clientOptions;
        private readonly IPhysicalConnection _physicalConnection;
        private readonly Queue _requestQueue = new Queue();
        private readonly object _writeLock = new object();
        private readonly Thread _thread;
        private readonly ManualResetEvent _exitEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent _newRequestsAvailable = new ManualResetEvent(false);
        private readonly ConnectionOptions _connectionOptions;
        private bool _disposed = false;
        private long _remaining = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestWriter"/> class.
        /// </summary>
        /// <param name="clientOptions">Client options.</param>
        /// <param name="physicalConnection">Physical connection <see cref="IPhysicalConnection"/> interface.</param>
        internal RequestWriter(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            _clientOptions = clientOptions;
            _physicalConnection = physicalConnection;
            _thread = new Thread(WriteFunction);
            _connectionOptions = _clientOptions.ConnectionOptions;
        }

        void IRequestWriter.BeginWriting()
        {
            if (_disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(RequestWriter));
#endif
            }

            _thread.Start();
        }

        bool IRequestWriter.IsConnected => !_disposed;

        void IRequestWriter.Write(byte[] request)
        {
            if (_disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(ResponseReader));
#endif
            }

            bool shouldSignal = false;
            lock (_writeLock)
            {
                _requestQueue.Enqueue(request);
                shouldSignal = _requestQueue.Count == 1;
            }

            if (shouldSignal)
            {
                _newRequestsAvailable.Set();
            }
        }

        private void WriteFunction()
        {
            var handles = new[] { _exitEvent, _newRequestsAvailable };
            var throttle = _connectionOptions.WriteThrottlePeriodInMs;

            while (true)
            {
                switch (WaitHandle.WaitAny(handles))
                {
                    case 0:
                        return;
                    case 1:
                        WriteRequests(_connectionOptions.WriteStreamBufferSize, _connectionOptions.MaxRequestsInBatch);

                        if (throttle > 0 && _remaining < _connectionOptions.MinRequestsWithThrottle)
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
            if (_requestQueue.Count > 0)
            {
                lock (_writeLock)
                {
                    if (_requestQueue.Count > 0)
                    {
                        _remaining = _requestQueue.Count + 1;
                        result = _requestQueue.Dequeue();
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

            while (GetRequest(out object? requestObject))
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

                _physicalConnection.Write(result, 0, result.Length);
            }

            lock (_writeLock)
            {
                if (_requestQueue.Count == 0)
                {
                    _newRequestsAvailable.Reset();
                }
            }

            _physicalConnection.Flush();
        }
        
        public void Dispose()
        {
            if (!_exitEvent.Set() || _disposed)
            {
                return;
            }

            _disposed = true;
        }
    }
}
