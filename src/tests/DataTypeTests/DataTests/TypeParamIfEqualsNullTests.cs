using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Reflection.TypeParam;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TypeParamIfEqualsNullTests
    {
        [TestMethod]
        public void IfEquals_EqualValues_Test()
        {
            var result = "a".IfEquals("a", "r");

            Assert.AreEqual("r", result);
        }

        [TestMethod]
        public void IfEquals_DifferentValues_Test()
        {
            var result = "a".IfEquals("g", "r");

            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void IfEquals_NullSourceNonNullSearch_Test()
        {
            var result = ((string)null).IfEquals("x", "r");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IfEquals_NonNullSourceNullSearch_Test()
        {
            var result = "a".IfEquals(null, "r");

            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void IfEquals_BothNull_Test()
        {
            var result = ((string)null).IfEquals(null, "r");

            Assert.AreEqual("r", result);
        }

        [TestMethod]
        public void IfEquals_NullableIntNullSource_Test()
        {
            var result = TExtensions.IfEquals<int?>(null, 5, 9);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IfNotEquals_EqualValues_Test()
        {
            var result = "a".IfNotEquals("a", "r");

            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void IfNotEquals_DifferentValues_Test()
        {
            var result = "a".IfNotEquals("g", "r");

            Assert.AreEqual("r", result);
        }

        [TestMethod]
        public void IfNotEquals_NullSourceNonNullSearch_Test()
        {
            var result = ((string)null).IfNotEquals("x", "r");

            Assert.AreEqual("r", result);
        }

        [TestMethod]
        public void IfNotEquals_NonNullSourceNullSearch_Test()
        {
            var result = "a".IfNotEquals(null, "r");

            Assert.AreEqual("r", result);
        }

        [TestMethod]
        public void IfNotEquals_BothNull_Test()
        {
            var result = ((string)null).IfNotEquals(null, "r");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void IfNotEquals_NullableIntBothNull_Test()
        {
            var result = TExtensions.IfNotEquals<int?>(null, null, 9);

            Assert.IsNull(result);
        }
    }
}
