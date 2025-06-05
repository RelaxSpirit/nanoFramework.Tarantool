// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Text;

namespace nanoFramework.Tarantool.Helpers
{
    /// <summary>
    /// The string helper class.
    /// </summary>
    internal static class StringHelper
    {
        internal static string Join(this IList arrayValue, string joinString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in arrayValue)
            {
                sb.Append(item.ToString());
                sb.Append(joinString);
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - joinString.Length, joinString.Length);
            }

            return sb.ToString();
        }
    }
}
