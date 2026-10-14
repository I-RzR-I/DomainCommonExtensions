using System.IO;
using System.Text;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.IO.Ini;

namespace DataTypeTests.HelperTests
{
    [TestClass]
    public class IniFileHelperLoadFileTests
    {
        private const string IniContent = "[s]\r\nkey=1";

        private string _sandbox;

        [TestInitialize]
        public void Init()
        {
            _sandbox = SandboxTestHelper.CreateSandbox();
        }

        [TestCleanup]
        public void Cleanup()
        {
            SandboxTestHelper.DeleteSandbox(_sandbox);
        }

        [TestMethod]
        public void LoadFile_OneArgumentReadsFile_Test()
        {
            var path = SandboxTestHelper.CreateFile(_sandbox, IniContent, "settings.ini");

            var helper = IniFileHelper.Instance.LoadFile(path);

            Assert.AreEqual("1", helper.GetEntry("s", "key", "none"));
        }

        [TestMethod]
        public void LoadFile_NullEncodingReadsFile_Test()
        {
            var path = SandboxTestHelper.CreateFile(_sandbox, IniContent, "settings.ini");

            var helper = IniFileHelper.Instance.LoadFile(path, null);

            Assert.AreEqual("1", helper.GetEntry("s", "key", "none"));
        }

        [TestMethod]
        public void LoadFile_MatchesLoadWithEncoding_Test()
        {
            var path = SandboxTestHelper.CreateFile(_sandbox, "[a]\r\nx=10\r\n[b]\r\ny=20", "settings.ini");

            var viaLoadFile = IniFileHelper.Instance.LoadFile(path);
            var viaLoad = IniFileHelper.Instance.Load(path, null);

            Assert.AreEqual(viaLoad.GetEntry("a", "x", "none"), viaLoadFile.GetEntry("a", "x", "none"));
            Assert.AreEqual(viaLoad.GetEntry("b", "y", "none"), viaLoadFile.GetEntry("b", "y", "none"));
            Assert.AreEqual("20", viaLoadFile.GetEntry("b", "y", "none"));
        }

        [TestMethod]
        public void Load_OneArgumentPathParsedAsText_Test()
        {
            var path = SandboxTestHelper.CreateFile(_sandbox, IniContent, "settings.ini");

            var helper = IniFileHelper.Instance.Load(path);

            Assert.AreEqual("none", helper.GetEntry("s", "key", "none"));
        }

        [TestMethod]
        public void LoadFile_ReturnsNewInstance_Test()
        {
            var path = SandboxTestHelper.CreateFile(_sandbox, IniContent, "settings.ini");

            var helper = IniFileHelper.Instance.LoadFile(path);

            Assert.AreNotSame(IniFileHelper.Instance, helper);
        }

        [TestMethod]
        public void LoadFile_ReceiverHoldsLoadedData_Test()
        {
            var path = SandboxTestHelper.CreateFile(_sandbox, IniContent, "settings.ini");
            var receiver = IniFileHelper.Instance.Load("[s]\r\nkey=old");

            receiver.LoadFile(path);

            Assert.AreEqual("1", receiver.GetEntry("s", "key", "none"));
        }

        [TestMethod]
        public void LoadFile_ResultIndependentOfLaterLoad_Test()
        {
            var path = SandboxTestHelper.CreateFile(_sandbox, IniContent, "settings.ini");
            var receiver = IniFileHelper.Instance.Load("[s]\r\nkey=old");

            var result = receiver.LoadFile(path);
            receiver.Load("[s]\r\nkey=later");

            Assert.AreNotSame(receiver, result);
            Assert.AreEqual("1", result.GetEntry("s", "key", "none"));
        }

        [TestMethod]
        public void LoadFile_MissingFileThrowsFileNotFound_Test()
        {
            var path = Path.Combine(_sandbox, "missing.ini");

            Assert.ThrowsException<FileNotFoundException>(() => IniFileHelper.Instance.LoadFile(path));
        }

        [TestMethod]
        public void Load_MissingFileWithEncodingThrowsFileNotFound_Test()
        {
            var path = Path.Combine(_sandbox, "missing.ini");

            Assert.ThrowsException<FileNotFoundException>(() => IniFileHelper.Instance.Load(path, null));
        }

        [TestMethod]
        public void LoadFile_Utf16WithoutBomReadWithUnicodeEncoding_Test()
        {
            var path = SandboxTestHelper.InSandbox(_sandbox, Path.Combine(_sandbox, "utf16.ini"));
            File.WriteAllBytes(path, new UnicodeEncoding(false, false).GetBytes(IniContent));

            var helper = IniFileHelper.Instance.LoadFile(path, Encoding.Unicode);

            Assert.AreEqual("1", helper.GetEntry("s", "key", "none"));
        }
    }
}
