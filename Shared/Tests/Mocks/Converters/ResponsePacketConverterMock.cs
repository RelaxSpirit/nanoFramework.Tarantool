// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Exceptions;
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
        internal ResponsePacketConverterMock()
        { 
        }

        public override void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is DataResponseMock dataResponse)
            {
                if (dataResponse.TestData != null)
                {
                    if (dataResponse.MetaData.Length > 0 || dataResponse.SqlInfo != SqlInfo.Empty)
                    {
                        writer.WriteMapHeader(2u);
                    }
                    else
                    {
                        writer.WriteMapHeader(1u);
                    }      
                }
                else
                {
                    if (dataResponse.MetaData.Length < 1 && dataResponse.SqlInfo == SqlInfo.Empty)
                    {
                        writer.WriteMapHeader(1u);
                    }
                    else
                    {
                        writer.WriteMapHeader(2u);
                    }
                }

                if (dataResponse.TestData != null)
                {
                    TarantoolContext.Instance.KeyConverter.Write(Key.Data, writer);
                    (ConverterContext.GetConverter(dataResponse.TestData.GetType()) ?? throw new ConverterNotFoundException(dataResponse.TestData.GetType())).Write(dataResponse.TestData, writer);
                }

                if (dataResponse.MetaData.Length > 0)
                {
                    TarantoolContext.Instance.KeyConverter.Write(Key.Metadata, writer);
                    WriteMetadata(dataResponse.MetaData, writer);
                }
                else
                {
                    if (dataResponse.SqlInfo != SqlInfo.Empty)
                    {
                        TarantoolContext.Instance.KeyConverter.Write(Key.SqlInfo_2_0_4, writer);
                        writer.WriteMapHeader(1u);
                        TarantoolContext.Instance.KeyConverter.Write(Key.SqlRowCount_2_0_4, writer);
                        TarantoolContext.Instance.IntConverter.Write(dataResponse.SqlInfo.RowCount, writer);
                    }
                }
            }
            else
            {
                throw new ArgumentException(nameof(value));
            }
        }

        private static void WriteMetadata(FieldMetadata[] fieldMetadata, IMessagePackWriter writer)
        {
            writer.WriteArrayHeader((uint)fieldMetadata.Length);

            foreach (var metaData in fieldMetadata)
            {
                TarantoolContext.Instance.KeyConverter.Write(Key.FieldName_2_0_4, writer);
                TarantoolContext.Instance.StringConverter.Write(metaData.Name, writer);
            }
        }
    }
}
