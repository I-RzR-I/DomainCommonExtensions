using System;
using System.Collections.Generic;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Validation;

namespace DataTypeTests.UtilsTests.Ensure
{
    [TestClass]
    public class DomainEnsureGuardExtensionsTests
    {
        [TestMethod]
        public void ThrowIfNullOrWhiteSpace_NullThrowsArgumentNull_Test()
        {
            string text = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => text.ThrowIfNullOrWhiteSpace());

            Assert.AreEqual("text", exception.ParamName);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        [DataRow("\t")]
        [DataRow("\r\n")]
        [DataRow("\u00A0")]
        public void ThrowIfNullOrWhiteSpace_EmptyOrWhiteSpaceThrows_Test(string text)
        {
            var exception = Assert.ThrowsException<ArgumentException>(() => text.ThrowIfNullOrWhiteSpace());

            Assert.AreEqual("text", exception.ParamName);
        }

        [TestMethod]
        [DataRow("a")]
        [DataRow("0")]
        [DataRow(" a ")]
        [DataRow("\ta\t")]
        public void ThrowIfNullOrWhiteSpace_ValuePasses_Test(string text)
        {
            text.ThrowIfNullOrWhiteSpace();
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpace_ExplicitParamName_Test()
        {
            var text = " ";

            var exception = Assert.ThrowsException<ArgumentException>(() => text.ThrowIfNullOrWhiteSpace("custom"));

            Assert.AreEqual("custom", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpace_NullParamName_Test()
        {
            string text = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => text.ThrowIfNullOrWhiteSpace(paramName: null));

            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_NullThrowsArgumentNull_Test()
        {
            List<int> items = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => items.ThrowIfNullOrEmptyCollection());

            Assert.AreEqual("items", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_EmptyListThrows_Test()
        {
            var items = new List<int>();

            var exception = Assert.ThrowsException<ArgumentException>(() => items.ThrowIfNullOrEmptyCollection());

            Assert.AreEqual("items", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_EmptyArrayThrows_Test()
        {
            var items = new string[0];

            var exception = Assert.ThrowsException<ArgumentException>(() => items.ThrowIfNullOrEmptyCollection());

            Assert.AreEqual("items", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_EmptyIteratorThrows_Test()
        {
            var helper = new PullCountingSequenceTestHelper();
            var items = helper.Sequence(0);

            var exception = Assert.ThrowsException<ArgumentException>(() => items.ThrowIfNullOrEmptyCollection());

            Assert.AreEqual("items", exception.ParamName);
            Assert.AreEqual(1, helper.EnumerationCount);
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_EmptyRangeThrows_Test()
        {
            var items = Enumerable.Range(1, 0);

            var exception = Assert.ThrowsException<ArgumentException>(() => items.ThrowIfNullOrEmptyCollection());

            Assert.AreEqual("items", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_NonEmptyListPasses_Test()
        {
            var items = new List<int> { 1 };

            items.ThrowIfNullOrEmptyCollection();

            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_NonEmptyArrayPasses_Test()
        {
            var items = new[] { "a", "b" };

            items.ThrowIfNullOrEmptyCollection();
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_NullElementPasses_Test()
        {
            var items = new List<string> { null };

            items.ThrowIfNullOrEmptyCollection();
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_IteratorReadsAtMostOneElement_Test()
        {
            var helper = new PullCountingSequenceTestHelper();
            var items = helper.Sequence(5);

            items.ThrowIfNullOrEmptyCollection();

            Assert.AreEqual(1, helper.EnumerationCount);
            Assert.AreEqual(1, helper.PulledCount);
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_EmptyStringThrows_Test()
        {
            var text = string.Empty;

            Assert.ThrowsException<ArgumentException>(() => text.ThrowIfNullOrEmptyCollection());
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_ExplicitParamName_Test()
        {
            var items = new List<int>();

            var exception = Assert.ThrowsException<ArgumentException>(() => items.ThrowIfNullOrEmptyCollection("custom"));

            Assert.AreEqual("custom", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfNullOrEmptyCollection_NullParamName_Test()
        {
            List<int> items = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => items.ThrowIfNullOrEmptyCollection(paramName: null));

            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfEmpty_EmptyGuidThrows_Test()
        {
            var id = Guid.Empty;

            var exception = Assert.ThrowsException<ArgumentException>(() => id.ThrowIfEmpty());

            Assert.AreEqual("id", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfEmpty_DefaultGuidThrows_Test()
        {
            var id = new Guid();

            var exception = Assert.ThrowsException<ArgumentException>(() => id.ThrowIfEmpty());

            Assert.AreEqual("id", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfEmpty_FixedGuidPasses_Test()
        {
            var id = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301");

            id.ThrowIfEmpty();
        }

        [TestMethod]
        public void ThrowIfEmpty_NewGuidPasses_Test()
        {
            var id = Guid.NewGuid();

            id.ThrowIfEmpty();
        }

        [TestMethod]
        public void ThrowIfEmpty_GuidExplicitParamName_Test()
        {
            var id = Guid.Empty;

            var exception = Assert.ThrowsException<ArgumentException>(() => id.ThrowIfEmpty("custom"));

            Assert.AreEqual("custom", exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfEmpty_GuidNullParamName_Test()
        {
            var id = Guid.Empty;

            var exception = Assert.ThrowsException<ArgumentException>(() => id.ThrowIfEmpty(paramName: null));

            Assert.IsNull(exception.ParamName);
        }

        [TestMethod]
        public void ThrowIfEmpty_StringEmptyKeepsMessage_Test()
        {
            var text = "";

            var exception = Assert.ThrowsException<Exception>(() => text.ThrowIfEmpty("Text is empty"));

            Assert.AreEqual("Text is empty", exception.Message);
        }

        [TestMethod]
        public void ThrowIfEmpty_StringNullKeepsMessage_Test()
        {
            string text = null;

            var exception = Assert.ThrowsException<Exception>(() => text.ThrowIfEmpty("Text is empty"));

            Assert.AreEqual("Text is empty", exception.Message);
        }

        [TestMethod]
        public void ThrowIfEmpty_StringWhiteSpacePasses_Test()
        {
            var text = " ";

            text.ThrowIfEmpty("Text is empty");
        }

        [TestMethod]
        public void ThrowIfNullOrEmpty_StringNullKeepsMessage_Test()
        {
            string text = null;

            var exception = Assert.ThrowsException<Exception>(() => text.ThrowIfNullOrEmpty("Text is required"));

            Assert.AreEqual("Text is required", exception.Message);
        }

        [TestMethod]
        public void ThrowIfNullOrEmpty_StringEmptyKeepsMessage_Test()
        {
            var text = "";

            var exception = Assert.ThrowsException<Exception>(() => text.ThrowIfNullOrEmpty("Text is required"));

            Assert.AreEqual("Text is required", exception.Message);
        }

        [TestMethod]
        public void ThrowIfNullOrEmpty_StringWhiteSpacePasses_Test()
        {
            var text = "  ";

            text.ThrowIfNullOrEmpty("Text is required");
        }
    }
}
