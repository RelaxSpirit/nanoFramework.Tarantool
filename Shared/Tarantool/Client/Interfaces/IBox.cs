// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    /// <summary>
    /// The interface for module box <see cref="Tarantool"/>.
    /// </summary>
#nullable enable
    public interface IBox : IDisposable
    {
        /// <summary>
        /// Gets <see cref="Tarantool"/> network connection interface.
        /// </summary>
        ITarantoolConnection TarantoolConnection { get; }

        /// <summary>
        /// Gets a value indicating whether the status of the network connection.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Gets a network connection statistics.
        /// </summary>
        Metrics Metrics { get; }

        /// <summary>
        /// Gets the box.schema submodule interface.
        /// </summary>
        ISchema Schema { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> box.info.
        /// </summary>
        BoxInfo? Info { get; }

        /// <summary>
        /// Reload <see cref="Tarantool"/> schema info.
        /// </summary>
        void ReloadSchema();

        /// <summary>
        /// Reload <see cref="Tarantool"/> box info.
        /// </summary>
        void ReloadBoxInfo();

        /// <summary>
        /// Call <see cref="Tarantool"/> function.
        /// </summary>
        /// <param name="functionName">Function name.</param>
        /// <param name="responseDataType"><see cref="Tarantool"/> function return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Call(string functionName, Type? responseDataType = null);

        /// <summary>
        /// Call <see cref="Tarantool"/> function.
        /// </summary>
        /// <param name="functionName">Function name.</param>
        /// <param name="parameters"><see cref="Tarantool"/> function parameters.</param>
        /// <param name="responseDataType"><see cref="Tarantool"/> function return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Call(string functionName, TarantoolTuple parameters, Type? responseDataType = null);

        /// <summary>
        /// Eval <see cref="Tarantool"/> expression.
        /// </summary>
        /// <param name="expression"><see cref="Tarantool"/> expression.</param>
        /// <param name="parameters"><see cref="Tarantool"/> expression parameters.</param>
        /// <param name="responseDataType"><see cref="Tarantool"/> eval return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Eval(string expression, TarantoolTuple parameters, Type? responseDataType = null);

        /// <summary>
        /// Eval <see cref="Tarantool"/> expression.
        /// </summary>
        /// <param name="expression"><see cref="Tarantool"/> expression.</param>
        /// <param name="responseDataType"><see cref="Tarantool"/> eval return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Eval(string expression, Type? responseDataType = null);

        /// <summary>
        /// Execute <see cref="Tarantool"/> SQL.
        /// </summary>
        /// <param name="query">SQL query text.</param>
        /// <param name="responseDataType"><see cref="Tarantool"/> SQL query return data type.</param>
        /// <param name="parameters">SQL query parameters.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? ExecuteSql(string query, Type? responseDataType = null, params SqlParameter[] parameters);
    }
}
