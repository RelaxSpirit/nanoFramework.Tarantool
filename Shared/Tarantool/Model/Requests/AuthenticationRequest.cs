// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// The <see cref="Tarantool"/> authentication request struct.
    /// </summary>
    internal struct AuthenticationRequest : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationRequest"/> struct.
        /// </summary>
        /// <param name="username">User name.</param>
        /// <param name="scramble">Scramble byte array.</param>
        internal AuthenticationRequest(string username, byte[] scramble)
        {
            Username = username;
            Scramble = scramble;
        }

        /// <summary>
        /// Gets user name.
        /// </summary>
        internal string Username { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> authentication request scramble.
        /// </summary>
        internal byte[] Scramble { get; }

        /// <summary>
        /// Gets <see cref="CommandCode.Auth"/> command code.
        /// </summary>
        CommandCode IRequest.Code => CommandCode.Auth;

        internal static AuthenticationRequest Create(GreetingsResponse greetings, TarantoolUri uri)
        {
            var scrable = GetScrable(greetings, uri.Password);
            var authenticationPacket = new AuthenticationRequest(uri.UserName, scrable);
            return authenticationPacket;
        }

        internal static byte[] GetScrable(GreetingsResponse greetings, string password)
        {
            var decodedSalt = greetings.Salt;
            var first20SaltBytes = new byte[20];
            Array.Copy(decodedSalt, first20SaltBytes, 20);

            var step1 = Sha1UHelper.Hash(password);
            var step2 = Sha1UHelper.Hash(step1);
            var step3 = Sha1UHelper.Hash(first20SaltBytes, step2);
            var scrambleBytes = Sha1UHelper.Xor(step1, step3);

            return scrambleBytes;
        }

        /// <summary>
        /// Overrides base class method <see cref="object.ToString"/>.
        /// </summary>
        /// <returns>User name and scramble string.</returns>
        public override string ToString()
        {
            return $"Username: {Username}, Scramble: {Scramble.ToReadableString()}";
        }
    }
}
