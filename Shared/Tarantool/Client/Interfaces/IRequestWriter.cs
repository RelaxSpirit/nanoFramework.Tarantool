// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    internal interface IRequestWriter : IDisposable
    {
        internal void BeginWriting();

        internal bool IsConnected { get; }

        internal void Write(byte[] request);
    }
}
