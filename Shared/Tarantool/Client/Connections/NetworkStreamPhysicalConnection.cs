// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using System.IO;
#endif
using System.Net;
using System.Net.Sockets;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Exceptions;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Client.Connections
{
    /// <summary>
    /// The network stream physical connection class.
    /// </summary>
    internal class NetworkStreamPhysicalConnection : IPhysicalConnection
    {
#nullable enable
        private System.IO.Stream? stream;
        private Socket? socket;
        private bool disposed;

        private static void Connect(Socket socket, string host, int port)
        {
            try
            {
                ConnectToIp(socket, host, port);
                return;
            }
            catch
            {
                ConnectToHostName(socket, host, port);
            }
        }

        private static void ConnectToIp(Socket socket, string ipString, int port)
        {
            var address = IPAddress.Parse(ipString);
            socket.Connect(new IPEndPoint(address, port));
            return;
        }

        private static void ConnectToHostName(Socket socket, string host, int port)
        {
            var resolved = Dns.GetHostEntry(host);
            for (var i = 0; i < resolved.AddressList.Length; i++)
            {
                try
                {
                    socket.Connect(new IPEndPoint(resolved.AddressList[i], port));
                    return;
                }
                catch
                {
                    // if we have tried all of them and still failed,
                    // then blow up.
                    if (i == resolved.AddressList.Length - 1)
                    {
                        throw;
                    }
                }
            }

            // we should never get here...
            throw new InvalidOperationException("Unable to resolve endpoint.");
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            this.stream?.Dispose();
            this.socket?.Close();
        }

        public void Connect(ClientOptions options)
        {
            if (options.ConnectionOptions.Nodes.Length < 1)
            {
                throw new ClientSetupException("There are zero configured nodes, you should provide one");
            }

            var singleNode = options.ConnectionOptions.Nodes[0];

            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            options.ConfigureSocket?.Invoke(this.socket);

            Connect(this.socket, singleNode.Uri.Host, singleNode.Uri.Port);

            this.stream = new NetworkStream(this.socket, true);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            this.CheckConnectionStatus();
            this.stream?.Write(buffer, offset, count);
        }

        public void Flush()
        {
            this.CheckConnectionStatus();
            this.stream?.Flush();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            this.CheckConnectionStatus();
            try
            {
                if (this.stream == null)
                {
                    return 0;
                }
                else
                {
                    return this.stream.Read(buffer, offset, count);
                }
            }
            catch (ObjectDisposedException)
            {
                return 0;
            }
            catch (IOException)
            {
                if (this.disposed)
                {
                    return 0;
                }
                else
                {
                    throw;
                }
            }
        }

        public bool IsConnected => !this.disposed && this.stream != null;

        private void CheckConnectionStatus()
        {
            if (this.disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(NetworkStreamPhysicalConnection));
#endif
            }

            if (!this.IsConnected)
            {
                throw ExceptionHelper.NotConnected();
            }
        }
    }
}
