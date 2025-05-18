// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Headers;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> response header converter class.
    /// </summary>
    internal class ResponseHeaderConverter : IConverter
    {
        private static void Write(ResponseHeader value, IMessagePackWriter writer)
        {
            writer.WriteMapHeader(3u);
            var uintConverter = ConverterContext.GetConverter(typeof(uint));
            uintConverter.Write(Key.Code, writer);
            uintConverter.Write((uint)value.Code, writer);
            uintConverter.Write(Key.Sync, writer);
            ConverterContext.GetConverter(typeof(RequestId)).Write(value.RequestId, writer);
            uintConverter.Write(Key.SchemaId, writer);
            ConverterContext.GetConverter(typeof(ulong)).Write(value.SchemaId, writer);
        }

#nullable enable
        internal static ResponseHeader Read(IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length > 3u && length < 2u)
            {
                throw ExceptionHelper.InvalidMapLength(length, 2u, 3u);
            }

            CommandCode code = CommandCode._;
            RequestId? sync = null;
            ulong schemaId = 0;

            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var ulongConverter = ConverterContext.GetConverter(typeof(ulong));
            var codeConverter = keyConverter;
            var requestIdConverter = ConverterContext.GetConverter(typeof(RequestId));

            for (int i = 0; i < length; i++)
            {
                var key = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

                switch (key)
                {
                    case Key.Code:
                        code = (CommandCode)(codeConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Sync:
                        sync = (RequestId?)requestIdConverter.Read(reader);
                        break;
                    case Key.SchemaId:
                        schemaId = (ulong)(ulongConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
            break;
                    default:
                        reader.SkipToken();
                        break;
                }
            }

            if (code == CommandCode._)
            {
                throw ExceptionHelper.PropertyUnspecified("Code");
            }

            if (sync == null)
            {
                throw ExceptionHelper.PropertyUnspecified("Sync");
            }

            return new ResponseHeader(code, sync, schemaId);
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((ResponseHeader)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        object? IConverter.Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader);
        }
    }
}
