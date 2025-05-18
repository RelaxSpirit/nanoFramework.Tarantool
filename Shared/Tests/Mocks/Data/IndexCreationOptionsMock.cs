// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Tests.Mocks.Data
{
    internal class IndexCreationOptionsMock : IndexCreationOptions
    {
        internal IndexCreationOptionsMock(bool unique) : base(unique)
        {
        }
    }
}
