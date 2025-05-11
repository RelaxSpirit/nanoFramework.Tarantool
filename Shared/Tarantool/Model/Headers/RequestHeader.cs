// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Headers
{
    /// <summary>
    /// The <see cref="Tarantool"/> request header class.
    /// </summary>
    internal class RequestHeader : HeaderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHeader"/> class.
        /// </summary>
        /// <param name="code"><see cref="Tarantool"/> command code.</param>
        /// <param name="requestId">Request id.</param>
        internal RequestHeader(CommandCode code, RequestId requestId)
            : base(code, requestId)
        {
        }
    }
}
