using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO.Ini;
namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class IniFileHelperParsingTests
    {
        [TestMethod]
        public void GetEntry_HyphenatedKeyIsParsed_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nmy-key=1");

            var result = helper.GetEntry("s", "my-key", "none");

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void GetEntry_SingleCharacterKeyIsParsed_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\na=1");

            var result = helper.GetEntry("s", "a", "none");

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void GetEntry_HyphenatedSectionIsParsed_Test()
        {
            var helper = IniFileHelper.Instance.Load("[my-section]\r\nkey=2");

            var result = helper.GetEntry("my-section", "key", "none");

            Assert.AreEqual("2", result);
        }

        [TestMethod]
        public void GetEntry_UnclosedSectionHeaderEndsPreviousSection_Test()
        {
            var helper = IniFileHelper.Instance.Load("[safe]\r\nkey=1\r\n[bad-section\r\nkey=2");

            var result = helper.GetEntry("safe", "key", "none");

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void GetEntry_EmptySectionHeaderEndsPreviousSection_Test()
        {
            var helper = IniFileHelper.Instance.Load("[safe]\r\nkey=1\r\n[]\r\nkey=2");

            var result = helper.GetEntry("safe", "key", "none");

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void GetEntry_CommentLinesAreNotEntries_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\n; note=1\r\n# other=2\r\nkey=3");

            Assert.AreEqual("none", helper.GetEntry("s", "note", "none"));
            Assert.AreEqual("none", helper.GetEntry("s", "other", "none"));
            Assert.AreEqual("3", helper.GetEntry("s", "key", "none"));
        }

        [TestMethod]
        public void GetEntry_ValueIsTrimmedAndKeepsEqualsSign_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nkey = a=b \r\n");

            var result = helper.GetEntry("s", "key", "none");

            Assert.AreEqual("a=b", result);
        }

        [TestMethod]
        public void GetEntry_SectionAndKeyMatchIgnoringCase_Test()
        {
            var helper = IniFileHelper.Instance.Load("[S]\r\nKEY=1");

            var result = helper.GetEntry("s", "key", "none");

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void GetEntry_SoftHyphenKeyIsDistinctKey_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nadmin\u00AD=1\r\nadmin=2");

            var result = helper.GetEntry("s", "admin", true);

            CollectionAssert.AreEqual(new[] { "2" }, result.Item2);
        }

        [TestMethod]
        [Timeout(5000)]
        public void GetEntry_LongLineWithoutEqualsParsesQuickly_Test()
        {
            var longLine = new string('a', 20000);
            var helper = IniFileHelper.Instance.Load("[s]\r\n" + longLine + "\r\nkey=1");

            var result = helper.GetEntry("s", "key", "none");

            Assert.AreEqual("1", result);
        }

        [TestMethod]
        public void GetEntry_DuplicatesIncludedInOrder_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nkey=1\r\nkey=2");

            var result = helper.GetEntry("s", "key", true);

            CollectionAssert.AreEqual(new[] { "1", "2" }, result.Item2);
        }

        [TestMethod]
        public void GetEntry_DuplicateKeyReturnsLastValue_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nkey=1\r\nkey=2");

            var result = helper.GetEntry("s", "key", "none");

            Assert.AreEqual("2", result);
        }

        [TestMethod]
        public void GetEntry_SingleCharacterDuplicatesIncluded_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nk=1\r\nk=2");

            var result = helper.GetEntry("s", "k", true);

            CollectionAssert.AreEqual(new[] { "1", "2" }, result.Item2);
        }

        [TestMethod]
        public void GetEntries_ShortAndHyphenatedKeysPresent_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\na=1\r\nb-c=2");

            var result = helper.GetEntries("s");

            Assert.IsTrue(result.ContainsKey("a"));
            Assert.IsTrue(result.ContainsKey("b-c"));
            Assert.AreEqual("1", result["a"][0]);
            Assert.AreEqual("2", result["b-c"][0]);
        }

        [TestMethod]
        public void GetDistinctEntryValue_ShortAndHyphenatedKeysPresent_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\na=1\r\nb-c=2");

            var result = helper.GetDistinctEntryValue("s");

            Assert.IsTrue(result.ContainsKey("a"));
            Assert.IsTrue(result.ContainsKey("b-c"));
            Assert.AreEqual("1", result["a"]);
            Assert.AreEqual("2", result["b-c"]);
        }

        [TestMethod]
        public void GetEntry_LeadingByteOrderMark_Test()
        {
            var helper = IniFileHelper.Instance.Load("\uFEFF[main]\r\nkey=v");

            Assert.AreEqual("v", helper.GetEntry("main", "key", "none"));
        }

        [TestMethod]
        public void GetEntry_LineFeedOnlyInput_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\nkey=1\nother=2\n");

            Assert.AreEqual("1", helper.GetEntry("s", "key", "none"));
            Assert.AreEqual("2", helper.GetEntry("s", "other", "none"));
        }

        [TestMethod]
        public void GetEntry_InlineCommentKeptInValue_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\nkey=v ; note");

            Assert.AreEqual("v ; note", helper.GetEntry("s", "key", "none"));
        }

        [TestMethod]
        public void GetEntry_NonBreakingSpaceIndent_Test()
        {
            var helper = IniFileHelper.Instance.Load("[s]\r\n\u00A0key=1");

            Assert.AreEqual("1", helper.GetEntry("s", "key", "none"));
        }
    }
}
