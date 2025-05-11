// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif

namespace nanoFramework.Tarantool.Exceptions
{
    /// <summary>
    /// Error returned <see cref="Tarantool"/> instance.
    /// </summary>
    public class TarantoolException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolException"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        internal TarantoolException(string message) : base(message) 
        { 
        }
    }
}
