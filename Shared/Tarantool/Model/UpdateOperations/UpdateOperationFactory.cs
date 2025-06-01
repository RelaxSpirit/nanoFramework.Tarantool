// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.UpdateOperations
{
    /// <summary>
    /// Base update operation class. Static update operation factory.
    /// </summary>
    public abstract class UpdateOperationFactory
    {
        #region Integer Operation Factory

        /// <summary>
        /// Create addition update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateAddition(byte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Addition, fieldNumber, argument);
        }

        /// <summary>
        /// Create subtraction update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateSubtraction(byte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'AND' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseAnd(byte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'XOR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseXor(byte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'OR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseOr(byte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        /// <summary>
        /// Create addition update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateAddition(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Addition, fieldNumber, argument);
        }

        /// <summary>
        /// Create subtraction update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateSubtraction(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'AND' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseAnd(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'XOR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseXor(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'OR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseOr(sbyte argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        /// <summary>
        /// Create addition update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateAddition(ushort argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Addition, fieldNumber, argument);
        }

        /// <summary>
        /// Create subtraction update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateSubtraction(ushort argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'AND' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseAnd(ushort argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'XOR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseXor(ushort argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'OR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseOr(ushort argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        /// <summary>
        /// Create addition update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateAddition(short argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Addition, fieldNumber, argument);
        }

        /// <summary>
        /// Create subtraction update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateSubtraction(short argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'AND' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseAnd(short argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'XOR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseXor(short argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'OR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseOr(short argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        /// <summary>
        /// Create addition update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateAddition(uint argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Addition, fieldNumber, argument);
        }

        /// <summary>
        /// Create subtraction update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateSubtraction(uint argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'AND' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseAnd(uint argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'XOR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseXor(uint argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'OR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseOr(uint argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        /// <summary>
        /// Create addition update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateAddition(int argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Addition, fieldNumber, argument);
        }

        /// <summary>
        /// Create subtraction update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateSubtraction(int argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'AND' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseAnd(int argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'XOR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseXor(int argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'OR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseOr(int argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        /// <summary>
        /// Create addition update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateAddition(ulong argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Addition, fieldNumber, argument);
        }

        /// <summary>
        /// Create subtraction update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateSubtraction(ulong argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'AND' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseAnd(ulong argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'XOR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseXor(ulong argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'OR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseOr(ulong argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        /// <summary>
        /// Create addition update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateAddition(long argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Addition, fieldNumber, argument);
        }

        /// <summary>
        /// Create subtraction update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateSubtraction(long argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.Subtraction, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'AND' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseAnd(long argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseAnd, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'XOR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseXor(long argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseXor, fieldNumber, argument);
        }

        /// <summary>
        /// Create bitwise 'OR' update operation.
        /// </summary>
        /// <param name="argument">Argument update operation.</param>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateBitwiseOr(long argument, int fieldNumber)
        {
            return new UpdateOperation(UpdateOperationType.BitwiseOr, fieldNumber, argument);
        }

        #endregion

        #region Object Operation Factory

        /// <summary>
        /// Create delete update operation.
        /// </summary>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <param name="argument">Argument update operation.</param>        
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateDelete(int fieldNumber, int argument)
        {
            return new UpdateOperation(UpdateOperationType.Delete, fieldNumber, argument);
        }

        /// <summary>
        /// Create insert update operation.
        /// </summary>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <param name="argument">Argument update operation.</param>    
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateInsert(int fieldNumber, object argument)
        {
            return new UpdateOperation(UpdateOperationType.Insert, fieldNumber, argument);
        }

        /// <summary>
        /// Create assign update operation.
        /// </summary>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <param name="argument">Argument update operation.</param>        
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static UpdateOperation CreateAssign(int fieldNumber, object argument)
        {
            return new UpdateOperation(UpdateOperationType.Assign, fieldNumber, argument);
        }

        #endregion

        #region String Operation Factory

        /// <summary>
        /// Create <see cref="string"/> slice update operation.
        /// </summary>
        /// <param name="fieldNumber">Number field for update operation.</param>
        /// <param name="position">Position to slice.</param>
        /// <param name="offset">Offset to slice.</param>
        /// <param name="argument">Argument update operation.</param>
        /// <returns>New <see cref="UpdateOperation"/> instances.</returns>
        public static StringSpliceOperation CreateStringSplice(int fieldNumber, int position, int offset, string argument)
        {
            return new StringSpliceOperation(fieldNumber, position, offset, argument);
        }

        #endregion
    }
}
