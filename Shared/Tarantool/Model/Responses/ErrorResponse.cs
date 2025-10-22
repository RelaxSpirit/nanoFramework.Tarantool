// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace nanoFramework.Tarantool.Model.Responses
{
    /// <summary>
    /// The <see cref="Tarantool"/> error response struct.
    /// </summary>
    internal struct ErrorResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> struct.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        internal ErrorResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> error message.
        /// </summary>
        internal string ErrorMessage { get; }
    }
}
