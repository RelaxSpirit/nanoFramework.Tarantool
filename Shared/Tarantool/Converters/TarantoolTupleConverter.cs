﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> tuple converter.
    /// </summary>
    internal class TarantoolTupleConverter : IConverter
    {
        private readonly Type[] _tupleItemTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolTupleConverter"/> class.
        /// </summary>
        internal TarantoolTupleConverter()
        {
#pragma warning disable IDE0300 // Simplify collection initialization
            _tupleItemTypes = new Type[0];
#pragma warning restore IDE0300 // Simplify collection initialization
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolTupleConverter"/> class.
        /// </summary>
        /// <param name="tupleItemTypes"><see cref="Tarantool"/> tuple items types.</param>
        internal TarantoolTupleConverter(params Type[] tupleItemTypes)
        {
            _tupleItemTypes = tupleItemTypes;
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

        private void Write(TarantoolTuple? value, IMessagePackWriter writer)
        {
            if (value == null)
            {
                ConverterContext.NullConverter.Write(null, writer);
                return;
            }

            writer.WriteArrayHeader((uint)value.Length);

            if (value.Length == _tupleItemTypes.Length)
            {
                for (int i = 0; i < _tupleItemTypes.Length; i++)
                {
                    var converter = ConverterContext.GetConverter(_tupleItemTypes[i]);

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
            else
            {
                for (int i = 0; i < value.Length; i++)
                {
                    WriteItem(value[i], writer);
                }
            }
        }

        public TarantoolTuple? Read(IMessagePackReader reader)
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

                if (_tupleItemTypes.Length > 0)
                {
                    if (actual != _tupleItemTypes.Length)
                    {
                        throw ExceptionHelper.InvalidArrayLength((uint)_tupleItemTypes.Length, actual);
                    }

                    tupleObjects = new object[actual];

                    for (int i = 0; i < actual; i++)
                    {
                        var converter = ConverterContext.GetConverter(_tupleItemTypes[i]);

                        if (converter != null)
                        {
                            tupleObjects[i] = converter.Read(messagePackToken) ?? throw ExceptionHelper.ActualValueIsNullReference();
                        }
                        else
                        {
                            var arraySegment = messagePackToken.ReadToken() ?? throw ExceptionHelper.ActualValueIsNullReference();
                            tupleObjects[i] = MessagePackSerializer.Deserialize(_tupleItemTypes[i], (byte[])arraySegment);
                        }
                    }
                }
                else
                {
                    messagePackToken.Seek(0, SeekOrigin.Begin);

                    var arrayListConverter = ConverterContext.GetConverter(typeof(ArrayList));

                    var arrayList = (ArrayList)(arrayListConverter.Read(messagePackToken) ?? throw ExceptionHelper.ActualValueIsNullReference());

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
