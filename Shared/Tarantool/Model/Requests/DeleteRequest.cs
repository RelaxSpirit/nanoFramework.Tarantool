// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Delete <see cref="Tarantool"/> request.
    /// </summary>
    public class DeleteRequest : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRequest"/> class.
        /// </summary>
        /// <param name="spaceId"><see cref="Tarantool"/> space id.</param>
        /// <param name="indexId"><see cref="Tarantool"/> index id.</param>
        /// <param name="key"><see cref="TarantoolTuple"/> key for delete.</param>
        public DeleteRequest(uint spaceId, uint indexId, TarantoolTuple key)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
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
        /// Gets <see cref="TarantoolTuple"/> key to delete.
        /// </summary>
        public TarantoolTuple Key { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> command code.
        /// </summary>
        public CommandCode Code => CommandCode.Delete;
    }
}
