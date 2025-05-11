// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif

namespace nanoFramework.Tarantool.Dto
{
    /// <summary>
    /// <see cref="Tarantool"/> connections uri.
    /// </summary>
    public class TarantoolUri
    {
        /// <summary>
        /// <see cref="Tarantool"/> uri scheme.
        /// </summary>
        public const string SCHEME = "tarantool";

        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolUri"/> class.
        /// </summary>
        /// <param name="uri"><see cref="Tarantool"/> uri string.</param>
        /// <exception cref="ArgumentException">If uri scheme is not tarantool.</exception>
        internal TarantoolUri(string uri)
        {
            var startIndex = uri.IndexOf("//");
            if (startIndex < 0)
            {
                this.Scheme = SCHEME;
                startIndex = 0;
            }
            else
            {
                this.Scheme = uri.Substring(0, startIndex - 1);

                if (this.Scheme != SCHEME)
                {
                    throw new ArgumentException("Uri scheme must be tarantool");
                }

                startIndex += 2;
            }

            uri = uri.Substring(startIndex, uri.Length - startIndex);
            startIndex = 0;

            var splitIndex = uri.IndexOf('@');

            if (splitIndex < 0)
            {
                this.Password = string.Empty;
                this.UserName = string.Empty;
            }
            else
            {
                var authString = uri.Substring(startIndex, splitIndex);
                var authArray = authString.Split(':');
                this.UserName = authArray[0];

                if (authArray.Length > 1)
                {
                    this.Password = authArray[1];
                }
                else
                {
                    this.Password = string.Empty;
                }

                splitIndex++;
                uri = uri.Substring(splitIndex, uri.Length - splitIndex);
            }

            splitIndex = uri.IndexOf('/');

            if (splitIndex < 0)
            {
                this.Path = string.Empty;
                splitIndex = uri.Length;
            }
            else
            {
                this.Path = uri.Substring(splitIndex, uri.Length - splitIndex);
            }

            var hostPortString = uri.Substring(startIndex, splitIndex);

            var hostPortArray = hostPortString.Split(':');
            this.Host = hostPortArray[0];

            if (hostPortArray.Length < 2 || !int.TryParse(hostPortArray[1], out int port))
            {
                throw new ArgumentException($"The URI '{uri}' does not contain a port number.", nameof(uri));
            }

            this.Port = port;
        }

        /// <summary>
        /// Gets user password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets user name.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> node host name or IP.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> node port number.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> uri path. Not used.
        /// </summary>
        internal string Path { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> uri scheme.
        /// </summary>
        public string Scheme { get; }
    }
}
