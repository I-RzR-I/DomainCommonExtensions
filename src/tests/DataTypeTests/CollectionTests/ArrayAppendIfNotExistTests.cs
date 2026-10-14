using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class ArrayAppendIfNotExistTests
    {
        [TestMethod]
        public void AppendIfNotExist_MissingItemIsAppendedOnce_Test()
        {
            var source = new[] { "a", "b" };

            var result = source.AppendIfNotExist("c");

            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, result);
            CollectionAssert.AreEqual(new[] { "a", "b" }, source);
        }

        [TestMethod]
        public void AppendIfNotExist_ExistingItemLeavesArrayUnchanged_Test()
        {
            var source = new[] { "a", "b" };

            var result = source.AppendIfNotExist("b");

            CollectionAssert.AreEqual(new[] { "a", "b" }, result);
            CollectionAssert.AreEqual(new[] { "a", "b" }, source);
        }

        [TestMethod]
        public void AppendIfNotExist_IntMissingValueIsAppended_Test()
        {
            var source = new[] { 1, 2 };

            var result = source.AppendIfNotExist(3);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [TestMethod]
        public void AppendIfNotExist_IntExistingValueNotDuplicated_Test()
        {
            var source = new[] { 1, 2 };

            var result = source.AppendIfNotExist(1);

            CollectionAssert.AreEqual(new[] { 1, 2 }, result);
        }

        [TestMethod]
        public void AppendIfNotExist_NullSourceReturnsSingleItem_Test()
        {
            string[] source = null;

            var result = source.AppendIfNotExist("x");

            CollectionAssert.AreEqual(new[] { "x" }, result);
        }

        [TestMethod]
        public void AppendIfNotExist_EmptySourceReturnsSingleItem_Test()
        {
            var source = new string[0];

            var result = source.AppendIfNotExist("x");

            CollectionAssert.AreEqual(new[] { "x" }, result);
        }

        [TestMethod]
        public void AppendIfNotExist_NullSourceNullItemReturnsEmpty_Test()
        {
            string[] source = null;

            var result = source.AppendIfNotExist((string)null);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void AppendIfNotExist_NullItemReturnsSameSource_Test()
        {
            var source = new[] { "a", "b" };

            var result = source.AppendIfNotExist((string)null);

            Assert.AreSame(source, result);
            CollectionAssert.AreEqual(new[] { "a", "b" }, source);
        }
    }
}
