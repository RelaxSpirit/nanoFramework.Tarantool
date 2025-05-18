// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System.IO;
#endif
using System.Net.Sockets;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// Configure socket delegate.
    /// </summary>
    /// <param name="socket">Socket instance to configure.</param>
    public delegate void ConfigureSocketDelegate(Socket socket);

    /// <summary>
    /// Get network stream delegate.
    /// </summary>
    /// <param name="clientOptions">Client options.</param>
    /// <returns>Stream to write and read.</returns>
    public delegate Stream GetNetworkStreamDelegate(ClientOptions clientOptions);

    /// <summary>
    /// Network client options class.
    /// </summary>
    public class ClientOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientOptions"/> class.
        /// </summary>
        public ClientOptions() : this(new ConnectionOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientOptions"/> class.
        /// </summary>
        /// <param name="replicationSource"><see cref="Tarantool"/> replica connection string.</param>
        public ClientOptions(string replicationSource) : this(new ConnectionOptions(replicationSource))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientOptions"/> class.
        /// </summary>
        /// <param name="options">Network connection options.</param>
        private ClientOptions(ConnectionOptions options)
        {
            ConnectionOptions = options;
        }

        /// <summary>
        /// Gets or sets configure socket а reference to the method corresponding to the <see cref="ConfigureSocketDelegate"/>.
        /// </summary>
#nullable enable
        public ConfigureSocketDelegate? ConfigureSocket { get; set; }

        /// <summary>
        /// Gets or sets configure socket а reference to the method corresponding to the <see cref="GetNetworkStreamDelegate"/>.
        /// </summary>
        public GetNetworkStreamDelegate? GetNetworkStream { get; set; }
#nullable disable

        /// <summary>
        /// Gets network connection options.
        /// </summary>
        public ConnectionOptions ConnectionOptions { get; }

        /// <summary>
        /// Gets or sets the total timeout for waiting for a response to a sent request.
        /// By default value 30,000 milliseconds.
        /// </summary>
        public int RequestTimeout { get; set; } = 30000;
    }
}
