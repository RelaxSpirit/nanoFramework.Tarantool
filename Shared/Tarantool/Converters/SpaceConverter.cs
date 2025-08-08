// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
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

            var id = (uint)(TarantoolContext.Instance.UintConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());

            //// TODO Find what skipped number means
            reader.SkipToken();

            var name = (string)(TarantoolContext.Instance.StringConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());
            var engine = (StorageEngine)(TarantoolContext.Instance.StorageEngineConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());
            var fieldCount = (uint)(TarantoolContext.Instance.UintConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());

            //// TODO Find what skipped dictionary used for
            reader.SkipToken();

            var fields = (SpaceField[])(TarantoolContext.Instance.SpaceFieldsConverter.Read(reader) ?? ExceptionHelper.ActualValueIsNullReference());

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
