// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> execute SQL request converter class.
    /// </summary>
    internal class ExecuteSqlRequestConverter : IConverter
    {
        public static void Write(ExecuteSqlRequest value, IMessagePackWriter writer)
        {
            var keyConverter = ConverterContext.GetConverter(typeof(uint));
            var stringConverter = ConverterContext.GetConverter(typeof(string));

            writer.WriteMapHeader(3u);

            keyConverter.Write(Key.SqlQueryText, writer);
            stringConverter.Write(value.Query, writer);

            keyConverter.Write(Key.SqlParameters, writer);
            writer.WriteArrayHeader((uint)value.Parameters.Length);
            foreach (var parameter in value.Parameters)
            {
                parameter.Write(writer);
            }

            keyConverter.Write(Key.SqlOptions, writer);

            ConverterContext.NullConverter.Write(null, writer);
        }

#nullable enable
        public virtual object? Read([NotNull] IMessagePackReader reader)
        {
            throw new NotSupportedException();
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((ExecuteSqlRequest)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
