// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Text;

namespace nanoFramework.Tarantool.Model.Responses
{
    /// <summary>
    /// The <see cref="Tarantool"/> greetings response class.
    /// </summary>
    internal class GreetingsResponse
    {
        private const int GreetingsMessageLength = 64;
        private const int GreetingsSaltLength = 44;

        /// <summary>
        /// Initializes a new instance of the <see cref="GreetingsResponse"/> class.
        /// </summary>
        /// <param name="response">Byte array by response.</param>
        internal GreetingsResponse(byte[] response)
        {
            Message = Encoding.UTF8.GetString(response, 0, GreetingsMessageLength);

            var saltString = Encoding.UTF8.GetString(response, GreetingsMessageLength, GreetingsSaltLength);
            Salt = Convert.FromBase64String(saltString);
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> hello message.
        /// </summary>
        internal string Message { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> salt.
        /// </summary>
        internal byte[] Salt { get; }
    }
}
