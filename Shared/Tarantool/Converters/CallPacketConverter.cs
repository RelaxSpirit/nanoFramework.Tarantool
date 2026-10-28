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
    /// The <see cref="Tarantool"/> call packet converter class.
    /// </summary>
    internal class CallPacketConverter : IConverter
    {
#nullable enable
        public static void Write(CallRequest value, [NotNull] IMessagePackWriter writer)
        {
            writer.WriteMapHeader(2u);

            TarantoolContext.Instance.KeyConverter.Write(Key.FunctionName, writer);
            TarantoolContext.Instance.StringConverter.Write(value.FunctionName, writer);

            TarantoolContext.Instance.KeyConverter.Write(Key.Tuple, writer);
            TarantoolContext.Instance.TarantoolTupleConverter.Write(value.Tuple, writer);
        }

        public virtual object? Read([NotNull] IMessagePackReader reader)
        {
            throw new NotImplementedException();
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            Write((CallRequest)(value ?? throw new ArgumentException()), writer);
        }
    }
}
