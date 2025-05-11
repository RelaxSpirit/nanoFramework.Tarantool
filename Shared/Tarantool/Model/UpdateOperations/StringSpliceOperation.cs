// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.UpdateOperations
{
    /// <summary>
    /// String splice update operation class.
    /// </summary>
    public class StringSpliceOperation : UpdateOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringSpliceOperation"/> class.
        /// </summary>
        /// <param name="fieldNumber">Field number.</param>
        /// <param name="position">Splice position.</param>
        /// <param name="offset">Splice offset.</param>
        /// <param name="argument">Splice argument.</param>
        internal StringSpliceOperation(
            int fieldNumber,
            int position,
            int offset,
            string argument)
            : base(UpdateOperationType.Splice, fieldNumber, argument)
        {
            this.Position = position;
            this.Offset = offset;
        }

        /// <summary>
        /// Gets number position to splice.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// Gets number offset to splice.
        /// </summary>
        public int Offset { get; }
    }
}
