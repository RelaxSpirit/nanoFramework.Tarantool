// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Enums
{
    /// <summary>
    /// Enum <see cref="Tarantool"/> field type.
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// No field type.
        /// </summary>
        _ = -1,

        /// <summary>
        /// String field type.
        /// </summary>
        Str,

        /// <summary>
        /// Unsigned field type.
        /// </summary>
        Num,

        /// <summary>
        /// All (*) field type.
        /// </summary>
        Star,

        /// <summary>
        /// Null field type.
        /// </summary>
        Nil,

        /// <summary>
        /// Scalar field type.
        /// </summary>
        Scalar,

        /// <summary>
        /// Integer field type.
        /// </summary>
        Integer,

        /// <summary>
        /// String field type.
        /// </summary>
        String,

        /// <summary>
        /// Binary field type.
        /// </summary>
        Binary,

        /// <summary>
        /// Array field type.
        /// </summary>
        Array,

        /// <summary>
        /// Map field type.
        /// </summary>
        Map,

        /// <summary>
        /// Boolean field type.
        /// </summary>
        Boolean,

        /// <summary>
        /// Float field type.
        /// </summary>
        Float,

        /// <summary>
        /// Double field type.
        /// </summary>
        Double,

        /// <summary>
        /// Unsigned field type.
        /// </summary>
        Unsigned,

        /// <summary>
        /// Any field type.
        /// </summary>
        Any,

        /// <summary>
        /// Uuid field type.
        /// </summary>
        Uuid,

        /// <summary>
        /// Datetime field type.
        /// </summary>
        Datetime,

        /// <summary>
        /// Unsigned field type.
        /// </summary>
        Number,

        /// <summary>
        /// Decimal field type.
        /// </summary>
        Decimal,

        /// <summary>
        /// Varbinary field type.
        /// </summary>
        Varbinary,

        /// <summary>
        /// Integer field type.
        /// </summary>
        Int,

        /// <summary>
        /// Unsigned field type.
        /// </summary>
        Uint,

        /// <summary>
        /// <see cref="Tarantool"/> <see cref="MessagePack"/> extension field type.
        /// </summary>
        Extension
    }
}
