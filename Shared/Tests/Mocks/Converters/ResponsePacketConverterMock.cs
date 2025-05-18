// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Responses;
using nanoFramework.Tarantool.Tests.Mocks.Data;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class ResponsePacketConverterMock : ResponsePacketConverter
    {
#nullable enable
        internal ResponsePacketConverterMock() : base(typeof(ResponsePacketConverterMock))
        { 
        }

        internal ResponsePacketConverterMock(Type dataType) : base(dataType)
        {
        }

        public override void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is DataResponseMock dataResponse)
            {
                if (dataResponse.TestData != null)
                {
                    if (dataResponse.MetaData.Length > 0 || dataResponse.SqlInfo != null)
                    {
                        writer.WriteMapHeader(2);
                    }
                    else
                    {
                        writer.WriteMapHeader(1);
                    }      
                }
                else
                {
                    if (dataResponse.MetaData.Length < 1 && dataResponse.SqlInfo == null)
                    {
                        writer.WriteMapHeader(1);
                    }
                    else
                    {
                        writer.WriteMapHeader(2);
                    }
                }

                var keyConverter = ConverterContext.GetConverter(typeof(uint));

                if (dataResponse.TestData != null)
                {
                    keyConverter.Write(Key.Data, writer);
                    ConverterContext.GetConverter(dataResponse.TestData.GetType()).Write(dataResponse.TestData, writer);
                }

                if (dataResponse.MetaData.Length > 0)
                {
                    keyConverter.Write(Key.Metadata, writer);
                    WriteMetadata(dataResponse.MetaData, writer, keyConverter);
                }
                else
                {
                    if (dataResponse.SqlInfo != null)
                    {
                        keyConverter.Write(Key.SqlInfo_2_0_4, writer);
                        writer.WriteMapHeader(1);
                        keyConverter.Write(Key.SqlRowCount_2_0_4, writer);
                        ConverterContext.GetConverter(typeof(int)).Write(dataResponse.SqlInfo.RowCount, writer);
                    }
                }
            }
            else
            {
                throw new ArgumentException(nameof(value));
            }
        }

        private static void WriteMetadata(FieldMetadata[] fieldMetadata, IMessagePackWriter writer, IConverter keyConverter)
        {
            writer.WriteArrayHeader((uint)fieldMetadata.Length);

            var stringConverter = ConverterContext.GetConverter(typeof(string));

            foreach (var metaData in fieldMetadata)
            {
                keyConverter.Write(Key.FieldName_2_0_4, writer);
                stringConverter.Write(metaData.Name, writer);
            }
        }
    }
}
