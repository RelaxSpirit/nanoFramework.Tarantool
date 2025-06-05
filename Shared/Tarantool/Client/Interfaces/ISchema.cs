// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    /// <summary>
    /// The interface for box.schema <see cref="Tarantool"/> submodule has data-definition functions for spaces,
    /// users, roles, function tuples, and sequences.
    /// </summary>
    public interface ISchema
    {
        /// <summary>
        /// Gets <see cref="ISpace"/> interface by <see cref="Tarantool"/> space name.
        /// </summary>
        /// <param name="name"><see cref="Tarantool"/> space name.</param>
        /// <returns>Interface <see cref="Tarantool"/> schema.space.</returns>
        ISpace this[string name] { get; }

        /// <summary>
        /// Gets <see cref="ISpace"/> interface by <see cref="Tarantool"/> space id.
        /// </summary>
        /// <param name="id"><see cref="Tarantool"/> space id.</param>
        /// <returns>Interface <see cref="Tarantool"/> schema.space.</returns>
        ISpace this[uint id] { get; }

        /// <summary>
        /// Reload <see cref="Tarantool"/> schema info.
        /// </summary>
        void Reload();

        /// <summary>
        /// Gets last reload datetime.
        /// </summary>
        DateTime LastReloadTime { get; }

        /// <summary>
        /// Gets <see cref="ISpace"/> interfaces collection <see cref="Tarantool"/> spaces.
        /// </summary>
        ICollection Spaces { get; }
    }
}
