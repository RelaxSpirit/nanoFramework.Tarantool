// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Responses
{
    /// <summary>
    /// <see cref="Tarantool"/> SQL info.
    /// </summary>
    public struct SqlInfo
    {
        /// <summary>
        /// Empty <see cref="SqlInfo"/> instance struct.
        /// </summary>
        public static readonly SqlInfo Empty = new SqlInfo(-1);

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlInfo"/> struct.
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

        /// <summary>
        /// Compare two instance <see cref="SqlInfo"/> struct.
        /// </summary>
        /// <param name="left">First instance <see cref="SqlInfo"/>.</param>
        /// <param name="right">Second instance <see cref="SqlInfo"/>.</param>
        /// <returns><see langword="true"/> if instances <see cref="SqlInfo"/> is equal, otherwise <see langword="false"/>.</returns>
        public static bool operator ==(SqlInfo left, SqlInfo right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compare two instance <see cref="SqlInfo"/> struct.
        /// </summary>
        /// <param name="left">First instance <see cref="SqlInfo"/>.</param>
        /// <param name="right">Second instance <see cref="SqlInfo"/>.</param>
        /// <returns><see langword="true"/> if instances <see cref="SqlInfo"/> is not equal, otherwise <see langword="false"/>.</returns>
        public static bool operator !=(SqlInfo left, SqlInfo right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Override base <see cref="object.GetHashCode()"/> method.
        /// </summary>
        /// <returns> A hash code for the current <see cref="SqlInfo"/>.</returns>
        public override int GetHashCode()
        {
            return RowCount.GetHashCode();
        }

        /// <summary>
        /// Override base <see cref="object.Equals(object?)"/> method.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="SqlInfo"/>.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
#nullable enable
        public override bool Equals(object? obj)
        {
            return obj is SqlInfo sqlInfo && RowCount == sqlInfo.RowCount;
        }
    }
}
