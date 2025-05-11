// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Headers
{
    /// <summary>
    /// <see cref="Tarantool"/> messages header base class.
    /// </summary>
    internal abstract class HeaderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderBase"/> class.
        /// </summary>
        /// <param name="code"><see cref="Tarantool"/> command code.</param>
        /// <param name="requestId">Request id.</param>
        protected HeaderBase(CommandCode code, RequestId requestId)
        {
            this.Code = code;
            RequestId = requestId;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> command code.
        /// </summary>
        internal CommandCode Code { get; }

        /// <summary>
        /// Gets or sets request id number.
        /// </summary>
        internal RequestId RequestId { get; set; }
    }
}
