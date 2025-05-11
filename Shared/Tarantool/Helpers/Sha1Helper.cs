// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Security.Cryptography;
using System.Text;

namespace nanoFramework.Tarantool.Helpers
{
    /// <summary>
    /// The SHA1 helper class.
    /// </summary>
    internal static class Sha1UHelper
    {
        public static byte[] Hash(string str)
        {
            var bytes = str == null ? new byte[0] : Encoding.UTF8.GetBytes(str);

            return Hash(bytes);
        }

        public static byte[] Hash(byte[] bytes)
        {
#pragma warning disable CS0436 // Type conflicts with imported type
            var sha1 = SHA1.Create();
#pragma warning restore CS0436 // Type conflicts with imported type
            var hashBytes = sha1.ComputeHash(bytes);

            return hashBytes;
        }

        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }

        public static byte[] Hash(byte[] salt, byte[] str)
        {
            var salted = new byte[salt.Length + str.Length];
            Array.Copy(salt, salted, salt.Length);
            Array.Copy(str, 0, salted, salt.Length, str.Length);

            return Hash(salted);
        }

        public static byte[] Xor(byte[] array1, byte[] array2)
        {
            var result = new byte[array1.Length];

            for (int i = 0; i < array1.Length; i++)
            {
                result[i] = (byte)(array1[i] ^ array2[i]);
            }

            return result;
        }
    }
}
