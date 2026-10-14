using System;
using System.Data;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataRecordTryGetOrdinalTests
    {
        [TestMethod]
        public void TryGetOrdinal_ExactNameReturnsOrdinal_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 1), ("Name", typeof(string), "a")))
            {
                var found = reader.TryGetOrdinal("Name", out var ordinal);

                Assert.IsTrue(found);
                Assert.AreEqual(1, ordinal);
            }
        }

        [TestMethod]
        public void TryGetOrdinal_DifferentCaseReturnsOrdinal_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 1), ("Name", typeof(string), "a")))
            {
                var found = reader.TryGetOrdinal("NAME", out var ordinal);

                Assert.IsTrue(found);
                Assert.AreEqual(1, ordinal);
            }
        }

        [TestMethod]
        public void TryGetOrdinal_ExactMatchWinsOverEarlierCaseMatch_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "NAME", "Name" }, new object[] { 1, 2 });

            var found = record.TryGetOrdinal("Name", out var ordinal);

            Assert.IsTrue(found);
            Assert.AreEqual(1, ordinal);
        }

        [TestMethod]
        public void TryGetOrdinal_FirstCaseInsensitiveMatchWins_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "NAME", "Name" }, new object[] { 1, 2 });

            var found = record.TryGetOrdinal("name", out var ordinal);

            Assert.IsTrue(found);
            Assert.AreEqual(0, ordinal);
        }

        [TestMethod]
        public void TryGetOrdinal_DuplicateExactNamesFirstWins_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "A", "A" }, new object[] { 1, 2 });

            var found = record.TryGetOrdinal("A", out var ordinal);

            Assert.IsTrue(found);
            Assert.AreEqual(0, ordinal);
        }

        [TestMethod]
        public void TryGetOrdinal_MissingColumnReturnsFalse_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 1)))
            {
                var found = reader.TryGetOrdinal("Missing", out var ordinal);

                Assert.IsFalse(found);
                Assert.AreEqual(-1, ordinal);
            }
        }

        [TestMethod]
        public void TryGetOrdinal_NullNameReturnsFalse_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 1)))
            {
                var found = reader.TryGetOrdinal(null, out var ordinal);

                Assert.IsFalse(found);
                Assert.AreEqual(-1, ordinal);
            }
        }

        [TestMethod]
        public void TryGetOrdinal_NoColumnsReturnsFalse_Test()
        {
            var record = new FakeDataRecordTestHelper(new string[0], new object[0]);

            var found = record.TryGetOrdinal("Id", out var ordinal);

            Assert.IsFalse(found);
            Assert.AreEqual(-1, ordinal);
        }

        [TestMethod]
        public void TryGetOrdinal_NullRecordThrowsArgumentNullException_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => ((IDataRecord)null).TryGetOrdinal("Id", out _));

            Assert.AreEqual("record", exception.ParamName);
        }

        [TestMethod]
        public void TryGetOrdinal_TurkishCultureUsesOrdinalIgnoreCase_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "TITLE" }, new object[] { 1 });

            using (new CultureScopeTestHelper("tr-TR"))
            {
                var found = record.TryGetOrdinal("title", out var ordinal);

                Assert.IsTrue(found);
                Assert.AreEqual(0, ordinal);
            }
        }

        [TestMethod]
        public void TryGetOrdinal_CanonicallyEquivalentNameNotMatched_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "Å" }, new object[] { 1 });

            var found = record.TryGetOrdinal("Å", out var ordinal);

            Assert.IsFalse(found);
            Assert.AreEqual(-1, ordinal);
        }
    }
}
