// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Threading;
using nanoFramework.Benchmark;
using nanoFramework.Networking;
using nanoFramework.Tarantool.Benchmark.Benchmarks;

namespace nanoFramework.Tarantool.Benchmark
{
    /// <summary>
    /// Program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        public static void Main()
        {
            const string Ssid = "YourSSID";
            const string Password = "YourWifiPassword";

            CancellationTokenSource cs = new CancellationTokenSource(60000);

            var success = WifiNetworkHelper.ConnectDhcp(Ssid, Password, requiresDateTime: true, token: cs.Token);
            if (!success)
            {
                Debug.WriteLine($"Can't connect to the network, error: {WifiNetworkHelper.Status}");
                if (WifiNetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"ex: {WifiNetworkHelper.HelperException}");
                }
            }
            else
            {
                Console.WriteLine("********** Starting benchmarks **********");

                try
                {
                    BenchmarkRunner.RunClass(typeof(TarantoolContextBenchmarks));
                    BenchmarkRunner.RunClass(typeof(TarantoolBoxBenchmarks));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error start benchmarks\n{ex}");
                }
                finally
                {
                    BenchmarkContext.Instance.BoxDisconnect();
                }

                Console.WriteLine("********** Completed benchmarks **********");
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
