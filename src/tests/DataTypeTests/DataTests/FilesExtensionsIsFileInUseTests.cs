using System.IO;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class FilesExtensionsIsFileInUseTests
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

        [TestMethod]
        public void IsFileInUse_MissingFile_Test()
        {
            var path = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "missing.txt"));

            var result = path.IsFileInUse();

            Assert.IsFalse(result);
            Assert.IsFalse(File.Exists(path), "The missing file was created by the check.");
        }

        [TestMethod]
        public void IsFileInUse_MissingDirectory_Test()
        {
            var directory = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "missing-dir"));
            var path = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(directory, "file.txt"));

            var result = path.IsFileInUse();

            Assert.IsFalse(result);
            Assert.IsFalse(Directory.Exists(directory));
            Assert.IsFalse(File.Exists(path));
            Assert.AreEqual(0, SandboxTestHelper.Snapshot(_sandbox).Length);
        }

        [TestMethod]
        public void IsFileInUse_ExistingClosedFile_Test()
        {
            var path = SandboxTestHelper.CreateFile(_sandbox, "content", "closed.txt");

            var result = path.IsFileInUse();

            Assert.IsFalse(result);
            Assert.AreEqual("content", File.ReadAllText(path));
        }

        [TestMethod]
        public void IsFileInUse_FileHeldOpenExclusively_Test()
        {
            var path = SandboxTestHelper.CreateFile(_sandbox, "content", "locked.txt");
            bool result;

            using (new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                result = path.IsFileInUse();
            }

            Assert.IsTrue(result);
            Assert.AreEqual("content", File.ReadAllText(path));
        }
    }
}
