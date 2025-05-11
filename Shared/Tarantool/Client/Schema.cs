// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
#endif
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
        private const int VSpace = 0x119;
        private const int VIndex = 0x121;
        internal const uint PrimaryIndexId = 0;

        private readonly ILogicalConnection logicalConnection;

        private Hashtable indexByName = new Hashtable();
        private Hashtable indexById = new Hashtable();

        /// <summary>
        /// Initializes a new instance of the <see cref="Schema"/> class.
        /// </summary>
        /// <param name="logicalConnection">Network logical connection.</param>
        internal Schema(ILogicalConnection logicalConnection)
        {
            this.logicalConnection = logicalConnection;
        }

        public ISpace this[string name]
        {
            get
            {
                var space = this.indexByName[name];
                if (space == null)
                {
                    throw ExceptionHelper.InvalidSpaceName(name);
                }

                return (ISpace)space;
            }
        }

        public ISpace this[uint id]
        {
            get
            {
                var space = this.indexById[id];
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

            var spaces = (Space[])this.Select(VSpace, typeof(Space[]));
            foreach (var space in spaces)
            {
                indByName[space.Name] = space;
                indById[space.Id] = space;
                space.LogicalConnection = this.logicalConnection;
                space.SetIndices((Index[])this.Select(VIndex, typeof(Index[]), Iterator.Eq, space.Id));
            }

            this.indexByName = indByName;
            this.indexById = indById;
            this.LastReloadTime = DateTime.UtcNow;
        }

        public ICollection Spaces => this.indexByName.Values;

        private object[] Select(uint spaceId, Type responseType, Iterator iterator = Iterator.All, uint id = 0u)
        {
            var request = new SelectRequest(spaceId, PrimaryIndexId, uint.MaxValue, 0, iterator, TarantoolTuple.Create(id));

            var response = this.logicalConnection.SendRequest(request, TimeSpan.Zero, responseType);

            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return (object[])Array.CreateInstance(responseType, 0);
            }
        }
    }
}
