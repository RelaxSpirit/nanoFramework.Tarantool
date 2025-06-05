// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> index creation options converter class.
    /// </summary>
    internal class IndexCreationOptionsConverter : IConverter
    {
        public static IndexCreationOptions Read(IMessagePackReader reader)
        {
            var optionsCount = reader.ReadMapLength();
            var stringConverter = ConverterContext.GetConverter(typeof(string));
            var boolConverter = ConverterContext.GetConverter(typeof(bool));

            var unique = false;
            for (int i = 0; i < optionsCount; i++)
            {
                var key = stringConverter.Read(reader);
                switch (key)
                {
                    case "unique":
                        unique = (bool)(boolConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    default:
                        reader.SkipToken();
                        break;
                }
            }

            return new IndexCreationOptions(unique);
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
