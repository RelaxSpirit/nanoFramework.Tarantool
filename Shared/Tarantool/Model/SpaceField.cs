// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// The <see cref="Tarantool"/> space field class.
    /// </summary>
    internal class SpaceField : ISpaceField
    {
        private readonly string name;
        private readonly FieldType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpaceField"/> class.
        /// </summary>
        /// <param name="name">Space field name.</param>
        /// <param name="type">Space field type.</param>
        internal SpaceField(string name, FieldType type)
        {
            this.name = name;
            this.type = type;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space field name.
        /// </summary>
        string ISpaceField.Name => this.name;

        /// <summary>
        /// Gets <see cref="Tarantool"/> space field type.
        /// </summary>
        FieldType ISpaceField.Type => this.type;
    }
}
