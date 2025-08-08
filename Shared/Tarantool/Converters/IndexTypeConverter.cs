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
    /// The <see cref="Tarantool"/> index type converter class.
    /// </summary>
    internal class IndexTypeConverter : IConverter
    {
#nullable enable
        private static IndexType Read(IMessagePackReader reader)
        {
            var enumString = (string)(TarantoolContext.Instance.StringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

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
            switch (value)
            {
                case IndexType.Bitset:
                    TarantoolContext.Instance.StringConverter.Write("bitset", writer);
                    break;
                case IndexType.RTree:
                    TarantoolContext.Instance.StringConverter.Write("rtree", writer);
                    break;
                case IndexType.Hash:
                    TarantoolContext.Instance.StringConverter.Write("hash", writer);
                    break;
                case IndexType.Tree:
                    TarantoolContext.Instance.StringConverter.Write("tree", writer);
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
