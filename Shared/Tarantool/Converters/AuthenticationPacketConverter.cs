// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
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
        private static void Write(AuthenticationRequest? value, IMessagePackWriter writer)
        {
            if (value == null)
            {
                ConverterContext.NullConverter.Write(null, writer);
                return;
            }

            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var bytesConverter = ConverterContext.GetConverter(typeof(byte[]));
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            writer.WriteMapHeader(2);

            keyConverter.Write(Key.Username, writer);
            stringConverter.Write(value.Username, writer);

            keyConverter.Write(Key.Tuple, writer);

            writer.WriteArrayHeader(2);
            stringConverter.Write("chap-sha1", writer);
            bytesConverter.Write(value.Scramble, writer);
        }

        public virtual object? Read([NotNull] IMessagePackReader reader)
        {
            throw new NotImplementedException();
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            Write((AuthenticationRequest?)value, writer);
        }
    }
}
