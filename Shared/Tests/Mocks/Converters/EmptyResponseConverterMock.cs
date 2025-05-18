// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;
using nanoFramework.MessagePack.Stream;
using nanoFramework.Tarantool.Converters;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Tests.Mocks.Converters
{
#nullable enable
    internal class EmptyResponseConverterMock : EmptyResponseConverter
    {
        public override void Write(object? value, [NotNull] IMessagePackWriter writer)
        {
            if (value is EmptyResponse)
            {
                writer.WriteMapHeader(0);
            }
            else
            {
                throw new ArgumentException();               
            }
        }
    }
}
