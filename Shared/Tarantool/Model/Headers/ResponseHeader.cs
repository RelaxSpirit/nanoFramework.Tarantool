// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Headers
{
    /// <summary>
    /// The <see cref="Tarantool"/> response header class.
    /// </summary>
    internal class ResponseHeader : HeaderBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHeader"/> class.
        /// </summary>
        /// <param name="code"><see cref="Tarantool"/> command code.</param>
        /// <param name="requestId">Request id.</param>
        /// <param name="schemaId"><see cref="Tarantool"/> schema id.</param>
        internal ResponseHeader(CommandCode code, RequestId requestId, ulong schemaId) : base(code, requestId)
        {
            this.SchemaId = schemaId;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> schema id.
        /// </summary>
        internal ulong SchemaId { get; }
    }
}
