// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using Index = nanoFramework.Tarantool.Client.Index;

namespace nanoFramework.Tarantool.Tests.Mocks.Data
{
    internal class IndexMock : Index
    {
        internal IndexMock(uint id, uint spaceId, string name, bool unique, IndexType type, IndexPart[] parts) : base(id, spaceId, name, unique, type, parts)
        {
        }
    }
}
