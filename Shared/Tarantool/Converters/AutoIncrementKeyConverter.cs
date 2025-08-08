// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using static nanoFramework.Tarantool.Model.Requests.InsertRequest;

namespace nanoFramework.Tarantool.Converters
{
    internal class AutoIncrementKeyConverter : IConverter
    {
#nullable enable
        public object? Read([NotNull] IMessagePackReader reader)
        {
            return (ulong)(TarantoolContext.Instance.UlongConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is AutoIncrementKey)
            {
                ConverterContext.NullConverter.Write(value, writer);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
