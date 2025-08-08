// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> error response packet converter class.
    /// </summary>
    internal class ErrorResponsePacketConverter : IConverter
    {
        public static void Write(ErrorResponse value, IMessagePackWriter writer)
        {
            writer.WriteMapHeader(1u);
            TarantoolContext.Instance.UintConverter.Write(Key.Error24, writer);
            TarantoolContext.Instance.StringConverter.Write(value.ErrorMessage, writer);
        }

        public static ErrorResponse Read(IMessagePackReader reader)
        {
            string errorMessage = string.Empty;
            var length = reader.ReadMapLength();

            for (var i = 0; i < length; i++)
            {
                var errorKey = (Key)(TarantoolContext.Instance.UintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

                switch (errorKey)
                {
                    case Key.Error24:
                        errorMessage = (string)(TarantoolContext.Instance.StringConverter.Read(reader) ?? string.Empty);
                        break;
                    case Key.Error:
                        // TODO: add parsing of new error metadata
                        reader.SkipToken();
                        break;
                    default:
                        reader.SkipToken();
                        break;
                }
            }

            return new ErrorResponse(errorMessage);
        }

#nullable enable
        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((ErrorResponse)value, writer);
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
