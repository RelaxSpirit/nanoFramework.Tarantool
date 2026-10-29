// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Exceptions;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The upsert packet converter class.
    /// </summary>
    internal class UpsertPacketConverter : IConverter
    {
        private static void Write(UpsertRequest value, IMessagePackWriter writer)
        {
            writer.WriteMapHeader(3u);

            TarantoolContext.Instance.KeyConverter.Write(Key.SpaceId, writer);
            TarantoolContext.Instance.UintConverter.Write(value.SpaceId, writer);

            TarantoolContext.Instance.KeyConverter.Write(Key.Tuple, writer);
            TarantoolContext.Instance.TarantoolTupleConverter.Write(value.Tuple, writer);

            TarantoolContext.Instance.KeyConverter.Write(Key.Ops, writer);
            writer.WriteArrayHeader((uint)value.UpdateOperations.Length);

            foreach (var updateOperation in value.UpdateOperations)
            {
                (ConverterContext.GetConverter(updateOperation.GetType()) ?? throw new ConverterNotFoundException(updateOperation.GetType())).Write(updateOperation, writer);
            }
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
                Write((UpsertRequest)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
