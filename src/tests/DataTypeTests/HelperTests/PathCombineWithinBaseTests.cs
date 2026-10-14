using System;
using System.IO;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO;

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class PathCombineWithinBaseTests
    {
        private string _baseName;
        private string _basePath;
        private string _fullBase;

        [TestInitialize]
        public void Setup()
        {
            _baseName = "PcwbBase" + Guid.NewGuid().ToString("N");
            _basePath = Path.Combine(Path.GetTempPath(), _baseName);
            _fullBase = Path.GetFullPath(_basePath);
        }

        [DataRow("a", "a")]
        [DataRow("a/b.txt", "a/b.txt")]
        [DataRow("./a", "a")]
        [DataRow("a/./b", "a/b")]
        [DataRow("a/../b", "b")]
        [DataRow("a/b/../../c", "c")]
        [DataRow("a/b/..", "a")]
        [DataRow("./a/../b/./c.txt", "b/c.txt")]
        [TestMethod]
        public void CombineWithinBase_InsidePaths_Test(string relativePath, string expectedRelative)
        {
            var expected = Path.GetFullPath(Path.Combine(_basePath, expectedRelative));

            var result = _basePath.CombineWithinBase(relativePath);

            Assert.AreEqual(expected, result);
        }

        [DataRow("")]
        [DataRow(".")]
        [DataRow("a/..")]
        [DataRow("a/b/../..")]
        [TestMethod]
        public void CombineWithinBase_ResolvesToBase_Test(string relativePath)
        {
            var result = _basePath.CombineWithinBase(relativePath);

            Assert.AreEqual(_fullBase, result);
        }

        [TestMethod]
        public void CombineWithinBase_ParentThenBackIntoBase_Test()
        {
            var expected = Path.Combine(_fullBase, "x");

            var result = _basePath.CombineWithinBase("../" + _baseName + "/x");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CombineWithinBase_RelativeBasePath_Test()
        {
            var relativeBase = "PcwbRel" + Guid.NewGuid().ToString("N");
            var expected = Path.GetFullPath(Path.Combine(relativeBase, "a"));

            var result = relativeBase.CombineWithinBase("a");

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CombineWithinBase_BaseWithTrailingSeparator_Test()
        {
            var baseWithSeparator = _basePath + Path.DirectorySeparatorChar;

            var result = baseWithSeparator.CombineWithinBase("a");

            Assert.AreEqual(Path.Combine(_fullBase, "a"), result);
        }

        [TestMethod]
        public void CombineWithinBase_BaseWithTrailingSeparatorEmptyRelative_Test()
        {
            var baseWithSeparator = _basePath + Path.DirectorySeparatorChar;

            var result = baseWithSeparator.CombineWithinBase(string.Empty);

            Assert.AreEqual(Path.GetFullPath(baseWithSeparator), result);
        }

        [TestMethod]
        public void CombineWithinBase_RelativeWithTrailingSeparator_Test()
        {
            var relativePath = "a" + Path.DirectorySeparatorChar;

            var result = _basePath.CombineWithinBase(relativePath);

            Assert.AreEqual(Path.GetFullPath(Path.Combine(_basePath, relativePath)), result);
        }

        [DataRow("..")]
        [DataRow("../")]
        [DataRow("../x")]
        [DataRow("./../x")]
        [DataRow("a/../../x")]
        [DataRow("a/./../../x")]
        [DataRow("a/b/../../..")]
        [DataRow("a/b/../../../x")]
        [DataRow("../../x")]
        [TestMethod]
        [TestCategory("Security")]
        public void CombineWithinBase_EscapingPaths_Test(string relativePath)
        {
            var exception = Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase(relativePath));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [TestMethod]
        [TestCategory("Security")]
        public void CombineWithinBase_EscapeWithTrailingSeparatorBase_Test()
        {
            var baseWithSeparator = _basePath + Path.DirectorySeparatorChar;

            var exception = Assert.ThrowsException<ArgumentException>(() => baseWithSeparator.CombineWithinBase(".."));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [DataRow("2")]
        [DataRow("2/x")]
        [DataRow("x")]
        [DataRow("-sibling/x")]
        [TestMethod]
        [TestCategory("Security")]
        public void CombineWithinBase_PrefixSiblingDirectory_Test(string siblingSuffix)
        {
            var relativePath = "../" + _baseName + siblingSuffix;

            var exception = Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase(relativePath));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [TestMethod]
        [TestCategory("Security")]
        public void CombineWithinBase_PrefixSiblingWithTrailingSeparatorBase_Test()
        {
            var baseWithSeparator = _basePath + Path.DirectorySeparatorChar;

            var exception = Assert.ThrowsException<ArgumentException>(
                () => baseWithSeparator.CombineWithinBase("../" + _baseName + "2/x"));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [TestMethod]
        [TestCategory("Security")]
        public void CombineWithinBase_AbsolutePathInsideBase_Test()
        {
            var absoluteInside = Path.Combine(_fullBase, "a");

            var exception = Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase(absoluteInside));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [DataRow("/")]
        [DataRow("/a")]
        [TestMethod]
        [TestCategory("Security")]
        public void CombineWithinBase_SlashRootedPath_Test(string relativePath)
        {
            var exception = Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase(relativePath));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [DataRow(@"\")]
        [DataRow(@"\a")]
        [DataRow(@"C:")]
        [DataRow(@"C:a")]
        [DataRow(@"C:\a")]
        [DataRow(@"c:/a")]
        [DataRow(@"\\server\share\a")]
        [DataRow(@"//server/share/a")]
        [DataRow(@"\\?\C:\a")]
        [DataRow(@"\\.\C:\a")]
        [TestMethod]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void CombineWithinBase_WindowsRootedShapes_Test(string relativePath)
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows rooted path shapes.");

            var exception = Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase(relativePath));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [TestMethod]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void CombineWithinBase_WindowsRootRelativeInsideBase_Test()
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows rooted path shapes.");
            Assert.AreEqual(':', _fullBase[1], "The temp path is expected to start with a drive letter.");
            var rootRelativeInside = _fullBase.Substring(2) + @"\a";

            var exception = Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase(rootRelativeInside));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [TestMethod]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void CombineWithinBase_WindowsDriveRelativeInsideBase_Test()
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows rooted path shapes.");
            Assert.AreEqual(':', _fullBase[1], "The temp path is expected to start with a drive letter.");
            var driveRelativeInside = _fullBase.Substring(0, 2) + _fullBase.Substring(3) + @"\a";

            var exception = Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase(driveRelativeInside));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [DataRow(@"..\x")]
        [DataRow(@"a\..\..\x")]
        [DataRow(@"a\b\..\..\..")]
        [DataRow(@"..\")]
        [TestMethod]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void CombineWithinBase_WindowsBackslashEscape_Test(string relativePath)
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows backslash separators.");

            var exception = Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase(relativePath));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [TestMethod]
        [TestCategory("WindowsOnly")]
        public void CombineWithinBase_WindowsCaseInsensitiveBase_Test()
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows case-insensitive path comparison.");
            var relativePath = "../" + _baseName.ToUpperInvariant() + "/a";

            var result = _basePath.CombineWithinBase(relativePath);

            Assert.IsTrue(string.Equals(Path.Combine(_fullBase, "a"), result, StringComparison.OrdinalIgnoreCase), result);
        }

        [TestMethod]
        [TestCategory("LinuxOnly")]
        public void CombineWithinBase_LinuxCaseSensitiveBase_Test()
        {
            if (SandboxTestHelper.IsWindows)
                return;
            var relativePath = "../" + _baseName.ToUpperInvariant() + "/a";

            var exception = Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase(relativePath));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [TestMethod]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void CombineWithinBase_DevicePathBaseEscapeRejected_Test()
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows device path shapes.");
            var deviceBase = @"\\?\" + _fullBase;

            var exception = Assert.ThrowsException<ArgumentException>(() => deviceBase.CombineWithinBase(@"..\..\x"));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [TestMethod]
        public void CombineWithinBase_NullBasePath_Test()
        {
            string basePath = null;

            var exception = Assert.ThrowsException<ArgumentNullException>(() => basePath.CombineWithinBase("a"));

            Assert.AreEqual("basePath", exception.ParamName);
        }

        [TestMethod]
        public void CombineWithinBase_NullRelativePath_Test()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() => _basePath.CombineWithinBase(null));

            Assert.AreEqual("relativePath", exception.ParamName);
        }

        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [DataRow(" \r\n ")]
        [TestMethod]
        public void CombineWithinBase_BlankBasePath_Test(string basePath)
        {
            var exception = Assert.ThrowsException<ArgumentException>(() => basePath.CombineWithinBase("a"));

            Assert.AreEqual("basePath", exception.ParamName);
        }

        [TestMethod]
        public void CombineWithinBase_InvalidCharInRelativePath_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => _basePath.CombineWithinBase("a\0b"));
        }

        [TestMethod]
        public void CombineWithinBase_InvalidCharInBasePath_Test()
        {
            var basePath = _basePath + "\0x";

            Assert.ThrowsException<ArgumentException>(() => basePath.CombineWithinBase("a"));
        }

        [DataRow("a", "a")]
        [DataRow("./a/../b/./c.txt", "b/c.txt")]
        [DataRow("a/b/../../c", "c")]
        [TestMethod]
        public void TryCombineWithinBase_InsidePaths_Test(string relativePath, string expectedRelative)
        {
            var expected = Path.GetFullPath(Path.Combine(_basePath, expectedRelative));

            var success = _basePath.TryCombineWithinBase(relativePath, out var combinedPath);

            Assert.IsTrue(success);
            Assert.AreEqual(expected, combinedPath);
        }

        [DataRow("")]
        [DataRow(".")]
        [DataRow("a/..")]
        [TestMethod]
        public void TryCombineWithinBase_ResolvesToBase_Test(string relativePath)
        {
            var success = _basePath.TryCombineWithinBase(relativePath, out var combinedPath);

            Assert.IsTrue(success);
            Assert.AreEqual(_fullBase, combinedPath);
        }

        [DataRow("..")]
        [DataRow("../x")]
        [DataRow("a/../../x")]
        [DataRow("a/b/../../..")]
        [TestMethod]
        [TestCategory("Security")]
        public void TryCombineWithinBase_EscapingPaths_Test(string relativePath)
        {
            var success = _basePath.TryCombineWithinBase(relativePath, out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [DataRow("2")]
        [DataRow("2/x")]
        [TestMethod]
        [TestCategory("Security")]
        public void TryCombineWithinBase_PrefixSiblingDirectory_Test(string siblingSuffix)
        {
            var success = _basePath.TryCombineWithinBase("../" + _baseName + siblingSuffix, out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [TestMethod]
        [TestCategory("Security")]
        public void TryCombineWithinBase_AbsolutePathInsideBase_Test()
        {
            var success = _basePath.TryCombineWithinBase(Path.Combine(_fullBase, "a"), out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [DataRow(@"\a")]
        [DataRow(@"C:a")]
        [DataRow(@"C:\a")]
        [DataRow(@"\\server\share\a")]
        [DataRow(@"\\?\C:\a")]
        [DataRow(@"..\x")]
        [TestMethod]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void TryCombineWithinBase_WindowsRootedOrEscaping_Test(string relativePath)
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows rooted path shapes.");

            var success = _basePath.TryCombineWithinBase(relativePath, out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [TestMethod]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void TryCombineWithinBase_DevicePathBaseEscapeRejected_Test()
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows device path shapes.");
            var deviceBase = @"\\?\" + _fullBase;

            var success = deviceBase.TryCombineWithinBase(@"..\..\x", out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [TestMethod]
        public void TryCombineWithinBase_NullBasePath_Test()
        {
            string basePath = null;

            var success = basePath.TryCombineWithinBase("a", out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [TestMethod]
        public void TryCombineWithinBase_NullRelativePath_Test()
        {
            var success = _basePath.TryCombineWithinBase(null, out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [DataRow("")]
        [DataRow(" ")]
        [DataRow("\t")]
        [TestMethod]
        public void TryCombineWithinBase_BlankBasePath_Test(string basePath)
        {
            var success = basePath.TryCombineWithinBase("a", out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [TestMethod]
        public void TryCombineWithinBase_InvalidCharInRelativePath_Test()
        {
            var success = _basePath.TryCombineWithinBase("a\0b", out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [TestMethod]
        public void TryCombineWithinBase_InvalidCharInBasePath_Test()
        {
            var success = (_basePath + "\0x").TryCombineWithinBase("a", out var combinedPath);

            Assert.IsFalse(success);
            Assert.IsNull(combinedPath);
        }

        [DataRow(".. ")]
        [DataRow("... ")]
        [DataRow("..  /x")]
        [DataRow(". ./..")]
        [DataRow("a/.. /.. ")]
        [DataRow("a/.../..")]
        [DataRow("..//x")]
        [DataRow("a//..//..")]
        [TestMethod]
        [TestCategory("Security")]
        public void TryCombineWithinBase_UnusualSegmentsNeverEscape_Test(string relativePath)
        {
            var comparison = SandboxTestHelper.IsWindows ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            var success = _basePath.TryCombineWithinBase(relativePath, out var combinedPath);

            if (success)
                Assert.IsTrue(
                    string.Equals(_fullBase, combinedPath, comparison)
                    || combinedPath.StartsWith(_fullBase + Path.DirectorySeparatorChar, comparison),
                    combinedPath);
            else
                Assert.IsNull(combinedPath);
        }
    }
}
