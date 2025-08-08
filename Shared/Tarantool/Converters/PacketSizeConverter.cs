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
    /// The <see cref="Tarantool"/> packet size converter class.
    /// </summary>
    internal class PacketSizeConverter : IConverter
    {
        public static void Write(PacketSize value, IMessagePackWriter writer)
        {
            writer.Write(DataTypes.UInt32); 

            unchecked
            {
                writer.Write((byte)((value.Value >> 24) & 0xff));
                writer.Write((byte)((value.Value >> 16) & 0xff));
                writer.Write((byte)((value.Value >> 8) & 0xff));
                writer.Write((byte)(value.Value & 0xff));
            }
        }

        public static PacketSize Read(IMessagePackReader reader)
        {
            var type = reader.ReadDataType();
            if (type != DataTypes.UInt32)
            {
                throw ExceptionHelper.UnexpectedDataType(DataTypes.UInt32, type);
            }

            byte[] bytes = new byte[4];
            bytes[3] = reader.ReadByte();
            bytes[2] = reader.ReadByte();
            bytes[1] = reader.ReadByte();
            bytes[0] = reader.ReadByte();

            return new PacketSize(BitConverter.ToUInt32(bytes, 0));
        }
#nullable enable
        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((PacketSize)value, writer);
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
