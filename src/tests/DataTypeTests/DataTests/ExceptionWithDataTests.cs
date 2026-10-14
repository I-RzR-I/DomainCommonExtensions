using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Diagnostics;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class ExceptionWithDataTests
    {
        [TestMethod]
        public void WithData_NullException_Test()
        {
            Exception source = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => source.WithData("key", 1));

            Assert.AreEqual("ex", exception.ParamName);
        }

        [TestMethod]
        public void WithData_NullKey_Test()
        {
            var source = new InvalidOperationException("failure");

            var exception = Assert.ThrowsException<ArgumentNullException>(() => source.WithData(null, 1));

            Assert.AreEqual("dataKey", exception.ParamName);
        }

        [TestMethod]
        public void WithData_ValidKey_Test()
        {
            var source = new InvalidOperationException("failure");

            var result = source.WithData("key", 42);

            Assert.AreSame(source, result);
            Assert.AreEqual(42, result.Data["key"]);
        }

        [TestMethod]
        public void WithData_ExistingKeyOverwritten_Test()
        {
            var source = new InvalidOperationException("failure");
            source.Data["key"] = "old";

            source.WithData("key", "new");

            Assert.AreEqual(1, source.Data.Count);
            Assert.AreEqual("new", source.Data["key"]);
        }

        [TestMethod]
        public void WithData_NullValue_Test()
        {
            var source = new InvalidOperationException("failure");

            var result = source.WithData("key", null);

            Assert.AreSame(source, result);
            Assert.IsTrue(result.Data.Contains("key"));
            Assert.IsNull(result.Data["key"]);
        }
    }
}
