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
    internal static class ExceptionHelper
    {
        public static ArgumentException UnexpectedGreetingBytesCount(int readCount)
        {
            return new ArgumentException($"Invalid greetings response length. 128 is expected, but got {readCount}.");
        }

        public static ArgumentException InvalidMapLength(uint actual, params uint[] expected)
        {
            return new ArgumentException($"Invalid map length: {expected.Join(", ")} is expected, but got {actual}.");
        }

        public static ArgumentException UnexpectedKey(Key actual, params Key[] expected)
        {
            return new ArgumentException($"Unexpected key: {expected.Join(", ")} is expected, but got {actual}.");
        }

        public static ArgumentException ActualValueIsNullReference()
        {
            return new ArgumentException($"Reading value from response is null.");
        }

        public static ArgumentException InvalidArrayLength(uint expected, uint actual)
        {
            return new ArgumentException($"Invalid array length: {expected} is expected, but got {actual}.");
        }

        public static ArgumentException UnexpectedDataType(DataTypes expected, DataTypes actual)
        {
            return new ArgumentException($"Unexpected data type: {expected} is expected, but got {actual}.");
        }

        public static InvalidOperationException NotConnected()
        {
            return new InvalidOperationException("Can't perform operation. Looks like we are not connected to tarantool. Call 'Connect' method before calling any other operations.");
        }

        public static TarantoolException TarantoolError(ResponseHeader header, ErrorResponse errorResponse)
        {
            var detailedMessage = GetDetailedTarantoolMessage(header.Code);
            return new TarantoolException($"Tarantool returns an error for request with id: {header.RequestId}, code: 0x{header.Code:X} and message: {errorResponse.ErrorMessage}. {detailedMessage}".TrimEnd());
        }

        public static ArgumentOutOfRangeException WrongRequestId(RequestId requestId)
        {
            return new ArgumentOutOfRangeException($"Can't find pending request with id = {requestId}");
        }

        public static ArgumentException InvalidSpaceName(string name)
        {
            return new ArgumentException($"Space with name '{name}' was not found!");
        }

        public static ArgumentException InvalidSpaceId(uint id)
        {
            return new ArgumentException($"Space with id '{id}' was not found!");
        }

        public static ArgumentException InvalidIndexName(string indexName, string space)
        {
            return new ArgumentException($"Index with name '{indexName}' was not found in space {space}!");
        }

        public static ArgumentException InvalidIndexId(uint indexId, string space)
        {
            return new ArgumentException($"Index with id '{indexId}' was not found in space {space}!");
        }

        public static Exception PropertyUnspecified(string propertyName)
        {
            return new ArgumentException($"Property '{propertyName}' is not specified!");
        }

        public static InvalidOperationException EnumValueExpected(Type enumType, object actual)
        {
            return new InvalidOperationException($"Enum expected, but got {enumType}, actual value {actual}");
        }

#nullable enable
        public static InvalidOperationException UnexpectedEnumUnderlyingType(Type enumUnderlyingType, string? actualValue)
        {
            return new InvalidOperationException($"Unexpected underlying enum type: {enumUnderlyingType}, actual value '{actualValue ?? "null"}'");
        }
#nullable disable

        public static NotSupportedException WrongIndexType(string indexType, string operation)
        {
            return new NotSupportedException($"Only {indexType} indices support {operation} operation.");
        }

        public static ArgumentException RequestWithSuchIdAlreadySent(RequestId requestId)
        {
            return new ArgumentException($"Task with id {requestId} already sent.");
        }

        public static ArgumentException VersionCantBeEmpty()
        {
            return new ArgumentNullException("version", "TarantoolVersion should be not null.");
        }

        public static SerializationException CantParseBoxInfoResponse()
        {
            return new SerializationException("Box info response is malformed.");
        }

        public static Exception CantCompareBuilds(TarantoolVersion left, TarantoolVersion right)
        {
            return new InvalidOperationException($"Versions '{left}' and '{right}' differs only by commit hash, can't compare them.");
        }

        public static Exception SqlIsNotAvailable([NotNull] TarantoolVersion version)
        {
            return new InvalidOperationException($"Can't use sql on '{version}' of tarantool. Upgrade to 1.8 (prefer latest one).");
        }

        public static Exception NoDataInDataResponse()
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
