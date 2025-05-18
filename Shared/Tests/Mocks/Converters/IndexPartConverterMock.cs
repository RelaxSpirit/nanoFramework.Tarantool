// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class IndexPartConverterMock : IndexPartConverter
    {
#nullable enable
        public override void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is IndexPart indexPart)
            {
                writer.WriteMapHeader(2);

                var uintConverter = ConverterContext.GetConverter(typeof(uint));
                var indexPartTypeConverter = ConverterContext.GetConverter(typeof(FieldType));
                var stringConverter = ConverterContext.GetConverter(typeof(string));

                stringConverter.Write("field", writer);
                uintConverter.Write(indexPart.FieldNo, writer);

                stringConverter.Write("type", writer);
                indexPartTypeConverter.Write(indexPart.Type, writer);
            }
            else
            {
                throw new ArgumentException(nameof(value));
            }
        }
    }
}
