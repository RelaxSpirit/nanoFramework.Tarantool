// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Enums
{
    /// <summary>
    /// Enum <see cref="Tarantool"/> iterator.
    /// </summary>
    public enum Iterator : uint
    {
        /// <summary>
        /// The comparison operator is ‘==’ (equal to).
        /// If an index key is equal to a search value, it matches.
        /// Tuples are returned in ascending order by index key. This is the default.
        /// </summary>
        Eq = 0,

        /// <summary>
        /// Matching is the same as for box.index.EQ. Tuples are returned in descending order by index key.
        /// </summary>
        Req = 1,

        /// <summary>
        /// All tuples.
        /// </summary>
        All = 2,

        /// <summary>
        /// The comparison operator is ‘&lt;’ (less than).
        /// If an index key is less than a search value, it matches.
        /// Tuples are returned in descending order by index key.
        /// </summary>
        Lt = 3,

        /// <summary>
        /// The comparison operator is ‘&lt;=’ (less than or equal to).
        /// If an index key is less than or equal to a search value, it matches.
        /// Tuples are returned in descending order by index key.
        /// </summary>
        Le = 4,

        /// <summary>
        /// The comparison operator is ‘&gt;=’ (greater than or equal to).
        /// If an index key is greater than or equal to a search value, it matches.
        /// Tuples are returned in ascending order by index key.
        /// </summary>
        Ge = 5,

        /// <summary>
        /// The comparison operator is ‘&gt;’ (greater than).
        /// If an index key is greater than a search value, it matches.
        /// Tuples are returned in ascending order by index key.
        /// </summary>
        Gt = 6,

        /// <summary>
        /// All bits of the value are set in the key.
        /// </summary>
        BitsAllSet = 7,

        /// <summary>
        /// At least one bit of the value is set.
        /// </summary>
        BitsAnySet = 8,

        /// <summary>
        /// No bits are set.
        /// </summary>
        BitsAllNotSet = 9,

        /// <summary>
        /// Overlaps the rectangle or box.
        /// </summary>
        Overlaps = 10,

        /// <summary>
        /// Neighbors the rectangle or box.
        /// </summary>
        Neighbour = 11,
    }
}
