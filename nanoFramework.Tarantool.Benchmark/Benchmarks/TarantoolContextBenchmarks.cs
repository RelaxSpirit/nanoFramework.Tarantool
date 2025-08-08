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
        private readonly TarantoolContext _tarantoolContext = TarantoolContext.Instance;
        private readonly BenchmarkContext _benchmarkContext = BenchmarkContext.Instance;
        private readonly Type _stringType = typeof(string);

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
            _tarantoolContext.GetTarantoolTupleType(_benchmarkContext.TestTypes);
        }

        /// <summary>
        /// <see cref="TarantoolContext.GetTarantoolTupleArrayType"/> benchmark method.
        /// </summary>
        [Benchmark]
        public void GetTarantoolTupleArrayType()
        {
            _tarantoolContext.GetTarantoolTupleArrayType(_benchmarkContext.TestTarantoolTupleType);
        }

        /// <summary>
        /// <see cref="TarantoolContext.GetTarantoolTupleConverter"/> benchmark method.
        /// </summary>
        [Benchmark]
        public void GetTarantoolTupleConverterByTuple()
        {
            _tarantoolContext.GetTarantoolTupleConverter(_benchmarkContext.TestTarantoolTuple);
        }

        /// <summary>
        /// <see cref="TarantoolContext.GetTarantoolTupleConverter"/> benchmark method.
        /// </summary>
        [Benchmark]
        public void GetTarantoolTupleConverterByType()
        {
            _tarantoolContext.GetTarantoolTupleConverter(_benchmarkContext.TestTarantoolTupleTypeForConverter);
        }

        /// <summary>
        /// <see cref="TarantoolContext.GetDataResponseDataTypeConverter"/> benchmark method.
        /// </summary>
        [Benchmark]
        public void GetDataResponseDataTypeConverterByType()
        {
            _tarantoolContext.GetDataResponseDataTypeConverter(_stringType);
        }

        /// <summary>
        /// <see cref="TarantoolContext.GetDataResponseDataTypeConverter"/> benchmark method.
        /// </summary>
        [Benchmark]
        public void GetDataResponseDataTypeConverterByTupleType()
        {
            _tarantoolContext.GetDataResponseDataTypeConverter(_benchmarkContext.TestTarantoolTupleType);
        }

        /// <summary>
        /// <see cref="TarantoolContext.GetDataResponseDataTypeConverter"/> benchmark method.
        /// </summary>
        [Benchmark]
        public void GetDataResponseDataTypeConverterByTupleArrayType()
        {
            _tarantoolContext.GetDataResponseDataTypeConverter(_benchmarkContext.TestTarantoolTupleArrayType);
        }
    }
}
