// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Base class for insert or replace <see cref="Tarantool"/> request.
    /// </summary>
    public abstract class InsertReplaceRequest : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertReplaceRequest"/> class.
        /// </summary>
        /// <param name="code">Request <see cref="Tarantool"/> command code.</param>
        /// <param name="spaceId"><see cref="Tarantool"/> space id.</param>
        /// <param name="tuple"><see cref="Tarantool"/> <see cref="TarantoolTuple"/> to be insert or replace.</param>
        protected InsertReplaceRequest(CommandCode code, uint spaceId, TarantoolTuple tuple)
        {
            Code = code;
            SpaceId = spaceId;
            Tuple = tuple;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space id.
        /// </summary>
        public uint SpaceId { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> tuple to be insert or replace.
        /// </summary>
        public TarantoolTuple Tuple { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> command code.
        /// </summary>
        public CommandCode Code { get; }
    }
}
