// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.UpdateOperations;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Upsert <see cref="Tarantool"/> request.
    /// </summary>
    public class UpsertRequest : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsertRequest"/> class.
        /// </summary>
        /// <param name="spaceId"><see cref="Tarantool"/> space id.</param>
        /// <param name="tuple"><see cref="Tarantool"/> tuple to upsert.</param>
        /// <param name="updateOperations"><see cref="Tarantool"/> <see cref="UpdateOperation"/> array.</param>
        public UpsertRequest(uint spaceId, TarantoolTuple tuple, UpdateOperation[] updateOperations)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperations = updateOperations;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> <see cref="UpdateOperation"/>s.
        /// </summary>
        public UpdateOperation[] UpdateOperations { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space id.
        /// </summary>
        public uint SpaceId { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> tuple to upsert.
        /// </summary>
        public TarantoolTuple Tuple { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> command code.
        /// </summary>
        public CommandCode Code => CommandCode.Upsert;
    }
}
