// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> select packet converter class.
    /// </summary>
    internal class SelectPacketConverter : IConverter
    {
        public static void Write(SelectRequest value, IMessagePackWriter writer)
        {
            writer.WriteMapHeader(6);

            var uintConverter = ConverterContext.GetConverter(typeof(uint));
            var keyConverter = uintConverter;
            var iteratorConverter = uintConverter;
            
            keyConverter.Write(Key.SpaceId, writer);
            uintConverter.Write(value.SpaceId, writer);

            keyConverter.Write(Key.IndexId, writer);
            uintConverter.Write(value.IndexId, writer);

            keyConverter.Write(Key.Limit, writer);
            uintConverter.Write(value.Limit, writer);

            keyConverter.Write(Key.Offset, writer);
            uintConverter.Write(value.Offset, writer);

            keyConverter.Write(Key.Iterator, writer);
            iteratorConverter.Write(value.Iterator, writer);

            keyConverter.Write(Key.Key, writer);

            if (value.SelectKey != null)
            {
                var selectKeyConverter = TarantoolContext.Instance.GetTarantoolTupleConverter(value.SelectKey);
                selectKeyConverter.Write(value.SelectKey, writer);
            }
            else
            {
                var selectKeyConverter = TarantoolContext.Instance.GetTarantoolTupleConverter(TarantoolTuple.Empty);
                selectKeyConverter.Write(TarantoolTuple.Empty, writer);
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
                Write((SelectRequest)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
