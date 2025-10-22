// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
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

            TarantoolContext.Instance.UintConverter.Write(Key.Code, writer);
            TarantoolContext.Instance.UintConverter.Write((uint)value.Code, writer);
            TarantoolContext.Instance.UintConverter.Write(Key.Sync, writer);
            TarantoolContext.Instance.RequestIdConverter.Write(value.RequestId, writer);
            TarantoolContext.Instance.UintConverter.Write(Key.SchemaId, writer);
            TarantoolContext.Instance.UlongConverter.Write(value.SchemaId, writer);
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
            RequestId sync = RequestId.Empty;
            ulong schemaId = 0;

            for (int i = 0; i < length; i++)
            {
                var key = (Key)(TarantoolContext.Instance.UintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

                switch (key)
                {
                    case Key.Code:
                        code = (CommandCode)(TarantoolContext.Instance.UintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Sync:
                        sync = (RequestId)(TarantoolContext.Instance.RequestIdConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.SchemaId:
                        schemaId = (ulong)(TarantoolContext.Instance.UlongConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
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

            if (sync == RequestId.Empty)
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
