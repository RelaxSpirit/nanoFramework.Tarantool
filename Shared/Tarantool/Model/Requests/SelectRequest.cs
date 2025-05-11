// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Select <see cref="Tarantool"/> request.
    /// </summary>
    public class SelectRequest : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectRequest"/> class.
        /// </summary>
        /// <param name="spaceId"><see cref="Tarantool"/> space id.</param>
        /// <param name="indexId"><see cref="Tarantool"/> index id.</param>
        /// <param name="limit">Maximum number of <see cref="Tarantool"/> tuples in the space.</param>
        /// <param name="offset">Number of <see cref="Tarantool"/> tuples to skip in the select.</param>
        /// <param name="iterator"><see cref="Tarantool"/> <see cref="Iterator"/> type.</param>
        /// <param name="selectKey"><see cref="Tarantool"/> tuple key for select.</param>
#nullable enable
        public SelectRequest(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, TarantoolTuple? selectKey)
        {
            this.SpaceId = spaceId;
            this.IndexId = indexId;
            this.Limit = limit;
            this.Offset = offset;
            this.Iterator = iterator;
            this.SelectKey = selectKey;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space id.
        /// </summary>
        public uint SpaceId { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> index id.
        /// </summary>
        public uint IndexId { get; }

        /// <summary>
        /// Gets maximum number of <see cref="Tarantool"/> tuples in the space.
        /// </summary>
        public uint Limit { get; }

        /// <summary>
        /// Gets number of <see cref="Tarantool"/> tuples to skip in the select.
        /// </summary>
        public uint Offset { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> <see cref="Iterator"/> type.
        /// </summary>
        public Iterator Iterator { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> tuple key for select.
        /// </summary>
        public TarantoolTuple? SelectKey { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> command code.
        /// </summary>
        public CommandCode Code => CommandCode.Select;
    }
}
