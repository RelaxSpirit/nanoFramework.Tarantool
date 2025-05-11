// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    internal interface IPhysicalConnection : IDisposable
    {
        void Connect(ClientOptions options);

        void Flush();

        bool IsConnected { get; }

        int Read(byte[] buffer, int offset, int count);

        void Write(byte[] buffer, int offset, int count);
    }
}
