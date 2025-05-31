// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Insert <see cref="Tarantool"/> request.
    /// </summary>
    public class InsertRequest : InsertReplaceRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertRequest"/> class.
        /// </summary>
        /// <param name="spaceId"><see cref="Tarantool"/> space id.</param>
        /// <param name="tuple"><see cref="Tarantool"/> <see cref="TarantoolTuple"/> to be insert.</param>
        public InsertRequest(uint spaceId, TarantoolTuple tuple)
            : base(CommandCode.Insert, spaceId, tuple)
        {
        }

        /// <summary>
        /// Gets insert <see cref="Tarantool"/> auto increment key value.
        /// </summary>
        public static AutoIncrementKey IncrementKey { get; } = new AutoIncrementKey(null);

        /// <summary>
        /// <see cref="Tarantool"/> auto increment key class.
        /// </summary>
        public class AutoIncrementKey
        {
#nullable enable
            /// <summary>
            /// Initializes a new instance of the <see cref="AutoIncrementKey"/> class.
            /// </summary>
            /// <param name="value">Actual value or <see langword="null"/>.</param>
            internal AutoIncrementKey(object? value)
            {
                if (value is ulong uLongValue)
                {
                    ActualValue = uLongValue;
                }
            }

            /// <summary>
            /// Gets value returned <see cref="Tarantool"/> after increment.
            /// </summary>
            public ulong ActualValue { get; }

            /// <summary>
            /// Explicit operator to <see cref="ulong"/>.
            /// </summary>
            /// <param name="autoIncrementKey"><see cref="ulong"/> <see cref="Tarantool"/> actual auto increment key value.</param>
            public static explicit operator ulong(AutoIncrementKey autoIncrementKey)
            {
                return autoIncrementKey.ActualValue;
            }

            /// <summary>
            /// Implicit operator from <see cref="ulong"/>.
            /// </summary>
            /// <param name="value"><see langword="ulong"/> actual value.</param>
            public static implicit operator AutoIncrementKey(ulong value)
            {
                return new AutoIncrementKey(value);
            }
        }
    }
}
