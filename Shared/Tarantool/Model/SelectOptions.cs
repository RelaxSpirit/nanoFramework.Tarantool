// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// <see cref="Tarantool"/> select options class.
    /// </summary>
    public struct SelectOptions
    {
        internal static SelectOptions Default = new SelectOptions(Iterator.Eq, uint.MaxValue, uint.MinValue);
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectOptions"/> struct.
        /// </summary>
        /// <param name="iterator">Option <see cref="Enums.Iterator"/> enum value.</param>
        /// <param name="limit">Option limit value.</param>
        /// <param name="offset">Option offset value.</param>
        public SelectOptions(Iterator iterator, uint limit, uint offset)
        {
            Iterator = iterator;
            Limit = limit;
            Offset = offset;
        }
        
        /// <summary>
        /// Gets or sets <see cref="Tarantool"/> <see cref="Iterator"/> type.
        /// </summary>
        public Iterator Iterator { get; set; }

        /// <summary>
        /// Gets or sets select rows result limit. Default <see cref="uint.MaxValue"/>.
        /// </summary>
        public uint Limit { get; set; }

        /// <summary>
        /// Gets or sets select rows offset. Default <see cref="uint.MinValue"/>.
        /// </summary>
        public uint Offset { get; set; }
    }
}
