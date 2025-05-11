// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Client;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The space converter class.
    /// </summary>
    internal class SpaceConverter : IConverter
    {
        private static Space Read(IMessagePackReader reader)
        {
            var actual = reader.ReadArrayLength();
            const uint Expected = 7u;

            if (actual != Expected)
            {
                throw ExceptionHelper.InvalidArrayLength(Expected, actual);
            }

            var uintConverter = ConverterContext.GetConverter(typeof(uint));
            var stringConverter = ConverterContext.GetConverter(typeof(string));
            var engineConverter = ConverterContext.GetConverter(typeof(StorageEngine));
            var fieldConverter = ConverterContext.GetConverter(typeof(SpaceField[]));

            var id = (uint)(uintConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());

            //// TODO Find what skipped number means
            reader.SkipToken();

            var name = (string)(stringConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());
            var engine = (StorageEngine)(engineConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());
            var fieldCount = (uint)(uintConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());

            //// TODO Find what skipped dictionary used for
            reader.SkipToken();

            var fields = (SpaceField[])(fieldConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());

            return new Space(id, fieldCount, name, engine, fields);
        }

#nullable enable
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
