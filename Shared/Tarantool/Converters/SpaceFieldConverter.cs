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

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The space field converter class.
    /// </summary>
#nullable enable
    internal class SpaceFieldConverter : IConverter
    {
        private static SpaceField Read(IMessagePackReader reader)
        {
            var dictLength = reader.ReadMapLength();

            var stringConverter = ConverterContext.GetConverter(typeof(string));
            var typeConverter = ConverterContext.GetConverter(typeof(FieldType));
            string? name = null;
            var type = FieldType._;

            for (int i = 0; i < dictLength; i++)
            {
                var key = (string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                switch (key)
                {
                    case "name":
                        name = (string?)stringConverter.Read(reader);
                        break;
                    case "type":
                        type = (FieldType)(typeConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    default:
                        reader.SkipToken();
                        break;
                }
            }

            if (name == null)
            {
                throw ExceptionHelper.ActualValueIsNullReference();
            }

            return new SpaceField(name, type);
        }

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
