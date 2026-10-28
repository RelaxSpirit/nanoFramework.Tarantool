// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;

namespace nanoFramework.Tarantool.Converters
{
    internal class KeyConverter : IConverter
    {
        private static void Write(uint value, IMessagePackWriter writer)
        {
            TarantoolContext.Instance.UintConverter.Write(value, writer);
        }

#nullable enable
        public object? Read([NotNull] IMessagePackReader reader)
        {
            return (uint)(TarantoolContext.Instance.UintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value == null)
            {
                throw new System.ArgumentNullException();
            }

            Write((uint)value, writer);
        }
    }
}
