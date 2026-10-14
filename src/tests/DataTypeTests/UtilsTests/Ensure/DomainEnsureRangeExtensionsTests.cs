using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Validation;

namespace DataTypeTests.UtilsTests.Ensure
{
    [TestClass]
    public class DomainEnsureRangeExtensionsTests
    {
        [TestMethod]
        public void ThrowIfNegative_LocalVariableName_Test()
        {
            int x = -1;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => x.ThrowIfNegative());

            Assert.AreEqual("x", exception.ParamName);
            Assert.AreEqual(-1, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfNegative_ExplicitParamName_Test()
        {
            var amount = -1;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative("custom"));

            Assert.AreEqual("custom", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNegative_NullParamName_Test()
        {
            var amount = -1;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative(paramName: null));

            Assert.IsNull(exception.ParamName);
            Assert.AreEqual(-1, exception.ActualValue);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(int.MaxValue)]
        public void ThrowIfNegative_IntNonNegativePasses_Test(int amount)
        {
            amount.ThrowIfNegative();
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(int.MinValue)]
        public void ThrowIfNegative_IntNegativeThrows_Test(int amount)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(1)]
        [DataRow(int.MinValue)]
        [DataRow(int.MaxValue)]
        public void ThrowIfZero_IntNonZeroPasses_Test(int amount)
        {
            amount.ThrowIfZero();
        }

        [TestMethod]
        public void ThrowIfZero_IntZeroThrows_Test()
        {
            var amount = 0;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfZero());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(0, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfZero_ExplicitParamName_Test()
        {
            var amount = 0;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfZero("custom"));

            Assert.AreEqual("custom", exception.ParamName);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(int.MaxValue)]
        public void ThrowIfNegativeOrZero_IntPositivePasses_Test(int amount)
        {
            amount.ThrowIfNegativeOrZero();
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(int.MinValue)]
        public void ThrowIfNegativeOrZero_IntNonPositiveThrows_Test(int amount)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegativeOrZero());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfNegativeOrZero_NullParamName_Test()
        {
            var amount = 0;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegativeOrZero(paramName: null));

            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        [DataRow(1, 1, 10)]
        [DataRow(10, 1, 10)]
        [DataRow(5, 1, 10)]
        [DataRow(0, 0, 0)]
        [DataRow(-5, -10, -1)]
        [DataRow(int.MinValue, int.MinValue, int.MaxValue)]
        [DataRow(int.MaxValue, int.MinValue, int.MaxValue)]
        public void ThrowIfOutOfRange_IntInsideInclusiveRangePasses_Test(int amount, int min, int max)
        {
            amount.ThrowIfOutOfRange(min, max);
        }

