// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class AuthenticationPacketConverterMock : AuthenticationPacketConverter
    {
#nullable enable
        public override object? Read([NotNull] IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length != 2)
            {
                throw ExceptionHelper.InvalidMapLength(length, 2);
            }

            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var bytesConverter = ConverterContext.GetConverter(typeof(byte[]));
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            string? userName = null;
            byte[] scramble = new byte[0];
            for (var i = 0; i < length; i++)
            {
                Key key = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

                switch (key)
                {
                    case Key.Username:
                        userName = (string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Tuple:
                        var arrayLength = reader.ReadArrayLength();
                        if (arrayLength != 2)
                        {
                            throw ExceptionHelper.InvalidArrayLength(length, 2);
                        }

                        stringConverter.Read(reader);
                        scramble = (byte[])(bytesConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                }
            }

            if (userName != null && scramble.Length > 0)
            {
                return new AuthenticationRequest(userName, scramble);
            }
            else
            {
                return null;
            }
        }
    }
}
