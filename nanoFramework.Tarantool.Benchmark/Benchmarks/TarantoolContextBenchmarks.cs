// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using nanoFramework.Benchmark;
using nanoFramework.Benchmark.Attributes;

namespace nanoFramework.Tarantool.Benchmark.Benchmarks
{
    /// <summary>
    /// <see cref="Tarantool"/> context benchmarks class.
    /// </summary>
    [IterationCount(10)]
    public class TarantoolContextBenchmarks
    {
        private readonly BenchmarkContext _benchmarkContext = BenchmarkContext.Instance;

        /// <summary>
        /// <see cref="TarantoolContext.GetTypeArrayTypesFullName"/> benchmark method.
        /// </summary>
        [Benchmark]
        public void GetTypeArrayTypesFullName()
        {
            TarantoolContext.GetTypeArrayTypesFullName(_benchmarkContext.TestTypes);
        }

        /// <summary>
        /// <see cref="TarantoolContext.GetTarantoolTupleType"/> benchmark method.
        /// </summary>
        [Benchmark]
        public void GetTarantoolTupleType()
        {
            TarantoolContext.GetTarantoolTupleType(_benchmarkContext.TestTypes);
        }

        /// <summary>
        /// <see cref="TarantoolContext.GetTarantoolTupleArrayType"/> benchmark method.
        /// </summary>
        [Benchmark]
        public void GetTarantoolTupleArrayType()
        {
            TarantoolContext.GetTarantoolTupleArrayType(_benchmarkContext.TestTarantoolTupleType);
        }
    }
}
