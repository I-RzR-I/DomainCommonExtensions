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
using System.IO;
using System.Linq;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.Helpers
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
        /// <remarks></remarks>
        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            if (target.Exists.IsFalse())
                target.Create();

            foreach (var fileInfo in source.GetFiles())
                fileInfo.CopyTo(Path.Combine(target.ToString(), fileInfo.Name), true);

            foreach (var directoryInfo in source.GetDirectories())
            {
                var childDirectory = target.CreateSubdirectory(directoryInfo.Name);
                CopyDirectory(directoryInfo, childDirectory);
            }
        }

        /// <summary>
        ///     Cody sourcePath directory to targetPath directory with all data
        /// </summary>
        /// <param name="sourcePath">Source directory path</param>
        /// <param name="targetPath">Target directory path</param>
        /// <remarks></remarks>
        public static void CopyDirectory(string sourcePath, string targetPath)
        {
            CopyDirectory(new DirectoryInfo(sourcePath), new DirectoryInfo(targetPath));
        }

        /// <summary>
        ///     Delete the directory and all files that are there
        /// </summary>
        /// <param name="source">Source directory info</param>
        /// <remarks></remarks>
        public static void DeleteDirectory(DirectoryInfo source)
        {
            if (source.Exists.IsFalse()) return;

            foreach (var fileInfo in source.GetFiles()) 
                fileInfo.Delete();
            foreach (var childDirectory in source.GetDirectories()) 
                DeleteDirectory(childDirectory);

            source.Delete();
        }

        /// <summary>
        ///     Delete the directory and all files that are there
        /// </summary>
        /// <param name="sourcePath">Source directory path</param>
        /// <remarks></remarks>
        public static void DeleteDirectory(string sourcePath)
        {
            DeleteDirectory(new DirectoryInfo(sourcePath));
        }

        /// <summary>
        ///     Count file in directory
        /// </summary>
        /// <param name="directory">Files directory</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int FileCount(string directory)
        {
            var dirInfo = new DirectoryInfo(directory);

            return dirInfo.EnumerateDirectories().AsParallel()
                .SelectMany(di => di.EnumerateFiles("*.*", SearchOption.AllDirectories)).Count();
        }

        /// <summary>
        ///     Count file in directory
        /// </summary>
        /// <param name="directory">Files directory</param>
        /// <param name="searchOption">File search option</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int FileCount(string directory, SearchOption searchOption)
        {
            var dirInfo = new DirectoryInfo(directory);

            return dirInfo.EnumerateDirectories().AsParallel()
                .SelectMany(di => di.EnumerateFiles("*.*", searchOption)).Count();
        }

        /// <summary>
        ///     Count file in directory
        /// </summary>
        /// <param name="directory">Files directory</param>
        /// <param name="searchOption">File search option</param>
        /// <param name="searchPattern">File search pattern</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static int FileCount(string directory, SearchOption searchOption, string searchPattern)
        {
            var dirInfo = new DirectoryInfo(directory);

            return dirInfo.EnumerateDirectories().AsParallel()
                .SelectMany(di => di.EnumerateFiles(searchPattern, searchOption)).Count();
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
    }
}