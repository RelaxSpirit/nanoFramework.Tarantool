// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using nanoFramework.Tarantool.Helpers;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// <see cref="Tarantool"/> version struct.
    /// </summary>
    public struct TarantoolVersion : IComparable
    {
        /// <summary>
        /// Empty <see cref="TarantoolVersion"/> instance struct.
        /// </summary>
        public static readonly TarantoolVersion Empty = new TarantoolVersion(new MajorVersion(0, 0), 0, 0, string.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolVersion"/> struct.
        /// </summary>
        /// <param name="major">Major <see cref="Tarantool"/> version.</param>
        /// <param name="minor">Minor <see cref="Tarantool"/> version number.</param>
        /// <param name="build">Build <see cref="Tarantool"/> version number.</param>
        /// <param name="commitHash">Commit hash <see cref="Tarantool"/> version.</param>
        internal TarantoolVersion(MajorVersion major, int minor, int build, string commitHash)
        {
            Major = major;
            Minor = minor;
            Build = build;
            CommitHash = commitHash;
        }

        /// <summary>
        /// Gets major version.
        /// </summary>
        public MajorVersion Major { get; }

        /// <summary>
        /// Gets minor version number.
        /// </summary>
        public int Minor { get; }

        /// <summary>
        /// Gets build version number.
        /// </summary>
        public int Build { get; }

        /// <summary>
        /// Gets commit hash string.
        /// </summary>
        public string CommitHash { get; }

        /// <summary>
        /// Operator '&lt;'. First is smaller than the other instances <see cref="TarantoolVersion"/>.
        /// </summary>
        /// <param name="left">First instances <see cref="TarantoolVersion"/>.</param>
        /// <param name="right">Second instances <see cref="TarantoolVersion"/>.</param>
        /// <returns><see langword="true"/> if first instances <see cref="TarantoolVersion"/> is smaller, other <see langword="false"/>.</returns>
        public static bool operator <(TarantoolVersion left, TarantoolVersion right) => left.CompareTo(right) < 0;

        /// <summary>
        /// Operator '&gt;'. First is bigger than the other instances <see cref="TarantoolVersion"/>.
        /// </summary>
        /// <param name="left">First instances <see cref="TarantoolVersion"/>.</param>
        /// <param name="right">Second instances <see cref="TarantoolVersion"/>.</param>
        /// <returns><see langword="true"/> if first instances <see cref="TarantoolVersion"/> is bigger, other <see langword="false"/>.</returns>
        public static bool operator >(TarantoolVersion left, TarantoolVersion right) => left.CompareTo(right) > 0;

        /// <summary>
        /// Operator '&lt;='. First is smaller or equal than the other instances <see cref="TarantoolVersion"/>.
        /// </summary>
        /// <param name="left">First instances <see cref="TarantoolVersion"/>.</param>
        /// <param name="right">Second instances <see cref="TarantoolVersion"/>.</param>
        /// <returns><see langword="true"/> if first instances <see cref="TarantoolVersion"/> is smaller or equal, other <see langword="false"/>.</returns>
        public static bool operator <=(TarantoolVersion left, TarantoolVersion right) => left.CompareTo(right) <= 0;

        /// <summary>
        /// Operator '&gt;='. First is bigger or equal than the other instances <see cref="TarantoolVersion"/>.
        /// </summary>
        /// <param name="left">First instances <see cref="TarantoolVersion"/>.</param>
        /// <param name="right">Second instances <see cref="TarantoolVersion"/>.</param>
        /// <returns><see langword="true"/> if first instances <see cref="TarantoolVersion"/> is bigger or equal, other <see langword="false"/>.</returns>
        public static bool operator >=(TarantoolVersion left, TarantoolVersion right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Operator '=='. Equals two instances <see cref="TarantoolVersion"/>.
        /// </summary>
        /// <param name="left">First instances <see cref="TarantoolVersion"/>.</param>
        /// <param name="right">Second instances <see cref="TarantoolVersion"/>.</param>
        /// <returns><see langword="true"/> if two instances <see cref="TarantoolVersion"/> is equal, other <see langword="false"/>.</returns>
        public static bool operator ==(TarantoolVersion left, TarantoolVersion right) => TarantoolVersion.Equals(left, right);

        /// <summary>
        /// Operator 'Q='. Equals two instances <see cref="TarantoolVersion"/>.
        /// </summary>
        /// <param name="left">First instances <see cref="TarantoolVersion"/>.</param>
        /// <param name="right">Second instances <see cref="TarantoolVersion"/>.</param>
        /// <returns><see langword="true"/> if two instances <see cref="TarantoolVersion"/> is not equal, other <see langword="false"/>.</returns>
        public static bool operator !=(TarantoolVersion left, TarantoolVersion right) => !TarantoolVersion.Equals(left, right);

        /// <summary>
        /// Implicit operator from <see cref="MajorVersion"/>
        /// </summary>
        /// <param name="major">Major <see cref="Tarantool"/> version.</param>
        public static implicit operator TarantoolVersion(MajorVersion major)
        {
            return new TarantoolVersion(major, 0, 0, string.Empty);
        }

        /// <summary>
        /// Implicit operator from <see cref="string"/>
        /// </summary>
        /// <param name="version"><see cref="string"/> <see cref="Tarantool"/> version.</param>
        public static implicit operator TarantoolVersion(string version)
        {
            return Parse(version);
        }

        /// <summary>
        /// Parse string to <see cref="TarantoolVersion"/> instance.
        /// </summary>
        /// <param name="stringVersion"><see cref="Tarantool"/> version string.</param>
        /// <returns>New <see cref="TarantoolVersion"/> instance.</returns>
        public static TarantoolVersion Parse(string stringVersion)
        {
            if (string.IsNullOrEmpty(stringVersion))
            {
                throw ExceptionHelper.VersionCantBeEmpty();
            }

            // 1.7.6-7-gce1a37741
            var parts = stringVersion.Split(new char[] { '.', '-', 'g' });

            return new TarantoolVersion(new MajorVersion(int.Parse(parts[0]), int.Parse(parts[1])), int.Parse(parts[2]), int.Parse(parts[3]), parts[5]);
        }

        /// <summary>
        /// Overrides base method <see cref="object.ToString"/>
        /// </summary>
        /// <returns><see cref="Tarantool"/> version string.</returns>
        public override string ToString() => $"{Major}.{Minor}-{Build}-g{CommitHash}";

        /// <summary>
        /// Overrides base <see cref="object.Equals(object?)"/> method. Equals instances <see cref="TarantoolVersion"/> and <see cref="object"/> as <see cref="TarantoolVersion"/>.
        /// </summary>
        /// <param name="obj">Object for equal.</param>
        /// <returns><see langword="true"/> if two object is equal as <see cref="TarantoolVersion"/>, other <see langword="false"/>.</returns>
#nullable enable
        public override bool Equals(object? obj)
        {
            return obj is TarantoolVersion tarantoolVersion && Equals(tarantoolVersion);
        }

        /// <summary>
        /// Compare instances <see cref="TarantoolVersion"/> and <see cref="object"/> as <see cref="TarantoolVersion"/>.
        /// </summary>
        /// <param name="obj">Object for compare.</param>
        /// <returns>The difference in the compared <see cref="TarantoolVersion"/>.</returns>
        /// <exception cref="ArgumentException">If object is not <see cref="TarantoolVersion"/>.</exception>
        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                return 1;
            }

            return obj is TarantoolVersion version
                ? CompareTo(version)
#if NANOFRAMEWORK_1_0
                : throw new ArgumentException();
#else
                : throw new ArgumentException($"Object must be of type {nameof(TarantoolVersion)}");
#endif
        }
#nullable disable

        /// <summary>
        /// Overrides base method <see cref="object.GetHashCode"/>
        /// </summary>
        /// <returns><see cref="Tarantool"/> version hash code.</returns>
        public override int GetHashCode()
        {
            return Major.GetHashCode() & Minor.GetHashCode() & Build.GetHashCode() & CommitHash.GetHashCode();
        }

        private bool Equals(TarantoolVersion other)
        {
           return Major.Equals(other.Major) && Minor == other.Minor && Build == other.Build && string.Equals(CommitHash, other.CommitHash);
        }

        private int CompareTo(TarantoolVersion other)
        {
            var majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0)
            {
                return majorComparison;
            }

            var minorComparison = Minor - other.Minor;
            if (minorComparison != 0)
            {
                return minorComparison;
            }

            var buildComparison = Build - other.Build;
            if (buildComparison != 0)
            {
                return buildComparison;
            }

            if (CommitHash == null || other.CommitHash == null)
            {
                return 0;
            }

            var hashComparison = string.Compare(CommitHash, other.CommitHash);
            if (hashComparison != 0)
            {
                throw ExceptionHelper.CantCompareBuilds(this, other);
            }

            return 0;
        }
    }
}
