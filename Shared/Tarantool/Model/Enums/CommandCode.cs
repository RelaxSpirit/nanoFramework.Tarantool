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
        Select = 0x01u,

        /// <summary>
        /// Insert request.
        /// </summary>
        Insert = 0x02u,

        /// <summary>
        /// Replace request.
        /// </summary>
        Replace = 0x03u,

        /// <summary>
        /// Update request.
        /// </summary>
        Update = 0x04u,

        /// <summary>
        /// Delete request.
        /// </summary>
        Delete = 0x05u,

        /// <summary>
        /// Request old <see cref="Tarantool"/> version (before 1.7).
        /// </summary>
        OldCall = 0x06u,

        /// <summary>
        /// User authorization request.
        /// </summary>
        Auth = 0x07u,

        /// <summary>
        /// Eval request.
        /// </summary>
        Eval = 0x08u,

        /// <summary>
        /// Upsert request.
        /// </summary>
        Upsert = 0x09u,

        /// <summary>
        /// Call function request.
        /// </summary>
        Call = 0x0Au,

        /// <summary>
        /// Execute request.
        /// </summary>
        Execute = 0x0Bu,

        /// <summary>
        /// Ping request.
        /// </summary>
        Ping = 0x40u,

        /// <summary>
        /// No error response.
        /// </summary>
        Ok = 0x00u,

        /// <summary>
        /// Error response. Mask code.
        /// </summary>
        ErrorMask = 0x8000u,

        /// <summary>
        /// Join replication code.
        /// </summary>
        Join = 0x41u,

        /// <summary>
        /// Subscribe replication code.
        /// </summary>
        Subscribe = 0x42u,

        /// <summary>
        /// No command code.
        /// </summary>
        _ = uint.MaxValue
    }
}
