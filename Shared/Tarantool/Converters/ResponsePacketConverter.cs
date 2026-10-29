// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Exceptions;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The response packet converter class.
    /// </summary>
    internal class ResponsePacketConverter : IConverter
    {
#nullable enable
        internal static DataResponse Read(IMessagePackReader reader, Type? dataType = null)
        {
            var length = reader.ReadMapLength();
            if (length != 1 && length != 2)
            {
                throw ExceptionHelper.InvalidMapLength(length, 1u, 2u);
            }

            IConverter? dataConverter = null;
            
            if (dataType == null)
            {
                dataConverter = TarantoolContext.Instance.ArrayListConverter;
            }

            object? data = null;
            var dataWasSet = false;
            FieldMetadata[] fieldMetaData = new FieldMetadata[0];
            SqlInfo sqlInfo = SqlInfo.Empty;

            for (var i = 0; i < length; i++)
            {
                var dataKey = (Key)(TarantoolContext.Instance.KeyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                switch (dataKey)
                {
                    case Key.Data:
                        if (dataConverter != null)
                        {
                            data = dataConverter.Read(reader);
                        }
                        else
                        {
                            if (dataType is TarantoolTupleType tarantoolTupleType)
                            {
                                data = TarantoolTupleConverter.Read(reader, tarantoolTupleType);
                            }
                            else if (dataType is TarantoolTupleArrayType tarantoolTupleArrayType)
                            {
                                data = TarantoolTupleArrayConverter.Read(reader, tarantoolTupleArrayType);
                            }
                            else
                            {
                                if (dataType != null)
                                {
                                    data = (ConverterContext.GetConverter(dataType) ?? throw new ConverterNotFoundException(dataType)).Read(reader);
                                }
                                else
                                {
                                    throw new ArgumentNullException(nameof(dataType));
                                }
                            }
                        }

                        dataWasSet = true;
                        break;
                    case Key.Metadata:
                        fieldMetaData = ReadMetadata(reader);
                        break;
                    case Key.SqlInfo:
                        sqlInfo = SqlInfoResponsePacketConverter.ReadSqlInfo(reader);
                        break;
                    case Key.SqlInfo_2_0_4:
                        sqlInfo = SqlInfoResponsePacketConverter.ReadSqlInfo(reader);
                        break;
                    default:
                        throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data, Key.Metadata);
                }
            }

            if (!dataWasSet && sqlInfo == SqlInfo.Empty)
            {
                throw ExceptionHelper.NoDataInDataResponse();
            }

            return new DataResponse(data, fieldMetaData, sqlInfo);
        }

        private static FieldMetadata[] ReadMetadata(IMessagePackReader reader)
        {
            var length = reader.ReadArrayLength();
            if (length == uint.MaxValue)
            {
                return new FieldMetadata[0];
            }

            var result = new FieldMetadata[length];

            for (var i = 0; i < length; i++)
            {
                var metadataLength = reader.ReadMapLength();
                if (metadataLength == uint.MaxValue)
                {
                    result[i] = default;
                    continue;
                }

                for (var j = 0; j < metadataLength; j++)
                {
                    var key = (Key)(TarantoolContext.Instance.KeyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                    switch (key)
                    {
                        case Key.FieldName:
                            result[i] = new FieldMetadata((string)(TarantoolContext.Instance.StringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference()));
                            continue;
                        case Key.FieldName_2_0_4:
                            result[i] = new FieldMetadata((string)(TarantoolContext.Instance.StringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference()));
                            continue;
                        default:
                            reader.SkipToken();
                            break;
                    }
                }
            }

            return result;
        }

        public virtual void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            throw new NotImplementedException();
        }

        object? IConverter.Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader, null);
        }
    }
}
