// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Responses
{
#nullable enable
    /// <summary>
    /// SQL info response.
    /// </summary>
    public class SqlInfoResponse
    {
        internal SqlInfoResponse(SqlInfo? sqlInfo)
        {
            SqlInfo = sqlInfo;
        }

        /// <summary>
        /// Gets SQL info.
        /// </summary>
        public SqlInfo? SqlInfo { get; }
    }
}
