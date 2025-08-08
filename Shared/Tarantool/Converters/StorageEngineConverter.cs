// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
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
            var enumString = (string)(TarantoolContext.Instance.StringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

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
                case "vinyl":
                    return StorageEngine.Vinyl;
                default:
                    throw ExceptionHelper.UnexpectedEnumUnderlyingType(typeof(FieldType), enumString);
            }
        }

        internal static void Write(StorageEngine value, [NotNull] IMessagePackWriter writer)
        {
            switch (value)
            {
                case StorageEngine.Service:
                    TarantoolContext.Instance.StringConverter.Write("service", writer);
                    break;
                case StorageEngine.Memtx:
                    TarantoolContext.Instance.StringConverter.Write("memtx", writer);
                    break;
                case StorageEngine.Sophia:
                    TarantoolContext.Instance.StringConverter.Write("sophia", writer);
                    break;
                case StorageEngine.Sysview:
                    TarantoolContext.Instance.StringConverter.Write("sysview", writer);
                    break;
                case StorageEngine.Blackhole:
                    TarantoolContext.Instance.StringConverter.Write("blackhole", writer);
                    break;
                case StorageEngine.Vinyl:
                    TarantoolContext.Instance.StringConverter.Write("vinyl", writer);
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
