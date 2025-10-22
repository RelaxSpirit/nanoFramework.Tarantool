// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Client.Extensions
{
    /// <summary>
    /// The <see cref="Tarantool"/> feature extensions class.
    /// </summary>
    internal static class FeatureExtensions
    {
        internal static readonly MajorVersion SqlAvailableMajorVersion = new MajorVersion(1, 8);

#nullable enable
        internal static bool IsSqlAvailable(this BoxInfo? info)
        {
            if (info == null || info.Version == TarantoolVersion.Empty)
            {
                return true;
            }

            return info.Version >= SqlAvailableMajorVersion;
        }
    }
}
