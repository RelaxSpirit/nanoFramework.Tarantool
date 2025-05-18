// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Client;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Tests.Mocks.Data;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class SpaceConverterMock : SpaceConverter
    {
#nullable enable
        public override void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is SpaceMock space) 
            {
                writer.WriteArrayHeader(7);

                var uintConverter = ConverterContext.GetConverter(typeof(uint));
                var stringConverter = ConverterContext.GetConverter(typeof(string));
                var engineConverter = ConverterContext.GetConverter(typeof(StorageEngine));
                var fieldConverter = ConverterContext.GetConverter(typeof(SpaceFieldMock[]));

                uintConverter.Write(space.Id, writer);
                ConverterContext.NullConverter.Write(null, writer);
                stringConverter.Write(space.Name, writer);
                engineConverter.Write(space.Engine, writer);
                uintConverter.Write(space.FieldCount, writer);
                ConverterContext.NullConverter.Write(null, writer);
                fieldConverter.Write(space.Fields, writer);
            }
            else
            {
                throw new ArgumentException(nameof(value));
            }
        }
    }
}
