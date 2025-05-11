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
    /// The <see cref="Tarantool"/> insert replace converter class.
    /// </summary>
    internal class InsertReplacePacketConverter : IConverter
    {
        public static void Write(InsertReplaceRequest value, IMessagePackWriter writer)
        {
            writer.WriteMapHeader(2);

            var uintConverter = ConverterContext.GetConverter(typeof(uint));
            var keyConverter = uintConverter;
            var tupleConverter = TarantoolContext.GetTarantoolTupleConverter(value.Tuple);

            keyConverter.Write(Key.SpaceId, writer);
            uintConverter.Write(value.SpaceId, writer);

            keyConverter.Write(Key.Tuple, writer);
            tupleConverter.Write(value.Tuple, writer);
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
