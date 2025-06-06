﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#if NANOFRAMEWORK_1_0
namespace System.Security.Cryptography
{
    /// <summary>
    /// The SHA1 class.
    /// </summary>
    internal class SHA1
    {
        private readonly SHA1Context _defaultContext;

        /// <summary>
        /// Prevents a default instance of the <see cref="SHA1" /> class from being created.
        /// </summary>
        private SHA1() 
        {
            _defaultContext = new SHA1Context();
        }

#pragma warning disable CS0436 // Type conflicts with imported type
        internal static SHA1 Create()
        {
            return new SHA1();
        }
#pragma warning restore CS0436 // Type conflicts with imported type

        internal static void Update(SHA1Context context, byte[] data, int len)
        {
            uint i;
            uint j;

            j = context.Count[0];

            if ((context.Count[0] += (uint)len << 3) < j)
            {
                context.Count[1]++;
            }

            context.Count[1] += (uint)len >> 29;
            j = (j >> 3) & 63;

            if ((j + len) > 63)
            {
                i = 64 - j;
                Array.Copy(data, 0, context.Buffer, (int)j, (int)i);
                Transform(context.State, context.Buffer, 0);

                for (; i + 63 < len; i += 64)
                {
                    Transform(context.State, data, (int)i);
                }

                j = 0;
            }
            else
            {
                i = 0;
            }

            Array.Copy(data, (int)i, context.Buffer, (int)j, (int)(len - i));
        }

        internal static byte[] Finalize(SHA1Context context)
        {
            int i;
            var finalCount = new byte[8];
            var c = new byte[1];

            for (i = 0; i < 8; i++)
            {
                finalCount[i] = (byte)((context.Count[(i >= 4 ? 0 : 1)] >> ((3 - (i & 3)) * 8)) & 255);      /* Endian independent */
            }

            c[0] = 0x80;
            Update(context, c, 1);

            while ((context.Count[0] & 504) != 448)
            {
                c[0] = 0x00;
                Update(context, c, 1);
            }

            Update(context, finalCount, 8); /* Should cause a SHA1Transform() */
            var hash = new byte[20];

            for (i = 0; i < 20; i++)
            {
                hash[i] = (byte)((context.State[i >> 2] >> ((3 - (i & 3)) * 8)) & 255);
            }

            return hash;
        }

        private static uint Rol(uint value, int bits) =>
            (value << bits) | (value >> (32 - bits));

        private static uint Blk0(int i, uint[] block) => 
            block[i] = (Rol(block[i], 24) & 0xFF00FF00) | (Rol(block[i], 8) & 0x00FF00FF);

        private static uint Blk(int i, uint[] block) =>
            block[i & 15] = Rol(block[(i + 13) & 15] ^ block[(i + 8) & 15] ^ block[(i + 2) & 15] ^ block[i & 15], 1);

        private static void R0(uint v, ref uint w, uint x, uint y, ref uint z, int i, uint[] block)
        {
            z += ((w & (x ^ y)) ^ y) + Blk0(i, block) + 0x5A827999 + Rol(v, 5);
            w = Rol(w, 30);
        }

        private static void R1(uint v, ref uint w, uint x, uint y, ref uint z, int i, uint[] block)
        {
            z += ((w & (x ^ y)) ^ y) + Blk(i, block) + 0x5A827999 + Rol(v, 5);
            w = Rol(w, 30);
        }

        private static void R2(uint v, ref uint w, uint x, uint y, ref uint z, int i, uint[] block)
        {
            z += (w ^ x ^ y) + Blk(i, block) + 0x6ED9EBA1 + Rol(v, 5);
            w = Rol(w, 30);
        }

        private static void R3(uint v, ref uint w, uint x, uint y, ref uint z, int i, uint[] block)
        {
            z += (((w | x) & y) | (w & x)) + Blk(i, block) + 0x8F1BBCDC + Rol(v, 5);
            w = Rol(w, 30);
        }

        private static void R4(uint v, ref uint w, uint x, uint y, ref uint z, int i, uint[] block)
        {
            z += (w ^ x ^ y) + Blk(i, block) + 0xCA62C1D6 + Rol(v, 5);
            w = Rol(w, 30);
        }

        private static void Copy(uint[] block, byte[] buffer, int ofs)
        {
            int i = 0;
            int j = 0;
            uint x = 0x0;

            while (i < 64)
            {
                x >>= 8;
                x |= (uint)(buffer[ofs++] << 24);
                if ((i & 0x3) == 3)
                {
                    block[j++] = x;
                    x = 0x0;
                }

                i++;
            }
        }

