// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> empty response converter class.
    /// </summary>
    internal class EmptyResponseConverter : IConverter
    {
        public static EmptyResponse Read(IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length > 1)
            {
                throw ExceptionHelper.InvalidMapLength(length, 0, 1);
            }

            if (length == 1)
            {
                var keyConverter = ConverterContext.GetConverter(typeof(uint));
                var intKey = keyConverter.Read(reader);

                if (intKey != null)
                {
                    Key dataKey = (Key)intKey;

                    if (dataKey != Key.Data)
                    {
                        throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data);
                    }

                    var arrayLength = reader.ReadArrayLength();
                    if (arrayLength != 0)
                    {
                        throw ExceptionHelper.InvalidArrayLength(0, length);
                    }
                }
                else
                {
                    throw ExceptionHelper.ActualValueIsNullReference();
                }
            }

            return new EmptyResponse();
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
