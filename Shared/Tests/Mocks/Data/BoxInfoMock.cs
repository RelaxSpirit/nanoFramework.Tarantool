// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Tests.Mocks.Data
{
    internal class BoxInfoMock : BoxInfo
    {
        private BoxInfoMock()
        {
            Id = 1;
            Version = new TarantoolVersion(new MajorVersion(3, 4), 0, 0, "test");
            Lsn = long.MinValue;
            Pid = 12345678;
            ReadOnly = true;
            Uuid = Guid.NewGuid();         
        }

        internal static BoxInfoMock GetBoxInfo()
        {
            return new BoxInfoMock();
        }

        internal class BoxInfoConverterMock : BoxInfoConverter
        {
#nullable enable
            public override void Write(object? value, [NotNull] IMessagePackWriter writer)
            {
                if (value is BoxInfo box)
                {
                    writer.WriteArrayHeader(1);
                    writer.WriteMapHeader(6);

                    var stringConverter = ConverterContext.GetConverter(typeof(string));
                    var longConverter = ConverterContext.GetConverter(typeof(long));
                    var boolConverter = ConverterContext.GetConverter(typeof(bool));

                    stringConverter.Write("id", writer);
                    longConverter.Write(box.Id, writer);

                    stringConverter.Write("lsn", writer);
                    longConverter.Write(box.Lsn, writer);

                    stringConverter.Write("pid", writer);
                    longConverter.Write(box.Pid, writer);

                    stringConverter.Write("ro", writer);
                    boolConverter.Write(box.ReadOnly, writer);

                    stringConverter.Write("uuid", writer);
                    stringConverter.Write(box.Uuid.ToString(), writer);

                    if (box.Version is TarantoolVersion tarantoolVersion)
                    {
                        stringConverter.Write("version", writer);
                        stringConverter.Write(tarantoolVersion.ToString(), writer);
                    }
                }
                else
                {
                    throw new ArgumentException(nameof(value));
                }
            }
        }
    }
}