        [TestMethod]
        [DataRow(0, 1, 10)]
        [DataRow(11, 1, 10)]
        [DataRow(1, 0, 0)]
        [DataRow(int.MinValue, int.MinValue + 1, int.MaxValue)]
        [DataRow(int.MaxValue, int.MinValue, int.MaxValue - 1)]
        public void ThrowIfOutOfRange_IntOutsideRangeThrows_Test(int amount, int min, int max)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfOutOfRange(min, max));

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_IntMinGreaterThanMax_Test()
        {
            var amount = 5;

            var exception = Assert.ThrowsException<ArgumentException>(() => amount.ThrowIfOutOfRange(min: 10, max: 1));

            Assert.AreEqual("min", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_IntExplicitParamName_Test()
        {
            var amount = 11;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfOutOfRange(1, 10, "custom"));

            Assert.AreEqual("custom", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_IntNullParamName_Test()
        {
            var amount = 11;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfOutOfRange(1, 10, paramName: null));

            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        [DataRow(0L)]
        [DataRow(1L)]
        [DataRow(long.MaxValue)]
        public void ThrowIfNegative_LongNonNegativePasses_Test(long amount)
        {
            amount.ThrowIfNegative();
        }

        [TestMethod]
        [DataRow(-1L)]
        [DataRow(int.MinValue - 1L)]
        [DataRow(long.MinValue)]
        public void ThrowIfNegative_LongNegativeThrows_Test(long amount)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        [DataRow(-1L)]
        [DataRow(1L)]
        [DataRow(long.MinValue)]
        [DataRow(long.MaxValue)]
        public void ThrowIfZero_LongNonZeroPasses_Test(long amount)
        {
            amount.ThrowIfZero();
        }

        [TestMethod]
        public void ThrowIfZero_LongZeroThrows_Test()
        {
            var amount = 0L;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfZero());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(0L, exception.ActualValue);
        }

        [TestMethod]
        [DataRow(1L)]
        [DataRow(long.MaxValue)]
        public void ThrowIfNegativeOrZero_LongPositivePasses_Test(long amount)
        {
            amount.ThrowIfNegativeOrZero();
        }

        [TestMethod]
        [DataRow(0L)]
        [DataRow(-1L)]
        [DataRow(long.MinValue)]
        public void ThrowIfNegativeOrZero_LongNonPositiveThrows_Test(long amount)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegativeOrZero());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        [DataRow(1L, 1L, 10L)]
        [DataRow(10L, 1L, 10L)]
        [DataRow(0L, 0L, 0L)]
        [DataRow(int.MaxValue + 1L, 0L, long.MaxValue)]
        [DataRow(long.MinValue, long.MinValue, long.MaxValue)]
        [DataRow(long.MaxValue, long.MinValue, long.MaxValue)]
        public void ThrowIfOutOfRange_LongInsideInclusiveRangePasses_Test(long amount, long min, long max)
        {
            amount.ThrowIfOutOfRange(min, max);
        }

        [TestMethod]
        [DataRow(0L, 1L, 10L)]
        [DataRow(11L, 1L, 10L)]
        [DataRow(int.MaxValue + 1L, 0L, int.MaxValue)]
        [DataRow(long.MinValue, long.MinValue + 1L, long.MaxValue)]
        [DataRow(long.MaxValue, long.MinValue, long.MaxValue - 1L)]
        public void ThrowIfOutOfRange_LongOutsideRangeThrows_Test(long amount, long min, long max)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfOutOfRange(min, max));

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_LongMinGreaterThanMax_Test()
        {
            var amount = 5L;

            var exception = Assert.ThrowsException<ArgumentException>(() => amount.ThrowIfOutOfRange(min: 10L, max: 1L));

            Assert.AreEqual("min", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNegative_LongExplicitParamName_Test()
        {
            var amount = -1L;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative("custom"));

            Assert.AreEqual("custom", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNegative_LongNullParamName_Test()
        {
            var amount = -1L;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative(paramName: null));

            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        [DataRow(0d)]
        [DataRow(1d)]
        [DataRow(double.Epsilon)]
        [DataRow(double.MaxValue)]
        [DataRow(double.PositiveInfinity)]
        [DataRow(double.NaN)]
        public void ThrowIfNegative_DoubleNonNegativePasses_Test(double amount)
        {
            amount.ThrowIfNegative();
        }

        [TestMethod]
        [DataRow(-1d)]
        [DataRow(-double.Epsilon)]
        [DataRow(double.MinValue)]
        [DataRow(double.NegativeInfinity)]
        public void ThrowIfNegative_DoubleNegativeThrows_Test(double amount)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfNegative_DoubleNegativeZeroPasses_Test()
        {
            var amount = BitConverter.Int64BitsToDouble(long.MinValue);

            amount.ThrowIfNegative();

            Assert.IsTrue(double.IsNegative(amount));
        }

        [TestMethod]
        [DataRow(1d)]
        [DataRow(-1d)]
        [DataRow(double.Epsilon)]
        [DataRow(double.PositiveInfinity)]
        [DataRow(double.NegativeInfinity)]
        [DataRow(double.NaN)]
        public void ThrowIfZero_DoubleNonZeroPasses_Test(double amount)
        {
            amount.ThrowIfZero();
        }

        [TestMethod]
        public void ThrowIfZero_DoubleZeroThrows_Test()
        {
            var amount = 0d;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfZero());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(0d, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfZero_DoubleNegativeZeroThrows_Test()
        {
            var amount = BitConverter.Int64BitsToDouble(long.MinValue);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfZero());

            Assert.AreEqual("amount", exception.ParamName);
        }

        [TestMethod]
        [DataRow(double.Epsilon)]
        [DataRow(1d)]
        [DataRow(double.MaxValue)]
        [DataRow(double.PositiveInfinity)]
        [DataRow(double.NaN)]
        public void ThrowIfNegativeOrZero_DoublePositiveOrNaNPasses_Test(double amount)
        {
            amount.ThrowIfNegativeOrZero();
        }

        [TestMethod]
        [DataRow(0d)]
        [DataRow(-double.Epsilon)]
        [DataRow(-1d)]
        [DataRow(double.MinValue)]
        [DataRow(double.NegativeInfinity)]
        public void ThrowIfNegativeOrZero_DoubleNonPositiveThrows_Test(double amount)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegativeOrZero());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfNegativeOrZero_DoubleNegativeZeroThrows_Test()
        {
            var amount = BitConverter.Int64BitsToDouble(long.MinValue);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegativeOrZero());

            Assert.AreEqual("amount", exception.ParamName);
        }

        [TestMethod]
        [DataRow(0d, 0d, 1d)]
        [DataRow(1d, 0d, 1d)]
        [DataRow(0.5d, 0d, 1d)]
        [DataRow(2d, 2d, 2d)]
        [DataRow(double.PositiveInfinity, 0d, double.PositiveInfinity)]
        [DataRow(double.NegativeInfinity, double.NegativeInfinity, 0d)]
        [DataRow(double.MaxValue, double.NegativeInfinity, double.PositiveInfinity)]
        [DataRow(double.MinValue, double.MinValue, double.MaxValue)]
        public void ThrowIfOutOfRange_DoubleInsideInclusiveRangePasses_Test(double amount, double min, double max)
        {
            amount.ThrowIfOutOfRange(min, max);
        }

        [TestMethod]
        [DataRow(-double.Epsilon, 0d, 1d)]
        [DataRow(1.0000001d, 0d, 1d)]
        [DataRow(double.PositiveInfinity, 0d, double.MaxValue)]
        [DataRow(double.NegativeInfinity, double.MinValue, 0d)]
        public void ThrowIfOutOfRange_DoubleOutsideRangeThrows_Test(double amount, double min, double max)
        {
            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfOutOfRange(min, max));

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_DoubleNaNValueThrows_Test()
        {
            var amount = double.NaN;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => amount.ThrowIfOutOfRange(double.NegativeInfinity, double.PositiveInfinity));

            Assert.AreEqual("amount", exception.ParamName);
            Assert.IsTrue(double.IsNaN((double)exception.ActualValue));
        }

        [TestMethod]
        public void ThrowIfOutOfRange_DoubleNaNMinThrows_Test()
        {
            var amount = 1d;

            var exception = Assert.ThrowsException<ArgumentException>(() => amount.ThrowIfOutOfRange(double.NaN, 10d));

            Assert.AreEqual("min", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_DoubleNaNMaxThrows_Test()
        {
            var amount = 1d;

            var exception = Assert.ThrowsException<ArgumentException>(() => amount.ThrowIfOutOfRange(0d, double.NaN));

            Assert.AreEqual("max", exception.ParamName);
        }

        [TestMethod]
        [DataRow(10d, 1d)]
        [DataRow(double.Epsilon, 0d)]
        [DataRow(double.PositiveInfinity, double.NegativeInfinity)]
        public void ThrowIfOutOfRange_DoubleMinGreaterThanMax_Test(double min, double max)
        {
            var amount = 5d;

            var exception = Assert.ThrowsException<ArgumentException>(() => amount.ThrowIfOutOfRange(min, max));

            Assert.AreEqual("min", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_DoubleExplicitParamName_Test()
        {
            var amount = 11d;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfOutOfRange(1d, 10d, "custom"));

            Assert.AreEqual("custom", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNegative_DoubleNullParamName_Test()
        {
            var amount = -1d;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative(paramName: null));

            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        [DataRow("0")]
        [DataRow("-0.0")]
        [DataRow("0.0000000001")]
        [DataRow("1")]
        [DataRow("79228162514264337593543950335")]
        public void ThrowIfNegative_DecimalNonNegativePasses_Test(string text)
        {
            var amount = decimal.Parse(text, CultureInfo.InvariantCulture);

            amount.ThrowIfNegative();
        }

        [TestMethod]
        [DataRow("-0.0000000001")]
        [DataRow("-1")]
        [DataRow("-79228162514264337593543950335")]
        public void ThrowIfNegative_DecimalNegativeThrows_Test(string text)
        {
            var amount = decimal.Parse(text, CultureInfo.InvariantCulture);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfNegative_DecimalMinValueThrows_Test()
        {
            var amount = decimal.MinValue;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegative());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(decimal.MinValue, exception.ActualValue);
        }

        [TestMethod]
        [DataRow("1")]
        [DataRow("-1")]
        [DataRow("0.0000000001")]
        [DataRow("79228162514264337593543950335")]
        [DataRow("-79228162514264337593543950335")]
        public void ThrowIfZero_DecimalNonZeroPasses_Test(string text)
        {
            var amount = decimal.Parse(text, CultureInfo.InvariantCulture);

            amount.ThrowIfZero();
        }

        [TestMethod]
        [DataRow("0")]
        [DataRow("0.000")]
        [DataRow("-0")]
        public void ThrowIfZero_DecimalZeroThrows_Test(string text)
        {
            var amount = decimal.Parse(text, CultureInfo.InvariantCulture);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfZero());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(0m, exception.ActualValue);
        }

        [TestMethod]
        [DataRow("0.0000000001")]
        [DataRow("1")]
        [DataRow("79228162514264337593543950335")]
        public void ThrowIfNegativeOrZero_DecimalPositivePasses_Test(string text)
        {
            var amount = decimal.Parse(text, CultureInfo.InvariantCulture);

            amount.ThrowIfNegativeOrZero();
        }

        [TestMethod]
        [DataRow("0")]
        [DataRow("-0.0000000001")]
        [DataRow("-1")]
        [DataRow("-79228162514264337593543950335")]
        public void ThrowIfNegativeOrZero_DecimalNonPositiveThrows_Test(string text)
        {
            var amount = decimal.Parse(text, CultureInfo.InvariantCulture);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfNegativeOrZero());

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        [DataRow("1", "1", "10")]
        [DataRow("10", "1", "10")]
        [DataRow("10.000", "1", "10")]
        [DataRow("0.5", "0", "1")]
        [DataRow("-79228162514264337593543950335", "-79228162514264337593543950335", "79228162514264337593543950335")]
        [DataRow("79228162514264337593543950335", "-79228162514264337593543950335", "79228162514264337593543950335")]
        public void ThrowIfOutOfRange_DecimalInsideInclusiveRangePasses_Test(string text, string minText, string maxText)
        {
            var amount = decimal.Parse(text, CultureInfo.InvariantCulture);
            var min = decimal.Parse(minText, CultureInfo.InvariantCulture);
            var max = decimal.Parse(maxText, CultureInfo.InvariantCulture);

            amount.ThrowIfOutOfRange(min, max);
        }

        [TestMethod]
        [DataRow("0.9999999999", "1", "10")]
        [DataRow("10.0000000001", "1", "10")]
        [DataRow("79228162514264337593543950335", "0", "79228162514264337593543950334")]
        [DataRow("-79228162514264337593543950335", "-79228162514264337593543950334", "0")]
        public void ThrowIfOutOfRange_DecimalOutsideRangeThrows_Test(string text, string minText, string maxText)
        {
            var amount = decimal.Parse(text, CultureInfo.InvariantCulture);
            var min = decimal.Parse(minText, CultureInfo.InvariantCulture);
            var max = decimal.Parse(maxText, CultureInfo.InvariantCulture);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfOutOfRange(min, max));

            Assert.AreEqual("amount", exception.ParamName);
            Assert.AreEqual(amount, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_DecimalMinGreaterThanMax_Test()
        {
            var amount = 5m;

            var exception = Assert.ThrowsException<ArgumentException>(() => amount.ThrowIfOutOfRange(min: 10m, max: 1m));

            Assert.AreEqual("min", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_DecimalExplicitParamName_Test()
        {
            var amount = 11m;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfOutOfRange(1m, 10m, "custom"));

            Assert.AreEqual("custom", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfOutOfRange_DecimalNullParamName_Test()
        {
            var amount = 11m;

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => amount.ThrowIfOutOfRange(1m, 10m, paramName: null));

            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNegative_TimeSpanKeepsNullParamName_Test()
        {
            var span = TimeSpan.FromSeconds(-1);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => span.ThrowIfNegative());

            Assert.IsNull(exception.ParamName);
            Assert.AreEqual(span, exception.ActualValue);
        }

        [TestMethod]
        public void ThrowIfNegative_TimeSpanExplicitParamName_Test()
        {
            var span = TimeSpan.FromTicks(-1);

            var exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => span.ThrowIfNegative("span"));

            Assert.AreEqual("span", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNegative_TimeSpanZeroPasses_Test()
        {
            var span = TimeSpan.Zero;

            span.ThrowIfNegative();
            TimeSpan.MaxValue.ThrowIfNegative();
        }
    }
}
