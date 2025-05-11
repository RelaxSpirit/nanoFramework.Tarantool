// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using Index = nanoFramework.Tarantool.Client.Index;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> index converter class.
    /// </summary>
    internal class IndexConverter : IConverter
    {
        public static Index Read(IMessagePackReader reader)
        {
            var length = reader.ReadArrayLength();

            if (length != 6u)
            {
                throw ExceptionHelper.InvalidArrayLength(6u, length);
            }

            var uintConverter = ConverterContext.GetConverter(typeof(uint));
            var stringConverter = ConverterContext.GetConverter(typeof(string));
            var indexTypeConverter = ConverterContext.GetConverter(typeof(IndexType));
            var optionsConverter = ConverterContext.GetConverter(typeof(IndexCreationOptions));
            var indexPartsConverter = ConverterContext.GetConverter(typeof(IndexPart[]));

            var spaceId = uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var id = uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var name = stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var type = indexTypeConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var options = optionsConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var indexParts = indexPartsConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();

            return new Index((uint)id, (uint)spaceId, (string)name, ((IndexCreationOptions)options).Unique, (IndexType)type, (IndexPart[])indexParts);
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
