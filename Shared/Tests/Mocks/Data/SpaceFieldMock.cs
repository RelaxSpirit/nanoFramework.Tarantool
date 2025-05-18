// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Tests.Mocks.Data
{
    internal class SpaceFieldMock : SpaceField
    {
        internal SpaceFieldMock(string name, FieldType type) : base(name, type)
        {
        }
    }
}
