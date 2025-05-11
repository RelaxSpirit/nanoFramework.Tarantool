// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    /// <summary>
    /// A interface <see cref="Tarantool"/> space field.
    /// </summary>
    public interface ISpaceField
    {
        /// <summary>
        /// Gets <see cref="Tarantool"/> field name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> field type.
        /// </summary>
        FieldType Type { get; }
    }
}
