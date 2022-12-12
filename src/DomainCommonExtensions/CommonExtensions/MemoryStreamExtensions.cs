// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="MemoryStreamExtensions.cs" company="">
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
    ///     MemoryStream extensions
    /// </summary>
    /// <remarks></remarks>
    public static class MemoryStreamExtensions
    {
        /// <summary>
        ///     Read all bytes from stream
        /// </summary>
        /// <param name="inStream"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static byte[] ReadAllBytes(this Stream inStream)
        {
            if (inStream is MemoryStream stream)
                return stream.ToArray();

            using var memoryStream = new MemoryStream();
            inStream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        /// <summary>
        ///     Write all bytes
        /// </summary>
        /// <param name="stream">Source stream</param>
        /// <param name="bytes">Bytes to write</param>
        /// <remarks></remarks>
        public static void WriteAll(this Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}