// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Client.Interfaces;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// Network connection statistics.
    /// </summary>
    public struct Metrics
    {
        private readonly ILogicalConnection _logicalConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Metrics"/> struct.
        /// </summary>
        /// <param name="logicalConnection">Network logical connection interface <see cref="ILogicalConnection"/>.</param>
        internal Metrics(ILogicalConnection logicalConnection)
        {
            _logicalConnection = logicalConnection;
        }

        /// <summary>
        /// Gets a pings failed by timeout count.
        /// </summary>
        public uint PingsFailedByTimeoutCount => _logicalConnection.PingsFailedByTimeoutCount;
    }
}
