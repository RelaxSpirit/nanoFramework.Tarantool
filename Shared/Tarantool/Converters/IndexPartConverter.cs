// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Exceptions;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> index part converter class.
    /// </summary>
    internal class IndexPartConverter : IConverter
    {
        public static IndexPart Read(IMessagePackReader reader)
        {
            var type = reader.ReadDataType();
            switch (type)
            {
                case DataTypes.Map16:
                    return ReadFromMap(reader, ReadUInt16(reader));

                case DataTypes.Map32:
                    return ReadFromMap(reader, ReadUInt32(reader));

                case DataTypes.Array16:
                    return ReadFromArray(reader, ReadUInt16(reader));

                case DataTypes.Array32:
                    return ReadFromArray(reader, ReadUInt32(reader));
            }

            if (TryGetLengthFromFixMap(type, out var length))
            {
                return ReadFromMap(reader, length);
            }

            if (TryGetLengthFromFixArray(type, out length))
            {
                return ReadFromArray(reader, length);
            }

            throw new SerializationException($"Got {type:G} (0x{type:X}), while expecting one of these: {DataTypes.Map16}, {DataTypes.Map32}, {DataTypes.FixMap}, {DataTypes.Array16}, {DataTypes.Array32}, {DataTypes.FixArray}");
        }

        private static IndexPart ReadFromArray(IMessagePackReader reader, uint length)
        {
            if (length != 2u)
            {
                throw ExceptionHelper.InvalidArrayLength(2u, length);
            }

            var uintConverter = ConverterContext.GetConverter(typeof(uint));
            var indexPartTypeConverter = ConverterContext.GetConverter(typeof(FieldType));

            var fieldNo = uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();
            var indexPartType = indexPartTypeConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference();

            return new IndexPart((uint)fieldNo, (FieldType)indexPartType);
        }

        private static IndexPart ReadFromMap(IMessagePackReader reader, uint length)
        {
            uint fieldNo = uint.MaxValue;
            FieldType indexPartType = FieldType._;

            var uintConverter = ConverterContext.GetConverter(typeof(uint));
            var indexPartTypeConverter = ConverterContext.GetConverter(typeof(FieldType));
            var stringConverter = ConverterContext.GetConverter(typeof(string));
            for (var i = 0; i < length; i++)
            {
                switch ((string)(stringConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference()))
                {
                    case "field":
                        fieldNo = (uint)(uintConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    case "type":
                        indexPartType = (FieldType)(indexPartTypeConverter.Read(reader) ?? throw ExceptionHelper.ActualValueIsNullReference());
                        break;
                    default:
                        reader.SkipToken();
                        break;
                }
            }

            if (fieldNo != uint.MaxValue && indexPartType != FieldType._)
            {
                return new IndexPart(fieldNo, indexPartType);
            }

            throw new SerializationException("Can't read fieldNo or indexPart from map of index metadata");
        }

        internal static byte GetHighBits(DataTypes type, byte bitsCount)
        {
            return (byte)((byte)type >> (8 - bitsCount));
        }

        private static bool TryGetLengthFromFixArray(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixArray;
            return GetHighBits(type, 4) == GetHighBits(DataTypes.FixArray, 4);
        }

        private static bool TryGetLengthFromFixMap(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixMap;
            return GetHighBits(type, 4) == GetHighBits(DataTypes.FixMap, 4);
        }

        internal static ushort ReadUInt16(IMessagePackReader reader)
        {
            return (ushort)((reader.ReadByte() << 8) + reader.ReadByte());
        }

        internal static uint ReadUInt32(IMessagePackReader reader)
        {
            var temp = (uint)(reader.ReadByte() << 24);
            temp += (uint)reader.ReadByte() << 16;
            temp += (uint)reader.ReadByte() << 8;
            temp += reader.ReadByte();

            return temp;
        }

#nullable enable
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
