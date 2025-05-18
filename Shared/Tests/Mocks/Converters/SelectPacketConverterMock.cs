// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class SelectPacketConverterMock : SelectPacketConverter
    {
#nullable enable
        public override object? Read([NotNull] IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length != 6)
            {
                throw ExceptionHelper.InvalidMapLength(length, 2);
            }

            var uintConverter = ConverterContext.GetConverter(typeof(uint));
            var keyConverter = uintConverter;
            var iteratorConverter = uintConverter;

            uint spaceId = 0;
            uint indexId = 0;
            uint limit = 0;
            uint offset = 0;
            Iterator iterator = Iterator._;
            TarantoolTuple? selectKey = null;

            for (var i = 0; i < length; i++)
            {
                Key key = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

                switch (key)
                {
                    case Key.SpaceId:
                        spaceId = (uint)(uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.IndexId:
                        indexId = (uint)(uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Limit:
                        limit = (uint)(uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Offset:
                        offset = (uint)(uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Iterator:
                        iterator = (Iterator)(iteratorConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Key:
                        var selectKeyConverter = ConverterContext.GetConverter(typeof(TarantoolTuple));
                        selectKey = (TarantoolTuple)(selectKeyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                }
            }

            return new SelectRequest(spaceId, indexId, limit, offset, iterator, selectKey);
        }
    }
}
