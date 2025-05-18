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

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class SpaceFieldConverterMock : SpaceFieldConverter
    {
#nullable enable
        public override void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is ISpaceField spaceField)
            {
                writer.WriteMapHeader(2);
                var stringConverter = ConverterContext.GetConverter(typeof(string));
                var typeConverter = ConverterContext.GetConverter(typeof(FieldType));

                stringConverter.Write("name", writer);
                stringConverter.Write(spaceField.Name, writer);

                stringConverter.Write("type", writer);
                typeConverter.Write(spaceField.Type, writer);
            }
            else
            {
                throw new ArgumentException(nameof(value));
            }
        }
    }
}
