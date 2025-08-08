// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
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

            string? name = null;
            var type = FieldType._;

            for (int i = 0; i < dictLength; i++)
            {
                var key = (string)(TarantoolContext.Instance.StringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                switch (key)
                {
                    case "name":
                        name = (string?)TarantoolContext.Instance.StringConverter.Read(reader);
                        break;
                    case "type":
                        type = (FieldType)(TarantoolContext.Instance.FieldTypeConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
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
