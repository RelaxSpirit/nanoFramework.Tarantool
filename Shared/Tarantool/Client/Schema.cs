// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Helpers;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Enums;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Client
{
    /// <summary>
    /// The <see cref="Tarantool"/> schema class.
    /// </summary>
    internal class Schema : ISchema
    {
        internal const int VSpace = 0x119;
        internal const int VIndex = 0x121;
        internal const uint PrimaryIndexId = 0;

        private readonly ILogicalConnection _logicalConnection;

        private Hashtable _spaceByName = new Hashtable();
        private Hashtable _spaceById = new Hashtable();

        /// <summary>
        /// Initializes a new instance of the <see cref="Schema"/> class.
        /// </summary>
        /// <param name="logicalConnection">Network logical connection.</param>
        internal Schema(ILogicalConnection logicalConnection)
        {
            _logicalConnection = logicalConnection;
        }

        public ISpace this[string name]
        {
            get
            {
                var space = _spaceByName[name];
                if (space == null)
                {
                    space = GetSpaceInfo(name);

                    if (space == null)
                    {
                        throw ExceptionHelper.InvalidSpaceName(name);
                    }
                    else
                    {
                        Space innerSpace = (Space)space;
                        innerSpace.LogicalConnection = _logicalConnection;

                        _spaceByName[innerSpace.Name] = innerSpace;
                        _spaceById[innerSpace.Id] = innerSpace;
                    }
                }

                return (ISpace)space;
            }
        }

        public ISpace this[uint id]
        {
            get
            {
                var space = _spaceById[id];
                if (space == null)
                {
                    throw ExceptionHelper.InvalidSpaceId(id);
                }

                return (ISpace)space;
            }
        }

        public DateTime LastReloadTime { get; private set; }

        public void Reload()
        {
            var indByName = new Hashtable();
            var indById = new Hashtable();

            var spaces = (Space[])Select(VSpace, typeof(Space[]));
            foreach (var space in spaces)
            {
                indByName[space.Name] = space;
                indById[space.Id] = space;
                space.LogicalConnection = _logicalConnection;
                space.SetIndices((Index[])Select(VIndex, typeof(Index[]), Iterator.Eq, space.Id));
            }

            _spaceByName = indByName;
            _spaceById = indById;
            LastReloadTime = DateTime.UtcNow;
        }

        public ICollection Spaces => _spaceByName.Values;

        private object[] Select(uint spaceId, Type responseType, Iterator iterator = Iterator.All, uint id = 0u)
        {
            var request = new SelectRequest(spaceId, PrimaryIndexId, uint.MaxValue, 0, iterator, TarantoolTuple.Create(id));

            var response = _logicalConnection.SendRequest(request, TimeSpan.Zero, responseType);

            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return (object[])Array.CreateInstance(responseType, 0);
            }
        }

#nullable enable
        private Space? GetSpaceInfo(string name)
        {
            var request = new SelectRequest(VSpace, 2, 1, 0, Iterator.Eq, TarantoolTuple.Create(name));

            var response = _logicalConnection.SendRequest(request, TimeSpan.Zero, typeof(Space[]));

            if (response != null && response.Data is Space[] spaces)
            {
                return spaces[0];
            }
            else
            {
                return null;
            }
        }
    }
}
