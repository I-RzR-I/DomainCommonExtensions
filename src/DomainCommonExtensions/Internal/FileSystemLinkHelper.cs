// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-07 21:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 12:09
//  ***********************************************************************
//  <copyright file="FileSystemLinkHelper.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace RzR.Extensions.Domain.Internal
{
    /// <summary>
    ///     Detects file system links (symbolic links, junctions and mount points) with the same rule
    ///     as the .NET runtime.
    /// </summary>
    internal static class FileSystemLinkHelper
    {
        /// <summary>
        ///     (Immutable) the size in bytes of the native WIN32_FIND_DATAW structure.
        /// </summary>
        private const int FindDataSize = 592;

        /// <summary>
        ///     (Immutable) the byte offset of WIN32_FIND_DATAW.dwFileAttributes.
        /// </summary>
        private const int FileAttributesOffset = 0;

        /// <summary>
        ///     (Immutable) the byte offset of WIN32_FIND_DATAW.dwReserved0, which holds the reparse tag.
        /// </summary>
        private const int ReparseTagOffset = 36;

        /// <summary>
        ///     (Immutable) the byte offset of WIN32_FIND_DATAW.cFileName.
        /// </summary>
        private const int FileNameOffset = 44;

        /// <summary>
        ///     (Immutable) the size in bytes of WIN32_FIND_DATAW.cFileName (260 UTF-16 characters).
        /// </summary>
        private const int FileNameSize = 520;

        /// <summary>
        ///     (Immutable) the reparse tag bit that marks a name surrogate (IsReparseTagNameSurrogate).
        /// </summary>
        private const uint NameSurrogateBit = 0x20000000;

        /// <summary>
        ///     (Immutable) the Win32 extended-length path prefix.
        /// </summary>
        private const string ExtendedPrefix = @"\\?\";

        /// <summary>
        ///     (Immutable) the handle value FindFirstFileW returns on failure.
        /// </summary>
        private static readonly IntPtr InvalidHandleValue = new(-1);

        /// <summary>
        ///     Checks whether a file system entry is a link (symbolic link, junction or mount point).
        /// </summary>
        /// <param name="entry">The file or directory entry.</param>
        /// <returns>
        ///     True if the entry is a link, or if it is a reparse point whose kind cannot be confirmed;
        ///     otherwise false.
        /// </returns>
        internal static bool IsLink(FileSystemInfo entry)
        {
            if (Path.DirectorySeparatorChar == '\\' && EndsWithDotOrSpace(entry.Name)) return true;
            if ((entry.Attributes & FileAttributes.ReparsePoint) == 0) return false;
            if (Path.DirectorySeparatorChar != '\\') return true;

            return IsWindowsNameSurrogate(entry.FullName);
        }

        /// <summary>
        ///     Removes a link entry (symbolic link, junction or mount point) without touching its target.
        /// </summary>
        /// <exception cref="IOException">Thrown when the link entry cannot be removed.</exception>
        /// <param name="entry">
        ///     The link entry; a <see cref="DirectoryInfo" /> for directory links, a <see cref="FileInfo" />
        ///     for file links.
        /// </param>
        internal static void RemoveLinkEntry(FileSystemInfo entry)
        {
            if (Path.DirectorySeparatorChar != '\\')
            {
                entry.Delete();

                return;
            }

            var path = ToExtendedPath(TrimTrailingSeparators(entry.FullName));
            var removed = entry is DirectoryInfo ? RemoveDirectoryW(path) : DeleteFileW(path);
            if (removed) return;

            var error = Marshal.GetLastWin32Error();
            throw new IOException(
                $"The link entry '{entry.FullName}' could not be removed: {new Win32Exception(error).Message}",
                unchecked((int)0x80070000 | (error & 0xFFFF)));
        }

        /// <summary>
        ///     Reads the reparse tag of a Windows entry and checks whether it is a name surrogate.
        /// </summary>
        /// <param name="fullPath">The full path of the entry.</param>
        /// <returns>
        ///     True if the entry is a name-surrogate reparse point, or if the lookup fails or returns a
        ///     different entry; false if the exact entry is no longer a reparse point or its tag is not
        ///     a name surrogate.
        /// </returns>
        private static bool IsWindowsNameSurrogate(string fullPath)
        {
            var path = TrimTrailingSeparators(fullPath);
            var name = Path.GetFileName(path);
            if (string.IsNullOrEmpty(name)) return true;

            var findData = new byte[FindDataSize];
            var handle = InvalidHandleValue;
            try
            {
                handle = FindFirstFileW(ToExtendedPath(path), findData);
                if (handle == InvalidHandleValue) 
                    return true;

                if (!string.Equals(ReadFileName(findData), name, StringComparison.OrdinalIgnoreCase)) 
                    return true;

                var attributes = (FileAttributes)BitConverter.ToInt32(findData, FileAttributesOffset);
                if ((attributes & FileAttributes.ReparsePoint) == 0) 
                    return false;

                return (BitConverter.ToUInt32(findData, ReparseTagOffset) & NameSurrogateBit) != 0;
            }
            finally
            {
                if (handle != InvalidHandleValue)
                    FindClose(handle);
            }
        }

        /// <summary>
        ///     Checks whether a file or directory name ends in a dot or a space.
        /// </summary>
        /// <param name="name">The entry name.</param>
        /// <returns>
        ///     True if <paramref name="name" /> is not empty and its last character is a dot or a space;
        ///     otherwise false.
        /// </returns>
        private static bool EndsWithDotOrSpace(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;

            var last = name[name.Length - 1];

            return last == '.' || last == ' ';
        }

        /// <summary>
        ///     Removes trailing directory separators, except from a root such as <c>C:\</c>.
        /// </summary>
        /// <param name="path">The full path.</param>
        /// <returns>
        ///     The path without trailing separators, or the root unchanged.
        /// </returns>
        private static string TrimTrailingSeparators(string path)
        {
            var trimmed = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var root = Path.GetPathRoot(path) ?? string.Empty;

            return trimmed.Length < root.Length ? root : trimmed;
        }

        /// <summary>
        ///     Converts a full Windows path to its extended-length form.
        /// </summary>
        /// <remarks>
        ///     Extended-length paths accept only <c>\</c> as a separator, so <c>/</c> is mapped first.
        /// </remarks>
        /// <param name="path">The full path.</param>
        /// <returns>
        ///     The path unchanged when it already starts with <c>\\?\</c>; <c>\\?\</c> plus the rest for
        ///     a
        ///     <c>\\.\</c> device path; <c>\\?\UNC\</c> plus the rest for a <c>\\server\share</c> path;
        ///     otherwise <c>\\?\</c> plus the path.
        /// </returns>
        private static string ToExtendedPath(string path)
        {
            if (path.StartsWith(ExtendedPrefix, StringComparison.Ordinal)) return path;

            var normalized = path.Replace('/', '\\');
            if (normalized.StartsWith(@"\\.\", StringComparison.Ordinal))
                return ExtendedPrefix + normalized.Substring(4);

            if (normalized.StartsWith(@"\\", StringComparison.Ordinal))
                return ExtendedPrefix + @"UNC\" + normalized.Substring(2);

            return ExtendedPrefix + normalized;
        }

        /// <summary>
        ///     Reads WIN32_FIND_DATAW.cFileName from the raw structure bytes.
        /// </summary>
        /// <param name="findData">The WIN32_FIND_DATAW structure as raw bytes.</param>
        /// <returns>
        ///     The file name up to the first NUL character.
        /// </returns>
        private static string ReadFileName(byte[] findData)
        {
            var fileName = Encoding.Unicode.GetString(findData, FileNameOffset, FileNameSize);
            var terminator = fileName.IndexOf('\0');

            return terminator >= 0 
                ? fileName.Substring(0, terminator)
                : fileName;
        }

        /// <summary>
        ///     Native FindFirstFileW from kernel32.
        /// </summary>
        /// <param name="lpFileName">The full path of the entry to describe.</param>
        /// <param name="lpFindFileData">Receives the WIN32_FIND_DATAW structure as raw bytes.</param>
        /// <returns>
        ///     A search handle, or <see cref="InvalidHandleValue" /> on failure.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr FindFirstFileW(string lpFileName, [Out] byte[] lpFindFileData);

        /// <summary>
        ///     Native FindClose from kernel32.
        /// </summary>
        /// <param name="hFindFile">The search handle returned by FindFirstFileW.</param>
        /// <returns>
        ///     True if the handle was closed; otherwise false.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FindClose(IntPtr hFindFile);

        /// <summary>
        ///     Native RemoveDirectoryW from kernel32.
        /// </summary>
        /// <param name="lpPathName">The extended-length path of the directory entry to remove.</param>
        /// <returns>
        ///     True if the entry was removed; otherwise false.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RemoveDirectoryW(string lpPathName);

        /// <summary>
        ///     Native DeleteFileW from kernel32.
        /// </summary>
        /// <param name="lpFileName">The extended-length path of the file entry to remove.</param>
        /// <returns>
        ///     True if the entry was removed; otherwise false.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteFileW(string lpFileName);
    }
}