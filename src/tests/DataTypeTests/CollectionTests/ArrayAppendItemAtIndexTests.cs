using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class ArrayAppendItemAtIndexTests
    {
        [TestMethod]
        public void AppendItemAtIndex_InRangeOverwritesInPlace_Test()
        {
            var source = new[] { "a", "b", "c" };

            var result = source.AppendItem(1, "x");

            Assert.AreSame(source, result);
            CollectionAssert.AreEqual(new[] { "a", "x", "c" }, source);
        }

        [TestMethod]
        public void AppendItemAtIndex_IntArrayPicksIndexOverload_Test()
        {
            var source = new[] { 1, 2, 3 };

            var result = source.AppendItem(1, 9);

            Assert.AreSame(source, result);
            CollectionAssert.AreEqual(new[] { 1, 9, 3 }, source);
        }

        [TestMethod]
        public void AppendItemAtIndex_IndexEqualsLength_Test()
        {
            var source = new[] { "a", "b", "c" };

            Assert.ThrowsException<IndexOutOfRangeException>(() => source.AppendItem(3, "x"));

            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void AppendItemAtIndex_NegativeIndex_Test()
        {
            var source = new[] { "a", "b", "c" };

            var result = source.AppendItem(-1, "x");

            Assert.AreSame(source, result);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void AppendItemAtIndex_NullItem_Test()
        {
            var source = new[] { "a", "b", "c" };

            var result = source.AppendItem(1, (string)null);

            Assert.AreSame(source, result);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void AppendItemAtIndex_EmptySource_Test()
        {
            var source = new string[0];

            var result = source.AppendItem(0, "x");

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void AppendItemAtIndex_NullSource_Test()
        {
            string[] source = null;

            var result = source.AppendItem(0, "x");

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }
    }
}
