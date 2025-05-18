// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Responses
{
    /// <summary>
    /// <see cref="Tarantool"/> SQL info.
    /// </summary>
    public class SqlInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlInfo"/> class.
        /// </summary>
        /// <param name="rowCount">Response rows count.</param>
        internal SqlInfo(int rowCount)
        {
            RowCount = rowCount;
        }

        /// <summary>
        /// Gets row count.
        /// </summary>
        public int RowCount { get; }
    }
}
