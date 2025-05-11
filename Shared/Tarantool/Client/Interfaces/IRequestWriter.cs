// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif

namespace nanoFramework.Tarantool.Client.Interfaces
{
    internal interface IRequestWriter : IDisposable
    {
        internal void BeginWriting();

        internal bool IsConnected { get; }

        internal void Write(byte[] request);
    }
}
