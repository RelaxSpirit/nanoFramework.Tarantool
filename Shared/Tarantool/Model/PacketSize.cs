// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// The <see cref="Tarantool"/> packet size class.
    /// </summary>
    internal class PacketSize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketSize"/> class.
        /// </summary>
        /// <param name="value">Size value.</param>
        internal PacketSize(uint value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets packet size value.
        /// </summary>
        internal uint Value { get; }
    }
}
