// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Exceptions;
using nanoFramework.Tarantool.Exceptions;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Headers;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Helpers
{
    /// <summary>
    /// Exception helper class.
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Gets property unspecified exception.
        /// </summary>
        /// <param name="propertyName">Unspecified property name.</param>
        /// <returns><see cref="Exception"/> by unspecified property.</returns>
        public static Exception PropertyUnspecified(string propertyName)
        {
            return new ArgumentException($"Property '{propertyName}' is not specified!");
        }

        /// <summary>
        /// Gets enum value expected exception.
        /// </summary>
        /// <param name="enumType">Enum instance type.</param>
        /// <param name="actual">Actual enum value.</param>
        /// <returns><see cref="InvalidOperationException"/> by enum value expected.</returns>
        public static InvalidOperationException EnumValueExpected(Type enumType, object actual)
        {
            return new InvalidOperationException($"Enum expected, but got {enumType}, actual value {actual}");
        }

        /// <summary>
        /// Gets unexpected enum underlying type exception.
        /// </summary>
        /// <param name="enumUnderlyingType">Enum underlying type.</param>
        /// <param name="actualValue">Actual enum value.</param>
        /// <returns><see cref="InvalidOperationException"/> by unexpected enum underlying type.</returns>
#nullable enable
        public static InvalidOperationException UnexpectedEnumUnderlyingType(Type enumUnderlyingType, string? actualValue)
        {
            return new InvalidOperationException($"Unexpected underlying enum type: {enumUnderlyingType}, actual value '{actualValue ?? "null"}'");
        }
#nullable disable

        /// <summary>
        /// Gets invalid <see cref="MessagePack"/> map length exception.
        /// </summary>
        /// <param name="actual">Actual map length.</param>
        /// <param name="expected">Expected map length.</param>
        /// <returns><see cref="ArgumentException"/> by invalid <see cref="MessagePack"/> map length.</returns>
        public static ArgumentException InvalidMapLength(uint actual, params uint[] expected)
        {
            return new ArgumentException($"Invalid map length: {expected.Join(", ")} is expected, but got {actual}.");
        }

        /// <summary>
        /// Gets actual value is <see langword="null"/> reference exception.
        /// </summary>
        /// <returns><see cref="ArgumentException"/> by actual value is <see langword="null"/> reference.</returns>
        public static ArgumentException ActualValueIsNullReference()
        {
            return new ArgumentException($"Reading value from response is null.");
        }

        /// <summary>
        /// Gets invalid <see cref="MessagePack"/> array length exception.
        /// </summary> 
        /// <param name="expected">Expected array length.</param>
        /// <param name="actual">Actual array length.</param>
        /// <returns><see cref="ArgumentException"/> by invalid <see cref="MessagePack"/> array length.</returns>
        public static ArgumentException InvalidArrayLength(uint expected, uint actual)
        {
            return new ArgumentException($"Invalid array length: {expected} is expected, but got {actual}.");
        }

        /// <summary>
        /// Gets unexpected <see cref="MessagePack"/> data type exception.
        /// </summary>
        /// <param name="expected">Expected <see cref="MessagePack"/> data type.</param>
        /// <param name="actual">Actual <see cref="MessagePack"/> data type.</param>
        /// <returns><see cref="ArgumentException"/> by unexpected <see cref="MessagePack"/> data type.</returns>
        public static ArgumentException UnexpectedDataType(DataTypes expected, DataTypes actual)
        {
            return new ArgumentException($"Unexpected data type: {expected} is expected, but got {actual}.");
        }

        internal static ArgumentException UnexpectedKey(Key actual, params Key[] expected)
        {
            return new ArgumentException($"Unexpected key: {expected.Join(", ")} is expected, but got {actual}.");
        }

        internal static ArgumentException UnexpectedGreetingBytesCount(int readCount)
        {
            return new ArgumentException($"Invalid greetings response length. 128 is expected, but got {readCount}.");
        }

        internal static InvalidOperationException NotConnected()
        {
            return new InvalidOperationException("Can't perform operation. Looks like we are not connected to tarantool. Call 'Connect' method before calling any other operations.");
        }

        internal static TarantoolException TarantoolError(ResponseHeader header, ErrorResponse errorResponse)
        {
            var detailedMessage = GetDetailedTarantoolMessage(header.Code);
            return new TarantoolException($"Tarantool returns an error for request with id: {header.RequestId}, code: 0x{header.Code:X} and message: {errorResponse.ErrorMessage}. {detailedMessage}".TrimEnd());
        }

        internal static ArgumentOutOfRangeException WrongRequestId(RequestId requestId)
        {
            return new ArgumentOutOfRangeException($"Can't find pending request with id = {requestId}");
        }

        internal static ArgumentException InvalidSpaceName(string name)
        {
            return new ArgumentException($"Space with name '{name}' was not found!");
        }

        internal static ArgumentException InvalidSpaceId(uint id)
        {
            return new ArgumentException($"Space with id '{id}' was not found!");
        }

        internal static ArgumentException InvalidIndexName(string indexName, string space)
        {
            return new ArgumentException($"Index with name '{indexName}' was not found in space {space}!");
        }

        internal static ArgumentException InvalidIndexId(uint indexId, string space)
        {
            return new ArgumentException($"Index with id '{indexId}' was not found in space {space}!");
        }

        internal static NotSupportedException WrongIndexType(string indexType, string operation)
        {
            return new NotSupportedException($"Only {indexType} indices support {operation} operation.");
        }

        internal static ArgumentException RequestWithSuchIdAlreadySent(RequestId requestId)
        {
            return new ArgumentException($"Task with id {requestId} already sent.");
        }

        internal static ArgumentException VersionCantBeEmpty()
        {
            return new ArgumentNullException("version", "TarantoolVersion should be not null.");
        }

        internal static SerializationException CantParseBoxInfoResponse()
        {
            return new SerializationException("Box info response is malformed.");
        }

        internal static Exception CantCompareBuilds(TarantoolVersion left, TarantoolVersion right)
        {
            return new InvalidOperationException($"Versions '{left}' and '{right}' differs only by commit hash, can't compare them.");
        }

        internal static Exception SqlIsNotAvailable([NotNull] TarantoolVersion version)
        {
            return new InvalidOperationException($"Can't use sql on '{version}' of tarantool. Upgrade to 1.8 (prefer latest one).");
        }

        internal static Exception NoDataInDataResponse()
        {
            return new InvalidOperationException("No data in data response");
        }

#nullable enable
        private static string? GetDetailedTarantoolMessage(CommandCode code)
        {
            var uintCode = (uint)code;

            switch (uintCode)
            {
                case 0x8012:
                    return "If index part type is NUM, unsigned int should be used.";
                default:
                    return null;
            }
        }
#nullable disable
    }
}
