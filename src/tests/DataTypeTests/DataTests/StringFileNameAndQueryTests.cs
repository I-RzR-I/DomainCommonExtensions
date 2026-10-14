using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringFileNameAndQueryTests
    {
        [TestMethod]
        public void GetFileNameAndExtenstion_RepeatedExtensionInName_Test()
        {
            var result = "my.gzip.gz".GetFileNameAndExtenstion();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("my.gzip", result[0]);
            Assert.AreEqual("gz", result[1]);
        }

        [TestMethod]
        public void GetFileNameAndExtenstion_ExtensionSegmentRepeated_Test()
        {
            var result = "a.b.c.b".GetFileNameAndExtenstion();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("a.b.c", result[0]);
            Assert.AreEqual("b", result[1]);
        }

        [TestMethod]
        public void GetFileNameAndExtenstion_MultiDotName_Test()
        {
            var result = "archive.tar.gz".GetFileNameAndExtenstion();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("archive.tar", result[0]);
            Assert.AreEqual("gz", result[1]);
        }

        [TestMethod]
        public void GetFileNameAndExtenstion_NoDotPinned_Test()
        {
            var result = "README".GetFileNameAndExtenstion();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("README", result[0]);
            Assert.AreEqual("README", result[1]);
        }

        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        [TestMethod]
        public void GetFileNameAndExtenstion_MissingValue_Test(string fullName)
        {
            var result = fullName.GetFileNameAndExtenstion();

            Assert.IsNull(result);
        }

        [DataRow("https://x/p#top", "a=1", "https://x/p?a=1#top")]
        [DataRow("https://h/?x=1#f", "a=1", "https://h/?x=1&a=1#f")]
        [DataRow("https://x/p#a?b", "q=1", "https://x/p?q=1#a?b")]
        [DataRow("https://x/p#", "a=1", "https://x/p?a=1#")]
        [DataRow("https://x/p?b=2&#f", "a=1", "https://x/p?b=2&a=1#f")]
        [TestMethod]
        public void AddQueryString_UrlWithFragment_Test(string url, string query, string expected)
        {
            var result = url.AddQueryString(query);

            Assert.AreEqual(expected, result);
        }

        [DataRow("https://x/?", "a=1", "https://x/?a=1")]
        [DataRow("https://x/p?#f", "a=1", "https://x/p?a=1#f")]
        [TestMethod]
        public void AddQueryString_TrailingQuestionMark_Test(string url, string query, string expected)
        {
            var result = url.AddQueryString(query);

            Assert.AreEqual(expected, result);
            Assert.IsFalse(result.Contains("??"));
            Assert.IsFalse(result.Contains("?&"));
        }

        [DataRow("https://x/p", "a=1", "https://x/p?a=1")]
        [DataRow("https://x/p?b=2", "a=1", "https://x/p?b=2&a=1")]
        [DataRow("https://x/p?b=2&", "a=1", "https://x/p?b=2&a=1")]
        [TestMethod]
        public void AddQueryString_UrlWithoutFragment_Test(string url, string query, string expected)
        {
            var result = url.AddQueryString(query);

            Assert.AreEqual(expected, result);
        }

        [DataRow(null)]
        [DataRow("")]
        [TestMethod]
        public void AddQueryString_MissingUrl_Test(string url)
        {
            var result = url.AddQueryString("a=1");

            Assert.IsNull(result);
        }

        [DataRow(null)]
        [DataRow("")]
        [TestMethod]
        public void AddQueryString_MissingQuery_Test(string query)
        {
            var result = "https://x/p#top".AddQueryString(query);

            Assert.AreEqual("https://x/p#top", result);
        }

        [TestMethod]
        public void AddQueryString_NameValueWithFragment_Test()
        {
            var result = "https://x/p#top".AddQueryString("a", "1");

            Assert.AreEqual("https://x/p?a=1#top", result);
        }

        [TestMethod]
        public void AddQueryString_NameValueWithoutFragment_Test()
        {
            var result = "https://x/p?b=2".AddQueryString("a", "1");

            Assert.AreEqual("https://x/p?b=2&a=1", result);
        }

        [DataRow("a.txt", "a", "txt")]
        [DataRow(".gitignore", "", "gitignore")]
        [DataRow("file.", "file", "")]
        [TestMethod]
        public void GetFileNameAndExtenstion_SingleDotPinned_Test(string fileName, string expectedName, string expectedExtension)
        {
            var result = fileName.GetFileNameAndExtenstion();

            Assert.AreEqual(expectedName, result[0]);
            Assert.AreEqual(expectedExtension, result[1]);
        }
    }
}
