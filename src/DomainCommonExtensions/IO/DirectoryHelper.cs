// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-10 02:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-09 17:53
// ***********************************************************************
//  <copyright file="DirectoryHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RzR.Extensions.Domain.Internal;
using RzR.Extensions.Domain.Primitives;

#endregion

namespace RzR.Extensions.Domain.IO
{
    /// <summary>
    ///     Directory helper
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        ///     Create directory if not exist
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <remarks></remarks>
        public static void CreateDirectory(string path)
        {
            if (Directory.Exists(path).IsFalse())
                Directory.CreateDirectory(path);
        }

        /// <summary>
        ///     Cody source directory to target directory with all data
        /// </summary>
        /// <param name="source">Source directory info</param>
        /// <param name="target">Target directory info</param>
        /// <exception cref="ArgumentException">
        ///     Thrown before anything is created when <paramref name="target"/> is the same directory as
        ///     <paramref name="source"/> or is located inside it.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     Thrown before anything is created when <paramref name="source"/> does not exist.
        /// </exception>
        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            source.Refresh();
            target.Refresh();

            if (LexicalPathHelper.IsSameOrDescendant(source.FullName, target.FullName))
                throw new ArgumentException(
                    "The target directory must not be the source directory or be located inside it.", nameof(target));

            var entries = new List<KeyValuePair<FileSystemInfo, string>>();
            CollectCopyEntries(source, target.FullName, entries);

            if (target.Exists.IsFalse())
                target.Create();

