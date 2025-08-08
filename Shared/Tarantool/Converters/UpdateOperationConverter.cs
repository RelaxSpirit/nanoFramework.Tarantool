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
    /// The update operation converter class.
    /// </summary>
    internal class UpdateOperationConverter : IConverter
    {
        private static void Write(UpdateOperation value, IMessagePackWriter writer)
        {
            writer.WriteArrayHeader(3);

            TarantoolContext.Instance.StringConverter.Write(value.OperationType, writer);
            TarantoolContext.Instance.IntConverter.Write(value.FieldNumber, writer);
            if (value.Argument != null)
            {
                ConverterContext.GetConverter(value.Argument.GetType()).Write(value.Argument, writer);
            }
            else
            {
                ConverterContext.NullConverter.Write(value.Argument, writer);
            }
        }

#nullable enable
        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value != null)
            {
                Write((UpdateOperation)value, writer);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public virtual object? Read([NotNull] IMessagePackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
