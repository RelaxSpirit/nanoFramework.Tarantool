// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Call stored-procedure request.
    /// This is a remote stored-procedure call. <see cref="Tarantool"/> 1.6 and earlier made use of the <see cref="CommandCode.OldCall"/> request (code: 0x06).
    /// It is now deprecated and superseded by <see cref="CommandCode.Call"/>.
    /// </summary>
    public struct CallRequest : IRequest
    {
        private readonly bool _use17;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallRequest"/> struct.
        /// </summary>
        /// <param name="functionName">Stored-procedure name.</param>
        /// <param name="tuple">Stored-procedure parameter <see cref="TarantoolTuple"/>.</param>
        /// <param name="use17"><see langword="true"/> if use on old <see cref="Tarantool"/> version, other <see langword="false"/>. Default <see langword="true"/>.</param>
        public CallRequest(string functionName, TarantoolTuple tuple, bool use17 = true)
        {
            _use17 = use17;
            FunctionName = functionName;
            Tuple = tuple;
        }

        /// <summary>
        /// Gets stored-procedure name.
        /// </summary>
        public string FunctionName { get; }

        /// <summary>
        /// Gets stored-procedure parameters <see cref="TarantoolTuple"/>.
        /// </summary>
        public TarantoolTuple Tuple { get; }

        /// <summary>
        /// Gets request command code.
        /// </summary>
        public CommandCode Code => _use17 ? CommandCode.Call : CommandCode.OldCall;
    }
}
