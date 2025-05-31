// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using nanoFramework.TestFramework;
#endif
using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Model;

namespace nanoFramework.Tarantool.Tests
{
    /// <summary>
    /// Test <see cref="Tarantool"/> index class.
    /// </summary>
    [TestClass]
    public class IndexTests
    {
        /// <summary>
        /// Getting min index tuple test method.
        /// </summary>
        [TestMethod]
        public void GetMinIndexTupleTest()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(true, false)))
            {
                Assert.AreNotEqual(0, box.Schema.Spaces.Count);
                Assert.IsNotNull(box.Schema["bands"] as ISpace);

                var index = box.Schema["bands"]["secondary"];
                var responseTupleType = TarantoolContext.Instance.GetTarantoolTupleType(typeof(int), typeof(string), typeof(uint));

                var responseTuple = index.MinTuple(responseTupleType);
                Assert.IsNotNull(responseTuple);
                Assert.AreEqual(3, responseTuple[0]);
                Assert.AreEqual("Ace of Base", responseTuple[1]);
                Assert.AreEqual(1987u, responseTuple[2]);

                responseTuple = index.MinTuple(TarantoolTuple.Create("Scorpions"), responseTupleType);
                Assert.IsNotNull(responseTuple);
                Assert.AreEqual(2, responseTuple[0]);
                Assert.AreEqual("Scorpions", responseTuple[1]);
                Assert.AreEqual(1965u, responseTuple[2]);
            }
        }

        /// <summary>
        /// Getting max index tuple test method.
        /// </summary>
        [TestMethod]
        public void GetMaxIndexTupleTest()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(true, false)))
            {
                Assert.AreNotEqual(0, box.Schema.Spaces.Count);
                Assert.IsNotNull(box.Schema["bands"] as ISpace);

                var index = box.Schema["bands"]["secondary"];
                var responseTupleType = TarantoolContext.Instance.GetTarantoolTupleType(typeof(int), typeof(string), typeof(uint));

                var responseTuple = index.MaxTuple(responseTupleType);
                Assert.IsNotNull(responseTuple);
                Assert.AreEqual(9, responseTuple[0]);
                Assert.AreEqual("The Rolling Stones", responseTuple[1]);
                Assert.AreEqual(1962u, responseTuple[2]);

                responseTuple = index.MaxTuple(TarantoolTuple.Create("Led Zeppelin"), responseTupleType);
                Assert.IsNotNull(responseTuple);
                Assert.AreEqual(12, responseTuple[0]);
                Assert.AreEqual("Led Zeppelin", responseTuple[1]);
                Assert.AreEqual(1968u, responseTuple[2]);
            }
        }
    }
}
