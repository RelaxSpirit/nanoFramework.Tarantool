// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if NANOFRAMEWORK_1_0
using nanoFramework.TestFramework;
#endif

namespace nanoFramework.Tarantool.Tests
{
    /// <summary>
    /// Test <see cref="Tarantool"/> schema class.
    /// </summary>
    [TestClass]
    public sealed class SchemaTests
    {
        /// <summary>
        /// Test load <see cref="Tarantool"/> schema method.
        /// </summary>
        [TestMethod]
        public void LoadSchemaTest()
        {
            using (var box = TarantoolContext.Connect(TestHelper.GetClientOptions(false, false)))
            {
                Assert.IsNotNull(box.Schema);
                Assert.AreEqual(0, box.Schema.Spaces.Count);
                box.Schema.Reload();
                Assert.AreNotEqual(0, box.Schema.Spaces.Count);
            }
        }
    }
}
