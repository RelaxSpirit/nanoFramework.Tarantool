// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using System.IO;
#endif
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
        protected readonly Type DataType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsePacketConverter"/> class.
        /// </summary>
        /// <param name="dataType"><see cref="Type"/> data of response.</param>
        internal ResponsePacketConverter(Type dataType)
        {
            this.DataType = dataType;
        }

#nullable enable
        public DataResponse Read(IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();
            if (length != 1 && length != 2)
            {
                throw ExceptionHelper.InvalidMapLength(length, 1u, 2u);
            }

            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var dataConverter = ConverterContext.GetConverter(this.DataType);
            var intConverter = ConverterContext.GetConverter(typeof(int));

            object? data = null;
            var dataWasSet = false;
            object? metadata = null;
            SqlInfo? sqlInfo = null;

            for (var i = 0; i < length; i++)
            {
                var dataKey = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                switch (dataKey)
                {
                    case Key.Data:
                        data = dataConverter.Read(reader);
                        dataWasSet = true;
                        break;
                    case Key.Metadata:
                        metadata = ReadMetadata(reader, keyConverter);
                        break;
                    case Key.SqlInfo:
                        sqlInfo = SqlInfoResponsePacketConverter.ReadSqlInfo(reader, keyConverter, intConverter);
                        break;
                    case Key.SqlInfo_2_0_4:
                        sqlInfo = SqlInfoResponsePacketConverter.ReadSqlInfo(reader, keyConverter, intConverter);
                        break;
                    default:
                        throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data, Key.Metadata);
                }
            }

            if (!dataWasSet && sqlInfo == null)
            {
                throw ExceptionHelper.NoDataInDataResponse();
            }

            if (metadata != null)
            {
                return new DataResponse(data, (FieldMetadata[])metadata, sqlInfo);
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

            var stringConverter = ConverterContext.GetConverter(typeof(string));
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
                            result[i] = new FieldMetadata((string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference()));
                            continue;
                        case Key.FieldName_2_0_4:
                            result[i] = new FieldMetadata((string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference()));
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
