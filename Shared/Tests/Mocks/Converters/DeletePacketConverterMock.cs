// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class DeletePacketConverterMock : DeletePacketConverter
    {
#nullable enable
        public override object? Read([NotNull] IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length != 3)
            {
                throw ExceptionHelper.InvalidMapLength(length, 3);
            }

            var uintConverter = TarantoolContext.Instance.UintConverter;
            var keyConverter = uintConverter;

            uint spaceId = uint.MaxValue;
            uint indexId = uint.MaxValue;
            TarantoolTuple? keyTuple = null;
            for (var i = 0; i < length; i++)
            {
                Key key = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

                switch (key)
                {
                    case Key.SpaceId:
                        spaceId = (uint)(uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Key:
                        keyTuple = (TarantoolTuple)(TarantoolMockContext.Instanse.TupleConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.IndexId:
                        indexId = (uint)(uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                }
            }

            if (spaceId != uint.MaxValue && keyTuple != null && indexId != uint.MaxValue)
            {
                return new DeleteRequest(spaceId, indexId, keyTuple);
            }
            else
            {
                return null;
            }
        }
    }
}
