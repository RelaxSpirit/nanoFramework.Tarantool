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
        private readonly TarantoolTupleType _tupleType;

        internal TarantoolTupleArrayConverter(TarantoolTupleType tupleType)
        {
            _tupleType = tupleType;
        }

#nullable enable
        public object? Read([NotNull] IMessagePackReader reader)
        {
            TarantoolTupleConverter tupleConverter = (TarantoolTupleConverter)TarantoolContext.Instance.GetTarantoolTupleConverter(_tupleType);
            var length = reader.ReadArrayLength();
            TarantoolTuple[] tarantoolTuples = new TarantoolTuple[length];

            for (int i = 0; i < length; i++)
            {
                tarantoolTuples[i] = (TarantoolTuple)(tupleConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
            }

            return tarantoolTuples;
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is TarantoolTuple[] tuples)
            {
                writer.WriteArrayHeader((uint)tuples.Length);

                foreach (TarantoolTuple tuple in tuples)
                {
                    TarantoolContext.Instance.GetTarantoolTupleConverter(tuple).Write(tuple, writer);
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
