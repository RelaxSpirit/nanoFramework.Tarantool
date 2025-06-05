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
    /// The update packet converter class.
    /// </summary>
    internal class UpdatePacketConverter : IConverter
    {
        private static void Write(UpdateRequest value, IMessagePackWriter writer)
        {
            writer.WriteMapHeader(4);

            var uintConverter = ConverterContext.GetConverter(typeof(uint));
            var keyConverter = uintConverter;

            keyConverter.Write(Key.SpaceId, writer);
            uintConverter.Write(value.SpaceId, writer);

            keyConverter.Write(Key.IndexId, writer);
            uintConverter.Write(value.IndexId, writer);

            keyConverter.Write(Key.Key, writer);
            TarantoolContext.Instance.GetTarantoolTupleConverter(value.Key).Write(value.Key, writer);

            keyConverter.Write(Key.Tuple, writer);
            writer.WriteArrayHeader((uint)value.UpdateOperations.Length);

            foreach (var updateOperation in value.UpdateOperations)
            {
                ConverterContext.GetConverter(updateOperation.GetType()).Write(updateOperation, writer);
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
                Write((UpdateRequest)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
