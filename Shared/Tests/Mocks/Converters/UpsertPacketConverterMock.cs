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
using nanoFramework.Tarantool.Model.UpdateOperations;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class UpsertPacketConverterMock : UpsertPacketConverter
    {
#nullable enable
        public override object? Read([NotNull] IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length != 3 && length != 4)
            {
                throw ExceptionHelper.InvalidMapLength(length, 4);
            }

            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var tupleConverter = ConverterContext.GetConverter(typeof(TarantoolTuple));

            uint spaceId = uint.MaxValue;
            TarantoolTuple? tuple = null;
            UpdateOperation[] updateOperations = new UpdateOperation[0];
            for (var i = 0; i < length; i++)
            {
                Key key = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

                switch (key)
                {
                    case Key.SpaceId:
                        spaceId = (uint)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Tuple:
                        tuple = (TarantoolTuple)(tupleConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Ops:
                        updateOperations = UpdatePacketConverterMock.GetUpdateOperations(reader);
                        break;
                    default:
                        reader.SkipToken();
                        break;
                }
            }

            if (tuple != null && updateOperations.Length > 0 && spaceId != uint.MaxValue)
            {
                return new UpsertRequest(spaceId, tuple, updateOperations);
            }
            else
            {
                return null;
            }
        }
    }
}
