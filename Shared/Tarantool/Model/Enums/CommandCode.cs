// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Enums
{
    /// <summary>
    /// Enum <see cref="Tarantool"/> request command code.
    /// </summary>
    public enum CommandCode : uint
    {
        /// <summary>
        /// Select request.
        /// </summary>
        Select = 0x01,

        /// <summary>
        /// Insert request.
        /// </summary>
        Insert = 0x02,

        /// <summary>
        /// Replace request.
        /// </summary>
        Replace = 0x03,

        /// <summary>
        /// Update request.
        /// </summary>
        Update = 0x04,

        /// <summary>
        /// Delete request.
        /// </summary>
        Delete = 0x05,

        /// <summary>
        /// Request old <see cref="Tarantool"/> version (before 1.7).
        /// </summary>
        OldCall = 0x06,

        /// <summary>
        /// User authorization request.
        /// </summary>
        Auth = 0x07,

        /// <summary>
        /// Eval request.
        /// </summary>
        Eval = 0x08,

        /// <summary>
        /// Upsert request.
        /// </summary>
        Upsert = 0x09,

        /// <summary>
        /// Call function request.
        /// </summary>
        Call = 0x0A,

        /// <summary>
        /// Execute request.
        /// </summary>
        Execute = 0x0B,

        /// <summary>
        /// Ping request.
        /// </summary>
        Ping = 0x40,

        /// <summary>
        /// No error response.
        /// </summary>
        Ok = 0x00,

        /// <summary>
        /// Error response. Mask code.
        /// </summary>
        ErrorMask = 0x8000,

        /// <summary>
        /// Join replication code.
        /// </summary>
        Join = 0x41,

        /// <summary>
        /// Subscribe replication code.
        /// </summary>
        Subscribe = 0x42,

        /// <summary>
        /// No command code.
        /// </summary>
        _ = uint.MaxValue
    }
}
