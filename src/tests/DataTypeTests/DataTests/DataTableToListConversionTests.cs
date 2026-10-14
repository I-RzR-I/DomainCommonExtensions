using System;
using System.Data;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataTableToListConversionTests
    {
        [TestMethod]
        public void ToList_EnumFromIntColumn_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Day", typeof(int), 5);

            var result = table.ToList<DataTableEnumTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(DayOfWeek.Friday, result[0].Day);
        }

        [TestMethod]
        public void ToList_EnumFromStringColumn_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Day", typeof(string), "Friday");

            var result = table.ToList<DataTableEnumTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(DayOfWeek.Friday, result[0].Day);
        }

        [TestMethod]
        public void ToList_GuidFromStringColumn_Test()
        {
            var expected = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e");
            var table = DataTableConversionTestHelper.SingleCell("Key", typeof(string), "0f8fad5b-d9cb-469f-a165-70867728950e");

            var result = table.ToList<DataTableGuidTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expected, result[0].Key);
        }

        [TestMethod]
        public void ToList_MissingColumnSkipped_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Id", typeof(int), 7);

            var result = table.ToList<DataTableMissingColumnTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(7, result[0].Id);
            Assert.IsNull(result[0].Missing);
        }

        [TestMethod]
        public void ToList_TypeMissingColumnSkipped_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Id", typeof(int), 7);

            var result = table.ToList(typeof(DataTableMissingColumnTestModel));

            Assert.AreEqual(1, result.Count);
            var item = (DataTableMissingColumnTestModel)result[0];
            Assert.AreEqual(7, item.Id);
            Assert.IsNull(item.Missing);
        }

        [TestMethod]
        public void ToList_GetOnlyPropertySkipped_Test()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Doubled", typeof(int));
            table.Rows.Add(7, 99);

            var result = table.ToList<DataTableGetOnlyTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(7, result[0].Id);
            Assert.AreEqual(14, result[0].Doubled);
        }

        [TestMethod]
        public void ToList_IndexerPropertySkipped_Test()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Item", typeof(string));
            table.Rows.Add(3, "x");

            var result = table.ToList<DataTableIndexerTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result[0].Id);
            Assert.IsNull(result[0]["any"]);
        }

        [TestMethod]
        public void ToList_DeDeDotStringToDouble_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Amount", typeof(string), "1.5");

            double amount;
            using (new CultureScopeTestHelper("de-DE"))
            {
                amount = table.ToList<DataTableDoubleTestModel>()[0].Amount;
            }

            Assert.AreEqual(1.5d, amount);
        }

        [TestMethod]
        public void ToList_DeDeCommaStringToDouble_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Amount", typeof(string), "1,5");

            double amount;
            using (new CultureScopeTestHelper("de-DE"))
            {
                amount = table.ToList<DataTableDoubleTestModel>()[0].Amount;
            }

            Assert.AreEqual(1.5d, amount);
        }

        [TestMethod]
        public void ToList_LongColumnToInt_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Count", typeof(long), 5L);

            var result = table.ToList<DataTableIntTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(5, result[0].Count);
        }

        [TestMethod]
        public void ToList_LongOverflowToInt_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Count", typeof(long), 5000000000L);

            Assert.ThrowsException<OverflowException>(() => table.ToList<DataTableIntTestModel>());
        }

        [TestMethod]
        public void ToList_PrivateSetterFilled_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Id", typeof(int), 7);

            var result = table.ToList<DataTablePrivateSetterTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(7, result[0].Id);
        }

        [TestMethod]
        public void ToList_HiddenPropertyMostDerived_Test()
        {
            var table = DataTableConversionTestHelper.SingleCell("Code", typeof(string), "D-1");

            var result = table.ToList<DataTableHiddenDerivedTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("D-1", result[0].Code);
        }

        [TestMethod]
        public void ToList_FractionalDecimalToIntRounds_Test()
        {
            var table = new DataTable();
            table.Columns.Add("Count", typeof(decimal));
            table.Rows.Add(2.5m);
            table.Rows.Add(3.5m);

            var result = table.ToList<DataTableIntTestModel>();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2, result[0].Count);
            Assert.AreEqual(4, result[1].Count);
        }

        [TestMethod]
        public void ToList_DbNullToDefault_Test()
        {
            var table = new DataTable();
            table.Columns.Add("Count", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("When", typeof(DateTime));
            table.Columns.Add("Maybe", typeof(int));
            table.Columns.Add("Key", typeof(Guid));
            table.Rows.Add(DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value);

            var result = table.ToList<DataTableNullableRowTestModel>();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(0, result[0].Count);
            Assert.IsNull(result[0].Name);
            Assert.AreEqual(DateTime.MinValue, result[0].When);
            Assert.IsNull(result[0].Maybe);
            Assert.AreEqual(Guid.Empty, result[0].Key);
        }
    }
}