            foreach (var entry in entries)
            {
                if (entry.Key is FileInfo fileInfo)
                    fileInfo.CopyTo(entry.Value, true);
                else
                    Directory.CreateDirectory(entry.Value);
            }
        }

        /// <summary>
        ///     Cody sourcePath directory to targetPath directory with all data
        /// </summary>
        /// <param name="sourcePath">Source directory path</param>
        /// <param name="targetPath">Target directory path</param>
        /// <exception cref="ArgumentException">
        ///     Thrown before anything is created when <paramref name="targetPath"/> is the same directory as
        ///     <paramref name="sourcePath"/> or is located inside it.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///     Thrown before anything is created when <paramref name="sourcePath"/> does not exist.
        /// </exception>
        /// <remarks>
        ///     Behaves as <see cref="CopyDirectory(DirectoryInfo, DirectoryInfo)"/>, including how links are
        ///     handled.
        /// </remarks>
        public static void CopyDirectory(string sourcePath, string targetPath)
        {
            CopyDirectory(new DirectoryInfo(sourcePath), new DirectoryInfo(targetPath));
        }

        /// <summary>
        ///     Delete the directory and all files that are there
        /// </summary>
        /// <param name="source">Source directory info</param>
        public static void DeleteDirectory(DirectoryInfo source)
        {
            source.Refresh();
            if (source.Exists.IsFalse()) return;

            DeleteDirectoryEntry(source);
        }

        /// <summary>
        ///     Delete the directory and all files that are there
        /// </summary>
        /// <param name="sourcePath">Source directory path</param>
        /// <remarks>
        ///     Behaves as <see cref="DeleteDirectory(DirectoryInfo)"/>, including how links are handled.
        /// </remarks>
        public static void DeleteDirectory(string sourcePath)
        {
            DeleteDirectory(new DirectoryInfo(sourcePath));
        }

        /// <summary>
        ///     Count file in directory
        /// </summary>
        /// <param name="directory">Files directory</param>
        /// <returns>
        ///     The number of files directly in <paramref name="directory"/> plus all files below it.
        /// </returns>
        /// <remarks></remarks>
        public static int FileCount(string directory)
        {
            var dirInfo = new DirectoryInfo(directory);

            return dirInfo.EnumerateFiles("*.*", SearchOption.AllDirectories).Count();
        }

        /// <summary>
        ///     Count file in directory
        /// </summary>
        /// <param name="directory">Files directory</param>
        /// <param name="searchOption">File search option</param>
        /// <returns>
        ///     The number of files in <paramref name="directory"/>, including subdirectories when
        ///     <paramref name="searchOption"/> is <see cref="SearchOption.AllDirectories"/>.
        /// </returns>
        /// <remarks></remarks>
        public static int FileCount(string directory, SearchOption searchOption)
        {
            var dirInfo = new DirectoryInfo(directory);

            return dirInfo.EnumerateFiles("*.*", searchOption).Count();
        }

        /// <summary>
        ///     Count file in directory
        /// </summary>
        /// <param name="directory">Files directory</param>
        /// <param name="searchOption">File search option</param>
        /// <param name="searchPattern">File search pattern</param>
        /// <returns>The number of files in the directory matching the pattern, using the search option.</returns>
        /// <remarks></remarks>
        public static int FileCount(string directory, SearchOption searchOption, string searchPattern)
        {
            var dirInfo = new DirectoryInfo(directory);

            return dirInfo.EnumerateFiles(searchPattern, searchOption).Count();
        }

        /// <summary>
        ///     File and directory count
        /// </summary>
        /// <param name="directory">Files directory</param>
        /// <returns></returns>
        /// <remarks>
        ///     Tuple.Item1 -> Directory count.
        ///     Tuple.Item2 -> File count.
        /// </remarks>
        public static Tuple<int, int> DirectoryFileCount(string directory)
        {
            var dictionary = new DirectoryInfo(directory)
                .EnumerateFileSystemInfos("*", SearchOption.AllDirectories)
                .GroupBy(fsi => fsi is DirectoryInfo)
                .ToDictionary(item => item.Key, s => s.Count());

            return new Tuple<int, int>(dictionary.ContainsKey(true)
                    ? dictionary[true]
                    : 0,
                dictionary.ContainsKey(false)
                    ? dictionary[false]
                    : 0);
        }

        /// <summary>
        ///     Check if exist file in directory.
        /// </summary>
        /// <param name="directoryWithFile">The directory with file.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        public static bool ExistFileInDirectory(string directoryWithFile)
        {
            return File.Exists(directoryWithFile);
        }

        /// <summary>
        ///     Records, in copy order, every file and directory below <paramref name="directory"/> that
        ///     is not a link, paired with the full path it is copied to.
        /// </summary>
        /// <remarks>
        ///     Recurses manually instead of using <see cref="SearchOption.AllDirectories"/>, because the
        ///     .NET Framework enumeration walks into junctions.
        /// </remarks>
        /// <param name="directory">The source directory to read.</param>
        /// <param name="targetPath">
        ///     The full path that mirrors <paramref name="directory"/> in the target.
        /// </param>
        /// <param name="entries">
        ///     Receives each source entry and its target path, parents before children.
        /// </param>
        private static void CollectCopyEntries(DirectoryInfo directory, string targetPath,
            List<KeyValuePair<FileSystemInfo, string>> entries)
        {
            foreach (var fileInfo in directory.GetFiles())
            {
                if (FileSystemLinkHelper.IsLink(fileInfo)) continue;

                entries.Add(new KeyValuePair<FileSystemInfo, string>(fileInfo, Path.Combine(targetPath, fileInfo.Name)));
            }

            foreach (var childDirectory in directory.GetDirectories())
            {
                if (FileSystemLinkHelper.IsLink(childDirectory)) continue;

                var childTargetPath = Path.Combine(targetPath, childDirectory.Name);
                entries.Add(new KeyValuePair<FileSystemInfo, string>(childDirectory, childTargetPath));
                CollectCopyEntries(childDirectory, childTargetPath, entries);
            }
        }

        /// <summary>
        ///     Deletes <paramref name="directory"/> and its content without following links.
        /// </summary>
        /// <remarks>
        ///     Directory and file links are removed with <see cref="FileSystemLinkHelper.RemoveLinkEntry"/>
        ///     , which removes the link entry by its exact name and leaves its target untouched. Regular
        ///     files and directories use the BCL delete; on .NET Framework that applies Win32 name
        ///     normalization, so a regular entry whose name ends in a dot or space can fail to delete (a
        ///     pre-existing BCL limitation).
        /// </remarks>
        /// <param name="directory">The directory to delete.</param>
        private static void DeleteDirectoryEntry(DirectoryInfo directory)
        {
            if (FileSystemLinkHelper.IsLink(directory))
            {
                FileSystemLinkHelper.RemoveLinkEntry(directory);

                return;
            }

            foreach (var fileInfo in directory.GetFiles())
            {
                if (FileSystemLinkHelper.IsLink(fileInfo))
                    FileSystemLinkHelper.RemoveLinkEntry(fileInfo);
                else
                    fileInfo.Delete();
            }

            foreach (var childDirectory in directory.GetDirectories())
                DeleteDirectoryEntry(childDirectory);

            directory.Delete();
        }
    }
}
