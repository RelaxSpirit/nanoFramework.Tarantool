// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// The <see cref="Tarantool"/> packet size struct.
    /// </summary>
    internal struct PacketSize
    {
        internal static readonly PacketSize Empty = new PacketSize();

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketSize"/> struct.
        /// </summary>
        /// <param name="value">Size value.</param>
        internal PacketSize(uint value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets packet size value.
        /// </summary>
        internal uint Value { get; }

        public static bool operator ==(PacketSize left, PacketSize right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PacketSize left, PacketSize right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
#nullable enable
        public override bool Equals(object? obj)
        {
            return obj is PacketSize packetSize && packetSize.Value == Value;
        }
    }
}
