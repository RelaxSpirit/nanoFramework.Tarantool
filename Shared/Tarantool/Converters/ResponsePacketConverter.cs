// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
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
        private readonly Type _dataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsePacketConverter"/> class.
        /// </summary>
        /// <param name="dataType"><see cref="Type"/> data of response.</param>
        internal ResponsePacketConverter(Type dataType)
        {
            _dataType = dataType;
        }

#nullable enable
        public DataResponse Read(IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();
            if (length != 1 && length != 2)
            {
                throw ExceptionHelper.InvalidMapLength(length, 1u, 2u);
            }

            var dataConverter = ConverterContext.GetConverter(_dataType);

            object? data = null;
            var dataWasSet = false;
            object? metadata = null;
            SqlInfo? sqlInfo = null;

            for (var i = 0; i < length; i++)
            {
                var dataKey = (Key)(TarantoolContext.Instance.UintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                switch (dataKey)
                {
                    case Key.Data:
                        data = dataConverter.Read(reader);
                        dataWasSet = true;
                        break;
                    case Key.Metadata:
                        metadata = ReadMetadata(reader, TarantoolContext.Instance.UintConverter);
                        break;
                    case Key.SqlInfo:
                        sqlInfo = SqlInfoResponsePacketConverter.ReadSqlInfo(reader, TarantoolContext.Instance.UintConverter, TarantoolContext.Instance.IntConverter);
                        break;
                    case Key.SqlInfo_2_0_4:
                        sqlInfo = SqlInfoResponsePacketConverter.ReadSqlInfo(reader, TarantoolContext.Instance.UintConverter, TarantoolContext.Instance.IntConverter);
                        break;
                    default:
                        throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data, Key.Metadata);
                }
            }

            if (!dataWasSet && sqlInfo == null)
            {
                throw ExceptionHelper.NoDataInDataResponse();
            }

            if (metadata is FieldMetadata[] fieldMetaData)
            {
                return new DataResponse(data, fieldMetaData, sqlInfo);
            }
            else
            {
                return new DataResponse(data, sqlInfo);
            }
        }

        private static object? ReadMetadata(IMessagePackReader reader, IConverter keyConverter)
        {
            var length = reader.ReadArrayLength();
            if (length == uint.MaxValue)
            {
                return null;
            }

            var result = new FieldMetadata?[length];

            for (var i = 0; i < length; i++)
            {
                var metadataLength = reader.ReadMapLength();
                if (metadataLength == uint.MaxValue)
                {
                    result[i] = null;
                    continue;
                }

                for (var j = 0; j < metadataLength; j++)
                {
                    var key = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
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
            return Read(reader);
        }
    }
}
