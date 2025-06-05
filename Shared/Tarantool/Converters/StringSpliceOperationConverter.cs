// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Model.UpdateOperations;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The string splice operation converter class.
    /// </summary>
    internal class StringSpliceOperationConverter : IConverter
    {
        private static void Write(StringSpliceOperation value, IMessagePackWriter writer)
        {
            writer.WriteArrayHeader(5);

            var stringConverter = ConverterContext.GetConverter(typeof(string));
            var intConverter = ConverterContext.GetConverter(typeof(int));

            stringConverter.Write(value.OperationType, writer);
            intConverter.Write(value.FieldNumber, writer);
            intConverter.Write(value.Position, writer);
            intConverter.Write(value.Offset, writer);
            stringConverter.Write(value.Argument, writer);
        }

#nullable enable 
        public virtual object? Read([NotNull] IMessagePackReader reader)
        {
            throw new NotImplementedException();
        }

        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((StringSpliceOperation)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
