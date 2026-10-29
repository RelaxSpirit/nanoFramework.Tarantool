// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> insert replace converter class.
    /// </summary>
    internal class InsertReplacePacketConverter : IConverter
    {
        public static void Write(InsertReplaceRequest value, IMessagePackWriter writer)
        {
            writer.WriteMapHeader(2u);

            TarantoolContext.Instance.KeyConverter.Write(Key.SpaceId, writer);
            TarantoolContext.Instance.UintConverter.Write(value.SpaceId, writer);

            TarantoolContext.Instance.KeyConverter.Write(Key.Tuple, writer);
            TarantoolContext.Instance.TarantoolTupleConverter.Write(value.Tuple, writer);
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
                Write((InsertReplaceRequest)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
