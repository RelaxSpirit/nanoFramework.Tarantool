// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// The box.info submodule provides access to information about a running <see cref="Tarantool"/> instance.
    /// </summary>
    public partial class BoxInfo
    {
        /// <summary>
        /// Gets a numeric identifier of the current instance within the replica set.
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Gets a log sequence number (LSN) for the latest entry in the instance’s write-ahead log (WAL).
        /// </summary>
        public long Lsn { get; private set; }

        /// <summary>
        /// Gets a process ID of the current instance.
        /// </summary>
        public long Pid { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current mode of the instance (writable or read-only).
        /// </summary>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// Gets a globally unique identifier of the current instance.
        /// </summary>
        public Guid Uuid { get; private set; }

        /// <summary>
        /// Gets the <see cref="Tarantool"/> version.
        /// </summary>
#nullable enable
        public TarantoolVersion? Version { get; private set; }
    }
}
