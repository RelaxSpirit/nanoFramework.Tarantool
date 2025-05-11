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
    /// The <see cref="Tarantool"/> index type converter class.
    /// </summary>
    internal class IndexTypeConverter : IConverter
    {
#nullable enable
        private static IndexType Read(IMessagePackReader reader)
        {
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            var enumString = (string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

            switch (enumString)
            {
                case "bitset":
                    return IndexType.Bitset;
                case "rtree":
                    return IndexType.RTree;
                case "hash":
                    return IndexType.Hash;
                case "tree":
                    return IndexType.Tree;
                default:
                    throw ExceptionHelper.UnexpectedEnumUnderlyingType(typeof(FieldType), enumString);
            }
        }

        private static void Write(IndexType value, [NotNull] IMessagePackWriter writer)
        {
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            switch (value)
            {
                case IndexType.Bitset:
                    stringConverter.Write("bitset", writer);
                    break;
                case IndexType.RTree:
                    stringConverter.Write("rtree", writer);
                    break;
                case IndexType.Hash:
                    stringConverter.Write("hash", writer);
                    break;
                case IndexType.Tree:
                    stringConverter.Write("tree", writer);
                    break;
                default:
                    throw ExceptionHelper.EnumValueExpected(value.GetType(), value);
            }
        }

        object? IConverter.Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader);
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((IndexType)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
