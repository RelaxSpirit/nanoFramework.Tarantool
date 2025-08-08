// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
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

            var spaceId = TarantoolContext.Instance.UintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var id = TarantoolContext.Instance.UintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var name = TarantoolContext.Instance.StringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var type = TarantoolContext.Instance.IndexTypeConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var options = TarantoolContext.Instance.IndexOptionsConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var indexParts = TarantoolContext.Instance.IndexPartsConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();

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
