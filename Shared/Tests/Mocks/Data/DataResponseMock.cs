// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Tests.Mocks.Data
{
    internal class DataResponseMock : DataResponse
    {
#nullable enable
        internal DataResponseMock(object? data, SqlInfo sqlInfo) : base(data, sqlInfo)
        {
            TestData = data;
        }

        internal DataResponseMock(object? data) : base(data, SqlInfo.Empty)
        {
            TestData = data;
        }

        internal DataResponseMock(object? data, FieldMetadata[] metadata, SqlInfo sqlInfo) : base(data, metadata, sqlInfo)
        {
            TestData = data;
        }

        internal object? TestData { get; private set; }
    }
}
