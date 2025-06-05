// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack.Converters;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Converters
{
    /// <summary>
    /// The <see cref="Tarantool"/> ping packet converter class.
    /// </summary>
    internal class PingPacketConverter : IConverter
    {
#nullable enable
        public void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
        }

        public object? Read([NotNull] IMessagePackReader reader)
        {
            return new PingRequest();
        }
    }
}
