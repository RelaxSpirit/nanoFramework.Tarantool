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
    /// The <see cref="Tarantool"/> delete packet converter class.
    /// </summary>
    internal class DeletePacketConverter : IConverter
    {
#nullable enable
        public static void Write(DeleteRequest? value, IMessagePackWriter writer)
        {
            if (value != null)
            {
                var uintConverter = ConverterContext.GetConverter(typeof(uint));
                var keyConverter = uintConverter;
                var selectKeyConverter = ConverterContext.GetConverter(value.Key.GetType());
                writer.WriteMapHeader(3);

                keyConverter.Write(Key.SpaceId, writer);
                uintConverter.Write(value.SpaceId, writer);

                keyConverter.Write(Key.IndexId, writer);
                uintConverter.Write(value.IndexId, writer);

                keyConverter.Write(Key.Key, writer);
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
