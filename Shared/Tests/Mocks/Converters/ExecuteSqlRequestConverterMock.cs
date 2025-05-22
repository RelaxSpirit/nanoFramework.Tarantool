// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using System.IO;
#endif
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Dto;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class ExecuteSqlRequestConverterMock : ExecuteSqlRequestConverter
    {
#nullable enable
        public override object? Read([NotNull] IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length != 3)
            {
                throw ExceptionHelper.InvalidMapLength(length, 3);
            }

            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var stringConverter = ConverterContext.GetConverter(typeof(string));
            var tupleConverter = ConverterContext.GetConverter(typeof(TarantoolTuple));

            string? query = null;
            ArrayList queryParameters = new ArrayList();

            for (var i = 0; i < length; i++)
            {
                Key key = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

                switch (key)
                {
                    case Key.SqlQueryText:
                        query = (string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.SqlOptions:
                        ConverterContext.NullConverter.Read(reader);
                        break;
                    case Key.SqlParameters:
                        var parametersCount = reader.ReadArrayLength();

                        for (int p = 0; p < parametersCount; p++)
                        {
                            var arraySegment = reader.ReadToken() ?? throw ExceptionHelper.ActualValueIsNullReference();

                            var tokenType = arraySegment.ReadDataType();
                            arraySegment.Seek(0, SeekOrigin.Begin);

                            if (IndexPartConverter.GetHighBits(tokenType, 4) == IndexPartConverter.GetHighBits(DataTypes.FixMap, 4))
                            {
                                var mapLength = arraySegment.ReadMapLength();

                                if (mapLength != 1)
                                {
                                    throw ExceptionHelper.InvalidMapLength(mapLength, 1);
                                }

                                var parameterName = (string)(stringConverter.Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference());
                                var parameterValue = GetObjectByDataType(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference();
                                queryParameters.Add(new SqlParameter(parameterValue, parameterName));
                            }
                            else
                            {
                                var parameterValue = GetObjectByDataType(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference();
                                queryParameters.Add(new SqlParameter(parameterValue));
                            }
                        }

                        break;
                }
            }

            if (query != null)
            {
                return new ExecuteSqlRequest(query, (SqlParameter[])queryParameters.ToArray(typeof(SqlParameter)));
            }
            else
            {
                return null;
            }
        }

        internal static object? GetObjectByDataType(ArraySegment reader)
        {
            var position = reader.Position;
            var type = reader.ReadDataType();
            reader.Seek(position, SeekOrigin.Begin);

            switch (type)
            {
                case DataTypes.Null:
                    return null;
                case DataTypes.True:
                    return true;
                case DataTypes.False:
                    return false;
                case DataTypes.Double:
                    return ConverterContext.GetConverter(typeof(double)).Read(reader);
                case DataTypes.Single:
                    return ConverterContext.GetConverter(typeof(float)).Read(reader);
                case DataTypes.Str8:
                case DataTypes.Str16:
                case DataTypes.Str32:
                    return ConverterContext.GetConverter(typeof(string)).Read(reader);
                case DataTypes.UInt8:
                    return ConverterContext.GetConverter(typeof(byte)).Read(reader);
                case DataTypes.UInt16:
                    return ConverterContext.GetConverter(typeof(ushort)).Read(reader);
                case DataTypes.UInt32:
                    return ConverterContext.GetConverter(typeof(uint)).Read(reader);
                case DataTypes.UInt64:
                    return ConverterContext.GetConverter(typeof(ulong)).Read(reader);
                case DataTypes.Int8:
                    return ConverterContext.GetConverter(typeof(sbyte)).Read(reader);
                case DataTypes.Int16:
                    return ConverterContext.GetConverter(typeof(short)).Read(reader);
                case DataTypes.Int32:
                    return ConverterContext.GetConverter(typeof(int)).Read(reader);
                case DataTypes.Int64:
                    return ConverterContext.GetConverter(typeof(long)).Read(reader);
                case DataTypes.Bin8:
                case DataTypes.Bin16:
                case DataTypes.Bin32:
                    return ConverterContext.GetConverter(typeof(byte[])).Read(reader);
                case DataTypes.Array16:
                case DataTypes.Array32:
                    return ConverterContext.GetConverter(typeof(ArrayList)).Read(reader);
                case DataTypes.Map16:
                case DataTypes.Map32:
                    return ConverterContext.GetConverter(typeof(Hashtable)).Read(reader);
                default:
                    return ReadObject(type, reader);
            }
        }

        private static object? ReadObject(DataTypes type, IMessagePackReader reader)
        {
            if (IndexPartConverter.GetHighBits(type, 3) == IndexPartConverter.GetHighBits(DataTypes.FixStr, 3))
            {
                return ConverterContext.GetConverter(typeof(string)).Read(reader);
            }

            if (IndexPartConverter.GetHighBits(type, 4) == IndexPartConverter.GetHighBits(DataTypes.FixArray, 4))
            {
                return ConverterContext.GetConverter(typeof(ArrayList)).Read(reader);
            }

            if (IndexPartConverter.GetHighBits(type, 4) == IndexPartConverter.GetHighBits(DataTypes.FixMap, 4))
            {
                return ConverterContext.GetConverter(typeof(Hashtable)).Read(reader);
            }

            if (IndexPartConverter.GetHighBits(type, 1) == IndexPartConverter.GetHighBits(DataTypes.PositiveFixNum, 1))
            {
                return (byte)type;
            }

            if (IndexPartConverter.GetHighBits(type, 3) == IndexPartConverter.GetHighBits(DataTypes.NegativeFixNum, 3))
            {
                return (sbyte)((byte)type - 1 - byte.MaxValue);
            }

            throw new NotSupportedException($"Bad data type: {type}");
        }
    }
}
