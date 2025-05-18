// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
using System.Diagnostics.CodeAnalysis;

namespace nanoFramework.Tarantool.Model.Responses
{
#nullable enable
    /// <summary>
    /// <see cref="Tarantool"/> field metadata.
    /// </summary>
    public class FieldMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMetadata"/> class.
        /// </summary>
        /// <param name="name">Field name.</param>
        /// <exception cref="ArgumentNullException">If field name is null.</exception>
        internal FieldMetadata([NotNull] string name)
        {
            Name = name ?? throw new ArgumentNullException();
        }
        
        /// <summary>
        /// Gets field name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Operator '=='. Equals two instances <see cref="FieldMetadata"/>.
        /// </summary>
        /// <param name="left">First instances <see cref="FieldMetadata"/>.</param>
        /// <param name="right">Second instances <see cref="FieldMetadata"/>.</param>
        /// <returns><see langword="true"/> if two instances <see cref="FieldMetadata"/> is equal, other <see langword="false"/>.</returns>
        public static bool operator ==(FieldMetadata left, FieldMetadata right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Operator '!='. Equals two instances <see cref="FieldMetadata"/>.
        /// </summary>
        /// <param name="left">First instances <see cref="FieldMetadata"/>.</param>
        /// <param name="right">Second instances <see cref="FieldMetadata"/>.</param>
        /// <returns><see langword="true"/> if two instances <see cref="FieldMetadata"/> is not equal, other <see langword="false"/>.</returns>
        public static bool operator !=(FieldMetadata left, FieldMetadata right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Overrides base <see cref="object.ToString"/> method.
        /// </summary>
        /// <returns>Field name.</returns>
        public override string ToString() => Name;

        /// <summary>
        /// Overrides base <see cref="object.Equals(object?)"/> method. Equals instances <see cref="FieldMetadata"/> and <see cref="object"/> as <see cref="FieldMetadata"/>.
        /// </summary>
        /// <param name="obj">Object for equal.</param>
        /// <returns><see langword="true"/> if two object is equal as <see cref="FieldMetadata"/>, other <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((FieldMetadata)obj);
        }

        /// <summary>
        /// Overrides base <see cref="object.GetHashCode"/> method. Return hash code for <see cref="Name"/>.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        private bool Equals(FieldMetadata other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Name, other.Name);
        }
    }
}
