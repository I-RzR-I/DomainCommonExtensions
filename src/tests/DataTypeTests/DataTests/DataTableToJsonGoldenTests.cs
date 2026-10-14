using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Data;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DataTableToJsonGoldenTests
    {
        [TestMethod]
        public void ToJson_GoldenTypedAndDbNullRows_Test()
        {
            var table = new DataTable();
            table.Columns.Add("IntValue", typeof(int));
            table.Columns.Add("LongValue", typeof(long));
            table.Columns.Add("DecimalValue", typeof(decimal));
            table.Columns.Add("DoubleValue", typeof(double));
            table.Columns.Add("BoolValue", typeof(bool));
            table.Columns.Add("DateValue", typeof(DateTime));
            table.Columns.Add("GuidValue", typeof(Guid));
            table.Columns.Add("TextValue", typeof(string));
            table.Rows.Add(
                42,
                9000000000L,
                12.5m,
                1.25d,
                true,
                new DateTime(2024, 1, 2, 3, 4, 5, 678, DateTimeKind.Unspecified),
                new Guid("0f8fad5b-d9cb-469f-a165-70867728950e"),
                "abc");
            table.Rows.Add(
                DBNull.Value,
                DBNull.Value,
                DBNull.Value,
                DBNull.Value,
                DBNull.Value,
                DBNull.Value,
                DBNull.Value,
                DBNull.Value);
            const string expected =
                "[{\"intValue\":42,\"longValue\":9000000000,\"decimalValue\":12.5,\"doubleValue\":1.25,\"boolValue\":true,\"dateValue\":\"2024-01-02T03:04:05.678\",\"guidValue\":\"0f8fad5b-d9cb-469f-a165-70867728950e\",\"textValue\":\"abc\"},"
                + "{\"intValue\":0,\"longValue\":0,\"decimalValue\":0,\"doubleValue\":0,\"boolValue\":false,\"dateValue\":\"0001-01-01T00:00:00\",\"guidValue\":\"00000000-0000-0000-0000-000000000000\",\"textValue\":null}]";

            var json = table.ToJson();

            Assert.AreEqual(expected, json);
        }

        [TestMethod]
        public void ToJson_ItemColumnValue_Test()
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Item", typeof(int));
            table.Rows.Add(1, 42);

            var json = table.ToJson();

            Assert.AreEqual("[{\"id\":1,\"item\":42}]", json);
        }

        [TestMethod]
        public void ToJson_CamelCaseNameCollision_Test()
        {
            var table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("name", typeof(string));
            table.Rows.Add("a", "b");

            Assert.ThrowsException<InvalidOperationException>(() => table.ToJson());
        }
    }
}
