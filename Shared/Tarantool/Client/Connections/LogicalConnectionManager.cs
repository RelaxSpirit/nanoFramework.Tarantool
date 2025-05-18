// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using System.Threading;
#endif
using nanoFramework.MessagePack.Dto;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Requests;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Client.Connections
{
    /// <summary>
    /// The logical connection manager class.
    /// </summary>
#nullable enable
    internal class LogicalConnectionManager : ILogicalConnection
    {
        private const int ConnectionTimeout = 1000;
        private static readonly PingRequest PingRequest = new PingRequest();

        private readonly ClientOptions _clientOptions;
        private readonly RequestIdCounter _requestIdCounter = new RequestIdCounter();
        private readonly ManualResetEvent _connected = new ManualResetEvent(true);
        private readonly AutoResetEvent _reconnectAvailable = new AutoResetEvent(true);
        private readonly int _pingCheckInterval = 1000;
        private readonly TimeSpan _pingTimeout;
        
        private LogicalConnection? _droppableLogicalConnection;
        private Timer? _timer;
        private int _disposing;
        private DateTime _nextPingTime = DateTime.MinValue;
        private bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalConnectionManager"/> class.
        /// </summary>
        /// <param name="options">Client options.</param>
        internal LogicalConnectionManager(ClientOptions options)
        {
            _clientOptions = options;

            if (_clientOptions.ConnectionOptions.PingCheckInterval >= 0)
            {
                _pingCheckInterval = _clientOptions.ConnectionOptions.PingCheckInterval;
            }

            _pingTimeout = _clientOptions.ConnectionOptions.PingCheckTimeout;
        }

        public uint PingsFailedByTimeoutCount => _droppableLogicalConnection == null ? 0 : _droppableLogicalConnection.PingsFailedByTimeoutCount;

        public bool IsConnected() => _connected.WaitOne(ConnectionTimeout, false) && IsConnectedInternal();

        public void Connect()
        {
            if (IsConnectedInternal())
            {
                return;
            }

            if (!_reconnectAvailable.WaitOne(ConnectionTimeout, false))
            {
                throw ExceptionHelper.NotConnected();
            }

            try
            {
                if (IsConnectedInternal())
                {
                    return;
                }

                _connected.Reset();

                //// Debug.WriteLine($"{nameof(LogicalConnectionManager)}: Connecting...");

                _timer?.Dispose();
                _droppableLogicalConnection?.Dispose();

                var newConnection = new LogicalConnection(_clientOptions, _requestIdCounter);              
                _droppableLogicalConnection = newConnection;
                _droppableLogicalConnection.Connect();

                _connected.Set();

                //// Debug.WriteLine($"{nameof(LogicalConnectionManager)}: Connected.");

                if (_pingCheckInterval > 0 && _timer == null)
                {
                    _timer = new Timer(x => CheckPing(), null, _pingCheckInterval, Timeout.Infinite);
                }
            }
            finally
            {
               _reconnectAvailable.Set();
            }
        }

        public DataResponse? SendRequest(IRequest request, TimeSpan timeout, Type? responseDataType)
        {
            Connect();

            ChangePingTimer();

            var result = _droppableLogicalConnection?.SendRequest(request, timeout, responseDataType);

            ScheduleNextPing();

            return result;
        }

        public ArraySegment? SendRawRequest(IRequest request, TimeSpan timeout)
        {
            Connect();

            ChangePingTimer();

            var result = _droppableLogicalConnection?.SendRawRequest(request, timeout);

            ScheduleNextPing();

            return result;
        }

        public void SendRequestWithEmptyResponse(IRequest request, TimeSpan timeout)
        {
            Connect();

            _droppableLogicalConnection?.SendRequestWithEmptyResponse(request, timeout);

            ScheduleNextPing();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
                if (disposing)
                {
                    if (Interlocked.Exchange(ref _disposing, 1) > 0)
                    {
                        return;
                    }

                    _droppableLogicalConnection?.Dispose();
                    _droppableLogicalConnection = null;
                    _timer?.Dispose();
                    _timer = null;
                }
            }
        }

        private void CheckPing()
        {
            try
            {
                if (_nextPingTime > DateTime.UtcNow || _disposedValue)
                {
                    return;
                } 

                SendRequestWithEmptyResponse(PingRequest, _pingTimeout);
            }
            catch (Exception)
            {
                //// Debug.WriteLine($"{nameof(LogicalConnectionManager)}: Ping failed with exception: {e.Message}. Dropping current connection.");
               
                _droppableLogicalConnection?.Dispose();
            }
            finally
            {
                if (!_disposedValue)
                {
                    ChangePingTimer();
                }
            }
        }

        private bool IsConnectedInternal()
        {
            if (_droppableLogicalConnection != null)
            {
                return _droppableLogicalConnection.IsConnected();
            }
            else
            {
                return false;
            }
        }

        private void ScheduleNextPing()
        {
            if (_pingCheckInterval > 0)
            {
                _nextPingTime = DateTime.UtcNow.AddMilliseconds(_pingCheckInterval);
            }
        }

        private void ChangePingTimer()
        {
            if (_timer != null)
            {
                _timer.Change(_pingCheckInterval, Timeout.Infinite);
            }
        }
    }
}
