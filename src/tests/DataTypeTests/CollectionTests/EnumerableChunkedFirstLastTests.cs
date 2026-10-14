using System.Collections.Generic;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class EnumerableChunkedFirstLastTests
    {
        [TestMethod]
        public void Chunked_FiveItemsSizeTwo_Test()
        {
            var source = new CountingEnumerableTestHelper<int>(new[] { 1, 2, 3, 4, 5 });

            var result = source.Chunked(2).Select(x => x.ToArray()).ToArray();

            Assert.AreEqual(3, result.Length);
            CollectionAssert.AreEqual(new[] { 1, 2 }, result[0]);
            CollectionAssert.AreEqual(new[] { 3, 4 }, result[1]);
            CollectionAssert.AreEqual(new[] { 5 }, result[2]);
            Assert.AreEqual(1, source.EnumerationCount);
        }

        [TestMethod]
        public void Chunked_OneShotSource_Test()
        {
            var source = new CountingEnumerableTestHelper<int>(new[] { 1, 2, 3, 4, 5 }, true);

            var result = source.Chunked(2).Select(x => x.ToArray()).ToArray();

            Assert.AreEqual(3, result.Length);
            CollectionAssert.AreEqual(new[] { 1, 2 }, result[0]);
            CollectionAssert.AreEqual(new[] { 3, 4 }, result[1]);
            CollectionAssert.AreEqual(new[] { 5 }, result[2]);
            Assert.AreEqual(1, source.EnumerationCount);
        }

        [TestMethod]
        public void Chunked_NullSource_Test()
        {
            IEnumerable<int> source = null;

            var result = source.Chunked(2).ToArray();

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Chunked_EmptySource_Test()
        {
            var source = new int[0];

            var result = source.Chunked(2).ToArray();

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Chunked_ZeroChunkSize_Test()
        {
            var source = new[] { 1, 2, 3, 4, 5 };

            var result = source.Chunked(0).Select(x => x.ToArray()).ToArray();

            Assert.AreEqual(1, result.Length);
            CollectionAssert.AreEqual(new[] { 1, 2, 3, 4, 5 }, result[0]);
        }

        [TestMethod]
        public void IsFirst_FirstItem_Test()
        {
            var result = new[] { 1, 2, 3 }.IsFirst(1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsFirst_NotFirstItem_Test()
        {
            var result = new[] { 1, 2, 3 }.IsFirst(2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsFirst_EmptySource_Test()
        {
            var result = new int[0].IsFirst(1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsFirst_NullSource_Test()
        {
            IEnumerable<int> items = null;

            var result = items.IsFirst(1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsFirst_SingleEnumeration_Test()
        {
            var source = new CountingEnumerableTestHelper<int>(new[] { 1, 2, 3 });

            var result = source.IsFirst(1);

            Assert.IsTrue(result);
            Assert.IsTrue(source.EnumerationCount <= 1);
        }

        [TestMethod]
        public void IsFirst_NullItemNotFirst_Test()
        {
            var result = new[] { "a" }.IsFirst(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsFirst_NullItemIsFirst_Test()
        {
            var result = new[] { null, "a" }.IsFirst(null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsLast_LastItem_Test()
        {
            var result = new[] { 1, 2, 3 }.IsLast(3);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsLast_NotLastItem_Test()
        {
            var result = new[] { 1, 2, 3 }.IsLast(2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsLast_EmptySource_Test()
        {
            var result = new int[0].IsLast(1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsLast_NullSource_Test()
        {
            IEnumerable<int> items = null;

            var result = items.IsLast(1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsFirst_SourceThrowsAfterFirst_Test()
        {
            var result = ThrowingSequenceTestHelper.FirstThenThrow(1).IsFirst(1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsLast_IndexedList_Test()
        {
            var source = new IndexOnlyListTestHelper<int>(new[] { 1, 2, 3 });

            Assert.IsTrue(source.IsLast(3));
            Assert.IsFalse(source.IsLast(2));
        }

        [TestMethod]
        public void IsLast_SingleEnumeration_Test()
        {
            var source = new CountingEnumerableTestHelper<int>(new[] { 1, 2, 3 });

            var result = source.IsLast(3);

            Assert.IsTrue(result);
            Assert.IsTrue(source.EnumerationCount <= 1);
        }

        [TestMethod]
        public void IsLast_NullItemIsLast_Test()
        {
            var result = new string[] { null }.IsLast(null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsLast_NullItemNotLast_Test()
        {
            var result = new[] { "a" }.IsLast(null);

            Assert.IsFalse(result);
        }
    }
}
