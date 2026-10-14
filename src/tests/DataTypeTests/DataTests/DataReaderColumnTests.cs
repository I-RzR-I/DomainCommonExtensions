using System;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data;
using RzR.Extensions.Domain.Data.DataReader;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataReaderColumnTests
    {
        private const string MissingColumn = "missing";

        [TestMethod]
        public void ContainsColumn_MissingColumnReturnsFalse_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 1))
            {
                Assert.IsFalse(reader.ContainsColumn(MissingColumn));
            }
        }

        [TestMethod]
        public void ContainsColumn_ExistingColumnReturnsTrue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 1))
            {
                Assert.IsTrue(reader.ContainsColumn(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ContainsColumnAndIsNotNullOrEmpty_MissingColumnReturnsFalse_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 1))
            {
                Assert.IsFalse(reader.ContainsColumnAndIsNotNullOrEmpty(MissingColumn));
            }
        }

        [TestMethod]
        public void ContainsColumnAndIsNotNullOrEmpty_ExistingValueReturnsTrue_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 1))
            {
                Assert.IsTrue(reader.ContainsColumnAndIsNotNullOrEmpty(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void ToInt32_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(int), 1))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToInt32(MissingColumn));
            }
        }

        [TestMethod]
        public void ToNullBool_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(bool), true))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToNullBool(MissingColumn));
            }
        }

        [TestMethod]
        public void ToDateTime_MissingColumnPinArgumentException_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(DateTime), DateTime.MinValue))
            {
                Assert.ThrowsException<ArgumentException>(() => reader.ToDateTime(MissingColumn));
            }
        }

        [TestMethod]
        public void IsWithValue_WhitespacePinFalse_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), " "))
            {
                Assert.IsFalse(reader.IsWithValue(DataReaderTestHelper.ColumnName));
            }
        }

        [TestMethod]
        public void IsDbNullOrEmpty_WhitespacePinFalse_Test()
        {
            using (var reader = DataReaderTestHelper.SingleValue(typeof(string), " "))
            {
                Assert.IsFalse(reader.IsDbNullOrEmpty(DataReaderTestHelper.ColumnName));
            }
        }
    }
}
