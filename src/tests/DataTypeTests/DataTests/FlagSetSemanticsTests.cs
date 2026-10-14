using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class FlagSetSemanticsTests
    {
        [TestMethod]
        public void IsFlagSet_LongPartialMatch_Test()
        {
            var result = 1L.IsFlagSet(3L);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsFlagSet_LongZeroFlag_Test()
        {
            var zeroResult = 0L.IsFlagSet(0L);
            var nonZeroResult = 5L.IsFlagSet(0L);

            Assert.IsFalse(zeroResult);
            Assert.IsFalse(nonZeroResult);
        }

        [DataRow(3L, 3L, true)]
        [DataRow(2L, 3L, true)]
        [DataRow(7L, 2L, true)]
        [DataRow(4L, 3L, false)]
        [DataRow(0L, 3L, false)]
        [DataRow(long.MinValue, long.MinValue, true)]
        [DataRow(-1L, 8L, true)]
        [TestMethod]
        public void IsFlagSet_LongTruthTable_Test(long value, long flag, bool expected)
        {
            var result = value.IsFlagSet(flag);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void IsFlagSet_IntPartialMatch_Test()
        {
            var result = 1.IsFlagSet(3);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsFlagSet_IntZeroFlag_Test()
        {
            var zeroResult = 0.IsFlagSet(0);
            var nonZeroResult = 5.IsFlagSet(0);

            Assert.IsFalse(zeroResult);
            Assert.IsFalse(nonZeroResult);
        }

        [DataRow(3, 3, true)]
        [DataRow(2, 3, true)]
        [DataRow(7, 2, true)]
        [DataRow(4, 3, false)]
        [DataRow(0, 3, false)]
        [DataRow(int.MinValue, int.MinValue, true)]
        [DataRow(-1, 8, true)]
        [TestMethod]
        public void IsFlagSet_IntTruthTable_Test(int value, int flag, bool expected)
        {
            var result = value.IsFlagSet(flag);

            Assert.AreEqual(expected, result);
        }
    }
}
