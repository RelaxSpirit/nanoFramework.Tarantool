// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.UpdateOperations
{
    /// <summary>
    /// Universal update operation class.
    /// </summary>
    public class UpdateOperation : UpdateOperationFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateOperation"/> class.
        /// </summary>
        /// <param name="operationType">Update operation type.</param>
        /// <param name="fieldNumber">Update fields number.</param>
        /// <param name="argument">Update argument.</param>
        internal UpdateOperation(string operationType, int fieldNumber, object argument)
        {
            this.OperationType = operationType;
            this.FieldNumber = fieldNumber;
            this.Argument = argument;
        }

        /// <summary>
        /// Gets update operation type <see cref="UpdateOperationType"/>.
        /// </summary>
        public string OperationType { get; }

        /// <summary>
        /// Gets update operation field number.
        /// </summary>
        public int FieldNumber { get; }

        /// <summary>
        /// Gets update argument.
        /// </summary>
        public object Argument { get; }
    }
}
