using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Text;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class StringRegexMembersTests
    {
        [TestMethod]
        public void ReplaceExact_SearchTextWithDot_Test()
        {
            var result = "axb a.b".ReplaceExact("a.b", "Z");

            Assert.AreEqual("axb Z", result);
        }

        [TestMethod]
        public void ReplaceExact_WholeWordOnly_Test()
        {
            var result = "cat category cat".ReplaceExact("cat", "dog");

            Assert.AreEqual("dog category dog", result);
        }

        [TestMethod]
        public void ReplaceExact_NoMatch_Test()
        {
            var result = "hello world".ReplaceExact("xyz", "Z");

            Assert.AreEqual("hello world", result);
        }

        [TestMethod]
        public void ReplaceExact_NullInput_Test()
        {
            var result = ((string)null).ReplaceExact("a", "b");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReplaceExact_WhitespaceInput_Test()
        {
            var result = "   ".ReplaceExact("a", "b");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReplaceExact_DollarReplacement_Test()
        {
            var result = "a b".ReplaceExact("a", "$1");

            Assert.AreEqual("$1 b", result);
        }

        [TestMethod]
        public void ReplaceExact_DollarZeroReplacement_Test()
        {
            var result = "a b".ReplaceExact("a", "$0");

            Assert.AreEqual("$0 b", result);
        }

        [TestMethod]
        public void ReplaceExact_NullOldValue_Test()
        {
            var result = "a b".ReplaceExact(null, "Z");

            Assert.AreEqual("a b", result);
        }

        [TestMethod]
        public void ReplaceExact_EmptyOldValue_Test()
        {
            var result = "a b".ReplaceExact(string.Empty, "Z");

            Assert.AreEqual("a b", result);
        }

        [TestMethod]
        public void ReplaceExact_SearchTextWithPlus_Test()
        {
            var result = "use c++ now".ReplaceExact("c++", "x");

            Assert.AreEqual("use x now", result);
        }

        [TestMethod]
        public void ReplaceExact_GluedToWordChar_Test()
        {
            var result = "xa.b a.b".ReplaceExact("a.b", "Z");

            Assert.AreEqual("xa.b Z", result);
        }

        [TestMethod]
        public void IsGuid_HyphenatedLowerCase_Test()
        {
            Assert.IsTrue("d3b07384-d9a0-4c9f-8c5a-1f2e3d4c5b6a".IsGuid());
        }

        [TestMethod]
        public void IsGuid_BracedUpperCase_Test()
        {
            Assert.IsTrue("{D3B07384-D9A0-4C9F-8C5A-1F2E3D4C5B6A}".IsGuid());
        }

        [TestMethod]
        public void IsGuid_DigitsOnlyFormat_Test()
        {
            Assert.IsTrue("d3b07384d9a04c9f8c5a1f2e3d4c5b6a".IsGuid());
        }

        [TestMethod]
        public void IsGuid_OnlyOpeningBrace_Test()
        {
            Assert.IsTrue("{d3b07384-d9a0-4c9f-8c5a-1f2e3d4c5b6a".IsGuid());
        }

        [TestMethod]
        public void IsGuid_GuidOnSecondLine_Test()
        {
            Assert.IsTrue("abc\nd3b07384-d9a0-4c9f-8c5a-1f2e3d4c5b6a".IsGuid());
        }

        [TestMethod]
        public void IsGuid_TooShort_Test()
        {
            Assert.IsFalse("d3b07384-d9a0-4c9f-8c5a-1f2e3d4c5b6".IsGuid());
        }

        [TestMethod]
        public void IsGuid_NonHexChar_Test()
        {
            Assert.IsFalse("g3b07384-d9a0-4c9f-8c5a-1f2e3d4c5b6a".IsGuid());
        }

        [TestMethod]
        public void IsGuid_NullOrEmpty_Test()
        {
            Assert.IsFalse(((string)null).IsGuid());
            Assert.IsFalse(string.Empty.IsGuid());
        }

        [TestMethod]
        public void IsValidUrl_HttpDomain_Test()
        {
            Assert.IsTrue("http://rzrdev.dev".IsValidUrl());
        }

        [TestMethod]
        public void IsValidUrl_HttpsWithPathAndQuery_Test()
        {
            Assert.IsTrue("https://example.com/path/page?x=1&y=2".IsValidUrl());
        }

        [TestMethod]
        public void IsValidUrl_UrlInsideText_Test()
        {
            Assert.IsTrue("see http://example.com here".IsValidUrl());
        }

        [TestMethod]
        public void IsValidUrl_HostWithoutDot_Test()
        {
            Assert.IsFalse("http://localhost".IsValidUrl());
        }

        [TestMethod]
        public void IsValidUrl_FtpScheme_Test()
        {
            Assert.IsFalse("ftp://example.com".IsValidUrl());
        }

        [TestMethod]
        public void IsValidUrl_UpperCaseScheme_Test()
        {
            Assert.IsFalse("HTTP://example.com".IsValidUrl());
        }

        [TestMethod]
        public void IsValidUrl_Null_Test()
        {
            Assert.IsFalse(((string)null).IsValidUrl());
        }

        [TestMethod]
        public void IsValidEmail_SimpleAddress_Test()
        {
            Assert.IsTrue("email1@comp.de".IsValidEmail());
        }

        [TestMethod]
        public void IsValidEmail_DottedLocalAndSubdomain_Test()
        {
            Assert.IsTrue("first.last@sub.example.com".IsValidEmail());
        }

        [TestMethod]
        public void IsValidEmail_BracketedIpDomain_Test()
        {
            Assert.IsTrue("user@[192.168.0.1]".IsValidEmail());
        }

        [TestMethod]
        public void IsValidEmail_LongTopLevelDomain_Test()
        {
            Assert.IsFalse("user@example.museum".IsValidEmail());
        }

        [TestMethod]
        public void IsValidEmail_MissingAt_Test()
        {
            Assert.IsFalse("email1".IsValidEmail());
        }

        [TestMethod]
        public void IsValidEmail_EmptyDomainLabel_Test()
        {
            Assert.IsFalse("email1@.de".IsValidEmail());
        }

        [TestMethod]
        public void IsValidEmail_SpaceInLocalPart_Test()
        {
            Assert.IsFalse("user name@example.com".IsValidEmail());
        }

        [TestMethod]
        public void IsValidEmail_Null_Test()
        {
            Assert.IsFalse(((string)null).IsValidEmail());
        }

        [TestMethod]
        public void IsValidIpAddress_Loopback_Test()
        {
            Assert.IsTrue("127.0.0.1".IsValidIpAddress());
        }

        [TestMethod]
        public void IsValidIpAddress_Broadcast_Test()
        {
            Assert.IsTrue("255.255.255.255".IsValidIpAddress());
        }

        [TestMethod]
        public void IsValidIpAddress_AddressInsideText_Test()
        {
            Assert.IsTrue("host 10.0.0.1 up".IsValidIpAddress());
        }

        [TestMethod]
        public void IsValidIpAddress_FiveOctets_Test()
        {
            Assert.IsTrue("1.2.3.4.5".IsValidIpAddress());
        }

        [TestMethod]
        public void IsValidIpAddress_ThreeOctets_Test()
        {
            Assert.IsFalse("127.1.1".IsValidIpAddress());
        }

        [TestMethod]
        public void IsValidIpAddress_OctetOutOfRange_Test()
        {
            Assert.IsFalse("256.1.1.1".IsValidIpAddress());
        }

        [TestMethod]
        public void IsValidIpAddress_Letters_Test()
        {
            Assert.IsFalse("abc".IsValidIpAddress());
        }

        [TestMethod]
        public void IsWebUrl_WwwHost_Test()
        {
            Assert.IsTrue("www.example.com".IsWebUrl());
        }

        [TestMethod]
        public void IsWebUrl_HttpsHost_Test()
        {
            Assert.IsTrue("https://example.com/path".IsWebUrl());
        }

        [TestMethod]
        public void IsWebUrl_PlainWord_Test()
        {
            Assert.IsFalse("hello".IsWebUrl());
        }

        [TestMethod]
        public void IsWebUrl_EmptyOrWhitespace_Test()
        {
            Assert.IsFalse(string.Empty.IsWebUrl());
            Assert.IsFalse("   ".IsWebUrl());
        }
    }
}
