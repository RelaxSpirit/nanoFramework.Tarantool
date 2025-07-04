﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Threading;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;
using nanoFramework.Tarantool.Model.Responses;
using nanoFramework.Tarantool.Model.UpdateOperations;

namespace nanoFramework.Tarantool.Client
{
    /// <summary>
    /// The <see cref="Tarantool"/> space class.
    /// </summary>
    internal class Space : ISpace
    {
        private Hashtable _indexByName = new Hashtable();
        private Hashtable _indexById = new Hashtable();

        /// <summary>
        /// Initializes a new instance of the <see cref="Space"/> class.
        /// </summary>
        /// <param name="id">Space id.</param>
        /// <param name="fieldCount">Space fields count.</param>
        /// <param name="name">Space name.</param>
        /// <param name="engine">Space engine type.</param>
        /// <param name="fields">Space fields array.</param>
        internal Space(uint id, uint fieldCount, string name, StorageEngine engine, SpaceField[] fields)
        {
            Id = id;
            FieldCount = fieldCount;
            Name = name;
            Engine = engine;
            Fields = fields;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space id.
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space field count.
        /// </summary>
        public uint FieldCount { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space storage engine.
        /// </summary>
        public StorageEngine Engine { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space indexes.
        /// </summary>
        public ICollection Indices => _indexByName.Values;

        /// <summary>
        /// Gets <see cref="Tarantool"/> space fields.
        /// </summary>
        public ISpaceField[] Fields { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space index by name.
        /// </summary>
        /// <param name="name">Index name.</param>
        /// <returns><see cref="IIndex"/> interface.</returns>
        public IIndex this[string name]
        {
            get
            {
                var index = _indexByName[name];
                if (index == null)
                {
                    index = GetIndexInfo(name);
                    if (index == null)
                    {
                        throw ExceptionHelper.InvalidIndexName(name, Name);
                    }
                    else
                    {
                        Index innerIndex = (Index)index;
                        innerIndex.LogicalConnection = LogicalConnection;

                        _indexByName[innerIndex.Name] = innerIndex;
                        _indexById[innerIndex.Id] = innerIndex;
                    }
                }

                return (IIndex)index;
            }
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> space index by id.
        /// </summary>
        /// <param name="id">Index id.</param>
        /// <returns><see cref="IIndex"/> interface.</returns>
        public IIndex this[uint id]
        {
            get
            {
                var index = _indexById[id];
                if (index == null)
                {
                    throw ExceptionHelper.InvalidIndexId(id, Name);
                }

                return (IIndex)index;
            }
        }    
        
        /// <summary>
        /// Gets or sets network connection.
        /// </summary>
#nullable enable
        internal ILogicalConnection? LogicalConnection { get; set; } = null;

        internal void SetIndices(Index[] value)
        {
            var indByName = new Hashtable();
            var indById = new Hashtable();

            if (value != null)
            {
                foreach (var index in value)
                {
                    indByName[index.Name] = index;
                    indById[index.Id] = index;
                    index.LogicalConnection = LogicalConnection;
                }
            }

            _indexByName = indByName;
            _indexById = indById;
        }

        public DataResponse? Insert(TarantoolTuple tuple)
        {
            var insertRequest = new InsertRequest(Id, tuple);
            return LogicalConnection?.SendRequest(insertRequest, Timeout.InfiniteTimeSpan, TarantoolContext.Instance.GetTarantoolTupleArrayType((TarantoolTupleType)tuple.GetType()));
        }

        public DataResponse? Select(TarantoolTuple selectKey, TarantoolTupleType? tarantoolTupleType = null)
        {
            var selectRequest = new SelectRequest(Id, Schema.PrimaryIndexId, uint.MaxValue, 0, Iterator.Eq, selectKey);
            return LogicalConnection?.SendRequest(selectRequest, Timeout.InfiniteTimeSpan, tarantoolTupleType != null ? TarantoolContext.Instance.GetTarantoolTupleArrayType(tarantoolTupleType) : null);
        }
        
        public DataResponse? Replace(TarantoolTuple tuple, TarantoolTupleType? tarantoolTupleType = null)
        {
            var replaceRequest = new ReplaceRequest(Id, tuple);
            return LogicalConnection?.SendRequest(replaceRequest, Timeout.InfiniteTimeSpan, tarantoolTupleType == null ? TarantoolContext.Instance.GetTarantoolTupleArrayType((TarantoolTupleType)tuple.GetType()) : TarantoolContext.Instance.GetTarantoolTupleArrayType(tarantoolTupleType));
        }

        public DataResponse? Update(TarantoolTuple key, UpdateOperation[] updateOperations, TarantoolTupleType? tarantoolTupleType = null)
        {
            var updateRequest = new UpdateRequest(Id, Schema.PrimaryIndexId, key, updateOperations);
            return LogicalConnection?.SendRequest(updateRequest, Timeout.InfiniteTimeSpan, tarantoolTupleType != null ? (Type)TarantoolContext.Instance.GetTarantoolTupleArrayType(tarantoolTupleType) : null);
        }

        public DataResponse? Delete(TarantoolTuple key, TarantoolTupleType? tarantoolTupleType = null)
        {
            var deleteRequest = new DeleteRequest(Id, Schema.PrimaryIndexId, key);
            return LogicalConnection?.SendRequest(deleteRequest, Timeout.InfiniteTimeSpan, tarantoolTupleType);
        }

        public TarantoolTuple? GetTuple(TarantoolTuple key, TarantoolTupleType? tarantoolTupleType)
        {
            var response = Get(key, tarantoolTupleType);
            if (response != null && response.Data.Length > 0)
            {
                return (TarantoolTuple)response.Data[0];
            }
            else
            {
                return null;
            }
        }

        public TarantoolTuple? PutTuple(TarantoolTuple tuple, TarantoolTupleType? tarantoolTupleType)
        {
            var response = Put(tuple, tarantoolTupleType ?? (TarantoolTupleType)tuple.GetType());
            if (response != null && response.Data.Length > 0)
            {
                return (TarantoolTuple)response.Data[0];
            }
            else
            {
                return null;
            }
        }

        public void Upsert(TarantoolTuple tuple, UpdateOperation[] updateOperations)
        {
            var upsertRequest = new UpsertRequest(Id, tuple, updateOperations);
            LogicalConnection?.SendRequestWithEmptyResponse(upsertRequest, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// Overrides base class method <see cref="object.ToString"/>
        /// </summary>
        /// <returns>Space name and id string.</returns>
        public override string ToString()
        {
            return $"{Name}, id={Id}";
        }

        private DataResponse? Put(TarantoolTuple tuple, TarantoolTupleType? tarantoolTupleType = null)
        {
            return Replace(tuple, tarantoolTupleType);
        }

        private DataResponse? Get(TarantoolTuple key, TarantoolTupleType? tarantoolTupleType = null)
        {
            var selectRequest = new SelectRequest(Id, Schema.PrimaryIndexId, 1, 0, Iterator.Eq, key);
            return LogicalConnection?.SendRequest(selectRequest, Timeout.InfiniteTimeSpan, tarantoolTupleType != null ? (Type)TarantoolContext.Instance.GetTarantoolTupleArrayType(tarantoolTupleType) : null);
        }

#nullable enable
        private Index? GetIndexInfo(string name)
        {
            var request = new SelectRequest(Schema.VIndex, 2, 1, 0, Iterator.Eq, TarantoolTuple.Create(Id, name));

            var response = LogicalConnection?.SendRequest(request, Timeout.InfiniteTimeSpan, typeof(Index[]));

            if (response != null && response.Data is Index[] indexes)
            {
                return indexes[0];
            }
            else
            {
                return null;
            }
        }
    }
}
