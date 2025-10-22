// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Headers
{
    /// <summary>
    /// The <see cref="Tarantool"/> response header class.
    /// </summary>
    internal struct ResponseHeader //: HeaderBase
    {
        internal static readonly ResponseHeader Empty = new ResponseHeader(CommandCode._, RequestId.Empty, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseHeader"/> struct.
        /// </summary>
        /// <param name="code"><see cref="Tarantool"/> command code.</param>
        /// <param name="requestId">Request id.</param>
        /// <param name="schemaId"><see cref="Tarantool"/> schema id.</param>
        internal ResponseHeader(CommandCode code, RequestId requestId, ulong schemaId)// : base(code, requestId)
        {
            SchemaId = schemaId;
            Code = code;
            RequestId = requestId;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> schema id.
        /// </summary>
        internal ulong SchemaId { get; }

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
