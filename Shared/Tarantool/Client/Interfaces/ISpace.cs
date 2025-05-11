// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Responses;
using nanoFramework.Tarantool.Model.UpdateOperations;

namespace nanoFramework.Tarantool.Client.Interfaces
{
    /// <summary>
    /// CRUD operations in <see cref="Tarantool"/> are implemented by the box.space submodule.
    /// It has the data-manipulation functions select, insert, replace, update, upsert, delete, get, put.
    /// It also has members, such as id, and whether or not a space is enabled.
    /// </summary>
    public interface ISpace
    {
        /// <summary>
        /// Gets numeric identifier of <see cref="Tarantool"/> space.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Gets required number of fields.
        /// </summary>
        uint FieldCount { get; }

        /// <summary>
        /// Gets a <see cref="Tarantool"/> space name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a <see cref="Tarantool"/> <see cref="StorageEngine"/> value.
        /// </summary>
        StorageEngine Engine { get; }

        /// <summary>
        /// Gets a <see cref="IIndex"/> interfaces collection for <see cref="Tarantool"/> container of space’s indexes.
        /// </summary>
        ICollection Indices { get; }

        /// <summary>
        /// Gets a <see cref="ISpaceField"/> interfaces by <see cref="Tarantool"/> space fields.
        /// </summary>
        ISpaceField[] Fields { get; }

        /// <summary>
        /// Gets <see cref="IIndex"/> interface by <see cref="Tarantool"/> index name.
        /// </summary>
        /// <param name="name"><see cref="Tarantool"/> index name.</param>
        /// <returns><see cref="IIndex"/> interface.</returns>
        IIndex this[string name] { get; }

        /// <summary>
        /// Gets <see cref="IIndex"/> interface by <see cref="Tarantool"/> index id.
        /// </summary>
        /// <param name="id"><see cref="Tarantool"/> index id.</param>
        /// <returns><see cref="IIndex"/> interface.</returns>
        IIndex this[uint id] { get; }

        /// <summary>
        /// Insert a <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="tuple"><see cref="Tarantool"/> tuple.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
#nullable enable
        DataResponse? Insert(TarantoolTuple tuple);

        /// <summary>
        /// Select one or more <see cref="Tarantool"/> tuples.
        /// </summary>
        /// <param name="selectKey"><see cref="Tarantool"/> tuple select key.</param>
        /// <param name="tarantoolTupleResponseType">Responses <see cref="Tarantool"/> tuple type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Select(TarantoolTuple selectKey, TarantoolTupleType? tarantoolTupleResponseType = null);

        /// <summary>
        /// Select a <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> tuple get key.</param>
        /// <param name="tarantoolTupleResponseType">Responses <see cref="Tarantool"/> tuple type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Get(TarantoolTuple key, TarantoolTupleType? tarantoolTupleResponseType = null);

        /// <summary>
        /// Create a replace trigger with a function that cannot change the <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="tuple">Replace <see cref="Tarantool"/> tuple.</param>
        /// <param name="tarantoolTupleResponseType">Responses <see cref="Tarantool"/> tuple type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Replace(TarantoolTuple tuple, TarantoolTupleType? tarantoolTupleResponseType = null);

        /// <summary>
        /// Insert or replace a <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="tuple">New <see cref="Tarantool"/> tuple.</param>
        /// <param name="tarantoolTupleResponseType">Responses <see cref="Tarantool"/> tuple type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Put(TarantoolTuple tuple, TarantoolTupleType? tarantoolTupleResponseType = null);

        /// <summary>
        /// Update a <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> tuple update key.</param>
        /// <param name="updateOperations"><see cref="Tarantool"/> update operations.</param>
        /// <param name="tarantoolTupleResponseType">Responses <see cref="Tarantool"/> tuple type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Update(TarantoolTuple key, UpdateOperation[] updateOperations, TarantoolTupleType? tarantoolTupleResponseType = null);

        /// <summary>
        /// Delete all <see cref="Tarantool"/> tuples.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> tuple delete key.</param>
        /// <param name="tarantoolTupleResponseType">Responses <see cref="Tarantool"/> tuple type.</param>
        /// <returns><see cref="Tarantool"/> data response.</returns>
        DataResponse? Delete(TarantoolTuple key, TarantoolTupleType? tarantoolTupleResponseType = null);

        /// <summary>
        /// Select a <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="key"><see cref="Tarantool"/> tuple get key.</param>
        /// <param name="tarantoolTupleType">Responses <see cref="Tarantool"/> tuple type.</param>
        /// <returns>Getting <see cref="TarantoolTuple"/> instance.</returns>
        TarantoolTuple? GetTuple(TarantoolTuple key, [NotNull] TarantoolTupleType tarantoolTupleType);

        /// <summary>
        /// Insert or replace a <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="tuple">New <see cref="Tarantool"/> tuple.</param>
        /// <param name="tarantoolTupleType">Responses <see cref="Tarantool"/> tuple type.</param>
        /// <returns>Putting <see cref="TarantoolTuple"/> instance.</returns>
        TarantoolTuple? PutTuple(TarantoolTuple tuple, [NotNull] TarantoolTupleType tarantoolTupleType);
#nullable disable

        /// <summary>
        /// Update a <see cref="Tarantool"/> tuple.
        /// </summary>
        /// <param name="tuple"><see cref="Tarantool"/> tuple to update.</param>
        /// <param name="updateOperations"><see cref="Tarantool"/> update operations.</param>
        void Upsert(TarantoolTuple tuple, UpdateOperation[] updateOperations);
    }
}
