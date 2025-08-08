// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> request id converter class.
    /// </summary>
    internal class RequestIdConverter : IConverter
    {
        private static void Write(RequestId value, IMessagePackWriter writer)
        {
            writer.Write(DataTypes.UInt64);

            if (BitConverter.IsLittleEndian)
            {
                unchecked
                {
                    writer.Write((byte)((value.Value >> 56) & 0xff));
                    writer.Write((byte)((value.Value >> 48) & 0xff));
                    writer.Write((byte)((value.Value >> 40) & 0xff));
                    writer.Write((byte)((value.Value >> 32) & 0xff));
                    writer.Write((byte)((value.Value >> 24) & 0xff));
                    writer.Write((byte)((value.Value >> 16) & 0xff));
                    writer.Write((byte)((value.Value >> 8) & 0xff));
                    writer.Write((byte)(value.Value & 0xff));
                }
            }
            else
            {
                writer.Write(BitConverter.GetBytes(value.Value));
            }
        }

        private static RequestId Read(IMessagePackReader reader)
        {
            var type = reader.ReadDataType();
            if (type != DataTypes.UInt64)
            {
                throw ExceptionHelper.UnexpectedDataType(DataTypes.UInt64, type);
            }

            if (BitConverter.IsLittleEndian)
            {
                byte[] bytes = new byte[8];

                bytes[7] = reader.ReadByte();
                bytes[6] = reader.ReadByte();
                bytes[5] = reader.ReadByte();
                bytes[4] = reader.ReadByte();
                bytes[3] = reader.ReadByte();
                bytes[2] = reader.ReadByte();
                bytes[1] = reader.ReadByte();
                bytes[0] = reader.ReadByte();

                return new RequestId(BitConverter.ToUInt64(bytes, 0));
            }
            else
            {
                return new RequestId(BitConverter.ToUInt64((byte[])reader.ReadBytes(8), 0));
            }
        }

#nullable enable
        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((RequestId)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        object? IConverter.Read([NotNull] IMessagePackReader reader)
        {
            return Read(reader);
        }
    }
}
