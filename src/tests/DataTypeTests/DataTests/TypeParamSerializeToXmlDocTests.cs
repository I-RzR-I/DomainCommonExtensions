using System.Xml;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Reflection.TypeParam;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class TypeParamSerializeToXmlDocTests
    {
        [TestMethod]
        public void SerializeToXmlDoc_DataContractModel_Test()
        {
            var model = new XmlDocModelTestHelper { Id = 7, Name = "seven" };

            var document = model.SerializeToXmlDoc("Envelope");

            Assert.IsNotNull(document);
            Assert.IsInstanceOfType(document, typeof(XmlDocument));
            Assert.AreEqual("Envelope", document.DocumentElement?.LocalName);
        }
    }
}
