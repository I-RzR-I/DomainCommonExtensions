using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Collections;

namespace DataTypeTests.CollectionTests
{
    [TestClass]
    public class EnumerableToDataTableDynamicTests
    {
        [TestMethod]
        public void ToDataTableDynamic_ColumnNamesAndTypes_Test()
        {
            var source = new[] { new { Id = 1, Name = "a", Active = true } };

            var result = source.ToDataTableDynamic();

            Assert.AreEqual(3, result.Columns.Count);
            Assert.AreEqual("Id", result.Columns[0].ColumnName);
            Assert.AreEqual("Name", result.Columns[1].ColumnName);
            Assert.AreEqual("Active", result.Columns[2].ColumnName);
            Assert.AreEqual(typeof(string), result.Columns[0].DataType);
            Assert.AreEqual(typeof(string), result.Columns[1].DataType);
            Assert.AreEqual(typeof(string), result.Columns[2].DataType);
        }

        [TestMethod]
        public void ToDataTableDynamic_StringValues_Test()
        {
            var source = new[]
            {
                new { Id = 1, Name = "a", Active = true },
                new { Id = 2, Name = "b", Active = false }
            };

            var result = source.ToDataTableDynamic();

            Assert.AreEqual(2, result.Rows.Count);
            Assert.AreEqual("1", result.Rows[0]["Id"]);
            Assert.AreEqual("a", result.Rows[0]["Name"]);
            Assert.AreEqual("True", result.Rows[0]["Active"]);
            Assert.AreEqual("2", result.Rows[1]["Id"]);
            Assert.AreEqual("b", result.Rows[1]["Name"]);
            Assert.AreEqual("False", result.Rows[1]["Active"]);
        }

        [TestMethod]
        public void ToDataTableDynamic_NullPropertyEmptyString_Test()
        {
            var source = new[] { new { Id = 1, Name = (string)null } };

            var result = source.ToDataTableDynamic();

            Assert.AreEqual(string.Empty, result.Rows[0]["Name"]);
        }
    }
}
