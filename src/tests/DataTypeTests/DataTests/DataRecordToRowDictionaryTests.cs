using System;
using System.Data;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataRecordToRowDictionaryTests
    {
        [TestMethod]
        public void ToRowDictionary_CopiesEveryColumn_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 1), ("Name", typeof(string), "a")))
            {
                var row = reader.ToRowDictionary();

                Assert.AreEqual(2, row.Count);
                Assert.AreEqual(1, row["Id"]);
                Assert.AreEqual("a", row["Name"]);
            }
        }

        [TestMethod]
        public void ToRowDictionary_DbNullStoredAsNull_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 1), ("Note", typeof(string), DBNull.Value)))
            {
                var row = reader.ToRowDictionary();

                Assert.IsTrue(row.ContainsKey("Note"));
                Assert.IsNull(row["Note"]);
            }
        }

        [TestMethod]
        public void ToRowDictionary_KeysAreCaseInsensitive_Test()
        {
            using (var reader = DataRecordTestHelper.SingleRow(("Id", typeof(int), 1), ("Name", typeof(string), "a")))
            {
                var row = reader.ToRowDictionary();

                Assert.AreEqual(1, row["ID"]);
                Assert.AreEqual("a", row["name"]);
            }
        }

        [TestMethod]
        public void ToRowDictionary_TurkishCultureKeysUseOrdinalIgnoreCase_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "TITLE" }, new object[] { "x" });

            using (new CultureScopeTestHelper("tr-TR"))
            {
                var row = record.ToRowDictionary();

                Assert.IsTrue(row.ContainsKey("title"));
            }
        }

        [TestMethod]
        public void ToRowDictionary_CanonicallyEquivalentKeyNotFound_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "Å" }, new object[] { "x" });

            var row = record.ToRowDictionary();

            Assert.IsFalse(row.ContainsKey("Å"));
        }

        [TestMethod]
        public void ToRowDictionary_DuplicateCaseInsensitiveNamesFirstWins_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "Id", "ID" }, new object[] { 1, 2 });

            var row = record.ToRowDictionary();

            Assert.AreEqual(1, row.Count);
            Assert.AreEqual(1, row["ID"]);
            Assert.AreEqual("Id", string.Join(",", row.Keys));
        }

        [TestMethod]
        public void ToRowDictionary_DuplicateExactNamesFirstWins_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { "A", "A" }, new object[] { 1, 2 });

            var row = record.ToRowDictionary();

            Assert.AreEqual(1, row.Count);
            Assert.AreEqual(1, row["A"]);
        }

        [TestMethod]
        public void ToRowDictionary_UnnamedColumnsKeepFirst_Test()
        {
            var record = new FakeDataRecordTestHelper(new[] { string.Empty, null, string.Empty }, new object[] { 1, 2, 3 });

            var row = record.ToRowDictionary();

            Assert.AreEqual(1, row.Count);
            Assert.IsTrue(row.ContainsKey(string.Empty));
            Assert.AreEqual(1, row[string.Empty]);
        }

        [TestMethod]
        public void ToRowDictionary_NoColumnsReturnsEmpty_Test()
        {
            var record = new FakeDataRecordTestHelper(new string[0], new object[0]);

            var row = record.ToRowDictionary();

            Assert.IsNotNull(row);
            Assert.AreEqual(0, row.Count);
        }

        [TestMethod]
        public void ToRowDictionary_ReturnsCurrentRowValues_Test()
        {
            using (var reader = DataRecordTestHelper.IdRows(1, 2))
            {
                reader.Read();
                reader.Read();

                var row = reader.ToRowDictionary();

                Assert.AreEqual(2, row[DataRecordTestHelper.IdColumn]);
            }
        }

        [TestMethod]
        public void ToRowDictionary_NullRecordThrowsArgumentNullException_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(
                () => ((IDataRecord)null).ToRowDictionary());

            Assert.AreEqual("record", exception.ParamName);
        }
    }
}
