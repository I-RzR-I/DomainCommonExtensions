// ***********************************************************************
//  Assembly          : RzR.Shared.Extensions.DomainCommonExtensions
//  Author            : RzR
//  Created On        : 2026-10-08 22:10
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-10-14 12:26
//  ***********************************************************************
//  <copyright file="PathExtensions.cs" company="RzR SOFT & TECH">
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
using System.Security;
using RzR.Extensions.Domain.Internal;
using RzR.Extensions.Domain.Validation;

#endregion

namespace RzR.Extensions.Domain.IO
{
    /// <summary>
    ///     Path extensions.
    /// </summary>
    public static class PathExtensions
    {
        /// <summary>
        ///     Combines <paramref name="basePath" /> with <paramref name="relativePath" /> and returns
        ///     the full path, provided that the result stays inside <paramref name="basePath" />. 
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when <paramref name="basePath" /> is empty or white space; when
        ///     <paramref name="relativePath" /> is rooted; when the combined path resolves outside
        ///     <paramref name="basePath" />; or when either path contains characters that are not valid
        ///     in a path.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="basePath" /> or <paramref name="relativePath" /> is null.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     Thrown on .NET Framework when a path contains a colon that is not part of a volume
        ///     identifier.
        /// </exception>
        /// <exception cref="PathTooLongException">
        ///     Thrown when the full path exceeds the system-defined maximum length.
        /// </exception>
        /// <exception cref="SecurityException">
        ///     Thrown on .NET Framework when the caller lacks the required permissions.
        /// </exception>
        /// <param name="basePath">
        ///     The base directory. A relative base path resolves against the current directory.
        /// </param>
        /// <param name="relativePath">
        ///     The path to combine, relative to <paramref name="basePath" />. It may contain <c>..</c>
        ///     segments as long as the result stays inside <paramref name="basePath" />.
        /// </param>
        /// <returns>
        ///     The full, normalized path. An empty <paramref name="relativePath" />, or one that
        ///     resolves to
        ///     <paramref name="basePath" /> itself, returns the full path of <paramref name="basePath" />.
        /// </returns>
        public static string CombineWithinBase(this string basePath, string relativePath)
        {
            DomainEnsure.IsNotNull(basePath, nameof(basePath));
            DomainEnsure.IsNotNull(relativePath, nameof(relativePath));

            if (string.IsNullOrWhiteSpace(basePath))
                throw new ArgumentException("The base path must not be empty or white space.", nameof(basePath));

            if (Path.IsPathRooted(relativePath))
                throw new ArgumentException("The path must be relative to the base directory.", nameof(relativePath));

            var combinedPath = CombineIfContained(basePath, relativePath);
            if (combinedPath == null)
                throw new ArgumentException("The path resolves outside the base directory.", nameof(relativePath));

            return combinedPath;
        }

        /// <summary>
        ///     Tries to combine <paramref name="basePath" /> with <paramref name="relativePath" /> into
        ///     a full path that stays inside <paramref name="basePath" />.
        /// </summary>
        /// <remarks>
        ///     It never throws: every case in which <see cref="CombineWithinBase" /> throws returns
        ///     false here. The same rules and limits apply: rooted <paramref name="relativePath" />
        ///     values are rejected, and the check is lexical only, so symbolic links and junctions are
        ///     not resolved.
        /// </remarks>
        /// <param name="basePath">
        ///     The base directory. A relative base path resolves against the current directory.
        /// </param>
        /// <param name="relativePath">
        ///     The path to combine, relative to <paramref name="basePath" />. It may contain <c>..</c>
        ///     segments as long as the result stays inside <paramref name="basePath" />.
        /// </param>
        /// <param name="combinedPath">
        ///     [out] When this method returns true, the full, normalized path; otherwise null.
        /// </param>
        /// <returns>
        ///     True when the paths are valid and the combined path stays inside <paramref name="basePath" />
        ///     ;
        ///     otherwise false.
        /// </returns>
        public static bool TryCombineWithinBase(this string basePath, string relativePath, out string combinedPath)
        {
            combinedPath = null;

            if (string.IsNullOrWhiteSpace(basePath) || relativePath == null)
                return false;

            try
            {
                if (Path.IsPathRooted(relativePath))
                    return false;

                combinedPath = CombineIfContained(basePath, relativePath);

                return combinedPath != null;
            }
            catch (Exception exception) when (IsInvalidPathException(exception))
            {
                return false;
            }
        }

        /// <summary>
        ///     Combines two paths and keeps the result only when it stays inside the base directory.
        /// </summary>
        /// <remarks>
        ///     <see cref="Path.GetFullPath(string)" /> leaves <c>\\?\</c> and <c>\??\</c> paths
        ///     unnormalized, so a remaining dot segment would let the prefix check pass for a path that
        ///     leaves the base.
        /// </remarks>
        /// <param name="basePath">The base directory.</param>
        /// <param name="relativePath">A path that is not rooted.</param>
        /// <returns>
        ///     The full combined path, or null when it resolves outside <paramref name="basePath" /> or
        ///     still contains a <c>.</c> or <c>..</c> segment.
        /// </returns>
        private static string CombineIfContained(string basePath, string relativePath)
        {
            var combinedPath = Path.GetFullPath(Path.Combine(basePath, relativePath));
            if (HasDotSegment(combinedPath))
                return null;

            return LexicalPathHelper.IsSameOrDescendant(basePath, combinedPath) ? combinedPath : null;
        }

        /// <summary>
        ///     Tells whether a path contains a <c>.</c> or <c>..</c> segment.
        /// </summary>
        /// <param name="path">The path to inspect.</param>
        /// <returns>
        ///     True when any segment is <c>.</c> or <c>..</c>; otherwise false.
        /// </returns>
        private static bool HasDotSegment(string path)
        {
            var segments = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            return Array.Exists(segments, segment => segment == "." || segment == "..");
        }

        /// <summary>
        ///     Tells whether an exception reports a path that is malformed, too long or not permitted.
        /// </summary>
        /// <param name="exception">The exception thrown by a <see cref="Path" /> method.</param>
        /// <returns>
        ///     True for the exceptions that <see cref="Path.GetFullPath(string)" /> documents; otherwise
        ///     false.
        /// </returns>
        private static bool IsInvalidPathException(Exception exception)
        {
            return exception is ArgumentException
                   || exception is NotSupportedException
                   || exception is PathTooLongException
                   || exception is SecurityException;
        }
    }
}