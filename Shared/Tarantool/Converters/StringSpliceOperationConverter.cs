// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
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

            TarantoolContext.Instance.StringConverter.Write(value.OperationType, writer);
            TarantoolContext.Instance.IntConverter.Write(value.FieldNumber, writer);
            TarantoolContext.Instance.IntConverter.Write(value.Position, writer);
            TarantoolContext.Instance.IntConverter.Write(value.Offset, writer);
            TarantoolContext.Instance.StringConverter.Write(value.Argument, writer);
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
