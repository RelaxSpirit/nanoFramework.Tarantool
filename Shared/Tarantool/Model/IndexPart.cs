// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// <see cref="Tarantool"/> index part class.
    /// </summary>
    public class IndexPart
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexPart"/> class.
        /// </summary>
        /// <param name="fieldNo">Index field number.</param>
        /// <param name="type">Index field type.</param>
        internal IndexPart(uint fieldNo, FieldType type)
        {
            this.FieldNo = fieldNo;
            this.Type = type;
        }

        /// <summary>
        /// Gets field sequence number.
        /// </summary>
        public uint FieldNo { get; }

        /// <summary>
        /// Gets field type.
        /// </summary>
        public FieldType Type { get; }
    }
}
