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

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class IndexCreationOptionsConverterMock : IndexCreationOptionsConverter
    {
#nullable enable
        public override void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is IndexCreationOptions options)
            {
                writer.WriteMapHeader(1);

                var stringConverter = ConverterContext.GetConverter(typeof(string));
                var boolConverter = ConverterContext.GetConverter(typeof(bool));

                stringConverter.Write("name", writer);
                boolConverter.Write(options.Unique, writer);
            }
            else
            {
                throw new ArgumentException(nameof(value));
            }
        }
    }
}
