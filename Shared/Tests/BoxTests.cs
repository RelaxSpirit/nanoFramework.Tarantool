// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using nanoFramework.TestFramework;
#endif
using System.Collections;
using nanoFramework.Tarantool.Client.Interfaces;
#if !NANOFRAMEWORK_1_0
using nanoFramework.Tarantool.Exceptions;
#endif
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Tests
{
    /// <summary>
    /// Test <see cref="Tarantool"/> box class.
    /// </summary>
    [TestClass]
    public sealed class BoxTests
    {
        ////[TestMethod]
        ////public void TestMockNetworkStream()
        ////{
        ////    var co = GetClientOptions(true, true);
        ////    Assert.IsNotNull(co);
        ////    Assert.IsNotNull(co.GetNetworkStream);

        ////    byte[] buff = new byte[512];
        ////    using (TarantoolStreamMock st = (TarantoolStreamMock)co.GetNetworkStream(co))
        ////    {
        ////        int r = st.Read(buff, 0, buff.Length);
        ////        Assert.AreEqual(128, r);

        ////        using (var stream = new MemoryStream())
        ////        {
        ////            var requestHeader = new RequestHeader(CommandCode.Eval, new RequestId(1));
        ////            stream.Seek(Constants.PacketSizeBufferSize, SeekOrigin.Begin);
        ////            MessagePackSerializer.Serialize(requestHeader, stream);
        ////            MessagePackSerializer.Serialize(new EvalRequest("return box.info", TarantoolTuple.Empty), stream);
        ////            var totalLength = stream.Position - Constants.PacketSizeBufferSize;
        ////            var packetLength = new PacketSize((uint)totalLength);
        ////            stream.Seek(0, SeekOrigin.Begin);
        ////            MessagePackSerializer.Serialize(packetLength, stream);
        ////            stream.Seek(0, SeekOrigin.Begin);
        ////            System.Diagnostics.Debug.WriteLine("Write request begin.....");
        ////            st.Write(stream.ToArray(), 0, (int)stream.Length);
        ////            System.Diagnostics.Debug.WriteLine("Request writing.");
        ////        }
        ////        System.Diagnostics.Debug.WriteLine("Flash begin.....");
        ////        st.Flush();
        ////        System.Diagnostics.Debug.WriteLine("Flushing.");

        ////        System.Diagnostics.Debug.WriteLine("Read begin.....");
        ////        r = st.Read(buff, 0, buff.Length);
        ////        System.Diagnostics.Debug.WriteLine($"Reading {r} bytes.");
        ////    }
        ////}

        /// <summary>
        /// Test <see cref="Tarantool"/> box network connections.
        /// </summary>
        [TestMethod]
        public void ConnectionTests()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(true, true)))
            {
                CheckBox(box);
            }
#if !NANOFRAMEWORK_1_0
            Assert.ThrowsException<TarantoolException>(
                () =>
                {
                    using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(false, false, userData: "test:test_password")))
                    {
                    };
                },
            "Tarantool returns an error for request with id: 1, code: 0x0000802F  and message: User not found or supplied credentials are invalid.");
