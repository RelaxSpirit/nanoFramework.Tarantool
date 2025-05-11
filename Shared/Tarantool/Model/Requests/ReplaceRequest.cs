// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Replace <see cref="Tarantool"/> request.
    /// </summary>
    public class ReplaceRequest : InsertReplaceRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceRequest"/> class.
        /// </summary>
        /// <param name="spaceId"><see cref="Tarantool"/> space id.</param>
        /// <param name="tuple"><see cref="Tarantool"/> <see cref="TarantoolTuple"/> to be replace.</param>
        public ReplaceRequest(uint spaceId, TarantoolTuple tuple)
            : base(CommandCode.Replace, spaceId, tuple)
        {
        }
    }
}
