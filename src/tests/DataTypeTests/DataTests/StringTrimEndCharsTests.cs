using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringTrimEndCharsTests
    {
        [TestMethod]
        public void TrimEndCharAndReduceLastWhiteSpace_ArrayMixedTrailingChars_Test()
        {
            var result = "a, b;, ".TrimEndCharAndReduceLastWhiteSpace(new[] { ',', ';' });

            Assert.AreEqual("a, b", result);
        }

        [TestMethod]
        public void TrimEndCharAndReduceLastWhiteSpace_NullValue_Test()
        {
            Assert.AreEqual(string.Empty, ((string)null).TrimEndCharAndReduceLastWhiteSpace(new[] { ',' }));
            Assert.AreEqual(string.Empty, ((string)null).TrimEndCharAndReduceLastWhiteSpace(new char[0]));
        }

        [TestMethod]
        public void TrimEndCharAndReduceLastWhiteSpace_NullArray_Test()
        {
            var result = " abc  ".TrimEndCharAndReduceLastWhiteSpace((char[])null);

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void TrimEndCharAndReduceLastWhiteSpace_ArrayOrderIndependent_Test()
        {
            var result = "a, b;, ".TrimEndCharAndReduceLastWhiteSpace(new[] { ';', ',' });

            Assert.AreEqual("a, b", result);
        }

        [TestMethod]
        public void TrimEndCharAndReduceLastWhiteSpace_ArrayRemovesEveryListedChar_Test()
        {
            var result = "abc,; ".TrimEndCharAndReduceLastWhiteSpace(new[] { ',', ';' });

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void TrimEndCharAndReduceLastWhiteSpace_ArraySingleChar_Test()
        {
            var result = "abc,; ".TrimEndCharAndReduceLastWhiteSpace(new[] { ';' });

            Assert.AreEqual("abc,", result);
        }

        [TestMethod]
        public void TrimEndCharAndReduceLastWhiteSpace_EmptyArray_Test()
        {
            var result = "  abc  ".TrimEndCharAndReduceLastWhiteSpace(new char[0]);

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void TrimEndCharAndReduceLastWhiteSpace_CharAndArrayParity_Test()
        {
            var fromChar = "****, ".TrimEndCharAndReduceLastWhiteSpace(',');
            var fromArray = "****, ".TrimEndCharAndReduceLastWhiteSpace(new[] { ',' });

            Assert.AreEqual("****", fromChar);
            Assert.AreEqual("****", fromArray);
        }
    }
}
