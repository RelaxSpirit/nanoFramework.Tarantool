// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Tarantool.Model.Enums;

namespace nanoFramework.Tarantool.Model.Requests
{
    /// <summary>
    /// Eval <see cref="Tarantool"/> request.
    /// Since the argument is a Lua expression, this is <see cref="Tarantool"/>’s way to handle non-binary with the binary protocol.
    /// Any request that does not have its own code, for example , will be handled either with <see cref="CallRequest"/> or <see cref="EvalRequest"/>.
    /// </summary>
    public class EvalRequest : IRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvalRequest"/> class.
        /// </summary>
        /// <param name="expression"><see cref="Tarantool"/> expression text to be eval.</param>
        /// <param name="tuple"><see cref="TarantoolTuple"/> parameter for eval.</param>
        public EvalRequest(string expression, TarantoolTuple tuple)
        {
            Expression = expression;
            Tuple = tuple;
        }

        /// <summary>
        /// Gets <see cref="Tarantool"/> expression text.
        /// </summary>
        public string Expression { get; }

        /// <summary>
        /// Gets <see cref="TarantoolTuple"/> parameter.
        /// </summary>
        public TarantoolTuple Tuple { get; }

        /// <summary>
        /// Gets <see cref="Tarantool"/> command code.
        /// </summary>
        public CommandCode Code => CommandCode.Eval;
    }
}
