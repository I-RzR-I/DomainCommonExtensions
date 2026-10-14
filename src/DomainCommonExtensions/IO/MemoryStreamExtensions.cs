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

namespace RzR.Extensions.Domain.IO
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
        /// <param name="inStream">Source stream; it is not closed or disposed.</param>
        /// <returns>A new byte array.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when a non-<see cref="MemoryStream"/> stream does not support reading.</exception>
        /// <remarks>
        ///     The two stream kinds behave differently:
        ///     a <see cref="MemoryStream"/> returns its whole content (<see cref="MemoryStream.ToArray"/>) regardless of
        ///     <see cref="Stream.Position"/>, and its position is left unchanged;
        ///     any other stream is read from its current <see cref="Stream.Position"/> to the end, and is left positioned at the end.
        /// </remarks>
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
