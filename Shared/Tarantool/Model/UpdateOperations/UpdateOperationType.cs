// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.UpdateOperations
{
    /// <summary>
    /// Class <see cref="Tarantool"/> update operation types.
    /// </summary>
    public static class UpdateOperationType
    {
        /// <summary>
        /// Operation Addition. Works only with integer.
        /// </summary>
        public static readonly string Addition = "+";

        /// <summary>
        /// Operation subtraction. Works only with integer.
        /// </summary>
        public static readonly string Subtraction = "-";

        /// <summary>
        /// Operation bitwise 'AND'. Works only with integer.
        /// </summary>
        public static readonly string BitwiseAnd = "&";

        /// <summary>
        /// Operation bitwise 'XOR'. Works only with integer.
        /// </summary>
        public static readonly string BitwiseXor = "^";

        /// <summary>
        /// Operation bitwise 'OR'. Works only with integer.
        /// </summary>
        public static readonly string BitwiseOr = "|";

        /// <summary>
        /// Operation delete. Works on any field.
        /// </summary>
        public static readonly string Delete = "#";

        /// <summary>
        /// Operation insert. Works on any field.
        /// </summary>
        public static readonly string Insert = "!";

        /// <summary>
        /// Operation assign. Works on any field.
        /// </summary>
        public static readonly string Assign = "=";

        /// <summary>
        /// Operation splice. Works only for string.
        /// </summary>
        public static readonly string Splice = ":";
    }
}
