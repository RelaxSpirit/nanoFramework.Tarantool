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
    /// The <see cref="Tarantool"/> delete packet converter class.
    /// </summary>
    internal class DeletePacketConverter : IConverter
    {
#nullable enable
        public static void Write(DeleteRequest? value, IMessagePackWriter writer)
        {
            if (value != null)
            {
                var selectKeyConverter = TarantoolContext.Instance.GetTarantoolTupleConverter(value.Key);
                writer.WriteMapHeader(3);

                TarantoolContext.Instance.UintConverter.Write(Key.SpaceId, writer);
                TarantoolContext.Instance.UintConverter.Write(value.SpaceId, writer);

                TarantoolContext.Instance.UintConverter.Write(Key.IndexId, writer);
                TarantoolContext.Instance.UintConverter.Write(value.IndexId, writer);

                TarantoolContext.Instance.UintConverter.Write(Key.Key, writer);
                selectKeyConverter.Write(value.Key, writer);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public virtual object? Read([NotNull] IMessagePackReader reader)
        {
            throw new NotImplementedException();
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            Write((DeleteRequest?)value, writer);
        }
    }
}
