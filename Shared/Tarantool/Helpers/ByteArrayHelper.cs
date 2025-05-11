// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Text;

namespace nanoFramework.Tarantool.Helpers
{
    /// <summary>
    /// The byte array helper class.
    /// </summary>
    internal static class ByteArrayHelper
    {
        internal static string ToReadableString(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2"));
                sb.Append(' ');
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}
