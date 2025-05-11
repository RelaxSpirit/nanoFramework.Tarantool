// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Execute <see cref="Tarantool"/> SQL query request.
    /// </summary>
    public class ExecuteSqlRequest : IRequest
    {
        private static readonly SqlParameter[] Empty = new SqlParameter[0];

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteSqlRequest"/> class.
        /// </summary>
        /// <param name="query"><see cref="Tarantool"/> SQL query text.</param>
        /// <param name="parameters"><see cref="Tarantool"/> SQL query <see cref="SqlParameter"/> array.</param>
        public ExecuteSqlRequest(string query, SqlParameter[] parameters)
        {
            this.Query = query;
            this.Parameters = parameters ?? Empty;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> SQL query text.
        /// </summary>
        public string Query { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> SQL query parameters.
        /// </summary>
        public SqlParameter[] Parameters { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> command code.
        /// </summary>
        public CommandCode Code => CommandCode.Execute;
    }
}