////#else
////         Assert.ThrowsException(typeof(TarantoolException), 
////            () => 
////            { 
////                  using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(false, false, userData: "test:test_password"))) 
////                  { 
////                  }; 
////            });
#endif

            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(true, true, userData: "testuser:test_password")))
            {
                CheckBox(box);
            }
        }

        /// <summary>
        /// Test <see cref="Tarantool"/> box methods call.
        /// </summary>
        [TestMethod]
        public void BoxCallTests()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(false, false, 256, 256)))
            {
                var tupleParam = TarantoolTuple.Create(1.3d);
                var callResult = box.Call("math.sqrt", tupleParam, tupleParam.GetType()).CheckResponseData();

                Assert.AreEqual(1, callResult.Data.Length);
                Assert.IsNotNull(callResult.Data[0]);

                var resultData = callResult.Data[0] as TarantoolTuple;
                Assert.IsNotNull(resultData);
                Assert.AreEqual(1, resultData.Length);

                var value = resultData[0];
                Assert.IsNotNull(value);

#if !NANOFRAMEWORK_1_0
                var diff = Math.Abs(((double)value) - Math.Sqrt(1.3d));
                Assert.AreEqual(0d, diff);
                Assert.ThrowsException<TarantoolException>(() => box.Call("math.pi"));
#else
                Assert.AreNotEqual(1.3d, (double)value);
////            Assert.ThrowsException(typeof(TarantoolException), () => box.Call("math.pi"));

#endif

                callResult = box.Call("string.reverse", TarantoolTuple.Create("krowemarFonan")).CheckResponseData();

                var result = callResult.Data[0] as string;
                Assert.AreEqual("nanoFramework", result);

                callResult = box.Call("os.tmpname").CheckResponseData();

                result = callResult.Data[0] as string;
                Assert.IsNotNull(result);
                Assert.IsTrue(result.StartsWith("/tmp/lua_"));
            }
        }

        /// <summary>
        /// Test <see cref="Tarantool"/> box eval.
        /// </summary>
        [TestMethod]
        public void BoxEvalTests()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(false, false, 512, 512)))
            {
                var tupleParam = TarantoolTuple.Create(1, 2, 3);
                var evalResult = box.Eval("return ...", tupleParam, tupleParam.GetType()).CheckResponseData();

                var resultData = evalResult.Data[0] as TarantoolTuple;
                Assert.IsNotNull(resultData);
                Assert.AreEqual(3, resultData.Length);
                Assert.AreEqual(1, resultData[0]);
                Assert.AreEqual(2, resultData[1]);
                Assert.AreEqual(3, resultData[2]);

                evalResult = box.Eval("return 1").CheckResponseData();

                Assert.AreEqual((byte)1, evalResult.Data[0]);

                evalResult = box.Eval("return box.space.bands:select{...}", typeof(TarantoolTuple[][])).CheckResponseData();

                var selectResult = evalResult.Data[0] as TarantoolTuple[];
                Assert.IsNotNull(selectResult);
                Assert.IsTrue(selectResult.Length >= 14);

                foreach (var tuple in selectResult)
                {
                    Console.WriteLine(tuple.ToString());
                }

                Console.WriteLine(new string('=', 10));

                evalResult = box.Eval("return box.space.bands:select({3}, {iterator='GT', limit = 3})").CheckResponseData();

                var arrayListResult = evalResult.Data[0] as ArrayList;
                Assert.IsNotNull(arrayListResult);
                Assert.AreEqual(3, arrayListResult.Count);

                foreach (IList tuple in arrayListResult)
                {
                    Console.WriteLine($"[{tuple[0]}, {tuple[1]}, {tuple[2]}]");
                }
            }
        }

        /// <summary>
        /// Test <see cref="Tarantool"/> execute SQL.
        /// </summary>
        [TestMethod]
        public void BoxExecuteSqlTests()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(false, false, 512, 512)))
            {
                var executeSqlResult = box.ExecuteSql("SELECT 1 as ABC, 'z', 3", typeof(TarantoolTuple[])).CheckResponseData();

                Assert.AreEqual(1, executeSqlResult.Data.Length);

                var selectResult = executeSqlResult.Data[0] as TarantoolTuple;
                Assert.IsNotNull(selectResult);
                Assert.AreEqual(3, selectResult.Length);
                Assert.AreEqual((byte)1, selectResult[0]);
                Assert.AreEqual("z", selectResult[1]);
                Assert.AreEqual((byte)3, selectResult[2]);

#if !NANOFRAMEWORK_1_0
                Assert.ThrowsException<TarantoolException>(() => box.ExecuteSql("create table sql_test(id int primary key, name text)"));
////#else
////                    Assert.ThrowsException(typeof(TarantoolException), () => box.ExecuteSql("create table sql_test(id int primary key, name text)"));
#endif
            }

            bool isTableCreate = false;

            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(false, false, 256, 256, "testuser:test_password")))
            {
                try
                {
                    var executeSqlResult = box.ExecuteSql("create table sql_test(id int primary key, name text)").CheckResponseData();
                    isTableCreate = true;
                    Assert.IsNotNull(executeSqlResult.SqlInfo);
                    Assert.AreEqual(1, executeSqlResult.SqlInfo.RowCount);

                    executeSqlResult = box.ExecuteSql("insert into sql_test values (1, 'asdf'), (2, 'zxcv'), (3, 'qwer')").CheckResponseData();
                    Assert.IsNotNull(executeSqlResult.SqlInfo);
                    Assert.AreEqual(3, executeSqlResult.SqlInfo.RowCount);

                    executeSqlResult = box.ExecuteSql("update sql_test set name = '1234' where id = 2").CheckResponseData();
                    Assert.IsNotNull(executeSqlResult.SqlInfo);
                    Assert.AreEqual(1, executeSqlResult.SqlInfo.RowCount);

                    executeSqlResult = box.ExecuteSql("SELECT * FROM sql_test WHERE id = $1", typeof(TarantoolTuple[]), new SqlParameter(2, "$1")).CheckResponseData();

                    var resultData = executeSqlResult.Data[0] as TarantoolTuple;
                    Assert.IsNotNull(resultData);
                    Assert.AreEqual((byte)2, resultData[0]);
                    Assert.AreEqual("1234", resultData[1]);
                }
                finally
                {
                    if (isTableCreate)
                    {
                        box.ExecuteSql("drop table sql_test");
                    }
                }
            }
        }

        private static void CheckBox(IBox box)
        {
            Assert.IsNotNull(box);
            Assert.IsTrue(box.IsConnected);
            Assert.IsNotNull(box.Schema);
            Assert.IsTrue(box.Schema.Spaces.Count > 0);
            Assert.IsTrue(box.Schema["_space"].Indices.Count > 0);
        }
    }
}
