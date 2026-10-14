using System;
using System.IO;
using System.Linq;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO;

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class DirectoryHelperTests
    {
        private string _sandbox;

        [TestInitialize]
        public void Setup()
        {
            _sandbox = SandboxTestHelper.CreateSandbox();
        }

        [TestCleanup]
        public void Cleanup()
        {
            SandboxTestHelper.DeleteSandbox(_sandbox);
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        [TestCategory("Security")]
        public void DeleteDirectory_RootIsDirectoryLink_Test(bool useDirectoryInfo)
        {
            var target = SandboxTestHelper.CreateDirectory(_sandbox, "target");
            var sentinel = SandboxTestHelper.CreateSentinel(_sandbox, "target");
            var link = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "link"));
            Assert.IsTrue(SandboxTestHelper.TryCreateDirectoryLink(_sandbox, link, target, out var output), output);

            if (useDirectoryInfo)
                DirectoryHelper.DeleteDirectory(new DirectoryInfo(link));
            else
                DirectoryHelper.DeleteDirectory(link);

            Assert.IsTrue(File.Exists(sentinel), "The link target content was deleted through the root link.");
            Assert.AreEqual(SandboxTestHelper.SentinelContent, File.ReadAllText(sentinel));
            Assert.IsTrue(Directory.Exists(target));
            Assert.IsFalse(SandboxTestHelper.EntryExists(link), "The root link entry was not removed.");
        }

        [TestMethod]
        [TestCategory("Security")]
        public void DeleteDirectory_StaleDirectoryInfoReplacedByLink_SentinelSurvives_Test()
        {
            var outside = SandboxTestHelper.CreateDirectory(_sandbox, "outside");
            var sentinel = SandboxTestHelper.CreateSentinel(_sandbox, "outside");
            var root = SandboxTestHelper.CreateDirectory(_sandbox, "root");
            SandboxTestHelper.CreateFile(_sandbox, "a", "root", "a.txt");
            var staleInfo = new DirectoryInfo(root);
            Assert.IsTrue(staleInfo.Exists);
            Assert.AreEqual((FileAttributes)0, staleInfo.Attributes & FileAttributes.ReparsePoint);
            Directory.Delete(root, true);
            Assert.IsTrue(SandboxTestHelper.TryCreateDirectoryLink(_sandbox, root, outside, out var output), output);

            DirectoryHelper.DeleteDirectory(staleInfo);

            Assert.IsTrue(File.Exists(sentinel), "The link target content was deleted through a stale DirectoryInfo whose path became a link.");
            Assert.AreEqual(SandboxTestHelper.SentinelContent, File.ReadAllText(sentinel));
            Assert.IsTrue(Directory.Exists(outside));
            Assert.IsFalse(SandboxTestHelper.EntryExists(root), "The link entry that replaced the root was not removed.");
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        [TestCategory("Security")]
        public void DeleteDirectory_DirectoryLinkInsideTree_SentinelSurvives_Test(bool useDirectoryInfo)
        {
            var outside = SandboxTestHelper.CreateDirectory(_sandbox, "outside");
            var sentinel = SandboxTestHelper.CreateSentinel(_sandbox, "outside");
            var tree = SandboxTestHelper.CreateDirectory(_sandbox, "tree");
            SandboxTestHelper.CreateFile(_sandbox, "a", "tree", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "tree", "sub", "b.txt");
            var link = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(tree, "sub", "link"));
            Assert.IsTrue(SandboxTestHelper.TryCreateDirectoryLink(_sandbox, link, outside, out var output), output);

            if (useDirectoryInfo)
                DirectoryHelper.DeleteDirectory(new DirectoryInfo(tree));
            else
                DirectoryHelper.DeleteDirectory(tree);

            Assert.IsTrue(File.Exists(sentinel), "The sentinel outside the tree was deleted through a link inside the tree.");
            Assert.AreEqual(SandboxTestHelper.SentinelContent, File.ReadAllText(sentinel));
            Assert.IsTrue(Directory.Exists(outside));
            Assert.IsFalse(SandboxTestHelper.EntryExists(tree));
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        [Timeout(10000)]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void DeleteDirectory_TrailingDotJunctionBesideRealDir_SentinelSurvives_Test(bool useDirectoryInfo)
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows trailing-dot name normalization.");
            var outside = SandboxTestHelper.CreateDirectory(_sandbox, "outside");
            var sentinel = SandboxTestHelper.CreateSentinel(_sandbox, "outside");
            var tree = SandboxTestHelper.CreateDirectory(_sandbox, "tree");
            SandboxTestHelper.CreateFile(_sandbox, "real", "tree", "foo", "real.txt");
            var junction = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(tree, "foo."));
            Assert.IsTrue(SandboxTestHelper.TryCreateExtendedDirectoryJunction(_sandbox, junction, outside, out var output),
                "A trailing-dot junction could not be created on this platform: " + output);

            try
            {
                if (useDirectoryInfo)
                    DirectoryHelper.DeleteDirectory(new DirectoryInfo(tree));
                else
                    DirectoryHelper.DeleteDirectory(tree);

                Assert.IsTrue(File.Exists(sentinel), "The link target content was deleted through a trailing-dot junction classified as a real directory.");
                Assert.AreEqual(SandboxTestHelper.SentinelContent, File.ReadAllText(sentinel));
                Assert.IsTrue(Directory.Exists(outside));
                Assert.IsFalse(SandboxTestHelper.EntryExists(tree));
            }
            finally
            {
                SandboxTestHelper.TryRemoveExtendedDirectoryLink(junction);
            }
        }

        [TestMethod]
        [Timeout(10000)]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void CopyDirectory_TrailingDotJunctionInSource_NotCopied_Test()
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows trailing-dot name normalization.");
            var outside = SandboxTestHelper.CreateDirectory(_sandbox, "outside");
            SandboxTestHelper.CreateSentinel(_sandbox, "outside");
            var outsideBefore = SandboxTestHelper.Snapshot(outside);
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "real", "source", "foo", "real.txt");
            var junction = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(source, "foo."));
            Assert.IsTrue(SandboxTestHelper.TryCreateExtendedDirectoryJunction(_sandbox, junction, outside, out var output),
                "A trailing-dot junction could not be created on this platform: " + output);
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "target"));

            try
            {
                DirectoryHelper.CopyDirectory(source, target);

                var sep = Path.DirectorySeparatorChar;
                var copied = SandboxTestHelper.Snapshot(target);
                Assert.IsFalse(copied.Any(x => x.Split(sep)[0] == "foo."),
                    "The trailing-dot junction was copied as a directory instead of being skipped as a link.");
                Assert.IsFalse(SandboxTestHelper.AnyFileNamed(target, SandboxTestHelper.SentinelFileName),
                    "The trailing-dot junction target content was copied.");
                CollectionAssert.AreEqual(new[] { "foo" + sep, "foo" + sep + "real.txt=real" }, copied);
                CollectionAssert.AreEqual(outsideBefore, SandboxTestHelper.Snapshot(outside));
            }
            finally
            {
                SandboxTestHelper.TryRemoveExtendedDirectoryLink(junction);
            }
        }

        [TestMethod]
        [Timeout(10000)]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void CopyDirectory_RealTrailingDotDirBesideJunction_JunctionTargetNotCopied_Test()
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows trailing-dot name normalization.");
            var outside = SandboxTestHelper.CreateDirectory(_sandbox, "outside");
            SandboxTestHelper.CreateSentinel(_sandbox, "outside");
            var outsideBefore = SandboxTestHelper.SnapshotExtended(outside);
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateExtendedDirectory(_sandbox, "source", "foo.");
            var junction = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(source, "foo"));
            Assert.IsTrue(SandboxTestHelper.TryCreateExtendedDirectoryJunction(_sandbox, junction, outside, out var output), output);
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "target"));

            try
            {
                DirectoryHelper.CopyDirectory(source, target);

                Assert.IsFalse(SandboxTestHelper.AnyFileNamed(target, SandboxTestHelper.SentinelFileName),
                    "The junction target sentinel was copied through the real trailing-dot directory.");
                Assert.IsFalse(SandboxTestHelper.SnapshotExtended(target).Any(x => x.Contains("sentinel")),
                    "The junction target content reached the copy target.");
                CollectionAssert.AreEqual(outsideBefore, SandboxTestHelper.SnapshotExtended(outside));
            }
            finally
            {
                SandboxTestHelper.TryRemoveExtendedDirectoryLink(junction);
                SandboxTestHelper.TryRemoveExtendedDirectoryTree(Path.Combine(source, "foo."));
                SandboxTestHelper.TryRemoveExtendedDirectoryTree(target);
            }
        }

        [TestMethod]
        [Timeout(10000)]
        [TestCategory("Security")]
        [TestCategory("WindowsOnly")]
        public void DeleteDirectory_RealTrailingDotDirBesideJunction_SentinelSurvives_Test()
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs Windows trailing-dot name normalization.");
            var outside = SandboxTestHelper.CreateDirectory(_sandbox, "outside");
            var sentinel = SandboxTestHelper.CreateSentinel(_sandbox, "outside");
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateExtendedFile(_sandbox, "own", "source", "foo.", "own.txt");
            var junction = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(source, "foo"));
            Assert.IsTrue(SandboxTestHelper.TryCreateExtendedDirectoryJunction(_sandbox, junction, outside, out var output), output);

            try
            {
                try
                {
                    DirectoryHelper.DeleteDirectory(source);
                }
                catch (IOException)
                {
                }

                Assert.IsTrue(File.Exists(sentinel), "The junction target sentinel was deleted through the real trailing-dot directory.");
                Assert.AreEqual(SandboxTestHelper.SentinelContent, File.ReadAllText(sentinel));
                Assert.IsTrue(Directory.Exists(outside));
            }
            finally
            {
                SandboxTestHelper.TryRemoveExtendedDirectoryLink(junction);
                SandboxTestHelper.TryRemoveExtendedDirectoryTree(Path.Combine(source, "foo."));
            }
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        [TestCategory("Security")]
        public void CopyDirectory_DirectoryLinkInSource_NotCopied_Test(bool useDirectoryInfo)
        {
            var outside = SandboxTestHelper.CreateDirectory(_sandbox, "outside");
            SandboxTestHelper.CreateFile(_sandbox, "secret", "outside", "secret.txt");
            SandboxTestHelper.CreateFile(_sandbox, "deep", "outside", "nested", "deep.txt");
            var outsideBefore = SandboxTestHelper.Snapshot(outside);
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "source", "sub", "b.txt");
            var link = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(source, "sub", "link"));
            Assert.IsTrue(SandboxTestHelper.TryCreateDirectoryLink(_sandbox, link, outside, out var output), output);
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "target"));

            if (useDirectoryInfo)
                DirectoryHelper.CopyDirectory(new DirectoryInfo(source), new DirectoryInfo(target));
            else
                DirectoryHelper.CopyDirectory(source, target);

            var sep = Path.DirectorySeparatorChar;
            var expected = new[] { "a.txt=a", "sub" + sep, "sub" + sep + "b.txt=b" }.OrderBy(x => x, StringComparer.Ordinal).ToArray();
            Assert.IsFalse(SandboxTestHelper.EntryExists(Path.Combine(target, "sub", "link")), "The directory link in the source was copied.");
            CollectionAssert.AreEqual(expected, SandboxTestHelper.Snapshot(target));
            CollectionAssert.AreEqual(outsideBefore, SandboxTestHelper.Snapshot(outside));
        }

        [TestMethod]
        [TestCategory("Security")]
        [TestCategory("LinuxOnly")]
        public void CopyDirectory_FileLinkInSource_NotCopied_Test()
        {
            var secret = SandboxTestHelper.CreateFile(_sandbox, "secret", "outside", "secret.txt");
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            var link = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(source, "secret-link.txt"));
            var linkCreated = SandboxTestHelper.TryCreateFileLink(_sandbox, link, secret, out var output);
            if (!linkCreated && SandboxTestHelper.IsWindows)
                return;
            Assert.IsTrue(linkCreated, "A file symbolic link could not be created on this platform: " + output);
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "target"));

            DirectoryHelper.CopyDirectory(source, target);

            Assert.IsFalse(SandboxTestHelper.EntryExists(Path.Combine(target, "secret-link.txt")), "The file link in the source was copied.");
            CollectionAssert.AreEqual(new[] { "a.txt=a" }, SandboxTestHelper.Snapshot(target));
            Assert.AreEqual("secret", File.ReadAllText(secret));
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        [Timeout(5000)]
        [TestCategory("Security")]
        [TestCategory("RedByConstruction")]
        public void CopyDirectory_TargetInsideSource_ThrowsBeforeAnyWrite_Test(bool useDirectoryInfo)
        {
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            SandboxTestHelper.CreateDirectory(_sandbox, "source", "sub");
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(source, "sub", "copy"));
            var before = SandboxTestHelper.Snapshot(source);

            if (useDirectoryInfo)
                Assert.ThrowsException<ArgumentException>(() => DirectoryHelper.CopyDirectory(new DirectoryInfo(source), new DirectoryInfo(target)));
            else
                Assert.ThrowsException<ArgumentException>(() => DirectoryHelper.CopyDirectory(source, target));

            Assert.IsFalse(SandboxTestHelper.EntryExists(target));
            CollectionAssert.AreEqual(before, SandboxTestHelper.Snapshot(source));
        }

        [TestMethod]
        [DoNotParallelize]
        [Timeout(5000)]
        [TestCategory("Security")]
        [TestCategory("RedByConstruction")]
        public void CopyDirectory_RelativeTargetInsideSource_Throws_Test()
        {
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            SandboxTestHelper.CreateDirectory(_sandbox, "source", "sub");
            var before = SandboxTestHelper.Snapshot(source);
            var previousDirectory = Environment.CurrentDirectory;

            try
            {
                Environment.CurrentDirectory = _sandbox;
                var relativeSource = SandboxTestHelper.InSandbox(_sandbox, "source");
                var relativeTarget = SandboxTestHelper.InSandbox(_sandbox, Path.Combine("source", "sub", "copy"));

                Assert.ThrowsException<ArgumentException>(() => DirectoryHelper.CopyDirectory(relativeSource, relativeTarget));
            }
            finally
            {
                Environment.CurrentDirectory = previousDirectory;
            }

            Assert.IsFalse(SandboxTestHelper.EntryExists(Path.Combine(source, "sub", "copy")));
            CollectionAssert.AreEqual(before, SandboxTestHelper.Snapshot(source));
        }

        [DataRow(true, true)]
        [DataRow(false, true)]
        [DataRow(true, false)]
        [TestMethod]
        [Timeout(5000)]
        [TestCategory("Security")]
        [TestCategory("RedByConstruction")]
        public void CopyDirectory_TrailingSeparatorTargetInsideSource_Throws_Test(bool sourceTrailing, bool targetTrailing)
        {
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            var sub = SandboxTestHelper.CreateDirectory(_sandbox, "source", "sub");
            var before = SandboxTestHelper.Snapshot(source);
            var sep = Path.DirectorySeparatorChar.ToString();
            var sourcePath = SandboxTestHelper.InSandbox(_sandbox, sourceTrailing ? source + sep : source);
            var targetPath = SandboxTestHelper.InSandbox(_sandbox, targetTrailing ? Path.Combine(sub, "copy") + sep : Path.Combine(sub, "copy"));

            Assert.ThrowsException<ArgumentException>(() => DirectoryHelper.CopyDirectory(sourcePath, targetPath));

            Assert.IsFalse(SandboxTestHelper.EntryExists(Path.Combine(sub, "copy")));
            CollectionAssert.AreEqual(before, SandboxTestHelper.Snapshot(source));
        }

        [TestMethod]
        [Timeout(5000)]
        [TestCategory("Security")]
        [TestCategory("RedByConstruction")]
        public void CopyDirectory_AltSeparatorTargetInsideSource_Throws_Test()
        {
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            SandboxTestHelper.CreateDirectory(_sandbox, "source", "sub");
            var before = SandboxTestHelper.Snapshot(source);
            var alt = Path.AltDirectorySeparatorChar.ToString();
            var target = SandboxTestHelper.InSandbox(_sandbox, source + alt + "sub" + alt + "copy");

            Assert.ThrowsException<ArgumentException>(() => DirectoryHelper.CopyDirectory(source, target));

            Assert.IsFalse(SandboxTestHelper.EntryExists(Path.Combine(source, "sub", "copy")));
            CollectionAssert.AreEqual(before, SandboxTestHelper.Snapshot(source));
        }

        [TestMethod]
        [Timeout(5000)]
        [TestCategory("Security")]
        [TestCategory("RedByConstruction")]
        public void CopyDirectory_DotSegmentTargetInsideSource_Throws_Test()
        {
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            SandboxTestHelper.CreateDirectory(_sandbox, "source", "sub");
            var before = SandboxTestHelper.Snapshot(source);
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "source", "sub", "..", "sub", ".", "copy"));

            Assert.ThrowsException<ArgumentException>(() => DirectoryHelper.CopyDirectory(source, target));

            Assert.IsFalse(SandboxTestHelper.EntryExists(Path.Combine(source, "sub", "copy")));
            CollectionAssert.AreEqual(before, SandboxTestHelper.Snapshot(source));
        }

        [TestMethod]
        [Timeout(5000)]
        [TestCategory("Security")]
        [TestCategory("RedByConstruction")]
        [TestCategory("WindowsOnly")]
        public void CopyDirectory_CaseVariantTarget_ThrowsOnWindows_Test()
        {
            Assert.IsTrue(SandboxTestHelper.IsWindows, "This test needs case-insensitive Windows paths.");
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            SandboxTestHelper.CreateDirectory(_sandbox, "source", "sub");
            var before = SandboxTestHelper.Snapshot(source);
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "SOURCE", "sub", "copy"));

            Assert.ThrowsException<ArgumentException>(() => DirectoryHelper.CopyDirectory(source, target));

            Assert.IsFalse(SandboxTestHelper.EntryExists(Path.Combine(source, "sub", "copy")));
            CollectionAssert.AreEqual(before, SandboxTestHelper.Snapshot(source));
        }

        [TestMethod]
        [Timeout(5000)]
        [TestCategory("Security")]
        [TestCategory("LinuxOnly")]
        public void CopyDirectory_CaseVariantTarget_CopiesOnCaseSensitivePaths_Test()
        {
            if (SandboxTestHelper.IsWindows)
                return;
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            SandboxTestHelper.CreateDirectory(_sandbox, "source", "sub");
            var before = SandboxTestHelper.Snapshot(source);
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "SOURCE", "sub", "copy"));

            DirectoryHelper.CopyDirectory(source, target);

            CollectionAssert.AreEqual(before, SandboxTestHelper.Snapshot(target));
            CollectionAssert.AreEqual(before, SandboxTestHelper.Snapshot(source));
        }

        [DataRow(false, false)]
        [DataRow(true, false)]
        [DataRow(false, true)]
        [DataRow(true, true)]
        [TestMethod]
        [Timeout(10000)]
        [TestCategory("Security")]
        public void CopyDirectory_TargetEqualsSource_Throws_Test(bool targetTrailing, bool useDirectoryInfo)
        {
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "source", "sub", "b.txt");
            var before = SandboxTestHelper.Snapshot(source);
            var target = SandboxTestHelper.InSandbox(_sandbox, targetTrailing ? source + Path.DirectorySeparatorChar : source);

            if (useDirectoryInfo)
                Assert.ThrowsException<ArgumentException>(() => DirectoryHelper.CopyDirectory(new DirectoryInfo(source), new DirectoryInfo(target)));
            else
                Assert.ThrowsException<ArgumentException>(() => DirectoryHelper.CopyDirectory(source, target));

            CollectionAssert.AreEqual(before, SandboxTestHelper.Snapshot(source));
        }

        [DataRow("ab")]
        [DataRow("a-copy")]
        [DataRow("a.bak")]
        [TestMethod]
        [Timeout(10000)]
        [TestCategory("Security")]
        public void CopyDirectory_SiblingWithSharedPrefix_Allowed_Test(string targetName)
        {
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "a");
            SandboxTestHelper.CreateFile(_sandbox, "x", "a", "x.txt");
            SandboxTestHelper.CreateFile(_sandbox, "y", "a", "sub", "y.txt");
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, targetName));

            DirectoryHelper.CopyDirectory(source, target);

            CollectionAssert.AreEqual(SandboxTestHelper.Snapshot(source), SandboxTestHelper.Snapshot(target));
        }

        [TestMethod]
        [Timeout(10000)]
        [TestCategory("Security")]
        public void CopyDirectory_SourceInsideTarget_Allowed_Test()
        {
            var parent = SandboxTestHelper.CreateDirectory(_sandbox, "parent");
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "parent", "child");
            SandboxTestHelper.CreateFile(_sandbox, "x", "parent", "child", "x.txt");
            SandboxTestHelper.CreateFile(_sandbox, "y", "parent", "child", "deep", "y.txt");

            DirectoryHelper.CopyDirectory(source, parent);

            Assert.AreEqual("x", File.ReadAllText(Path.Combine(parent, "x.txt")));
            Assert.AreEqual("y", File.ReadAllText(Path.Combine(parent, "deep", "y.txt")));
            Assert.AreEqual("x", File.ReadAllText(Path.Combine(source, "x.txt")));
        }

        [TestMethod]
        [TestCategory("Security")]
        public void CopyDirectory_SourceRootIsDirectoryLink_IsFollowed_Test()
        {
            var real = SandboxTestHelper.CreateDirectory(_sandbox, "real");
            SandboxTestHelper.CreateFile(_sandbox, "x", "real", "x.txt");
            SandboxTestHelper.CreateFile(_sandbox, "y", "real", "sub", "y.txt");
            var link = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "link"));
            Assert.IsTrue(SandboxTestHelper.TryCreateDirectoryLink(_sandbox, link, real, out var output), output);
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "target"));

            DirectoryHelper.CopyDirectory(link, target);

            CollectionAssert.AreEqual(SandboxTestHelper.Snapshot(real), SandboxTestHelper.Snapshot(target));
            Assert.IsFalse(SandboxTestHelper.IsLink(target));
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public void DeleteDirectory_MissingRoot_NoOp_Test(bool useDirectoryInfo)
        {
            var missing = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "missing"));

            if (useDirectoryInfo)
                DirectoryHelper.DeleteDirectory(new DirectoryInfo(missing));
            else
                DirectoryHelper.DeleteDirectory(missing);

            Assert.IsFalse(SandboxTestHelper.EntryExists(missing));
            Assert.IsTrue(Directory.Exists(_sandbox));
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public void DeleteDirectory_NormalTree_Test(bool useDirectoryInfo)
        {
            var tree = SandboxTestHelper.CreateDirectory(_sandbox, "tree");
            SandboxTestHelper.CreateFile(_sandbox, "a", "tree", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "tree", "sub", "b.txt");
            SandboxTestHelper.CreateFile(_sandbox, "c", "tree", "sub", "deeper", "c.txt");
            SandboxTestHelper.CreateDirectory(_sandbox, "tree", "empty");
            var sibling = SandboxTestHelper.CreateSentinel(_sandbox, "sibling");

            if (useDirectoryInfo)
                DirectoryHelper.DeleteDirectory(new DirectoryInfo(tree));
            else
                DirectoryHelper.DeleteDirectory(tree);

            Assert.IsFalse(SandboxTestHelper.EntryExists(tree));
            Assert.IsTrue(File.Exists(sibling));
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public void CopyDirectory_NormalTree_Test(bool useDirectoryInfo)
        {
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "a", "source", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "b", "source", "sub", "b.txt");
            SandboxTestHelper.CreateFile(_sandbox, "c", "source", "sub", "deeper", "c.txt");
            SandboxTestHelper.CreateDirectory(_sandbox, "source", "empty");
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "target"));

            if (useDirectoryInfo)
                DirectoryHelper.CopyDirectory(new DirectoryInfo(source), new DirectoryInfo(target));
            else
                DirectoryHelper.CopyDirectory(source, target);

            CollectionAssert.AreEqual(SandboxTestHelper.Snapshot(source), SandboxTestHelper.Snapshot(target));
        }

        [TestMethod]
        public void CopyDirectory_ExistingTarget_OverwritesFiles_Test()
        {
            var source = SandboxTestHelper.CreateDirectory(_sandbox, "source");
            SandboxTestHelper.CreateFile(_sandbox, "new-a", "source", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "new-b", "source", "sub", "b.txt");
            var target = SandboxTestHelper.CreateDirectory(_sandbox, "target");
            SandboxTestHelper.CreateFile(_sandbox, "old-a", "target", "a.txt");
            SandboxTestHelper.CreateFile(_sandbox, "old-b", "target", "sub", "b.txt");
            SandboxTestHelper.CreateFile(_sandbox, "extra", "target", "extra.txt");

            DirectoryHelper.CopyDirectory(source, target);

            Assert.AreEqual("new-a", File.ReadAllText(Path.Combine(target, "a.txt")));
            Assert.AreEqual("new-b", File.ReadAllText(Path.Combine(target, "sub", "b.txt")));
            Assert.AreEqual("extra", File.ReadAllText(Path.Combine(target, "extra.txt")));
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public void CopyDirectory_MissingSource_ThrowsWithoutCreatingTarget_Test(bool useDirectoryInfo)
        {
            var source = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "missing"));
            var target = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "target"));

            if (useDirectoryInfo)
                Assert.ThrowsException<DirectoryNotFoundException>(() => DirectoryHelper.CopyDirectory(new DirectoryInfo(source), new DirectoryInfo(target)));
            else
                Assert.ThrowsException<DirectoryNotFoundException>(() => DirectoryHelper.CopyDirectory(source, target));

            Assert.IsFalse(SandboxTestHelper.EntryExists(source));
            Assert.IsFalse(SandboxTestHelper.EntryExists(target), "The target was created although the source does not exist.");
        }
    }
}
