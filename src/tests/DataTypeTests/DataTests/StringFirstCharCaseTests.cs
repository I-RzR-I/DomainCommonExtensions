using System.Globalization;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringFirstCharCaseTests
    {
        [TestMethod]
        public void FirstCharToUpper_SingleChar_Test()
        {
            var result = "a".FirstCharToUpper();

            Assert.AreEqual("A", result);
        }

        [TestMethod]
        public void FirstCharToLower_SingleChar_Test()
        {
            var result = "A".FirstCharToLower();

            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void FirstCharToUpper_Null_Test()
        {
            string input = null;

            var result = input.FirstCharToUpper();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void FirstCharToLower_Null_Test()
        {
            string input = null;

            var result = input.FirstCharToLower();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void FirstCharToUpper_Empty_Test()
        {
            var result = string.Empty.FirstCharToUpper();

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void FirstCharToLower_Empty_Test()
        {
            var result = string.Empty.FirstCharToLower();

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void FirstCharToUpper_Word_Test()
        {
            var result = "hello".FirstCharToUpper();

            Assert.AreEqual("Hello", result);
        }

        [TestMethod]
        public void FirstCharToLower_Word_Test()
        {
            var result = "Hello".FirstCharToLower();

            Assert.AreEqual("hello", result);
        }

        [TestMethod]
        public void FirstCharToUpper_TurkishCulture_Test()
        {
            string result;
            string activeCulture;
            string cultureUpperI;
            using (new CultureScopeTestHelper("tr-TR"))
            {
                activeCulture = CultureInfo.CurrentCulture.Name;
                cultureUpperI = "i".ToUpper(CultureInfo.CurrentCulture);
                result = "istanbul".FirstCharToUpper();
            }

            Assert.AreEqual("tr-TR", activeCulture);
            Assert.AreEqual("İ", cultureUpperI);
            Assert.AreEqual("Istanbul", result);
        }

        [TestMethod]
        public void UppercaseWords_TurkishCulture_Test()
        {
            string result;
            string activeCulture;
            string cultureUpperI;
            using (new CultureScopeTestHelper("tr-TR"))
            {
                activeCulture = CultureInfo.CurrentCulture.Name;
                cultureUpperI = "i".ToUpper(CultureInfo.CurrentCulture);
                result = "istanbul izmir".UppercaseWords();
            }

            Assert.AreEqual("tr-TR", activeCulture);
            Assert.AreEqual("İ", cultureUpperI);
            Assert.AreEqual("Istanbul Izmir", result);
        }

        [TestMethod]
        public void FirstCharToLower_TurkishCulture_Test()
        {
            string result;
            string activeCulture;
            string cultureUpperI;
            using (new CultureScopeTestHelper("tr-TR"))
            {
                activeCulture = CultureInfo.CurrentCulture.Name;
                cultureUpperI = "i".ToUpper(CultureInfo.CurrentCulture);
                result = "Istanbul".FirstCharToLower();
            }

            Assert.AreEqual("tr-TR", activeCulture);
            Assert.AreEqual("İ", cultureUpperI);
            Assert.AreEqual("istanbul", result);
        }
    }
}
