// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.UpdateOperations;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Update <see cref="Tarantool"/> request.
    /// </summary>
    public class UpdateRequest : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateRequest"/> class.
        /// </summary>
        /// <param name="spaceId"><see cref="Tarantool"/> space id.</param>
        /// <param name="indexId"><see cref="Tarantool"/> index id.</param>
        /// <param name="key"><see cref="TarantoolTuple"/> key for update.</param>
        /// <param name="updateOperations"><see cref="Tarantool"/> <see cref="UpdateOperation"/> array.</param>
        public UpdateRequest(uint spaceId, uint indexId, TarantoolTuple key, UpdateOperation[] updateOperations)
        {
            this.SpaceId = spaceId;
            this.IndexId = indexId;
            this.Key = key;
            this.UpdateOperations = updateOperations;
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
        /// Gets <see cref="TarantoolTuple"/> key for update.
        /// </summary>
        public TarantoolTuple Key { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> <see cref="UpdateOperation"/>s.
        /// </summary>
        public UpdateOperation[] UpdateOperations { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> command code.
        /// </summary>
        public CommandCode Code => CommandCode.Update;
    }
}
