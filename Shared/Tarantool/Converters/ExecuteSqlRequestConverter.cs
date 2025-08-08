// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
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
            writer.WriteMapHeader(3u);

            TarantoolContext.Instance.UintConverter.Write(Key.SqlQueryText, writer);
            TarantoolContext.Instance.StringConverter.Write(value.Query, writer);

            TarantoolContext.Instance.UintConverter.Write(Key.SqlParameters, writer);
            writer.WriteArrayHeader((uint)value.Parameters.Length);
            foreach (var parameter in value.Parameters)
            {
                parameter.Write(writer);
            }

            TarantoolContext.Instance.UintConverter.Write(Key.SqlOptions, writer);

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
