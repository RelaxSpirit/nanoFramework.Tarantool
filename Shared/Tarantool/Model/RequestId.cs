// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// The <see cref="Tarantool"/> request id class.
    /// </summary>
    internal sealed class RequestId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestId"/> class.
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
            if (obj is RequestId requestId)
            {
                return Value == requestId.Value;
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }
}
