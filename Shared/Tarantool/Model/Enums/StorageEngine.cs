// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Enums
{
    /// <summary>
    /// Enum a storage engine is a set of low-level routines which actually store and retrieve tuple values.
    /// </summary>
    public enum StorageEngine
    {
        /// <summary>
        /// No storage engine.
        /// </summary>
        _ = -1,

        /// <summary>
        /// The memtx storage engine is used in <see cref="Tarantool"/> by default.
        /// The engine keeps all data in random-access memory (RAM), and therefore has a low read latency.
        /// </summary>
        Memtx,

        /// <summary>
        /// Storing data with vinyl.
        /// </summary>
        Vinyl,

        /// <summary>
        /// Old <see cref="Tarantool"/> version.
        /// The disk-based storage engine, which was called <see cref="Sophia"/> or phia in earlier versions,
        /// is superseded by the <see cref="Vinyl"/> storage engine.
        /// </summary>
        Sophia,

        /// <summary>
        /// A system space view, also called a ‘<see cref="Sysview"/>’, is a restricted read-only copy of a system space.
        /// </summary>
        Sysview,

        /// <summary>
        /// A system space view, also called a ‘<see cref="Blackhole"/>’, is a restricted read-only copy of a system space.
        /// </summary>
        Blackhole,

        /// <summary>
        /// A system space view, also called a ‘<see cref="Service"/>’, is a restricted read-only copy of a system space.
        /// </summary>
        Service
    }
}
