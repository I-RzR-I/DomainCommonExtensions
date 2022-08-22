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

using System.IO;

#endregion

namespace DomainCommonExtensions.CommonExtensions
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

                return canWrite != true;
            }
            catch (IOException)
            {
                return true;
            }
        }
    }
}