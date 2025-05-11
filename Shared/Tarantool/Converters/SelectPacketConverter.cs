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

            if (value.SelectKey != null)
            {
                var selectKeyConverter = TarantoolContext.GetTarantoolTupleConverter(value.SelectKey);

                keyConverter.Write(Key.Key, writer);
                selectKeyConverter.Write(value.SelectKey, writer);
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
