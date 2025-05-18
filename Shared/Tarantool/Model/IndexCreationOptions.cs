// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// The <see cref="Tarantool"/> index creation class.
    /// </summary>
    internal class IndexCreationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IndexCreationOptions"/> class.
        /// </summary>
        /// <param name="unique"><see langword="true"/> if created index must be unique.</param>
        internal IndexCreationOptions(bool unique)
        {
            Unique = unique;
        }

        /// <summary>
        /// Gets a value indicating whether index is unique.
        /// </summary>
        internal bool Unique { get; }
    }
}
