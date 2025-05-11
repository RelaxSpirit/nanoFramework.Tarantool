// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
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

            var binary = BitConverter.GetBytes(value.Value);

            byte[] bytes;
            if (BitConverter.IsLittleEndian)
            {
                bytes = new[]
                {
                    binary[7],
                    binary[6],
                    binary[5],
                    binary[4],
                    binary[3],
                    binary[2],
                    binary[1],
                    binary[0]
                };

                writer.Write(bytes);
            }
            else
            {
                writer.Write(binary);
            }
        }

        private static RequestId Read(IMessagePackReader reader)
        {
            var type = reader.ReadDataType();
            if (type != DataTypes.UInt64)
            {
                throw ExceptionHelper.UnexpectedDataType(DataTypes.UInt64, type);
            }

            var binary = (byte[])reader.ReadBytes(8);
            if (BitConverter.IsLittleEndian)
            {
                byte[] bytes = new[]
                {
                    binary[7],
                    binary[6],
                    binary[5],
                    binary[4],
                    binary[3],
                    binary[2],
                    binary[1],
                    binary[0]
                };
                return new RequestId(BitConverter.ToUInt64(bytes, 0));
            }
            else
            {
                return new RequestId(BitConverter.ToUInt64(binary, 0));
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
