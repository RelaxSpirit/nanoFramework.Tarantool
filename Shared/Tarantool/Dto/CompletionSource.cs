// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System.Threading;
#endif

namespace nanoFramework.Tarantool.Dto
{
    /// <summary>
    /// Complete result delegate.
    /// </summary>
    /// <param name="result">Response result object.</param>
    internal delegate void CompleteResultDelegate(object result);

    /// <summary>
    /// Request completion source class.
    /// </summary>
    internal class CompletionSource
    {
        /// <summary>
        /// Gets event by received response.
        /// </summary>
        internal ManualResetEvent ResponseEvent { get; } = new ManualResetEvent(false);

        /// <summary>
        /// Gets or sets request response result object.
        /// </summary>
#nullable enable
        internal object? Result { get; set; }

        /// <summary>
        /// Gets or sets complete result callback methid.
        /// </summary>
        internal CompleteResultDelegate? CompleteResultCallback { get; set; }
    }
}
