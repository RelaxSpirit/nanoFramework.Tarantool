// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Client;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Tests.Mocks.Data
{
    internal class SpaceMock : Space
    {
        internal SpaceMock(uint id, uint fieldCount, string name, StorageEngine engine, SpaceField[] fields) : base(id, fieldCount, name, engine, fields)
        {
        }
    }
}
