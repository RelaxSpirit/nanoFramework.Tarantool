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
        private System.IO.Stream? _stream;
        private Socket? _socket;
        private bool _disposed;

        private static void Connect(Socket socket, string host, int port)
        {
            try
            {
                ConnectToIp(socket, host, port);
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
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            _stream?.Dispose();
            _socket?.Close();
        }

        public void Connect(ClientOptions options)
        {
            if (options.ConnectionOptions.Nodes.Length < 1)
            {
                throw new ClientSetupException("There are zero configured nodes, you should provide one");
            }

            var singleNode = options.ConnectionOptions.Nodes[0];

            if (options.GetNetworkStream == null)
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                options.ConfigureSocket?.Invoke(_socket);

                Connect(_socket, singleNode.Uri.Host, singleNode.Uri.Port);

                _stream = new NetworkStream(_socket, true);
            }
            else
            {
                _stream = options.GetNetworkStream(options);
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();
            _stream?.Write(buffer, offset, count);
        }

        public void Flush()
        {
            CheckConnectionStatus();
            _stream?.Flush();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();
            try
            {
                if (_stream == null)
                {
                    return 0;
                }
                else
                {
                    return _stream.Read(buffer, offset, count);
                }
            }
            catch (ObjectDisposedException)
            {
                return 0;
            }
            catch (IOException)
            {
                if (_disposed)
                {
                    return 0;
                }
                else
                {
                    throw;
                }
            }
        }

        public bool IsConnected => !_disposed && _stream != null;

        private void CheckConnectionStatus()
        {
            if (_disposed)
            {
#if NANOFRAMEWORK_1_0
                throw new ObjectDisposedException();
#else
                throw new ObjectDisposedException(nameof(NetworkStreamPhysicalConnection));
#endif
            }

            if (!IsConnected)
            {
                throw ExceptionHelper.NotConnected();
            }
        }
    }
}
