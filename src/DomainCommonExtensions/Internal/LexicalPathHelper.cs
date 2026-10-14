// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-06 21:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 12:07
//  ***********************************************************************
//  <copyright file="LexicalPathHelper.cs" company="RzR SOFT & TECH">
//      Copyright (c) RzR. All rights reserved.
//  </copyright>
//  <contact>
//      https://iamrzr.dev/contact
//  </contact>
//  <summary></summary>
//  ***********************************************************************

#region U S I N G

using System;
using System.IO;

#endregion

namespace RzR.Extensions.Domain.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Lexical (string-only) directory path comparisons.
    /// </summary>
    /// =================================================================================================
    internal static class LexicalPathHelper
    {
        /// <summary>
        ///     Gets the string comparison used for paths on the current platform.
        /// </summary>
        /// <remarks>
        ///     Windows paths compare case-insensitively; every other platform compares ordinally. The
        ///     platform is detected from <see cref="Path.DirectorySeparatorChar" /> so the same code
        ///     runs on every target framework.
        /// </remarks>
        /// <value>
        ///     The path comparison.
        /// </value>
        private static StringComparison PathComparison
            => Path.DirectorySeparatorChar == '\\'
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

        /// <summary>
        ///     Checks whether <paramref name="candidatePath" /> is the same directory as
        ///     <paramref name="basePath" /> or lies inside it, by comparing the path text only.
        /// </summary>
        /// <param name="basePath">
        ///     The base directory path. Relative paths resolve against the current directory.
        /// </param>
        /// <param name="candidatePath">
        ///     The directory path to test. Relative paths resolve against the current directory.
        /// </param>
        /// <returns>
        ///     True if <paramref name="candidatePath" /> equals <paramref name="basePath" /> or is
        ///     located under it; otherwise false.
        /// </returns>
        internal static bool IsSameOrDescendant(string basePath, string candidatePath)
        {
            var normalizedBase = NormalizeDirectoryPath(basePath);
            var normalizedCandidate = NormalizeDirectoryPath(candidatePath);

            return normalizedCandidate.StartsWith(normalizedBase, PathComparison);
        }

        /// <summary>
        ///     Normalizes a directory path for prefix comparison.
        /// </summary>
        /// <remarks>
        ///     Ending every path with exactly one separator makes the prefix test match whole path
        ///     segments only, so <c>C:\a\</c> is not a prefix of <c>C:\ab\</c>.
        /// </remarks>
        /// <param name="path">The directory path.</param>
        /// <returns>
        ///     The full path with alternate separators mapped to <see cref="Path.DirectorySeparatorChar" />
        ///     and exactly one trailing separator.
        /// </returns>
        private static string NormalizeDirectoryPath(string path)
        {
            var fullPath = Path.GetFullPath(path)
                .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            return fullPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        }
    }
}