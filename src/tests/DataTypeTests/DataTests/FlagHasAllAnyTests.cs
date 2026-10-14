using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class FlagHasAllAnyTests
    {
        [DataRow(3L, 3L, true)]
        [DataRow(1L, 3L, false)]
        [DataRow(7L, 5L, true)]
        [DataRow(2L, 5L, false)]
        [DataRow(0L, 1L, false)]
        [DataRow(0L, 0L, true)]
        [DataRow(5L, 0L, true)]
        [DataRow(-1L, long.MinValue, true)]
        [TestMethod]
        public void HasAllFlags_LongTruthTable_Test(long value, long flags, bool expected)
        {
            var result = value.HasAllFlags(flags);

            Assert.AreEqual(expected, result);
        }

        [DataRow(1L, 3L, true)]
        [DataRow(4L, 3L, false)]
        [DataRow(3L, 3L, true)]
        [DataRow(0L, 3L, false)]
        [DataRow(0L, 0L, false)]
        [DataRow(5L, 0L, false)]
        [DataRow(long.MinValue, long.MinValue, true)]
        [TestMethod]
        public void HasAnyFlag_LongTruthTable_Test(long value, long flags, bool expected)
        {
            var result = value.HasAnyFlag(flags);

            Assert.AreEqual(expected, result);
        }

        [DataRow(3, 3, true)]
        [DataRow(1, 3, false)]
        [DataRow(7, 5, true)]
        [DataRow(2, 5, false)]
        [DataRow(0, 1, false)]
        [DataRow(0, 0, true)]
        [DataRow(5, 0, true)]
        [DataRow(-1, int.MinValue, true)]
        [TestMethod]
        public void HasAllFlags_IntTruthTable_Test(int value, int flags, bool expected)
        {
            var result = value.HasAllFlags(flags);

            Assert.AreEqual(expected, result);
        }

        [DataRow(1, 3, true)]
        [DataRow(4, 3, false)]
        [DataRow(3, 3, true)]
        [DataRow(0, 3, false)]
        [DataRow(0, 0, false)]
        [DataRow(5, 0, false)]
        [DataRow(int.MinValue, int.MinValue, true)]
        [TestMethod]
        public void HasAnyFlag_IntTruthTable_Test(int value, int flags, bool expected)
        {
            var result = value.HasAnyFlag(flags);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void HasAnyFlag_LongMatchesIsFlagSet_Test()
        {
            var values = new[] { 0L, 1L, 2L, 3L, 4L, 7L, -1L, long.MinValue };

            foreach (var value in values)
                foreach (var flags in values)
                    Assert.AreEqual(value.IsFlagSet(flags), value.HasAnyFlag(flags), $"value={value}, flags={flags}");
        }

        [TestMethod]
        public void HasAnyFlag_IntMatchesIsFlagSet_Test()
        {
            var values = new[] { 0, 1, 2, 3, 4, 7, -1, int.MinValue };

            foreach (var value in values)
                foreach (var flags in values)
                    Assert.AreEqual(value.IsFlagSet(flags), value.HasAnyFlag(flags), $"value={value}, flags={flags}");
        }
    }
}
