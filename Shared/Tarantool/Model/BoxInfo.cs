// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// The box.info submodule provides access to information about a running <see cref="Tarantool"/> instance.
    /// </summary>
    public partial class BoxInfo
    {
        /// <summary>
        /// Gets or sets a numeric identifier of the current instance within the replica set.
        /// </summary>
        public long Id { get; protected set; }

        /// <summary>
        /// Gets or sets a log sequence number (LSN) for the latest entry in the instance’s write-ahead log (WAL).
        /// </summary>
        public long Lsn { get; protected set; }

        /// <summary>
        /// Gets or sets a process ID of the current instance.
        /// </summary>
        public long Pid { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current mode of the instance (writable or read-only).
        /// </summary>
        public bool ReadOnly { get; protected set; }

        /// <summary>
        /// Gets or sets a globally unique identifier of the current instance.
        /// </summary>
        public Guid Uuid { get; protected set; }

        /// <summary>
        /// Gets or sets the <see cref="Tarantool"/> version.
        /// </summary>
#nullable enable
        public TarantoolVersion Version { get; protected set; }
    }
}
