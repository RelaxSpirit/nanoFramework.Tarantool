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
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// Storage engine converter class.
    /// </summary>
    internal class StorageEngineConverter : IConverter
    {
        private static StorageEngine Read(IMessagePackReader reader)
        {
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            var enumString = (string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

            switch (enumString)
            {
                case "service":
                    return StorageEngine.Service;
                case "memtx":
                    return StorageEngine.Memtx;
                case "sophia":
                    return StorageEngine.Sophia;
                case "sysview":
                    return StorageEngine.Sysview;
                case "blackhole":
                    return StorageEngine.Blackhole;
                default:
                    throw ExceptionHelper.UnexpectedEnumUnderlyingType(typeof(FieldType), enumString);
            }
        }

        internal static void Write(StorageEngine value, [NotNull] IMessagePackWriter writer)
        {
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            switch (value)
            {
                case StorageEngine.Service:
                    stringConverter.Write("service", writer);
                    break;
                case StorageEngine.Memtx:
                    stringConverter.Write("memtx", writer);
                    break;
                case StorageEngine.Sophia:
                    stringConverter.Write("sophia", writer);
                    break;
                case StorageEngine.Sysview:
                    stringConverter.Write("sysview", writer);
                    break;
                case StorageEngine.Blackhole:
                    stringConverter.Write("blackhole", writer);
                    break;
                default:
                    throw ExceptionHelper.EnumValueExpected(value.GetType(), value);
            }
        }

#nullable enable
        object? IConverter.Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader);
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((StorageEngine)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
