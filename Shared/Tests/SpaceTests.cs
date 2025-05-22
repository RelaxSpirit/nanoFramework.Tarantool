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
    /// Test <see cref="Tarantool"/> space class.
    /// </summary>
    [TestClass]
    public sealed class SpaceTests
    {
        /// <summary>
        /// Insert tuple and delete tuple test method.
        /// </summary>
        [TestMethod]
        public void InsertAndDeleteTests()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(true, false)))
            {
                Assert.AreNotEqual(0, box.Schema.Spaces.Count);
                Assert.IsNotNull(box.Schema["bands"] as ISpace);

                var space = box.Schema["bands"];
                bool recordCreate = false;
                var testTuple = TarantoolTuple.Create(15, "Black Sabbath", 1968);
                var responseData = space.Insert(testTuple).CheckResponseData();
                recordCreate = true;
#nullable enable
                TarantoolTuple? keyTuple = null;
#nullable disable
                try
                {
                    Assert.AreNotEqual(0, responseData.Data.Length);
                    Assert.IsNotNull(responseData.Data[0]);
                    var responseTuple = responseData.Data[0] as TarantoolTuple;
                    Assert.IsNotNull(responseTuple);
                    Assert.AreEqual(15, responseTuple[0]);
                    Assert.AreEqual("Black Sabbath", responseTuple[1]);
                    Assert.AreEqual(1968, responseTuple[2]);

                    keyTuple = TarantoolTuple.Create(15);
                    var selectedTuple = space.GetTuple(keyTuple, (TarantoolTupleType)testTuple.GetType());
                    Assert.IsNotNull(selectedTuple);
                    Assert.AreEqual(15, selectedTuple[0]);
                    Assert.AreEqual("Black Sabbath", selectedTuple[1]);
                    Assert.AreEqual(1968, selectedTuple[2]);
                }
                finally
                {
                    if (recordCreate)
                    {
                        var deleteKeyTuple = TarantoolTuple.Create(15);

                        responseData = space.Delete(deleteKeyTuple).CheckResponseData();
                        if (keyTuple != null)
                        {
                            Assert.AreEqual(keyTuple.GetType(), deleteKeyTuple.GetType());
                        }

                        var deletedTuple = space.GetTuple(deleteKeyTuple, (TarantoolTupleType)testTuple.GetType());
                        Assert.IsNull(deletedTuple);
                    }
                }
            }
        }
    }
}
