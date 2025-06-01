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
        internal static UpdateOperation[] GetUpdateOperations(IMessagePackReader reader)
        {
            var updateOperationsCount = reader.ReadArrayLength();
            UpdateOperation[] updateOperations = new UpdateOperation[updateOperationsCount];
            var stringConverter = ConverterContext.GetConverter(typeof(string));
            var intConverter = ConverterContext.GetConverter(typeof(int));
            for (int opIndex = 0; opIndex < updateOperationsCount; opIndex++)
            {
                ArraySegment arraySegment = reader.ReadToken() ?? throw ExceptionHelper.ActualValueIsNullReference();

                var tupleItemsCount = arraySegment.ReadArrayLength();
                if (tupleItemsCount == 2)
                {
                    updateOperations[opIndex] = new UpdateOperation(
                        (string)(stringConverter.Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()),
                        (int)(intConverter.Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()),
                        null);
                }
                else
                {
                    if (tupleItemsCount == 3)
                    {
                        updateOperations[opIndex] = new UpdateOperation(
                        (string)(stringConverter.Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()),
                        (int)(intConverter.Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()),
                        ExecuteSqlRequestConverterMock.GetObjectByDataType(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference());
                    }
                    else
                    {
                        if (tupleItemsCount == 5)
                        {
                            var op = stringConverter.Read(arraySegment);
                            updateOperations[opIndex] = UpdateOperation.CreateStringSplice(
                                (int)(intConverter.Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()),
                                (int)(intConverter.Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()),
                                (int)(intConverter.Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()),
                                (string)(stringConverter.Read(arraySegment) ?? throw ExceptionHelper.ActualValueIsNullReference()));
                        }
                        else
                        {
                            throw ExceptionHelper.InvalidArrayLength(5, tupleItemsCount);
                        }
                    }
                }
            }

            return updateOperations;
        }

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
                        updateOperations = GetUpdateOperations(reader);
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
