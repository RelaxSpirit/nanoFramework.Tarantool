// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> eval packet converter class.
    /// </summary>
    internal class EvalPacketConverter : IConverter
    {
        public static void Write(EvalRequest value, IMessagePackWriter writer)
        {
            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var stringConverter = ConverterContext.GetConverter(typeof(string));
            var tupleConverter = TarantoolContext.Instance.GetTarantoolTupleConverter(value.Tuple);

            writer.WriteMapHeader(2);

            keyConverter.Write(Key.Expression, writer);
            stringConverter.Write(value.Expression, writer);

            keyConverter.Write(Key.Tuple, writer);
            tupleConverter.Write(value.Tuple, writer);
        }

#nullable enable
        public virtual object? Read([NotNull] IMessagePackReader reader)
        {
            throw new NotImplementedException();
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((EvalRequest)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
