// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Collections;
using nanoFramework.Tarantool.Client.Interfaces;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// Tarantool connection client settings.
    /// </summary>
    public class ConnectionOptions
    {
        private readonly ArrayList nodes = new ArrayList();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOptions" /> class.
        /// </summary>
        public ConnectionOptions()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOptions" /> class.
        /// </summary>
        /// <param name="replicationSource"><see cref="Tarantool"/> replication source.</param>
        public ConnectionOptions(string replicationSource) : this()
        {
            if (!string.IsNullOrEmpty(replicationSource))
            {
                this.Parse(replicationSource);
            }
        }

        /// <summary>
        /// Gets or sets write stream buffer size. It also defines the maximum request size.
        /// Default value 8192 bytes.
        /// </summary>
        public int WriteStreamBufferSize { get; set; } = 8192;

        /// <summary>
        /// Gets or sets minimum request count with throttle.
        /// If number of pending requests more than the value, Throttle does not apply.
        /// Default value 16.
        /// </summary>
        public int MinRequestsWithThrottle { get; set; } = 16;

        /// <summary>
        /// Gets or sets maximum request count in batch
        /// 0 - unlimited.
        /// </summary>
        public int MaxRequestsInBatch { get; set; } = 0;

        /// <summary>
        /// Gets or sets maximum delay in milliseconds for sending requests during throttle.
        /// Default value 10.
        /// </summary>
        public int WriteThrottlePeriodInMs { get; set; } = 10;

        /// <summary>
        /// Gets or sets read stream buffer size. It also defines the maximum sum responses length size.
        /// Default value 8192 bytes.
        /// </summary>
        public int ReadStreamBufferSize { get; set; } = 8192;

        /// <summary>
        /// Gets or sets interval in milliseconds fo sending ping packet request.
        /// Default value 1000.
        /// </summary>
        public int PingCheckInterval { get; set; } = 1000;

        /// <summary>
        /// Gets or sets timeout  waiting ping response.
        /// Default value 30 seconds.
        /// </summary>
        public TimeSpan PingCheckTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Gets or sets connecting <see cref="TarantoolNode"/> nodes
        /// Default value empty.
        /// </summary>
        public TarantoolNode[] Nodes { get; set; } = new TarantoolNode[0];

        /// <summary>
        /// Gets or sets a value indicating whether determines whether to read the information about the <see cref="ISchema"/> during the connection.
        /// </summary>
        public bool ReadSchemaOnConnect { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether determines whether to read the information about the <see cref="BoxInfo"/> during the connection.
        /// </summary>
        public bool ReadBoxInfoOnConnect { get; set; } = true;

        private void Parse(string replicationSource)
        {
            var urls = replicationSource.Split(',');

            foreach (var url in urls)
            {
                var node = TarantoolNode.TryParse(url);
                if (node != null)
                {
                    this.nodes.Add(node);
                }
            }

            this.Nodes = (TarantoolNode[])this.nodes.ToArray(typeof(TarantoolNode));
        }
    }
}