        private static void Transform(uint[] state, byte[] buffer, int ofs)
        {
            uint a, b, c, d, e;
            uint[] block = new uint[16];

            Copy(block, buffer, ofs);

            /* Copy context->state[] to working vars */
            a = state[0];
            b = state[1];
            c = state[2];
            d = state[3];
            e = state[4];
            /* 4 rounds of 20 operations each. Loop unrolled. */
            R0(a, ref b, c, d, ref e, 0, block);
            R0(e, ref a, b, c, ref d, 1, block);
            R0(d, ref e, a, b, ref c, 2, block);
            R0(c, ref d, e, a, ref b, 3, block);
            R0(b, ref c, d, e, ref a, 4, block);
            R0(a, ref b, c, d, ref e, 5, block);
            R0(e, ref a, b, c, ref d, 6, block);
            R0(d, ref e, a, b, ref c, 7, block);
            R0(c, ref d, e, a, ref b, 8, block);
            R0(b, ref c, d, e, ref a, 9, block);
            R0(a, ref b, c, d, ref e, 10, block);
            R0(e, ref a, b, c, ref d, 11, block);
            R0(d, ref e, a, b, ref c, 12, block);
            R0(c, ref d, e, a, ref b, 13, block);
            R0(b, ref c, d, e, ref a, 14, block);
            R0(a, ref b, c, d, ref e, 15, block);
            R1(e, ref a, b, c, ref d, 16, block);
            R1(d, ref e, a, b, ref c, 17, block);
            R1(c, ref d, e, a, ref b, 18, block);
            R1(b, ref c, d, e, ref a, 19, block);
            R2(a, ref b, c, d, ref e, 20, block);
            R2(e, ref a, b, c, ref d, 21, block);
            R2(d, ref e, a, b, ref c, 22, block);
            R2(c, ref d, e, a, ref b, 23, block);
            R2(b, ref c, d, e, ref a, 24, block);
            R2(a, ref b, c, d, ref e, 25, block);
            R2(e, ref a, b, c, ref d, 26, block);
            R2(d, ref e, a, b, ref c, 27, block);
            R2(c, ref d, e, a, ref b, 28, block);
            R2(b, ref c, d, e, ref a, 29, block);
            R2(a, ref b, c, d, ref e, 30, block);
            R2(e, ref a, b, c, ref d, 31, block);
            R2(d, ref e, a, b, ref c, 32, block);
            R2(c, ref d, e, a, ref b, 33, block);
            R2(b, ref c, d, e, ref a, 34, block);
            R2(a, ref b, c, d, ref e, 35, block);
            R2(e, ref a, b, c, ref d, 36, block);
            R2(d, ref e, a, b, ref c, 37, block);
            R2(c, ref d, e, a, ref b, 38, block);
            R2(b, ref c, d, e, ref a, 39, block);
            R3(a, ref b, c, d, ref e, 40, block);
            R3(e, ref a, b, c, ref d, 41, block);
            R3(d, ref e, a, b, ref c, 42, block);
            R3(c, ref d, e, a, ref b, 43, block);
            R3(b, ref c, d, e, ref a, 44, block);
            R3(a, ref b, c, d, ref e, 45, block);
            R3(e, ref a, b, c, ref d, 46, block);
            R3(d, ref e, a, b, ref c, 47, block);
            R3(c, ref d, e, a, ref b, 48, block);
            R3(b, ref c, d, e, ref a, 49, block);
            R3(a, ref b, c, d, ref e, 50, block);
            R3(e, ref a, b, c, ref d, 51, block);
            R3(d, ref e, a, b, ref c, 52, block);
            R3(c, ref d, e, a, ref b, 53, block);
            R3(b, ref c, d, e, ref a, 54, block);
            R3(a, ref b, c, d, ref e, 55, block);
            R3(e, ref a, b, c, ref d, 56, block);
            R3(d, ref e, a, b, ref c, 57, block);
            R3(c, ref d, e, a, ref b, 58, block);
            R3(b, ref c, d, e, ref a, 59, block);
            R4(a, ref b, c, d, ref e, 60, block);
            R4(e, ref a, b, c, ref d, 61, block);
            R4(d, ref e, a, b, ref c, 62, block);
            R4(c, ref d, e, a, ref b, 63, block);
            R4(b, ref c, d, e, ref a, 64, block);
            R4(a, ref b, c, d, ref e, 65, block);
            R4(e, ref a, b, c, ref d, 66, block);
            R4(d, ref e, a, b, ref c, 67, block);
            R4(c, ref d, e, a, ref b, 68, block);
            R4(b, ref c, d, e, ref a, 69, block);
            R4(a, ref b, c, d, ref e, 70, block);
            R4(e, ref a, b, c, ref d, 71, block);
            R4(d, ref e, a, b, ref c, 72, block);
            R4(c, ref d, e, a, ref b, 73, block);
            R4(b, ref c, d, e, ref a, 74, block);
            R4(a, ref b, c, d, ref e, 75, block);
            R4(e, ref a, b, c, ref d, 76, block);
            R4(d, ref e, a, b, ref c, 77, block);
            R4(c, ref d, e, a, ref b, 78, block);
            R4(b, ref c, d, e, ref a, 79, block);
            /* Add the working vars back into context.state[] */
            state[0] += a;
            state[1] += b;
            state[2] += c;
            state[3] += d;
            state[4] += e;
        }

        internal byte[] ComputeHash(byte[] buf)
        {
            Update(_defaultContext, buf, buf.Length);
            return Finalize(_defaultContext);
        }

        internal void Update(byte[] buf) => Update(_defaultContext, buf, buf.Length);

        internal byte[] Finalize() => Finalize(_defaultContext);

        /// <summary>
        /// The SHA1 context class.
        /// </summary>
        internal sealed class SHA1Context
        {
            internal uint[] State { get; } = new uint[]
            {
                0x67452301,
                0xEFCDAB89,
                0x98BADCFE,
                0x10325476,
                0xC3D2E1F0
            };

            internal uint[] Count { get; } = new uint[]
            {
                0,
                0
            };

            internal byte[] Buffer { get; } = new byte[64];
        }
    }
}
#endif