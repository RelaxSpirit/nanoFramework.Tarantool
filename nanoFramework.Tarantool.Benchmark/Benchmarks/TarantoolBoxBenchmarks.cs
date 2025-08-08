// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using nanoFramework.Benchmark;
using nanoFramework.Benchmark.Attributes;

namespace nanoFramework.Tarantool.Benchmark.Benchmarks
{
    /// <summary>
    /// <see cref="Tarantool"/> box benchmarks class.
    /// </summary>
    [IterationCount(10)]
    public class TarantoolBoxBenchmarks
    {
        private readonly BenchmarkContext _benchmarkContext = BenchmarkContext.Instance;

        /// <summary>
        /// Box Call benchmark method.
        /// </summary>
        [Benchmark]
        public void BoxCallBenchmark()
        {
            _benchmarkContext.BoxCall();
        }

        /// <summary>
        /// Box Call with empty response benchmark method.
        /// </summary>
        [Benchmark]
        public void BoxCallWithEmptyResponse()
        {
            _benchmarkContext.BoxCallWithEmptyResponse();
        }

        /// <summary>
        /// Box Call raw request benchmark method.
        /// </summary>
        [Benchmark]
        public void SendRawRequest()
        {
            _benchmarkContext.SendRawRequest();
        }

        /// <summary>
        /// Box Eval benchmark method.
        /// </summary>
        [Benchmark]
        public void BoxEvalBenchmark()
        {
            _benchmarkContext.BoxEval();
        }

        /// <summary>
        /// Box ExecuteSql benchmark method.
        /// </summary>
        [Benchmark]
        public void BoxExecuteSqlBenchmark()
        {
            _benchmarkContext.BoxExecuteSql();
        }
    }
}
