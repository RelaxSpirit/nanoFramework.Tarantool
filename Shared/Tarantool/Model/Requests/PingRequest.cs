// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// The <see cref="Tarantool"/> ping request class.
    /// </summary>
    internal class PingRequest : IRequest
    {
        /// <summary>
        /// Gets <see cref="CommandCode.Ping"/> command code.
        /// </summary>
        CommandCode IRequest.Code => CommandCode.Ping;
    }
}
