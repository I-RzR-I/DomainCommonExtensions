using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class IntExcelColumnNameTests
    {
        [DataRow(-1)]
        [DataRow(18278)]
        [TestMethod]
        public void GetExcelColumnName_OutOfRange_Test(int columnIndex)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => columnIndex.GetExcelColumnName());

            Assert.AreEqual("columnIndex", exception.ParamName);
        }

        [DataRow(0, "A")]
        [DataRow(25, "Z")]
        [DataRow(26, "AA")]
        [DataRow(701, "ZZ")]
        [DataRow(702, "AAA")]
        [DataRow(18277, "ZZZ")]
        [TestMethod]
        public void GetExcelColumnName_Boundaries_Test(int columnIndex, string expected)
        {
            var result = columnIndex.GetExcelColumnName();

            Assert.AreEqual(expected, result);
        }
    }
}
