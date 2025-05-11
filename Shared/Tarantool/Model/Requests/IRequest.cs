// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Base <see cref="Tarantool"/> request interface.
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Gets <see cref="Tarantool"/> request code.
        /// </summary>
        CommandCode Code { get; }
    }
}
