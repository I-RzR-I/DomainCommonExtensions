// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="SocketExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace DomainCommonExtensions.DataTypeExtensions
{
    /// <summary>
    ///     Socket (TCP) extensions
    /// </summary>
    /// <remarks></remarks>
    public static class SocketExtensions
    {
        /// <summary>
        ///     Using IOControl code to configure socket KeepAliveValues for line disconnection detection(because default is toooo
        ///     slow)
        /// </summary>
        /// <param name="tcpClient">TcpClient</param>
        /// <param name="keepAliveTime">The keep alive time. (ms)</param>
        /// <param name="keepAliveInterval">The keep alive interval. (ms)</param>
        public static void SetSocketKeepAliveValues(this TcpClient tcpClient, int keepAliveTime, int keepAliveInterval)
        {
            if (tcpClient.IsNull())
                throw new ArgumentNullException(nameof(tcpClient));

            //KeepAliveTime: default value is 2hr
            //KeepAliveInterval: default value is 1s and Detect 5 times

            uint dummy = 0; //lenth = 4
            var inOptionValues = new byte[Marshal.SizeOf(dummy) * 3]; //size = lenth * 3 = 12

            BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes((uint)keepAliveTime).CopyTo(inOptionValues, Marshal.SizeOf(dummy));
            BitConverter.GetBytes((uint)keepAliveInterval).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);
            tcpClient.Client.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
        }
    }
}