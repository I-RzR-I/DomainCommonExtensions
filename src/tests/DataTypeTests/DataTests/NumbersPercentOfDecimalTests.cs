using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class NumbersPercentOfDecimalTests
    {
        [DataRow(50d, 200d, 25d)]
        [DataRow(150d, 100d, 150d)]
        [DataRow(-50d, 100d, -50d)]
        [DataRow(50d, -100d, -50d)]
        [DataRow(1d, 8d, 12.5d)]
        [DataRow(0d, 7d, 0d)]
        [DataRow(7d, 0d, 0d)]
        [TestMethod]
        public void PercentOf_DecimalNormalInput_Test(double value, double total, double expected)
        {
            var result = ((decimal)value).PercentOf((decimal)total);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PercentOf_DecimalRepeatingFraction_Test()
        {
            var result = 1m.PercentOf(3m);

            Assert.AreEqual(100d / 3d, result, 1e-9);
        }

        [TestMethod]
        public void PercentOf_DecimalOverflowDivision_Test()
        {
            var result = decimal.MaxValue.PercentOf(0.5m);

            Assert.AreEqual(0d, result);
        }

        [TestMethod]
        public void PercentOf_DecimalOverflowMultiplication_Test()
        {
            var maxResult = decimal.MaxValue.PercentOf(1m);
            var minResult = decimal.MinValue.PercentOf(1m);

            Assert.AreEqual(0d, maxResult);
            Assert.AreEqual(0d, minResult);
        }

        [TestMethod]
        [DoNotParallelize]
        public void PercentOf_DecimalOverflowSilent_Test()
        {
            var original = Console.Out;
            var writer = new StringWriter();
            double divisionResult;
            double multiplicationResult;

            try
            {
                Console.SetOut(writer);
                divisionResult = decimal.MaxValue.PercentOf(0.5m);
                multiplicationResult = decimal.MinValue.PercentOf(1m);
            }
            finally
            {
                Console.SetOut(original);
            }

            Assert.AreEqual(0d, divisionResult);
            Assert.AreEqual(0d, multiplicationResult);
            Assert.AreEqual(string.Empty, writer.ToString());
        }
    }
}
