// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    /// <summary>
    /// The logical connection interface.
    /// </summary>
    internal interface ILogicalConnection : ITarantoolConnection, IDisposable
    {
        /// <summary>
        /// Connect to <see cref="Tarantool"/>.
        /// </summary>
        void Connect();
    }
}
