using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using DataTypeTests.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzR.Extensions.Domain.Cryptography.Rsa;

namespace DataTypeTests.DataTests
{
    [TestClass]
    public class CryptoRSACertificateTests
    {
        [TestMethod]
        public void EncryptRSACertificate_SelfSignedCertificate_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();

            var encrypted = "hello".EncryptRSACertificate(certificate);

            Assert.IsFalse(string.IsNullOrEmpty(encrypted));
            Assert.AreEqual(256, Convert.FromBase64String(encrypted).Length);
        }

        [TestMethod]
        public void DecryptRSACertificate_RoundTrip_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();

            var encrypted = "hello".EncryptRSACertificate(certificate);
            var decrypted = encrypted.DecryptRSACertificate(certificate);

            Assert.AreEqual("hello", decrypted);
        }

        [TestMethod]
        public void DecryptRSACertificate_NonAsciiRoundTrip_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();
            const string message = "héllo ✓ Ünïcode";

            var encrypted = message.EncryptRSACertificate(certificate);
            var decrypted = encrypted.DecryptRSACertificate(certificate);

            Assert.AreEqual(message, decrypted);
        }

        [TestMethod]
        public void DecryptRSACertificate_PublicKeyPkcs1Ciphertext_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();
            using var publicKey = certificate.GetRSAPublicKey();
            var cipherText = Convert.ToBase64String(publicKey.Encrypt(Encoding.Unicode.GetBytes("hi"), RSAEncryptionPadding.Pkcs1));

            var decrypted = cipherText.DecryptRSACertificate(certificate);

            Assert.AreEqual("hi", decrypted);
        }

        [TestMethod]
        public void EncryptRSACertificate_PublicOnlyCertificate_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();
            using var publicOnly = RsaCertificateTestHelper.CreatePublicOnly(certificate);

            var encrypted = "hello".EncryptRSACertificate(publicOnly);
            var decrypted = encrypted.DecryptRSACertificate(certificate);

            Assert.AreEqual("hello", decrypted);
        }

        [TestMethod]
        public void DecryptRSACertificate_InvalidBase64_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();

            var decrypted = "not-base64!!".DecryptRSACertificate(certificate);

            Assert.IsNull(decrypted);
        }

        [TestMethod]
        public void DecryptRSACertificate_GarbageCiphertext_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();
            var garbage = Convert.ToBase64String(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            var decrypted = garbage.DecryptRSACertificate(certificate);

            Assert.IsNull(decrypted);
        }

        [TestMethod]
        public void DecryptRSACertificate_PublicOnlyCertificate_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();
            using var publicOnly = RsaCertificateTestHelper.CreatePublicOnly(certificate);
            using var publicKey = certificate.GetRSAPublicKey();
            var cipherText = Convert.ToBase64String(publicKey.Encrypt(Encoding.Unicode.GetBytes("hi"), RSAEncryptionPadding.Pkcs1));

            var decrypted = cipherText.DecryptRSACertificate(publicOnly);

            Assert.IsNull(decrypted);
        }

        [TestMethod]
        public void EncryptRSACertificate_NullCertificate_Test()
        {
            var encrypted = "hello".EncryptRSACertificate((X509Certificate2)null);

            Assert.IsNull(encrypted);
        }

        [TestMethod]
        public void EncryptRSACertificate_EmptyInput_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();

            var encrypted = string.Empty.EncryptRSACertificate(certificate);

            Assert.IsNull(encrypted);
        }

        [TestMethod]
        public void DecryptRSACertificate_NullCertificate_Test()
        {
            var decrypted = "aGVsbG8=".DecryptRSACertificate((X509Certificate2)null);

            Assert.IsNull(decrypted);
        }

        [TestMethod]
        public void DecryptRSACertificate_CspOverloadInterop_Test()
        {
            using var certificate = RsaCertificateTestHelper.CreateWithPrivateKey();
            using var privateKey = certificate.GetRSAPrivateKey();
            using var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(privateKey.ExportParameters(true));

            var fromCertificate = "interop".EncryptRSACertificate(certificate);
            var fromCsp = "interop".EncryptRSACertificate(csp);

            Assert.AreEqual("interop", fromCertificate.DecryptRSACertificate(csp));
            Assert.AreEqual("interop", fromCsp.DecryptRSACertificate(certificate));
        }
    }
}
