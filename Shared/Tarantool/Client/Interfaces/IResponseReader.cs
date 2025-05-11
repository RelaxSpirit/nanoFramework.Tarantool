// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    internal interface IResponseReader : IDisposable
    {
        internal void BeginReading();

        internal CompletionSource GetResponseCompletionSource(RequestId requestId);

#nullable enable
        internal CompletionSource? PopResponseCompletionSource(RequestId requestId);

        internal bool IsConnected { get; }
    }
}
