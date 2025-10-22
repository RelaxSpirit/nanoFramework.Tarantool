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
    /// The <see cref="Tarantool"/> authentication packet converter class.
    /// </summary>
    internal class AuthenticationPacketConverter : IConverter
    {
#nullable enable
        private static void Write(AuthenticationRequest value, IMessagePackWriter writer)
        {
            writer.WriteMapHeader(2);

            TarantoolContext.Instance.UintConverter.Write(Key.Username, writer);
            TarantoolContext.Instance.StringConverter.Write(value.Username, writer);

            TarantoolContext.Instance.UintConverter.Write(Key.Tuple, writer);

            writer.WriteArrayHeader(2);
            TarantoolContext.Instance.StringConverter.Write("chap-sha1", writer);
            TarantoolContext.Instance.BytesConverter.Write(value.Scramble, writer);
        }

        public virtual object? Read([NotNull] IMessagePackReader reader)
        {
            throw new NotImplementedException();
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value == null)
            {
                ConverterContext.NullConverter.Write(null, writer);
            }
            else
            {
                Write((AuthenticationRequest)value, writer);
            }
        }
    }
}
