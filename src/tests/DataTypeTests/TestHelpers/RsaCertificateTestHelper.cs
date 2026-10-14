using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DataTypeTests.TestHelpers
{
    internal static class RsaCertificateTestHelper
    {
        internal static X509Certificate2 CreateWithPrivateKey()
        {
            using var rsa = RSA.Create(2048);
            var request = new CertificateRequest("CN=test", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddDays(1));
        }

        internal static X509Certificate2 CreatePublicOnly(X509Certificate2 certificate)
        {
            return new X509Certificate2(certificate.Export(X509ContentType.Cert));
        }
    }
}
