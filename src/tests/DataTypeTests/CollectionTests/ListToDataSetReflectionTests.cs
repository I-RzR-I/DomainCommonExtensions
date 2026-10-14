using System;
using System.Collections.Generic;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class ListToDataSetReflectionTests
    {
        [TestMethod]
        public void ToDataSet_IndexerPropertyIgnored_Test()
        {
            var source = new List<DataTableIndexerTestModel>
            {
                new DataTableIndexerTestModel { Id = 3 },
                new DataTableIndexerTestModel { Id = 4 }
            };

            var result = source.ToDataSet();

            var table = result.Tables[0];
            Assert.IsTrue(table.Columns.Contains("Id"));
            Assert.IsFalse(table.Columns.Contains("Item"));
            Assert.AreEqual(2, table.Rows.Count);
            Assert.AreEqual(3, table.Rows[0]["Id"]);
            Assert.AreEqual(4, table.Rows[1]["Id"]);
        }

        [TestMethod]
        public void ToDataSet_NullValueToDbNull_Test()
        {
            var source = new List<DataTableMissingColumnTestModel>
            {
                new DataTableMissingColumnTestModel { Id = 1, Missing = null }
            };

            var result = source.ToDataSet();

            var table = result.Tables[0];
            Assert.AreEqual(2, table.Columns.Count);
            Assert.AreEqual(typeof(int), table.Columns["Id"].DataType);
            Assert.AreEqual(typeof(string), table.Columns["Missing"].DataType);
            Assert.AreEqual(1, table.Rows[0]["Id"]);
            Assert.AreEqual(DBNull.Value, table.Rows[0]["Missing"]);
        }
    }
}
