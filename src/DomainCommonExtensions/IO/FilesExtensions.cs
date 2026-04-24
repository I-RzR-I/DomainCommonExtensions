// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="FilesExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Validation;

#endregion

namespace RzR.Extensions.Domain.IO
{
    /// <summary>
    ///     File extensions
    /// </summary>
    /// <remarks></remarks>
    public static class FilesExtensions
    {
        /// <summary>
        ///     Check if file is used
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsFileInUse(this string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.OpenOrCreate);
                var canWrite = fs.CanWrite;

                return canWrite.IsFalse();
            }
            catch (IOException)
            {
                return true;
            }
        }

        /// <summary>
        ///     Atomically write text to a file. The data is first written to a temporary file in the
        ///     same directory and flushed to disk, then renamed/replaced over the destination so a reader
        ///     never observes a partially-written file. If the process or machine crashes mid-write, the
        ///     destination is either the previous content or the new content - never a truncated mix.
        /// </summary>
        /// <param name="path">Destination file path.</param>
        /// <param name="contents">Text to write. <see langword="null"/> is treated as an empty string.</param>
        /// <param name="encoding">Encoding to use. Defaults to UTF-8 without BOM.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is null or empty.</exception>
        public static void SafeWriteAllText(this string path, string contents, Encoding encoding = null)
        {
            DomainEnsure.IsNotNullOrEmpty(path, nameof(path));

            var enc = encoding ?? new UTF8Encoding(false);
            var bytes = enc.GetBytes(contents ?? string.Empty);
            SafeWriteAllBytes(path, bytes);
        }

        /// <summary>
        ///     Atomically write bytes to a file. The data is first written to a temporary file in the
        ///     same directory and flushed to disk, then renamed/replaced over the destination so a reader
        ///     never observes a partially-written file.
        /// </summary>
        /// <param name="path">Destination file path.</param>
        /// <param name="bytes">Bytes to write.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="bytes"/> is null.</exception>
        public static void SafeWriteAllBytes(this string path, byte[] bytes)
        {
            DomainEnsure.IsNotNullOrEmpty(path, nameof(path));
            DomainEnsure.IsNotNull(bytes, nameof(bytes));

            var fullPath = Path.GetFullPath(path);
            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            // Same-directory temp file is required for File.Replace / atomic rename to be valid.
            var tempPath = fullPath + "." + Guid.NewGuid().ToString("N") + ".tmp";

            try
            {
                using (var fs = new FileStream(
                    tempPath,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 4096,
                    options: FileOptions.WriteThrough))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }

                if (File.Exists(fullPath))
                {
                    File.Replace(tempPath, fullPath, destinationBackupFileName: null, ignoreMetadataErrors: true);
                }
                else
                {
                    File.Move(tempPath, fullPath);
                }
            }
            catch
            {
                TryDeleteSilently(tempPath);
                throw;
            }
        }

        /// <summary>
        ///     Compute the SHA-256 checksum of a file's contents and return it as a lower-case
        ///     hexadecimal string. The file is read in a streaming fashion so memory usage is constant
        ///     regardless of file size.
        /// </summary>
        /// <param name="path">Path to the file to hash.</param>
        /// <returns>64-character lower-case hexadecimal SHA-256 digest.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is null or empty.</exception>
        /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
        public static string Sha256HexFromFile(this string path)
        {
            DomainEnsure.IsNotNullOrEmpty(path, nameof(path));
            if (!File.Exists(path))
                throw new FileNotFoundException("File not found.", path);

            using var sha = SHA256.Create();
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 81920,
                useAsync: false);
            var hash = sha.ComputeHash(stream);

            var sb = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        /// <summary>
        ///     Try delete silently.
        /// </summary>
        /// <param name="path">Destination file path.</param>
        private static void TryDeleteSilently(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                // Best-effort cleanup; never mask the original exception.
            }
        }
    }
}
