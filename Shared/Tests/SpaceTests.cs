// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using System;
using nanoFramework.TestFramework;
#endif

using nanoFramework.Tarantool.Client.Interfaces;
using nanoFramework.Tarantool.Dto;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.UpdateOperations;

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

        /// <summary>
        /// Put and update tuple test method.
        /// </summary>
        [TestMethod]
        public void PutAndUpdateTests()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(true, false)))
            {
                Assert.AreNotEqual(0, box.Schema.Spaces.Count);
                Assert.IsNotNull(box.Schema["bands"] as ISpace);

                var space = box.Schema["bands"];
                bool recordCreate = false;
                var testTuple = TarantoolTuple.Create(16, "Nazareth", 1988);
                var responseTuple = space.PutTuple(testTuple);
                recordCreate = true;
                try
                {
                    Assert.IsNotNull(responseTuple);
                    Assert.AreEqual(testTuple[0], responseTuple[0]);
                    Assert.AreEqual(testTuple[1], responseTuple[1]);
                    Assert.AreEqual(testTuple[2], responseTuple[2]);

                    var updateKey = TarantoolTuple.Create(16);
                    var responseData = space.Update(updateKey, new UpdateOperation[] { UpdateOperation.CreateAssign(2, 1968) }, (TarantoolTupleType)testTuple.GetType()).CheckResponseData();
                    Assert.AreNotEqual(0, responseData.Data.Length);
                    Assert.IsNotNull(responseData.Data[0]);
                    responseTuple = responseData.Data[0] as TarantoolTuple;
                    Assert.IsNotNull(responseTuple);
                    Assert.AreEqual(16, responseTuple[0]);
                    Assert.AreEqual("Nazareth", responseTuple[1]);
                    Assert.AreEqual(1968, responseTuple[2]);
                }
                finally
                {
                    if (recordCreate)
                    {
                        var deleteKeyTuple = TarantoolTuple.Create(16);

                        var responseData = space.Delete(deleteKeyTuple).CheckResponseData();
                    }
                }
            }
        }

        /// <summary>
        /// Upsert tuple test method.
        /// </summary>
        [TestMethod]
        public void UpsertTests()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(true, false)))
            {
                Assert.AreNotEqual(0, box.Schema.Spaces.Count);
                Assert.IsNotNull(box.Schema["bands"] as ISpace);

                var space = box.Schema["bands"];
               
                var testTuple = TarantoolTuple.Create(17, "BonJovi", 1983);
                bool recordCreate = false;
                space.Upsert(testTuple, new UpdateOperation[] { UpdateOperation.CreateStringSlice(1, 3, 0, " ") });           
                try
                {
                    recordCreate = true;

                    var upsertTuple = space.GetTuple(TarantoolTuple.Create(17), (TarantoolTupleType)testTuple.GetType());
                    Assert.AreEqual("BonJovi", upsertTuple[1].ToString());

                    space.Upsert(testTuple, new UpdateOperation[] { UpdateOperation.CreateStringSlice(1, 3, 0, " ") });

                    upsertTuple = space.GetTuple(TarantoolTuple.Create(17), (TarantoolTupleType)testTuple.GetType());

                    Assert.AreEqual("Bon Jovi", upsertTuple[1].ToString());
                }
                finally
                {
                    if (recordCreate)
                    {
                        var deleteKeyTuple = TarantoolTuple.Create(17);

                        var responseData = space.Delete(deleteKeyTuple).CheckResponseData();
                    }
                }
            }
        }
    }
}
