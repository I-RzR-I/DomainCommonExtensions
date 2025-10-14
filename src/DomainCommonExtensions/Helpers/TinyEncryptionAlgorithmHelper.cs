// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-10-13 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-13 22:52
// ***********************************************************************
//  <copyright file="TinyEncryptionAlgorithmHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Text;
using CodeSource;
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Utilities;

// ReSharper disable IntVariableOverflowInUncheckedContext
// ReSharper disable ArrangeRedundantParentheses
// ReSharper disable RedundantCast
// ReSharper disable CommentTypo

#endregion

namespace DomainCommonExtensions.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A TEA (Tiny Encryption Algorithm) helper.
    /// </summary>
    /// =================================================================================================
    [CodeSource(SourceUrl = "https://www.codeproject.com/articles/Tiny-Encryption-Algorithm-TEA-for-the-Compact-Fram", Comment = "Inspired from the article.")]
    [CodeSource(SourceUrl = "https://www.movable-type.co.uk/scripts/tea-block.html", Comment = "Inspired from the article.")]
    [CodeSource(SourceUrl = "https://en.wikipedia.org/wiki/Tiny_Encryption_Algorithm", Comment = "Inspired from the article.")]
    public static class TinyEncryptionAlgorithmHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) an uint to process.
        ///     The 'magic' delta number.
        /// </summary>
        /// =================================================================================================
        private const uint D = 0x9e3779b9;

        /// <summary>
        ///     Tea encrypt.
        /// </summary>
        /// <param name="data">[in,out] The data array.</param>
        /// <param name="key">The key.</param>
        public static void TeaEncrypt(ref uint[] data, uint[] key)
        {
            byte a;

            var y = data[0];
            var z = data[1];
            uint sum = 0;
            for (a = 0; a <= 31; a++)
            {
                sum += D;
                y += ((z << 4) + key[0]) ^ (z + sum) ^ ((z >> 5) + key[1]);
                z += ((y << 4) + key[2]) ^ (y + sum) ^ ((y >> 5) + key[3]);
            }

            data[0] = y;
            data[1] = z;
        }

        /// <summary>
        ///     TEA decrypt.
        /// </summary>
        /// <param name="data">[in,out] The encrypted data array.</param>
        /// <param name="key">The key.</param>
        public static void TeaDecrypt(ref uint[] data, uint[] key)
        {
            byte a;

            var y = data[0];
            var z = data[1];
            var sum = D << 5;

            for (a = 0; a <= 31; a++)
            {
                z -= ((y << 4) + key[2]) ^ (y + sum) ^ ((y >> 5) + key[3]);
                y -= ((z << 4) + key[0]) ^ (z + sum) ^ ((z >> 5) + key[1]);
                sum -= D;
            }

            data[0] = y;
            data[1] = z;
        }

        /// <summary>
        ///     XTEA encrypt.
        /// </summary>
        /// <param name="data">[in,out] The data array.</param>
        /// <param name="key">The key.</param>
        /// <param name="n">(Optional) An uint to process.</param>
        public static void XTeaEncrypt(ref uint[] data, uint[] key, uint n = 32)
        {
            var y = data[0];
            var z = data[1];
            uint sum = 0;
            var limit = D * n;

            while (sum != limit)
            {
                y += (((z << 4) ^ (z >> 5)) + z) ^ (sum + key[sum & 3]);
                sum += D;
                z += (((y << 4) ^ (y >> 5)) + y) ^ (sum + key[(sum >> 11) & 3]);
            }

            data[0] = y;
            data[1] = z;
        }

        /// <summary>
        ///     XTEA decrypt.
        /// </summary>
        /// <param name="data">[in,out] The encrypted data array.</param>
        /// <param name="key">The key.</param>
        /// <param name="n">(Optional) An uint to process.</param>
        public static void XTeaDecrypt(ref uint[] data, uint[] key, uint n = 32)
        {
            var y = data[0];
            var z = data[1];
            var sum = D * n;
            while (sum != 0)
            {
                z -= (((y << 4) ^ (y >> 5)) + y) ^ (sum + key[(sum >> 11) & 3]);
                sum -= D;
                y -= (((z << 4) ^ (z >> 5)) + z) ^ (sum + key[sum & 3]);
            }

            data[0] = y;
            data[1] = z;
        }

        /// <summary>
        ///     Block TEA encrypt.
        /// </summary>
        /// <param name="data">[in,out] The data array.</param>
        /// <param name="key">The key.</param>
        public static void BlockTeaEncrypt(ref uint[] data, uint[] key)
        {
            var n = data.Length;
            var q = 6 + 52 / n;
            var z = data[n - 1];
            uint sum = 0;

            do
            {
                sum += D;
                var e = (sum >> 2) & 3;
                uint p;
                for (p = 0; p < n; p++)
                {
                    var y = data[p];
                    y += Mx(z, p, e, sum, key);
                    data[p] = y;
                    z = y;
                }

                q--;
            } while (q != 0);
        }

        /// <summary>
        ///     Block TEA decrypt.
        /// </summary>
        /// <param name="data">[in,out] The encrypted data array.</param>
        /// <param name="key">The key.</param>
        public static void BlockTeaDecrypt(ref uint[] data, uint[] key)
        {
            var n = (uint)data.Length;
            var q = 6 + 52 / n;
            var sum = q * D;
            while (sum != 0)
            {
                var e = (sum >> 2) & 3;
                uint z, y, p;

                for (p = n - 1; p >= 1; p--)
                {
                    z = data[p - 1];
                    y = data[p];
                    y -= Mx(z, p, e, sum, key);
                    data[p] = y;
                }

                z = data[n - 1];
                y = data[0];
                y -= Mx(z, p, e, sum, key);
                data[0] = y;
                sum -= D;
            }
        }

        /// <summary>
        ///     XXTEA encrypt.
        /// </summary>
        /// <param name="data">[in,out] The data array.</param>
        /// <param name="key">The key.</param>
        public static void XxTeaEncrypt(ref uint[] data, uint[] key)
        {
            var n = data.Length;
            var q = 6 + 52 / n;
            var z = data[n - 1];
            uint sum = 0;
            do
            {
                sum += D;
                var e = (sum >> 2) & 3;
                uint p, x, y;

                for (p = 0; p <= n - 2; p++)
                {
                    y = data[p + 1];
                    x = data[p];
                    x += Mxx(z, p, e, sum, y, key);
                    data[p] = x;
                    z = x;
                }

                y = data[0];
                x = data[n - 1];
                x += Mxx(z, p, e, sum, y, key);
                data[n - 1] = x;
                z = x;
                q--;
            } while (q != 0);
        }

        /// <summary>
        ///     XXTEA decrypt.
        /// </summary>
        /// <param name="data">[in,out] The encrypted data array.</param>
        /// <param name="key">The key.</param>
        public static void XxTeaDecrypt(ref uint[] data, uint[] key)
        {
            var n = (uint)data.Length;
            var q = 6 + 52 / n;
            var y = data[0];
            var sum = q * D;
            while (sum != 0)
            {
                var e = (sum >> 2) & 3;
                uint z, x, p;
                for (p = n - 1; p >= 1; p--)
                {
                    z = data[p - 1];
                    x = data[p];
                    x -= Mxx(z, p, e, sum, y, key);
                    data[p] = x;
                    y = x;
                }

                z = data[n - 1];
                x = data[0];
                x -= Mxx(z, p, e, sum, y, key);
                data[0] = x;
                y = x;
                sum -= D;
            }
        }

        /// <summary>
        ///     Check if the keys (<see cref="key1"/> and <see cref="key2"/>) are the same.
        /// </summary>
        /// <param name="key1">The first key.</param>
        /// <param name="key2">The second key.</param>
        /// <returns>
        ///     True if same key, false if not.
        /// </returns>
        public static bool IsSameKey(uint[] key1, uint[] key2)
        {
            if (key1.Length != key2.Length)
                return false;

            for (var i = 0; i < key1.Length; i++)
                if (key1[i] != key2[i])
                    return false;
            return true;
        }

        /// <summary>
        ///     Convert text to data.
        /// </summary>
        /// <param name="sourceText">[in,out] Source text.</param>
        /// <param name="data">[in,out] The encrypted data array.</param>
        public static void ConvertTextToData(ref string sourceText, ref uint[] data)
        {
            data = data.NotNull();

            int i;
            var sa = sourceText;
            var n = sa.Length / 4;
            var m = sa.Length % 4;
            if (m != 0)
            {
                n++;
                for (var j = 0; j < 4 - m; j++)
                    sa = sa + " ";
            }

            if (n < 2) // n = 1
            {
                n = 2;
                for (var j = 0; j < 4; j++)
                    sa = sa + " ";
            }

            data = new uint[n];

            for (i = 0; i < n; i++)
            {
                data[i] = 0;
                for (m = 0; m <= 3; m++)
                {
                    var ba = Encoding.GetEncoding("ISO-8859-1").GetBytes(sa.Substring(i * 4 + m, 1));
                    data[i] += (uint)(ba[0] << (m * 8));
                }
            }
        }

        /// <summary>
        ///     Convert data to text.
        /// </summary>
        /// <param name="sourceText">[in,out] Source text.</param>
        /// <param name="data">The encrypted data array.</param>
        public static void ConvertDataToText(ref string sourceText, uint[] data)
        {
            if (sourceText.IsNull()) return;

            int i, m;
            var n = data.Length;
            var sa = new string(' ', n * 4);
            for (i = 0; i <= n - 1; i++)
                for (m = 0; m <= 3; m++)
                {
                    var b = ConvertUnsingInt32ToByte(data[i], m);
                    if (i * 4 + m + 2 < sa.Length)
                        sa = sa.Substring(0, i * 4 + m) + (char)b + sa.Substring(i * 4 + m + 2);
                    else
                        sa = sa.Substring(0, i * 4 + m) + (char)b;
                }

            sourceText = sa.Trim();
        }

        /// <summary>
        ///     Reads an encrypted data and decode from BASE64.
        /// </summary>
        /// <param name="encryptedData">[in,out] Information describing the encrypted data.</param>
        /// <param name="data">[in,out] The encrypted data array.</param>
        public static void ReadData(ref string encryptedData, ref uint[] data)
        {
            var dataArray = encryptedData.DeCodeBytesFromBase64();
            var totalOctets = dataArray.Length;
            data = new uint[totalOctets / 4];
            var n = 0;
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = ((uint)dataArray[n++]) +
                          ((uint)dataArray[n++] << 8) +
                          ((uint)dataArray[n++] << 16) +
                          ((uint)dataArray[n++] << 24);
            }
        }

        /// <summary>
        ///     Writes an encrypted data array and convert to BASE64.
        /// </summary>
        /// <param name="encryptedData">[in,out] Information describing the encrypted data array.</param>
        /// <param name="resultData">[in,out] Information describing the encrypted result.</param>
        public static void WriteData(ref uint[] encryptedData, ref string resultData)
        {
            var dataArray = new byte[] { };
            for (var i = 0; i < encryptedData.Length; i++)
            {
                dataArray = dataArray.AppendItem(
                    (byte)(encryptedData[i] & 0xFF),
                    (byte)((encryptedData[i] & 0xFF00) >> 8),
                    (byte)((encryptedData[i] & 0xFF0000) >> 16),
                    (byte)((encryptedData[i] & 0xFF000000) >> 24));
            }

            resultData = dataArray.ToBase64String();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Generates a pass key from pass code.
        /// </summary>
        /// <param name="passCode">The pass code.</param>
        /// <param name="passKey">[out] The pass key.</param>
        /// <remarks>
        ///     In case when the <see cref="passCode"/> length is lss than 16 character, 
        ///     the rest of pass code will be generated and supplied.
        /// </remarks>
        /// <returns>
        ///     The pass key from pass code.
        /// </returns>
        /// =================================================================================================
        public static string GeneratePassKeyFromPassCode(string passCode, out uint[] passKey)
        {
            var newCode = passCode.IfFuncIsTrue(
                (passCode + (PasswordGenerateUtils.Generate(16))).Truncate(16), 
                () => passCode.Length < 16);

            passKey = new uint[] { };
            ConvertTextToData(ref newCode, ref passKey);

            return newCode;
        }

        /// <summary>
        ///     MX => XTEA.
        /// </summary>
        /// <param name="z">An uint to process.</param>
        /// <param name="p">An uint to process.</param>
        /// <param name="e">An uint to process.</param>
        /// <param name="sum">Number of.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     An uint.
        /// </returns>
        private static uint Mx(uint z, uint p, uint e, uint sum, uint[] key)
        {
            return (((z << 4) ^ (z >> 5)) + z) ^ (key[(p & 3) ^ e] + sum);
        }

        /// <summary>
        ///     MX => XXTEA.
        /// </summary>
        /// <param name="z">An uint to process.</param>
        /// <param name="p">An uint to process.</param>
        /// <param name="e">An uint to process.</param>
        /// <param name="sum">Number of.</param>
        /// <param name="y">An uint to process.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///     An uint.
        /// </returns>
        private static uint Mxx(uint z, uint p, uint e, uint sum, uint y, uint[] key)
        {
            return (((z >> 5) ^ (y << 2)) + ((y >> 3) ^ (z << 4))) ^ ((sum ^ y) + (key[(p & 3) ^ e] ^ z));
        }

        /// <summary>
        ///     Convert unsing int 32 to byte.
        /// </summary>
        /// <param name="dataItem">An uint to process.</param>
        /// <param name="idx">Zero-based index of the array.</param>
        /// <returns>
        ///     The unsing converted int 32 to byte.
        /// </returns>
        private static byte ConvertUnsingInt32ToByte(uint dataItem, int idx)
        {
            switch (idx)
            {
                case 0:
                    dataItem = dataItem & 0xFF;
                    return (byte)dataItem;
                case 1:
                    dataItem = dataItem & 0xFF00;
                    return (byte)(dataItem >> 8);
                case 2:
                    dataItem = dataItem & 0xFF0000;
                    return (byte)(dataItem >> 16);
                default:
                    dataItem = dataItem & 0xFF000000;
                    return (byte)(dataItem >> 24);
            }
        }
    }
}