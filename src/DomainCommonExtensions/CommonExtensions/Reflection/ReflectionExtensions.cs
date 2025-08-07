// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="ReflectionExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Reflection;
using DomainCommonExtensions.Utilities.Ensure;

#endregion

namespace DomainCommonExtensions.CommonExtensions.Reflection
{
    /// <summary>
    ///     Reflection extensions
    /// </summary>
    /// <remarks></remarks>
    public static class ReflectionExtensions
    {
        /// <summary>
        ///     Copy the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyProperties(this object source, object destination)
        {
            DomainEnsure.IsNotNullAll("Source or/and Destination Objects are null", source, destination);

            var typeDest = destination.GetType();
            var typeSrc = source.GetType();

            //Iterate the Properties of the source instance and populate them from their destination counterparts
            var srcProps = typeSrc.GetProperties();
            foreach (var srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                    continue;
                var targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty.IsNull())
                    continue;
                if (!targetProperty!.CanWrite)
                    continue;
                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                    continue;
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                    continue;
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                    continue;

                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }
    }
}