// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> tuple converter.
    /// </summary>
    internal class TarantoolTupleConverter : IConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolTupleConverter"/> class.
        /// </summary>
        internal TarantoolTupleConverter()
        {
        }

#nullable enable
        private static void WriteItem(object? item, IMessagePackWriter writer)
        {
            if (item != null)
            {
                writer.Write(MessagePackSerializer.Serialize(item));
            }
            else
            {
                ConverterContext.NullConverter.Write(item, writer);
            }
        }

        private static void Write(TarantoolTuple? value, IMessagePackWriter writer)
        {
            if (value == null)
            {
                ConverterContext.NullConverter.Write(null, writer);
                return;
            }

            writer.WriteArrayHeader((uint)value.Length);

            Type[] tupleItemTypes = value.GetItemTypes();

            for (int i = 0; i < value.Length; i++)
            {
                var converter = ConverterContext.GetConverter(tupleItemTypes[i]);

                if (converter != null)
                {
                    converter.Write(value[i], writer);
                }
                else
                {
                    WriteItem(value[i], writer);
                }
            }
        }

        internal static TarantoolTuple? Read(IMessagePackReader reader, TarantoolTupleType? tarantoolTupleType = null)
        {
            var messagePackToken = reader.ReadToken() ?? throw ExceptionHelper.ActualValueIsNullReference();

            var actual = messagePackToken.ReadArrayLength();

            if (actual == uint.MaxValue)
            {
                return null;
            }

            if (((int)actual) < 1)
            {
                return TarantoolTuple.Empty;
            }
            else
            {
                object?[] tupleObjects;

                if (tarantoolTupleType != null && tarantoolTupleType.TupleTypes.Length > 0)
                {
                    if (actual != tarantoolTupleType.TupleTypes.Length)
                    {
                        throw ExceptionHelper.InvalidArrayLength((uint)tarantoolTupleType.TupleTypes.Length, actual);
                    }

                    tupleObjects = new object[actual];

                    for (int i = 0; i < actual; i++)
                    {
                        var converter = ConverterContext.GetConverter(tarantoolTupleType.TupleTypes[i]);

                        if (converter != null)
                        {
                            tupleObjects[i] = converter.Read(messagePackToken) ?? throw ExceptionHelper.ActualValueIsNullReference();
                        }
                        else
                        {
                            var arraySegment = messagePackToken.ReadToken() ?? throw ExceptionHelper.ActualValueIsNullReference();
                            tupleObjects[i] = MessagePackSerializer.Deserialize(tarantoolTupleType.TupleTypes[i], (byte[])arraySegment);
                        }
                    }
                }
                else
                {
                    messagePackToken.Seek(0, SeekOrigin.Begin);

                    var arrayList = (ArrayList)(TarantoolContext.Instance.ArrayListConverter.Read(messagePackToken) ?? throw ExceptionHelper.ActualValueIsNullReference());

                    tupleObjects = arrayList.ToArray();
                }

                return TarantoolTuple.Create(tupleObjects);
            }
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            Write((TarantoolTuple?)value, writer);
        }

        object? IConverter.Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader);
        }
    }
}
