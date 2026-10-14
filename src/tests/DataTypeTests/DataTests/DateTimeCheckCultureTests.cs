using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Primitives;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class DateTimeCheckCultureTests
    {
        [TestMethod]
        public void CheckCultureDateTime_IsoDateEnUs_Test()
        {
            var result = "2024-01-15".CheckCultureDateTime("en-US");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckCultureDateTime_MonthFirstDateEnUs_Test()
        {
            var result = "12/31/2024".CheckCultureDateTime("en-US");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckCultureDateTime_NotADateEnUs_Test()
        {
            var result = "not a date".CheckCultureDateTime("en-US");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckCultureDateTime_DayFirstDateEnUs_Test()
        {
            var result = "31/12/2024".CheckCultureDateTime("en-US");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckCultureDateTime_InvalidCultureName_Test()
        {
            var result = "2024-01-15".CheckCultureDateTime("this is not a culture!!");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckCultureDateTime_NullValue_Test()
        {
            string value = null;

            var result = value.CheckCultureDateTime("en-US");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckCultureDateTime_EmptyValue_Test()
        {
            var result = string.Empty.CheckCultureDateTime("en-US");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckCultureDateTime_NullCulture_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() => "2024-01-15".CheckCultureDateTime(null));
        }

        [TestMethod]
        public void CheckCultureDateTime_EmptyCulture_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => "2024-01-15".CheckCultureDateTime(string.Empty));
        }
    }
}
