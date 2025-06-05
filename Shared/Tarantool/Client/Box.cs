// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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
        private readonly ClientOptions _clientOptions;
        private readonly LogicalConnectionManager _logicalConnection;
        private BoxInfo? _info;
        private bool _sqlReady;
        private bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Box"/> class.
        /// </summary>
        /// <param name="options">Client options.</param>
        internal Box(ClientOptions options)
        {
            _clientOptions = options;
            _logicalConnection = new LogicalConnectionManager(options);
            Metrics = new Metrics(_logicalConnection);
            Schema = new Schema(_logicalConnection);
        }

        public Metrics Metrics { get; }

        bool IBox.IsConnected => _logicalConnection.IsConnected();

        public ISchema Schema { get; }

        public BoxInfo? Info
        {
            get => _info;
            private set
            {
                _info = value;
                _sqlReady = value != null && value.IsSqlAvailable();
            }
        }

        public ITarantoolConnection TarantoolConnection => _logicalConnection;

        public void ReloadBoxInfo()
        {
            var report = Eval("return box.info", typeof(BoxInfo));
            if (report != null)
            {
                if (report.Data.Length != 1)
                {
                    throw ExceptionHelper.CantParseBoxInfoResponse();
                }

                Info = (BoxInfo)report.Data[0];
            }
        }

        public void ReloadSchema()
        {
            Schema.Reload();
        }

        public DataResponse? Call(string functionName, Type? responseDataType)
        {
            return Call(functionName, TarantoolTuple.Empty, responseDataType);
        }

        public DataResponse? Call(string functionName, TarantoolTuple parameters, Type? responseDataType)
        {
            var callRequest = new CallRequest(functionName, parameters);
            return _logicalConnection.SendRequest(callRequest, TimeSpan.Zero, responseDataType);
        }

        public DataResponse? Eval(string expression, TarantoolTuple parameters, Type? responseDataType)
        {
            var evalRequest = new EvalRequest(expression, parameters);
            return _logicalConnection.SendRequest(evalRequest, TimeSpan.Zero, responseDataType);
        }

        public DataResponse? Eval(string expression, Type? responseDataType)
        {
            return Eval(expression, TarantoolTuple.Empty, responseDataType);
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
            if (Info != null && Info.Version is null)
            {
                if (!_sqlReady)
                {
                    throw ExceptionHelper.SqlIsNotAvailable(Info.Version);
                }
            }

            return _logicalConnection.SendRequest(new ExecuteSqlRequest(query, parameters), TimeSpan.Zero, responseDataType);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Connect()
        {
            _logicalConnection.Connect();

            if (_clientOptions.ConnectionOptions.ReadSchemaOnConnect)
            {
                ReloadSchema();
            }

            if (_clientOptions.ConnectionOptions.ReadBoxInfoOnConnect)
            {
                ReloadBoxInfo();
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
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _logicalConnection.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
