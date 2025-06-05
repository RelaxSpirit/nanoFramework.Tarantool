// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Headers;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> request header converter class.
    /// </summary>
    internal class RequestHeaderConverter : IConverter
    {
        private static void Write(RequestHeader value, IMessagePackWriter writer)
        {
            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var requestIdConverter = ConverterContext.GetConverter(typeof(RequestId));
            var codeConverter = ConverterContext.GetConverter(typeof(uint));

            writer.WriteMapHeader(2u);

            keyConverter.Write(Key.Code, writer);
            codeConverter.Write(value.Code, writer);

            keyConverter.Write(Key.Sync, writer);
            requestIdConverter.Write(value.RequestId, writer);
        }

#nullable enable
        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((RequestHeader)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public virtual object? Read([NotNull] IMessagePackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
