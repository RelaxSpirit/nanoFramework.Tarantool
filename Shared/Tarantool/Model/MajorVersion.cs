// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// <see cref="Tarantool"/> major version class.
    /// </summary>
    public class MajorVersion : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MajorVersion"/> class.
        /// </summary>
        /// <param name="majorFirst">Major version first number.</param>
        /// <param name="majorSecond">Major version second number.</param>
        internal MajorVersion(int majorFirst, int majorSecond)
        {
            MajorFirst = majorFirst;
            MajorSecond = majorSecond;
        }

        /// <summary>
        /// Gets major first number.
        /// </summary>
        public int MajorFirst { get; }

        /// <summary>
        /// Gets major second number.
        /// </summary>
        public int MajorSecond { get; }

        /// <summary>
        /// Overrides base method <see cref="object.ToString"/>.
        /// </summary>
        /// <returns>Major version string.</returns>
        public override string ToString()
        {
            return $"{MajorFirst}.{MajorSecond}";
        }

        /// <summary>
        /// Overrides base method <see cref="object.GetHashCode"/>.
        /// </summary>
        /// <returns>Major version hash code.</returns>
        public override int GetHashCode()
        {
            return MajorFirst.GetHashCode() & MajorSecond.GetHashCode();
        }

        /// <summary>
        /// Overrides base <see cref="object.Equals(object?)"/> method. Equals instances <see cref="MajorVersion"/> and <see cref="object"/> as <see cref="MajorVersion"/>.
        /// </summary>
        /// <param name="obj">Object for equal.</param>
        /// <returns><see langword="true"/> if two object is equal as <see cref="MajorVersion"/>, other <see langword="false"/>.</returns>
#nullable enable
        public override bool Equals(object? obj)
        {
            return (obj is MajorVersion major) &&
                MajorFirst == major.MajorFirst &&
                MajorSecond == major.MajorSecond;
        }
#nullable disable

        /// <summary>
        /// Compare instances <see cref="MajorVersion"/> and <see cref="object"/> as <see cref="MajorVersion"/>.
        /// </summary>
        /// <param name="obj">Object for compare.</param>
        /// <returns>The difference in the compared <see cref="MajorVersion"/> or -1 if object is not <see cref="MajorVersion"/>.</returns>
        public int CompareTo(object obj)
        {
            if (obj is MajorVersion major)
            {
                var firstMajor = MajorFirst - major.MajorFirst;
                if (firstMajor != 0)
                {
                    return firstMajor;
                }

                var secondMajor = MajorSecond - major.MajorSecond;
                if (secondMajor != 0)
                {
                    return secondMajor;
                }

                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
