// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Enums
{
    /// <summary>
    /// Enum <see cref="Tarantool"/> index type.
    /// </summary>
    public enum IndexType
    {
        /// <summary>
        /// No index type.
        /// </summary>
        _ = -1,

        /// <summary>
        /// Tree index type.
        /// </summary>
        Tree,

        /// <summary>
        /// Hash index type.
        /// </summary>
        Hash,

        /// <summary>
        /// Bitset index type.
        /// </summary>
        Bitset,

        /// <summary>
        /// RTree index type.
        /// </summary>
        RTree
    }
}
