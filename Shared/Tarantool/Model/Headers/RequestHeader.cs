// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Headers
{
    /// <summary>
    /// The <see cref="Tarantool"/> request header struct.
    /// </summary>
    internal struct RequestHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHeader"/> struct.
        /// </summary>
        /// <param name="code"><see cref="Tarantool"/> command code.</param>
        /// <param name="requestId">Request id.</param>
        internal RequestHeader(CommandCode code, RequestId requestId)
        {
            Code = code;
            RequestId = requestId;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> command code.
        /// </summary>
        internal CommandCode Code { get; }

        /// <summary>
        /// Gets request id number.
        /// </summary>
        internal RequestId RequestId { get; }
    }
}
