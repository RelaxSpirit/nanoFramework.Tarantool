// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// The <see cref="Tarantool"/> request id struct.
    /// </summary>
    internal struct RequestId
    {
        internal static readonly RequestId Empty = new RequestId(ulong.MaxValue);

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestId"/> struct.
        /// </summary>
        /// <param name="value">Request id value.</param>
        internal RequestId(ulong value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets request id value.
        /// </summary>
        internal ulong Value { get; }

        public static implicit operator ulong(RequestId id)
        {
            return id.Value;
        }

        public static explicit operator RequestId(ulong id)
        {
            return new RequestId(id);
        }

        public static bool operator ==(RequestId left, RequestId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RequestId left, RequestId right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

#nullable enable
        public override bool Equals(object? obj)
        {
            return obj is RequestId requestId && Value == requestId.Value;
        }
    }
}
