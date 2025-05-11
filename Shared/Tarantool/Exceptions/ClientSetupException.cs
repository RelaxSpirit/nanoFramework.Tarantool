// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif

namespace nanoFramework.Tarantool.Exceptions
{
    /// <summary>
    /// The client setup exception class.
    /// </summary>
    public class ClientSetupException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSetupException"/> class.
        /// </summary>
        /// <param name="msg">Error massage.</param>
        internal ClientSetupException(string msg) : base(msg)
        {
        }
    }
}
