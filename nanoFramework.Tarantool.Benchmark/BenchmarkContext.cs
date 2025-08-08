// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.Tarantool.Benchmark
{
    internal class BenchmarkContext
    {
        internal const string TarantoolHostIp = "192.168.1.116";
#nullable enable
        private static readonly object Lock = new object();
        private static BenchmarkContext? _instance = null;
        private readonly ClientOptions _clientOptions;
        private readonly TarantoolTuple _tarantoolTuple = TarantoolTuple.Create("krowemarFonan");
        private readonly CallRequest _callRequest;
        private IBox _box;

        private BenchmarkContext()
        {
            _clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
            _clientOptions.ConnectionOptions.WriteStreamBufferSize = 512;
            _clientOptions.ConnectionOptions.ReadStreamBufferSize = 512;
            _clientOptions.ConnectionOptions.ReadBoxInfoOnConnect = false;
            _clientOptions.ConnectionOptions.ReadSchemaOnConnect = false;
            _clientOptions.ConnectionOptions.WriteThrottlePeriodInMs = 0;

            _callRequest = new CallRequest("string.reverse", _tarantoolTuple);
            _box = TarantoolContext.Connect(_clientOptions);
        }

        internal static BenchmarkContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BenchmarkContext();
                        }
                    }
                }

                return _instance;
            }
        }

        internal Type[] TestTypes { get; } = new Type[] { typeof(string), typeof(uint), typeof(int) };

        internal TarantoolTupleType TestTarantoolTupleType { get; } = TarantoolContext.Instance.GetTarantoolTupleType(typeof(string), typeof(int), typeof(uint));

        internal TarantoolTuple TestTarantoolTuple { get; } = TarantoolTuple.Create(typeof(int), typeof(string), typeof(uint));

        internal TarantoolTupleType TestTarantoolTupleTypeForConverter { get; } = TarantoolContext.Instance.GetTarantoolTupleType(typeof(uint), typeof(string), typeof(int));

        internal TarantoolTupleArrayType TestTarantoolTupleArrayType { get; } = TarantoolContext.Instance.GetTarantoolTupleArrayType(TarantoolContext.Instance.GetTarantoolTupleType(typeof(uint), typeof(int), typeof(string)));

        internal void BoxDisconnect()
        {
            _box.Dispose();
        }

        internal void BoxCall()
        {
            _box.Call("string.reverse", _tarantoolTuple);
        }

        internal void BoxCallWithEmptyResponse()
        {
            _box.CallWithEmptyResponse("string.reverse", _tarantoolTuple);
        }

        internal void SendRawRequest()
        {
            _box.TarantoolConnection.SendRawRequest(_callRequest, Timeout.InfiniteTimeSpan);
        }

        internal void BoxEval()
        {
            _box.Eval("return ...", _tarantoolTuple, _tarantoolTuple.GetType());
        }

        internal void BoxExecuteSql()
        {
            _box.ExecuteSql("SELECT 1 as ABC, 'z', 3", typeof(TarantoolTuple[]));
        }
    }
}
