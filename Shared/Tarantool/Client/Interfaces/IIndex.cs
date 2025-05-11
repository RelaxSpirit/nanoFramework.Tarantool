// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Responses;
using nanoFramework.Tarantool.Model.UpdateOperations;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    /// <summary>
    /// The interface box.index submodule provides read-only access for index definitions and index keys.
    /// Indexes are contained in box.space.space-name.index array within each space object.
    /// They provide an API for ordered iteration over tuples. This API is a direct binding to corresponding
    /// methods of index objects of type box.index in the storage engine.
    /// </summary>
    public interface IIndex
    {
        /// <summary>
        /// Gets <see cref="Tarantool"/> index id.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> index space id.
        /// </summary>
        uint SpaceId { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> index name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="Tarantool"/> index is unique.
        /// </summary>
        bool Unique { get; }

        /// <summary>
        /// Gets <see cref="IndexType"/> value.
        /// </summary>
        IndexType Type { get; }

        /// <summary>
        /// Gets array <see cref="IndexPart"/> value.
        /// </summary>
        IndexPart[] Parts { get; }

        /// <summary>
        /// Prepare for <see cref="Tarantool"/> index iterating.
        /// Search for a tuple or a set of tuples via the given index, and allow iterating over one tuple at a time.
        /// </summary>
        /// <param name="value">The parameter specifies what must match within the index.</param>
        /// <param name="iterator"><see cref="Iterator"/> value. The parameter specifies the rule for matching and ordering. Different index types support different iterators. For example, a TREE index maintains a strict order of keys and can return all tuples in ascending or descending order, starting from the specified key. Other index types, however, do not support ordering.</param>
        /// <returns>The iterator, which can be used in a loop.</returns>
        /// <remarks>TODO not implemented.</remarks>
        object Pairs(object value, Iterator iterator);

        /// <summary>
        /// Search for a <see cref="Tarantool"/> tuple or a set of tuples by the current index. To search by the primary index in the specified space.
        /// </summary>
        /// <param name="key">A <see cref="Tarantool"/> tuple value to be matched against the index key, which may be multi-part.</param>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <param name="options"><see cref="Tarantool"/> <see cref="SelectOptions"/>.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
#nullable enable
        DataResponse? Select(TarantoolTuple key, TarantoolTupleType? responseType = null, SelectOptions? options = null);

        /// <summary>
        /// Find the minimum value in the specified index.
        /// </summary>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Min(TarantoolTupleType? responseType = null);

        /// <summary>
        /// Find the minimum value in the specified index.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> values to be matched against the index key.</param>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Min(TarantoolTuple key, TarantoolTupleType? responseType = null);

        /// <summary>
        /// Find the maximum value in the specified index.
        /// </summary>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Max(TarantoolTupleType? responseType = null);

        /// <summary>
        /// Find the maximum value in the specified index.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> values to be matched against the index key.</param>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Max(TarantoolTuple key, TarantoolTupleType? responseType = null);

        /// <summary>
        /// Delete a <see cref="Tarantool"/> tuple identified by a key.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> values to be matched against the index key.</param>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Delete(TarantoolTuple key, TarantoolTupleType? responseType = null);

        /// <summary>
        /// Update a <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> values to be matched against the index key.</param>
        /// <param name="updateOperations"><see cref="Tarantool"/> update operations.</param>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Update(TarantoolTuple key, UpdateOperation[] updateOperations, TarantoolTupleType? responseType = null);

        /// <summary>
        /// Find the minimum value in the specified index.
        /// </summary>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> <see cref="TarantoolTuple"/> response.</returns>
        TarantoolTuple? MinTuple([NotNull] TarantoolTupleType responseType);

        /// <summary>
        /// Find the minimum value in the specified index.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> values to be matched against the index key.</param>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> <see cref="TarantoolTuple"/> response.</returns>
        TarantoolTuple? MinTuple(TarantoolTuple key, [NotNull] TarantoolTupleType responseType);

        /// <summary>
        /// Find the minimum value in the specified index.
        /// </summary>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> <see cref="TarantoolTuple"/> response.</returns>
        TarantoolTuple? MaxTuple([NotNull] TarantoolTupleType responseType);

        /// <summary>
        /// Find the minimum value in the specified index.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> values to be matched against the index key.</param>
        /// <param name="responseType"><see cref="Tarantool"/> tuple return data type.</param>
        /// <returns><see cref="Tarantool"/> <see cref="TarantoolTuple"/> response.</returns>
        TarantoolTuple? MaxTuple(TarantoolTuple key, [NotNull] TarantoolTupleType responseType);
#nullable disable

        /// <summary>
        /// Find a random value in the specified index.
        /// This method is useful when it’s important to get insight into data distribution in an index without having to iterate over the entire data set.
        /// </summary>
        /// <param name="seed">An arbitrary non-negative integer.</param>
        /// <returns>The <see cref="Tarantool"/> tuple for the random key in the index.</returns>
        /// <remarks>TODO not implemented.</remarks>
        TarantoolTuple Random(int seed);

        /// <summary>
        /// Iterate over an index, counting the number of <see cref="Tarantool"/> tuples which match the key-value.
        /// </summary>
        /// <param name="key"><see cref="TarantoolTuple"/> values to be matched against the index key.</param>
        /// <param name="it">Comparison method, default <see cref="Iterator.Eq"/>.</param>
        /// <returns>The number of matching tuples.</returns>
        /// <remarks>TODO not implemented.</remarks>
        uint Count(TarantoolTuple key, Iterator it = Iterator.Eq);
    }
}
