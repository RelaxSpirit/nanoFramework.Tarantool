// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using nanoFramework.MessagePack.Dto;
using nanoFramework.Tarantool.Model.Requests;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    /// <summary>
    /// A interface the network <see cref="Tarantool"/> connection.
    /// </summary>
    public interface ITarantoolConnection
    {
        /// <summary>
        /// Get network connection status.
        /// </summary>
        /// <returns><see langword="true"/> if connection open, other <see langword="false"/></returns>
        bool IsConnected();

        /// <summary>
        /// Send request with empty response.
        /// </summary>
        /// <param name="request">Request to <see cref="Tarantool"/>.</param>
        /// <param name="timeout">Request timeout.</param>
        void SendRequestWithEmptyResponse(IRequest request, TimeSpan timeout);

        /// <summary>
        /// Send request to <see cref="Tarantool"/>.
        /// </summary>
        /// <param name="request">Request to <see cref="Tarantool"/>.</param>
        /// <param name="timeout">Request timeout.</param>
        /// <param name="responseDataType"><see cref="Tarantool"/> response data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
#nullable enable
        DataResponse? SendRequest(IRequest request, TimeSpan timeout, Type? responseDataType);

        /// <summary>
        /// Send request to <see cref="Tarantool"/>.
        /// </summary>
        /// <param name="request">Request to <see cref="Tarantool"/>.</param>
        /// <param name="timeout">Request timeout.</param>
        /// <returns><see cref="ArraySegment"/> bytes to responses.</returns>
        ArraySegment? SendRawRequest(IRequest request, TimeSpan timeout);
#nullable disable

        /// <summary>
        /// Gets count failed pings request by timeout.
        /// </summary>
        uint PingsFailedByTimeoutCount { get; }
    }
}
