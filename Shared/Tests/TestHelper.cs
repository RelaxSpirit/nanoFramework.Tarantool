// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using nanoFramework.TestFramework;
#endif
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Responses;

namespace nanoFramework.Tarantool.Tests
{
    /// <summary>
    /// Helper test <see cref="Tarantool"/> class.
    /// </summary>
    internal static class TestHelper
    {
        /// <summary>
        /// <see cref="Tarantool"/> instance ip or host name.
        /// </summary>
#nullable enable
        internal const string TarantoolHostIp = "192.168.1.116";

        /// <summary>
        /// Gets client options method.
        /// </summary>
        /// <param name="isReadSchemaOnConnect">Determines whether to load schema information during connection.</param>
        /// <param name="isReadBoxInfoOnConnect">Determines whether to load box info information during connection.</param>
        /// <param name="writeStreamBufferSize">Write buffer size. Default 8192 bytes.</param>
        /// <param name="readStreamBufferSize">Read buffer size. Default 8192 bytes.</param>
        /// <param name="userData">User name and password. Default <see langword="null"/></param>
        /// <returns>New <see cref="ClientOptions"/> instance.</returns>
        internal static ClientOptions GetClientOptions(
            bool isReadSchemaOnConnect,
            bool isReadBoxInfoOnConnect,
            int writeStreamBufferSize = 8192,
            int readStreamBufferSize = 8192,
            string? userData = null)
        {
            string replicationSource = $"{TarantoolHostIp}:3301";

            if (userData != null)
            {
                replicationSource = $"{userData}@{replicationSource}";
            }

            ClientOptions clientOptions = new ClientOptions(replicationSource);
            clientOptions.ConnectionOptions.ReadSchemaOnConnect = isReadSchemaOnConnect;
            clientOptions.ConnectionOptions.ReadBoxInfoOnConnect = isReadBoxInfoOnConnect;
#if NANOFRAMEWORK_1_0
            clientOptions.ConnectionOptions.WriteStreamBufferSize = writeStreamBufferSize > 512 ? 512 : writeStreamBufferSize;
            clientOptions.ConnectionOptions.ReadStreamBufferSize = readStreamBufferSize > 512 ? 512 : readStreamBufferSize;
            
#else
            clientOptions.ConnectionOptions.WriteStreamBufferSize = writeStreamBufferSize;
            clientOptions.ConnectionOptions.ReadStreamBufferSize = readStreamBufferSize;
#endif
            return clientOptions;
        }

        /// <summary>
        /// Assert check <see cref="Tarantool"/> data response.
        /// </summary>
        /// <param name="dataResponse"><see cref="Tarantool"/> data response to be check.</param>
        /// <returns>Checked <see cref="Tarantool"/> data response.</returns>
        internal static DataResponse CheckResponseData(this DataResponse? dataResponse)
        {
            Assert.IsNotNull(dataResponse);
            Assert.IsNotNull(dataResponse.Data);
            return dataResponse;
        }
    }
}
