using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DateTimeMonthEdgeTests
    {
        [DataRow(1)]
        [DataRow(15)]
        [DataRow(31)]
        [TestMethod]
        public void EndOfMonth_LastRepresentableMonth_Test(int day)
        {
            var input = new DateTime(9999, 12, day, 10, 30, 0);

            var result = input.EndOfMonth();

            Assert.AreEqual(new DateTime(9999, 12, 31), result);
        }

        [TestMethod]
        public void EndOfMonth_MaxValue_Test()
        {
            var result = DateTime.MaxValue.EndOfMonth();

            Assert.AreEqual(new DateTime(9999, 12, 31), result);
            Assert.AreEqual(TimeSpan.Zero, result.TimeOfDay);
        }

        [DataRow(DateTimeKind.Utc)]
        [DataRow(DateTimeKind.Local)]
        [DataRow(DateTimeKind.Unspecified)]
        [TestMethod]
        public void EndOfMonth_EdgeKind_Test(DateTimeKind kind)
        {
            var input = new DateTime(9999, 12, 15, 0, 0, 0, kind);

            var result = input.EndOfMonth();

            Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
        }

        [DataRow(2024, 2, 10, 29)]
        [DataRow(2023, 2, 1, 28)]
        [DataRow(2024, 4, 30, 30)]
        [DataRow(2024, 12, 5, 31)]
        [DataRow(1, 1, 1, 31)]
        [DataRow(1, 1, 15, 31)]
        [DataRow(9999, 11, 30, 30)]
        [TestMethod]
        public void EndOfMonth_RegularMonth_Test(int year, int month, int day, int expectedDay)
        {
            var input = new DateTime(year, month, day, 13, 45, 30);

            var result = input.EndOfMonth();

            Assert.AreEqual(new DateTime(year, month, expectedDay), result);
            Assert.AreEqual(TimeSpan.Zero, result.TimeOfDay);
        }

        [DataRow(DateTimeKind.Utc)]
        [DataRow(DateTimeKind.Local)]
        [DataRow(DateTimeKind.Unspecified)]
        [TestMethod]
        public void EndOfMonth_InputKind_Test(DateTimeKind kind)
        {
            var input = new DateTime(2024, 6, 15, 0, 0, 0, kind);

            var result = input.EndOfMonth();

            Assert.AreEqual(new DateTime(2024, 6, 30), result);
            Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
        }

        [DataRow(1)]
        [DataRow(15)]
        [DataRow(31)]
        [TestMethod]
        public void EndOfPreviousMonth_FirstRepresentableMonth_Test(int day)
        {
            var input = new DateTime(1, 1, day);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => input.EndOfPreviousMonth());
        }

        [TestMethod]
        public void EndOfPreviousMonth_MinValue_Test()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => DateTime.MinValue.EndOfPreviousMonth());
        }

        [DataRow(1, 2, 15, 1, 1, 31)]
        [DataRow(9999, 12, 15, 9999, 11, 30)]
        [DataRow(9999, 12, 31, 9999, 11, 30)]
        [DataRow(2024, 3, 31, 2024, 2, 29)]
        [DataRow(2023, 3, 1, 2023, 2, 28)]
        [DataRow(2024, 1, 10, 2023, 12, 31)]
        [TestMethod]
        public void EndOfPreviousMonth_RegularMonth_Test(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
        {
            var input = new DateTime(year, month, day, 8, 15, 0);

            var result = input.EndOfPreviousMonth();

            Assert.AreEqual(new DateTime(expectedYear, expectedMonth, expectedDay), result);
            Assert.AreEqual(TimeSpan.Zero, result.TimeOfDay);
        }

        [TestMethod]
        public void EndOfPreviousMonth_MaxValue_Test()
        {
            var result = DateTime.MaxValue.EndOfPreviousMonth();

            Assert.AreEqual(new DateTime(9999, 11, 30), result);
        }

        [DataRow(DateTimeKind.Utc)]
        [DataRow(DateTimeKind.Local)]
        [DataRow(DateTimeKind.Unspecified)]
        [TestMethod]
        public void EndOfPreviousMonth_InputKind_Test(DateTimeKind kind)
        {
            var input = new DateTime(2024, 3, 15, 0, 0, 0, kind);

            var result = input.EndOfPreviousMonth();

            Assert.AreEqual(new DateTime(2024, 2, 29), result);
            Assert.AreEqual(DateTimeKind.Unspecified, result.Kind);
        }

        [DataRow(1, 1, 1)]
        [DataRow(1, 1, 15)]
        [DataRow(9999, 12, 1)]
        [DataRow(9999, 12, 31)]
        [TestMethod]
        public void DaysInMonth_RepresentableEdge_Test(int year, int month, int day)
        {
            var input = new DateTime(year, month, day);

            var result = input.DaysInMonth();

            Assert.AreEqual(31, result);
        }

        [TestMethod]
        public void DaysInMonth_MinAndMaxValue_Test()
        {
            var minResult = DateTime.MinValue.DaysInMonth();
            var maxResult = DateTime.MaxValue.DaysInMonth();

            Assert.AreEqual(31, minResult);
            Assert.AreEqual(31, maxResult);
        }

        [DataRow(2024, 2, 15, 29)]
        [DataRow(2023, 2, 15, 28)]
        [DataRow(1900, 2, 1, 28)]
        [DataRow(2000, 2, 1, 29)]
        [DataRow(2024, 4, 1, 30)]
        [DataRow(1, 2, 1, 28)]
        [DataRow(9999, 11, 30, 30)]
        [TestMethod]
        public void DaysInMonth_RegularMonth_Test(int year, int month, int day, int expected)
        {
            var input = new DateTime(year, month, day, 23, 59, 59);

            var result = input.DaysInMonth();

            Assert.AreEqual(expected, result);
        }
    }
}
