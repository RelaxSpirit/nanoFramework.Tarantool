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
#nullable enable
    /// <summary>
    /// The SQL info response packet converter class.
    /// </summary>
    internal class SqlInfoResponsePacketConverter : IConverter
    {
        private static SqlInfoResponse Read(IMessagePackReader reader)
        {
            var length = reader.ReadMapLength();
            if (length != 1 && length != 2)
            {
                throw ExceptionHelper.InvalidMapLength(length, 1u, 2u);
            }

            var sqlInfo = SqlInfo.Empty;

            for (var i = 0; i < length; i++)
            {
                var dataKey = (Key)(TarantoolContext.Instance.UintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                switch (dataKey)
                {
                    case Key.SqlInfo:
                        sqlInfo = ReadSqlInfo(reader, TarantoolContext.Instance.UintConverter, TarantoolContext.Instance.IntConverter);
                        break;
                    case Key.SqlInfo_2_0_4:
                        sqlInfo = ReadSqlInfo(reader, TarantoolContext.Instance.UintConverter, TarantoolContext.Instance.IntConverter);
                        break;
                    default:
                        throw ExceptionHelper.UnexpectedKey(dataKey, Key.Data, Key.Metadata);
                }
            }

            return new SqlInfoResponse(sqlInfo);
        }

        internal static SqlInfo ReadSqlInfo(IMessagePackReader reader, IConverter keyConverter, IConverter intConverter)
        {
            SqlInfo result = SqlInfo.Empty;

            var length = reader.ReadMapLength();
            if (length == uint.MaxValue)
            {
                return result;
            }

            for (var i = 0; i < length; i++)
            {
                var keyValue = (Key)(keyConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                switch (keyValue)
                {
                    case Key.SqlRowCount:
                        result = new SqlInfo((int)(intConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference()));
                        break;
                    case Key.SqlRowCount_2_0_4:
                        result = new SqlInfo((int)(intConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference()));
                        break;
                    default:
                        reader.SkipToken();
                        break;
                }
            }

            return result;
        }

        public virtual void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            throw new NotImplementedException();
        }

        object? IConverter.Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader);
        }
    }
}
