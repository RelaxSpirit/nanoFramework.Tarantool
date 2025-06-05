// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.Tarantool.Dto;

namespace nanoFramework.Tarantool.Model
{
    /// <summary>
    /// <see cref="Tarantool"/> node.
    /// </summary>
    public class TarantoolNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TarantoolNode" /> class.
        /// </summary>
        /// <param name="url">URL by <see cref="Tarantool"/> node.</param>
        public TarantoolNode([NotNull] string url)
        {
            Uri = new TarantoolUri(url);
        }

        /// <summary>
        /// Gets <see cref="TarantoolUri"/> path by <see cref="Tarantool"/> node.
        /// </summary>
        public TarantoolUri Uri { get; }

        /// <summary>
        /// Parse string URL for <see cref="Tarantool"/>
        /// </summary>
        /// <param name="url">String URL <see cref="Tarantool"/></param>
        /// /// <param name="uriParceException">Out exception <see cref="ArgumentException"/> if uri is not parse.</param>
        /// <returns>Instance of the <see cref="TarantoolNode" /> or <see langword="null"/> if string URL is not parsed.</returns>
#nullable enable
        public static TarantoolNode? TryParse(string url, out ArgumentException? uriParceException)
        {
            try
            {
                uriParceException = null;
                return new TarantoolNode(url);
            }
            catch (Exception e)
            {
                uriParceException = new ArgumentException($"Url parsing failed. Url: {url}", e);
                return null;
            }
        }
    }
}
