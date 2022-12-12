// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-10 02:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-10 02:01
// ***********************************************************************
//  <copyright file="DirectoryHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System.IO;

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
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        /// <summary>
        ///     Cody source directory to target directory with all data
        /// </summary>
        /// <param name="source">Source directory info</param>
        /// <param name="target">Target directory info</param>
        /// <remarks></remarks>
        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            if (!target.Exists)
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
            if (!source.Exists) return;

            foreach (var fileInfo in source.GetFiles()) fileInfo.Delete();
            foreach (var childDirectory in source.GetDirectories()) DeleteDirectory(childDirectory);

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
    }
}