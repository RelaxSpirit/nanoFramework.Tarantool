// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Headers;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class RequestHeaderConverterMock : RequestHeaderConverter
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
            var requestIdConverter = ConverterContext.GetConverter(typeof(RequestId));
            var codeConverter = ConverterContext.GetConverter(typeof(uint));

            CommandCode commandCode = CommandCode._;
            RequestId? requestId = null;

            for (var i = 0; i < length; i++)
            {
                Key key = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                switch (key)
                {
                    case Key.Code:
                        commandCode = (CommandCode)(codeConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Sync:
                        requestId = (RequestId)(requestIdConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                }
            }

            if (commandCode != CommandCode._ && requestId != null)
            {
                return new RequestHeader(commandCode, requestId);
            }
            else
            {
                return null;
            }
        }
    }
}
