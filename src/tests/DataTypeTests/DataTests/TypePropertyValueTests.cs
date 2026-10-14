using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Reflection;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TypePropertyValueTests
    {
        [TestMethod]
        public void GetPropertyValue_PascalCaseProperty_Test()
        {
            var source = new { Name = "abc" };

            Assert.AreEqual("abc", source.GetPropertyValue("name"));
            Assert.AreEqual("abc", source.GetStringPropertyValue("Name"));
        }

        [TestMethod]
        public void GetPropertyValue_OneLetterLowercaseProperty_Test()
        {
            var source = new { x = 7 };

            Assert.AreEqual(7, source.GetPropertyValue("x"));
            Assert.AreEqual("7", source.GetStringPropertyValue("x"));
        }

        [TestMethod]
        public void GetPropertyValue_LowercaseDeclaredProperty_Test()
        {
            var source = new { id = 5 };

            Assert.AreEqual(5, source.GetPropertyValue("id"));
            Assert.AreEqual("5", source.GetStringPropertyValue("id"));
        }

        [TestMethod]
        public void GetPropertyValue_MissingProperty_Test()
        {
            var source = new { Name = "abc" };

            Assert.IsNull(source.GetPropertyValue("missing"));
            Assert.AreEqual(string.Empty, source.GetStringPropertyValue("missing"));
        }
    }
}
