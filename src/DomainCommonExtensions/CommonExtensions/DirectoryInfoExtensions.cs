// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-10-09 18:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-10-09 18:39
// ***********************************************************************
//  <copyright file="DirectoryInfoExtensions.cs" company="RzR SOFT & TECH">
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
using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

namespace DomainCommonExtensions.CommonExtensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A directory information extensions.
    /// </summary>
    /// =================================================================================================
    public static class DirectoryInfoExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A DirectoryInfo extension method that gets matching files.
        /// </summary>
        /// <param name="directory">Pathname of the directory.</param>
        /// <param name="searchPatterns">The search patterns. Multiple search pattern separated by ';'.</param>
        /// <param name="searchOption">The search option.</param>
        /// <returns>
        ///     An array of file information.
        /// </returns>
        /// =================================================================================================
        public static FileInfo[] GetMatchingFiles(this DirectoryInfo directory, string searchPatterns,
            SearchOption searchOption)
        {
            var searchPatternArray = searchPatterns.Split(';');
            var matches = new FileInfo[] { };
            foreach (var searchPattern in searchPatternArray)
                matches.AddRange(directory.GetFiles(searchPattern, searchOption));

            return matches;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A System.IO.DirectoryInfo extension method that empties the given directory.
        /// </summary>
        /// <param name="directory">Pathname of the directory.</param>
        /// =================================================================================================
        public static void Empty(this DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles()) 
                file.Delete();

            foreach (var subDirectory in directory.GetDirectories()) 
                subDirectory.Delete(true);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Checks for the given folder in a directory path and returns the parent of that folder.
        /// </summary>
        /// <param name="dir">The DirectoryInfo.</param>
        /// <param name="folder">Pathname of the folder.</param>
        /// <returns>
        ///     The parent of.
        /// </returns>
        /// =================================================================================================
        public static DirectoryInfo GetParentOf(this DirectoryInfo dir, string folder)
        {
            dir = dir.Where(d => d.Name.Equals(folder, StringComparison.OrdinalIgnoreCase));

            return dir.IsNotNull() ? dir.Parent : null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Checks parent directories against the given predicate and returns the first match.
        /// </summary>
        /// <param name="dir">The DirectoryInfo.</param>
        /// <param name="test">.</param>
        /// <returns>
        ///     A DirectoryInfo.
        /// </returns>
        /// =================================================================================================
        public static DirectoryInfo Where(this DirectoryInfo dir, Func<DirectoryInfo, bool> test)
        {
            while (dir.IsNotNull() && !test(dir))
                dir = dir!.Parent;

            return dir;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A DirectoryInfo extension method that query if 'dir' has directories.
        /// </summary>
        /// <param name="dir">The DirectoryInfo.</param>
        /// <returns>
        ///     True if directories, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool HasDirectories(this DirectoryInfo dir)
        {
            return dir.Exists && dir.EnumerateDirectories().Any();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A DirectoryInfo extension method that query if 'dir' has files.
        /// </summary>
        /// <param name="dir">The DirectoryInfo.</param>
        /// <returns>
        ///     True if files, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool HasFiles(this DirectoryInfo dir)
        {
            return dir.Exists && dir.EnumerateFiles().Any();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A DirectoryInfo extension method that query if 'DirectoryInfo' is empty.
        /// </summary>
        /// <param name="dir">The DirectoryInfo.</param>
        /// <returns>
        ///     True if empty, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsEmpty(this DirectoryInfo dir)
        {
            return dir.HasFiles().IsFalse() && dir.HasDirectories().IsFalse();
        }
    }
}