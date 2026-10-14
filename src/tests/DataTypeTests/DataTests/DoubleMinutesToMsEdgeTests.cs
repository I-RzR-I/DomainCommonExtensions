using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DoubleMinutesToMsEdgeTests
    {
        [TestMethod]
        public void MinutesToMs_OneAndHalfMinutes_Test()
        {
            var result = 1.5.MinutesToMs();

            Assert.AreEqual(90000, result);
        }

        [TestMethod]
        public void MinutesToMs_PositiveOverflow_Test()
        {
            var result = 1e6.MinutesToMs();

            Assert.AreEqual(int.MaxValue, result);
        }

        [TestMethod]
        public void MinutesToMs_NegativeOverflow_Test()
        {
            var result = (-1e6).MinutesToMs();

            Assert.AreEqual(int.MinValue, result);
        }
    }
}
