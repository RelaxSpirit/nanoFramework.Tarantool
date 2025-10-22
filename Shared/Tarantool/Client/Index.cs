// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
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
    /// The <see cref="Tarantool"/> index class.
    /// </summary>
#nullable enable
    internal class Index : IIndex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        /// <param name="id">Index id.</param>
        /// <param name="spaceId">Space id.</param>
        /// <param name="name">Index name.</param>
        /// <param name="unique"><see langword="true"/> if created index must be unique.</param>
        /// <param name="type">Index type.</param>
        /// <param name="parts">Index parts array.</param>
        internal Index(uint id, uint spaceId, string name, bool unique, IndexType type, IndexPart[] parts)
        {
            Id = id;
            SpaceId = spaceId;
            Name = name;
            Unique = unique;
            Type = type;
            Parts = parts;
        }
        
        internal ILogicalConnection? LogicalConnection { get; set; }

        public uint Id { get; }

        public uint SpaceId { get; }

        public string Name { get; }

        public bool Unique { get; }

        public IndexType Type { get; }

        public IndexPart[] Parts { get; }

        public DataResponse? Select(TarantoolTuple key, SelectOptions options, TarantoolTupleType? responseType = null)
        {
            var selectRequest = new SelectRequest(
                SpaceId,
                Id,
                options.Limit,
                options.Offset,
                options.Iterator,
                key);

            if (responseType != null)
            {
                return LogicalConnection?.SendRequest(selectRequest, Timeout.InfiniteTimeSpan, TarantoolContext.Instance.GetTarantoolTupleArrayType(responseType));
            }
            else
            {
                return LogicalConnection?.SendRequest(selectRequest, Timeout.InfiniteTimeSpan, responseType);
            }
        }

        public DataResponse? Select(TarantoolTuple key, TarantoolTupleType? responseType = null)
        {
            var selectRequest = new SelectRequest(
                SpaceId,
                Id,
                SelectOptions.Default.Limit,
                SelectOptions.Default.Offset,
                SelectOptions.Default.Iterator,
                key);

            if (responseType != null)
            {
                return LogicalConnection?.SendRequest(selectRequest, Timeout.InfiniteTimeSpan, TarantoolContext.Instance.GetTarantoolTupleArrayType(responseType));
            }
            else
            {
                return LogicalConnection?.SendRequest(selectRequest, Timeout.InfiniteTimeSpan, responseType);
            }
        }

        public DataResponse? Delete(TarantoolTuple key, TarantoolTupleType? responseType = null)
        {
            var deleteRequest = new DeleteRequest(SpaceId, Id, key);

            if (responseType != null)
            {
                return LogicalConnection?.SendRequest(deleteRequest, Timeout.InfiniteTimeSpan, TarantoolContext.Instance.GetTarantoolTupleArrayType(responseType));
            }
            else
            {
                return LogicalConnection?.SendRequest(deleteRequest, Timeout.InfiniteTimeSpan, responseType);
            }
        }

        public DataResponse? Update(TarantoolTuple key, UpdateOperation[] updateOperations, TarantoolTupleType? responseType = null)
        {
            var updateRequest = new UpdateRequest(
                SpaceId,
                Id,
                key,
                updateOperations);

            if (responseType != null)
            {
                return LogicalConnection?.SendRequest(updateRequest, Timeout.InfiniteTimeSpan, TarantoolContext.Instance.GetTarantoolTupleArrayType(responseType));
            }
            else
            {
                return LogicalConnection?.SendRequest(updateRequest, Timeout.InfiniteTimeSpan, responseType);
            }
        }

        public TarantoolTuple? MinTuple([NotNull] TarantoolTupleType responseType)
        {
            return MinTuple(null, responseType);
        }

        public TarantoolTuple? MinTuple(TarantoolTuple? key, [NotNull] TarantoolTupleType responseType)
        {
            var minResponse = Min(key, responseType);
            if (minResponse != null && minResponse.Data.Length > 0)
            {
                return (TarantoolTuple)minResponse.Data[0];
            }
            else
            {
                return null;
            }
        }

        public TarantoolTuple? MaxTuple([NotNull] TarantoolTupleType responseType)
        {
            return MaxTuple(null, responseType);
        }

        public TarantoolTuple? MaxTuple(TarantoolTuple? key, [NotNull] TarantoolTupleType responseType)
        {
            var maxResponse = Max(key, responseType);
            if (maxResponse != null && maxResponse.Data.Length > 0)
            {
                return (TarantoolTuple)maxResponse.Data[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Overrides base class method <see cref="object.ToString"/>.
        /// </summary>
        /// <returns>Id, name and space id string.</returns>
        public override string ToString()
        {
            return $"{Name}, id={Id}, spaceId={SpaceId}";
        }

        private DataResponse? Min(TarantoolTuple? key, TarantoolTupleType? responseType = null)
        {
            if (Type != IndexType.Tree)
            {
                throw ExceptionHelper.WrongIndexType("TREE", "min");
            }

            var iterator = key == null ? Iterator.Eq : Iterator.Ge;

            var selectPacket = new SelectRequest(SpaceId, Id, 1, 0, iterator, key);

            if (responseType != null)
            {
                return LogicalConnection?.SendRequest(selectPacket, Timeout.InfiniteTimeSpan, new TarantoolTupleArrayType(responseType));
            }
            else
            {
                return LogicalConnection?.SendRequest(selectPacket, Timeout.InfiniteTimeSpan, responseType);
            }
        }

        private DataResponse? Max(TarantoolTuple? key, TarantoolTupleType? responseType = null)
        {
            if (Type != IndexType.Tree)
            {
                throw ExceptionHelper.WrongIndexType("TREE", "max");
            }

            var iterator = key == null ? Iterator.Req : Iterator.Le;

            var selectPacket = new SelectRequest(SpaceId, Id, 1, 0, iterator, key);

            if (responseType != null)
            {
                return LogicalConnection?.SendRequest(selectPacket, Timeout.InfiniteTimeSpan, new TarantoolTupleArrayType(responseType));
            }
            else
            {
                return LogicalConnection?.SendRequest(selectPacket, Timeout.InfiniteTimeSpan, responseType);
            }
        }
    }
}
