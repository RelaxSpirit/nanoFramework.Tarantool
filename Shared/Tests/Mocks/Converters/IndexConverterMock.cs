// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Tests.Mocks.Data;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class IndexConverterMock : IndexConverter
    {
#nullable enable
        public override void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is IIndex index)
            {
                writer.WriteArrayHeader(6);

                var uintConverter = ConverterContext.GetConverter(typeof(uint));
                var stringConverter = ConverterContext.GetConverter(typeof(string));
                var indexTypeConverter = ConverterContext.GetConverter(typeof(IndexType));
                var optionsConverter = ConverterContext.GetConverter(typeof(IndexCreationOptionsMock));
                var indexPartsConverter = ConverterContext.GetConverter(typeof(IndexPartMock[]));

                uintConverter.Write(index.SpaceId, writer);
                uintConverter.Write(index.Id, writer);
                stringConverter.Write(index.Name, writer);
                indexTypeConverter.Write(index.Type, writer);
                optionsConverter.Write(new IndexCreationOptionsMock(index.Unique), writer);
                indexPartsConverter.Write(index.Parts, writer);
            }
            else
            {
                throw new ArgumentException(nameof(value));
            }
        }
    }
}
