// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using nanoFramework.MessagePack;
#endif
using nanoFramework.Tarantool.Client.Connections;
using nanoFramework.Tarantool.Client.Extensions;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Requests;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Client
{
    /// <summary>
    /// The class for module box <see cref="Tarantool"/>.
    /// </summary>
#nullable enable
    internal class Box : IBox
    {
        private readonly ClientOptions clientOptions;
        private readonly LogicalConnectionManager logicalConnection;
        private BoxInfo? info;
        private bool sqlReady;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Box"/> class.
        /// </summary>
        /// <param name="options">Client options.</param>
        internal Box(ClientOptions options)
        {
            this.clientOptions = options;
            this.logicalConnection = new LogicalConnectionManager(options);
            this.Metrics = new Metrics(this.logicalConnection);
            this.Schema = new Schema(this.logicalConnection);
        }

        public Metrics Metrics { get; }

        bool IBox.IsConnected => this.logicalConnection.IsConnected();

        public ISchema Schema { get; }

        public BoxInfo? Info
        {
            get => this.info;
            private set
            {
                this.info = value;
                this.sqlReady = value != null && value.IsSqlAvailable();
            }
        }

        public ITarantoolConnection TarantoolConnection => this.logicalConnection;

        public void ReloadBoxInfo()
        {
            var report = this.Eval("return box.info", typeof(BoxInfo));
            if (report != null)
            {
                if (report.Data.Length != 1)
                {
                    throw ExceptionHelper.CantParseBoxInfoResponse();
                }

                this.Info = (BoxInfo)report.Data[0];
            }
        }

        public void ReloadSchema()
        {
            this.Schema.Reload();
        }

        public DataResponse? Call(string functionName, Type? responseDataType)
        {
            return this.Call(functionName, TarantoolTuple.Empty, responseDataType);
        }

        public DataResponse? Call(string functionName, TarantoolTuple parameters, Type? responseDataType)
        {
            var callRequest = new CallRequest(functionName, parameters);
            return this.logicalConnection.SendRequest(callRequest, TimeSpan.Zero, responseDataType);
        }

        public DataResponse? Eval(string expression, TarantoolTuple parameters, Type? responseDataType)
        {
            var evalRequest = new EvalRequest(expression, parameters);
            return this.logicalConnection.SendRequest(evalRequest, TimeSpan.Zero, responseDataType);
        }

        public DataResponse? Eval(string expression, Type? responseDataType)
        {
            return this.Eval(expression, TarantoolTuple.Empty, responseDataType);
        }

        /// <summary>
        /// Execute <see cref="Tarantool"/> SQL.
        /// </summary>
        /// <param name="query">SQL query text.</param>
        /// <param name="responseDataType"><see cref="Tarantool"/> SQL query return data type.</param>
        /// <param name="parameters">SQL query parameters.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        /// <exception cref="NullReferenceException">If box info or <see cref="Tarantool"/> version is <see langword="null"/>.</exception>
        public DataResponse? ExecuteSql(string query, Type? responseDataType, params SqlParameter[] parameters)
        {
            if (this.Info != null && this.Info.Version is null)
            {
                if (!this.sqlReady)
                {
                    throw ExceptionHelper.SqlIsNotAvailable(this.Info.Version);
                }
            }

            return logicalConnection.SendRequest(new ExecuteSqlRequest(query, parameters), TimeSpan.Zero, responseDataType);
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Connect()
        {
            this.logicalConnection.Connect();

            if (this.clientOptions.ConnectionOptions.ReadSchemaOnConnect)
            {
                this.ReloadSchema();
            }

            if (this.clientOptions.ConnectionOptions.ReadBoxInfoOnConnect)
            {
                this.ReloadBoxInfo();
            }
        }

        internal static Box Connect(string replicationSource)
        {
            return Connect(new ClientOptions(replicationSource));
        }

        internal static Box Connect(string host, int port)
        {
            return Connect($"{host}:{port}");
        }

        internal static Box Connect(string host, int port, string user, string password)
        {
            return Connect($"{user}:{password}@{host}:{port}");
        }

        internal static Box Connect(ClientOptions clientOptions)
        {
            var box = new Box(clientOptions);
            box.Connect();
            return box;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.logicalConnection.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
