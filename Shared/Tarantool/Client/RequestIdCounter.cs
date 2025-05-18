// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Runtime.CompilerServices;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Client
{
    /// <summary>
    /// The <see cref="Tarantool"/> request id counter class.
    /// </summary>
    internal class RequestIdCounter
    {
        private ulong _currentRequestId = 0;

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal RequestId GetRequestId()
        {
            return (RequestId)(++_currentRequestId);
        }
    }
}
