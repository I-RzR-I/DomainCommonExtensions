using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class ArrayInsertAtSetAtTests
    {
        [TestMethod]
        public void InsertAt_MiddleIndexReturnsNewArray_Test()
        {
            var source = new[] { "a", "b", "c" };

            var result = source.InsertAt(1, "x");

            Assert.AreNotSame(source, result);
            CollectionAssert.AreEqual(new[] { "a", "x", "b", "c" }, result);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void InsertAt_IndexZeroInsertsAtStart_Test()
        {
            var source = new[] { "a", "b", "c" };

            var result = source.InsertAt(0, "x");

            CollectionAssert.AreEqual(new[] { "x", "a", "b", "c" }, result);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void InsertAt_IndexEqualsLengthAppends_Test()
        {
            var source = new[] { "a", "b", "c" };

            var result = source.InsertAt(3, "x");

            CollectionAssert.AreEqual(new[] { "a", "b", "c", "x" }, result);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void InsertAt_EmptySourceIndexZero_Test()
        {
            var source = new string[0];

            var result = source.InsertAt(0, "x");

            Assert.AreNotSame(source, result);
            CollectionAssert.AreEqual(new[] { "x" }, result);
            Assert.AreEqual(0, source.Length);
        }

        [TestMethod]
        public void InsertAt_NegativeIndexThrows_Test()
        {
            var source = new[] { "a", "b", "c" };

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => source.InsertAt(-1, "x"));

            Assert.AreEqual("index", exception.ParamName);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void InsertAt_IndexAboveLengthThrows_Test()
        {
            var source = new[] { "a", "b", "c" };

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => source.InsertAt(4, "x"));

            Assert.AreEqual("index", exception.ParamName);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void InsertAt_NullSourceThrows_Test()
        {
            string[] source = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => source.InsertAt(0, "x"));

            Assert.AreEqual("source", exception.ParamName);
        }

        [TestMethod]
        public void InsertAt_NullItemIsInserted_Test()
        {
            var source = new[] { "a", "b" };

            var result = source.InsertAt(1, (string)null);

            CollectionAssert.AreEqual(new[] { "a", null, "b" }, result);
            CollectionAssert.AreEqual(new[] { "a", "b" }, source);
        }

        [TestMethod]
        public void InsertAt_IntArrayInsertsValue_Test()
        {
            var source = new[] { 1, 2, 3 };

            var result = source.InsertAt(1, 9);

            CollectionAssert.AreEqual(new[] { 1, 9, 2, 3 }, result);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, source);
        }

        [TestMethod]
        public void SetAt_InRangeReturnsReplacedCopy_Test()
        {
            var source = new[] { "a", "b", "c" };

            var result = source.SetAt(1, "x");

            Assert.AreNotSame(source, result);
            CollectionAssert.AreEqual(new[] { "a", "x", "c" }, result);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void SetAt_LastIndexReplacesLastItem_Test()
        {
            var source = new[] { 1, 2, 3 };

            var result = source.SetAt(2, 9);

            CollectionAssert.AreEqual(new[] { 1, 2, 9 }, result);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, source);
        }

        [TestMethod]
        public void SetAt_NullItemIsSet_Test()
        {
            var source = new[] { "a", "b" };

            var result = source.SetAt(0, (string)null);

            CollectionAssert.AreEqual(new[] { null, "b" }, result);
            CollectionAssert.AreEqual(new[] { "a", "b" }, source);
        }

        [TestMethod]
        public void SetAt_CovariantArraySetsOtherType_Test()
        {
            object[] source = new[] { "a", "b" };

            var result = source.SetAt(0, (object)5);

            Assert.AreEqual(typeof(object[]), result.GetType());
            CollectionAssert.AreEqual(new object[] { 5, "b" }, result);
            CollectionAssert.AreEqual(new[] { "a", "b" }, source);
        }

        [TestMethod]
        public void SetAt_IndexEqualsLengthThrows_Test()
        {
            var source = new[] { "a", "b", "c" };

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => source.SetAt(3, "x"));

            Assert.AreEqual("index", exception.ParamName);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void SetAt_NegativeIndexThrows_Test()
        {
            var source = new[] { "a", "b", "c" };

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => source.SetAt(-1, "x"));

            Assert.AreEqual("index", exception.ParamName);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, source);
        }

        [TestMethod]
        public void SetAt_NullSourceThrows_Test()
        {
            string[] source = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => source.SetAt(0, "x"));

            Assert.AreEqual("source", exception.ParamName);
        }
    }
}
