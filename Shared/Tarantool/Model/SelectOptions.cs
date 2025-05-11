// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// <see cref="Tarantool"/> select options class.
    /// </summary>
    public class SelectOptions
    {
        /// <summary>
        /// Gets or sets <see cref="Tarantool"/> <see cref="Iterator"/> type.
        /// </summary>
        public Iterator Iterator { get; set; } = Iterator.Eq;

        /// <summary>
        /// Gets or sets select rows result limit. Default <see cref="uint.MaxValue"/>.
        /// </summary>
        public uint Limit { get; set; } = uint.MaxValue;

        /// <summary>
        /// Gets or sets select rows offset. Default <see cref="uint.MinValue"/>.
        /// </summary>
        public uint Offset { get; set; } = uint.MinValue;
    }
}
