// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Tests.Mocks.Data
{
    internal class IndexPartMock : IndexPart
    {
        internal IndexPartMock(uint fieldNo, FieldType type) : base(fieldNo, type)
        {
        }
    }
}
