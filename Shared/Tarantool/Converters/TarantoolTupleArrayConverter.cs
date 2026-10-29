// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Converters
{
    internal class TarantoolTupleArrayConverter : IConverter
    {
#nullable enable
        internal static TarantoolTuple[] Read(IMessagePackReader reader, TarantoolTupleArrayType? tarantoolTupleArrayType = null)
        {
            var length = reader.ReadArrayLength();
            TarantoolTuple[] tarantoolTuples = new TarantoolTuple[length];

            for (int i = 0; i < length; i++)
            {
                tarantoolTuples[i] = TarantoolTupleConverter.Read(reader, (TarantoolTupleType?)tarantoolTupleArrayType?.GetElementType()) ?? throw ExceptionHelper.ActualValueIsNullReference();
            }

            return tarantoolTuples;
        }

        public object? Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader, null);
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is TarantoolTuple[] tuples)
            {
                writer.WriteArrayHeader((uint)tuples.Length);

                foreach (TarantoolTuple tuple in tuples)
                {
                    TarantoolContext.Instance.TarantoolTupleConverter.Write(tuple, writer);
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
