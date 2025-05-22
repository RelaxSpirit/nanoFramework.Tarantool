// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Dto;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;
using nanoFramework.Tarantool.Model.UpdateOperations;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
    internal class UpdatePacketConverterMock : UpdatePacketConverter
    {
#nullable enable
        public override object? Read([NotNull] IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length != 4)
            {
                throw ExceptionHelper.InvalidMapLength(length, 4);
            }

            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var tupleConverter = ConverterContext.GetConverter(typeof(TarantoolTuple));

            uint spaceId = uint.MaxValue;
            uint indexId = uint.MaxValue;
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
                    case Key.IndexId:
                        indexId = (uint)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Key:
                        tuple = (TarantoolTuple)(tupleConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Tuple:
                        var updateOperationsCount = reader.ReadArrayLength();
                        updateOperations = new UpdateOperation[updateOperationsCount];
                        for (int opIndex = 0; opIndex < updateOperationsCount; opIndex++)
                        {
                            ArraySegment arraySegment = reader.ReadToken() ?? throw ExceptionHelper.ActualValueIsNullReference();

                            var tupleItemsCount = arraySegment.ReadArrayLength();
                            if (tupleItemsCount != 3)
                            {
                                throw ExceptionHelper.InvalidArrayLength(length, 3);
                            }

                            updateOperations[opIndex] = new UpdateOperation(
                            (string)(ConverterContext.GetConverter(typeof(string)).Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()),
                            (int)(ConverterContext.GetConverter(typeof(int)).Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()),
                            ExecuteSqlRequestConverterMock.GetObjectByDataType(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        }

                        break;
                }
            }

            if (tuple != null && updateOperations.Length > 0 && spaceId != uint.MaxValue && indexId != uint.MaxValue)
            {
                return new UpdateRequest(spaceId, indexId, tuple, updateOperations);
            }
            else
            {
                return null;
            }
        }
    }
}
