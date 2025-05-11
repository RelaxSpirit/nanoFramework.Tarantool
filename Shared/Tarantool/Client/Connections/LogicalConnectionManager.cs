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

        private readonly ClientOptions clientOptions;
        private readonly RequestIdCounter requestIdCounter = new RequestIdCounter();
        private readonly ManualResetEvent connected = new ManualResetEvent(true);
        private readonly AutoResetEvent reconnectAvailable = new AutoResetEvent(true);
        private readonly int pingCheckInterval = 1000;
        private readonly TimeSpan pingTimeout;
        
        private LogicalConnection? droppableLogicalConnection;
        private Timer? timer;
        private int disposing;
        private DateTime nextPingTime = DateTime.MinValue;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicalConnectionManager"/> class.
        /// </summary>
        /// <param name="options">Client options.</param>
        internal LogicalConnectionManager(ClientOptions options)
        {
            this.clientOptions = options;

            if (this.clientOptions.ConnectionOptions.PingCheckInterval >= 0)
            {
                this.pingCheckInterval = this.clientOptions.ConnectionOptions.PingCheckInterval;
            }

            this.pingTimeout = this.clientOptions.ConnectionOptions.PingCheckTimeout;
        }

        public uint PingsFailedByTimeoutCount => this.droppableLogicalConnection == null ? 0 : this.droppableLogicalConnection.PingsFailedByTimeoutCount;

        public bool IsConnected() => this.connected.WaitOne(ConnectionTimeout, false) && this.IsConnectedInternal();

        public void Connect()
        {
            if (this.IsConnectedInternal())
            {
                return;
            }

            if (!this.reconnectAvailable.WaitOne(ConnectionTimeout, false))
            {
                throw ExceptionHelper.NotConnected();
            }

            try
            {
                if (this.IsConnectedInternal())
                {
                    return;
                }

                this.connected.Reset();

                //// Debug.WriteLine($"{nameof(LogicalConnectionManager)}: Connecting...");

                this.timer?.Dispose();
                this.droppableLogicalConnection?.Dispose();

                var newConnection = new LogicalConnection(this.clientOptions, this.requestIdCounter);              
                this.droppableLogicalConnection = newConnection;
                this.droppableLogicalConnection.Connect();

                this.connected.Set();

                //// Debug.WriteLine($"{nameof(LogicalConnectionManager)}: Connected.");

                if (this.pingCheckInterval > 0 && this.timer == null)
                {
                    this.timer = new Timer(x => this.CheckPing(), null, this.pingCheckInterval, Timeout.Infinite);
                }
            }
            finally
            {
               this.reconnectAvailable.Set();
            }
        }

        public DataResponse? SendRequest(IRequest request, TimeSpan timeout, Type? responseDataType)
        {
            this.Connect();

            this.ChangePingTimer();

            var result = this.droppableLogicalConnection?.SendRequest(request, timeout, responseDataType);

            this.ScheduleNextPing();

            return result;
        }

        public ArraySegment? SendRawRequest(IRequest request, TimeSpan timeout)
        {
            this.Connect();

            this.ChangePingTimer();

            var result = this.droppableLogicalConnection?.SendRawRequest(request, timeout);

            this.ScheduleNextPing();

            return result;
        }

        public void SendRequestWithEmptyResponse(IRequest request, TimeSpan timeout)
        {
            this.Connect();

            this.droppableLogicalConnection?.SendRequestWithEmptyResponse(request, timeout);

            this.ScheduleNextPing();
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                this.disposedValue = true;
                if (disposing)
                {
                    if (Interlocked.Exchange(ref this.disposing, 1) > 0)
                    {
                        return;
                    }

                    this.droppableLogicalConnection?.Dispose();
                    this.droppableLogicalConnection = null;
                    this.timer?.Dispose();
                    this.timer = null;
                }
            }
        }

        private void CheckPing()
        {
            try
            {
                if (this.nextPingTime > DateTime.UtcNow || this.disposedValue)
                {
                    return;
                } 

                this.SendRequestWithEmptyResponse(PingRequest, this.pingTimeout);
            }
            catch (Exception)
            {
                //// Debug.WriteLine($"{nameof(LogicalConnectionManager)}: Ping failed with exception: {e.Message}. Dropping current connection.");
               
                this.droppableLogicalConnection?.Dispose();
            }
            finally
            {
                if (!this.disposedValue)
                {
                    this.ChangePingTimer();
                }
            }
        }

        private bool IsConnectedInternal()
        {
            if (this.droppableLogicalConnection != null)
            {
                return this.droppableLogicalConnection.IsConnected();
            }
            else
            {
                return false;
            }
        }

        private void ScheduleNextPing()
        {
            if (this.pingCheckInterval > 0)
            {
                this.nextPingTime = DateTime.UtcNow.AddMilliseconds(this.pingCheckInterval);
            }
        }

        private void ChangePingTimer()
        {
            if (this.timer != null)
            {
                this.timer.Change(this.pingCheckInterval, Timeout.Infinite);
            }
        }
    }
}
