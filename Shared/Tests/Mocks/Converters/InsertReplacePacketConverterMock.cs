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
    internal class InsertReplacePacketConverterMock : InsertReplacePacketConverter
    {
        private readonly bool _isReplace;

        internal InsertReplacePacketConverterMock(bool isReplace)
        {
            _isReplace = isReplace;
        }

#nullable enable
        public override object? Read([NotNull] IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();

            if (length != 2)
            {
                throw ExceptionHelper.InvalidMapLength(length, 2);
            }

            uint spaceId = uint.MaxValue;
            TarantoolTuple? tuple = null;
            for (var i = 0; i < length; i++)
            {
                Key key = (Key)(TarantoolContext.Instance.KeyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());

                switch (key)
                {
                    case Key.SpaceId:
                        spaceId = (uint)(TarantoolContext.Instance.UintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case Key.Tuple:
                        tuple = (TarantoolTuple)(TarantoolMockContext.Instanse.TupleConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                }
            }

            if (spaceId != uint.MaxValue && tuple != null)
            {
                if (_isReplace)
                {
                    return new ReplaceRequest(spaceId, tuple);
                }
                else
                {
                    return new InsertRequest(spaceId, tuple);
                }
            }
            else
            {
                return null;
            }
        }
    }
}
