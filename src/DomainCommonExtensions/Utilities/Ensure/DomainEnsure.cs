// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-08-05 18:06
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-08-05 20:09
// ***********************************************************************
//  <copyright file="DomainEnsure.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Resources.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

namespace DomainCommonExtensions.Utilities.Ensure
{
    /// <summary>
    ///     A domain ensures constraints.
    /// </summary>
    public static class DomainEnsure
    {
        /// <summary>
        ///     (Immutable) message describing the not null.
        /// </summary>
        private const string NotNullMessage = "Cannot be null!";

        /// <summary>
        ///     (Immutable) message describing the not null or empty value.
        /// </summary>
        private const string NotNullOrEmptyValueMessage = "Value can not be null or empty!";

        /// <summary>
        ///     (Immutable) type of the value invalid for enum.
        /// </summary>
        private const string ValueInvalidForEnumType = "The value of argument '{0}' ({1}) is invalid for Enum type '{2}'!";

        /// <summary>
        ///     Is not null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="source">Source object to be validated.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public static void IsNotNull(object source, string parameterName)
        {
            if (source.IsNull())
                InternalThrowException(ExceptionType.ArgumentNullException, null, parameterName, null);
        }

        /// <summary>
        ///     Is not null all.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="message">The error message.</param>
        /// <param name="sourceParameters">
        ///     A variable-length parameters list containing source parameters.
        /// </param>
        public static void IsNotNullAll(string message, params object[] sourceParameters)
        {
            // ReSharper disable once UseArrayEmptyMethod
            var anyNull = (sourceParameters ?? new object[0]).Any(x => x.IsNull());
            if (anyNull.IsTrue())
                InternalThrowException(ExceptionType.ArgumentNullException, null, null, message.IfIsNull(NotNullMessage));
        }

        /// <summary>
        ///     Is not null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="source">Source object to be validated.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The error message.</param>
        public static void IsNotNull(object source, string parameterName, string message)
        {
            if (source.IsNull())
                InternalThrowException(ExceptionType.ArgumentNullException, null, parameterName,
                    message.IfIsNull(NotNullMessage));
        }

        /// <summary>
        ///     Is not null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="source">Source object to be validated.</param>
        /// <param name="message">The error message.</param>
        /// <param name="exception">The exception.</param>
        public static void IsNotNull(object source, string message, Exception exception)
        {
            if (source.IsNull())
                InternalThrowException(ExceptionType.ArgumentNullException, null, null,
                    message.IfIsNull(NotNullMessage), exception);
        }

        /// <summary>
        ///     Is not null or empty.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="source">Source object to be validated.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">(Optional) The error message.</param>
        public static void IsNotNullOrEmpty(string source, string parameterName, string message = null)
        {
            IsNotNull(source, parameterName);

            if (source.IsMissing())
                InternalThrowException(ExceptionType.ArgumentException, null, parameterName,
                    message.IfIsNull(NotNullOrEmptyValueMessage));
        }

        /// <summary>
        ///     Is not null or empty.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="source">Source object to be validated.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">(Optional) The error message.</param>
        public static void IsNotNullOrEmptyArgNull(string source, string parameterName, string message = null)
        {
            IsNotNull(source, parameterName);

            if (source.IsMissing())
                InternalThrowException(ExceptionType.ArgumentNullException, null, parameterName,
                    message.IfIsNull(NotNullOrEmptyValueMessage));
        }

        /// <summary>
        ///     Is valid enum.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <typeparam name="TEnum">Type of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public static void IsValidEnum<TEnum>(TEnum value, string parameterName)
            where TEnum : struct
        {
            if (Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Contains(value).IsFalse())
            {
                InternalThrowException(ExceptionType.ArgumentException, value, parameterName,
                    ValueInvalidForEnumType.FormatWith(parameterName, value, typeof(TEnum).Name));
            }
        }

        /// <summary>
        ///     Throw argument exception if function result is false.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <param name="func">The function to execute.</param>
        /// <param name="message">The error message.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public static void ThrowArgumentExceptionIfFuncIsFalse(Func<bool> func, string message, string parameterName)
        {
            var result = func.Invoke();
            if (result.IsFalse())
                InternalThrowException(ExceptionType.ArgumentException, null, parameterName, message);
        }

        /// <summary>
        ///     Throw argument exception if function result is false.
        /// </summary>
        /// <param name="exceptionType">The type of exception.</param>
        /// <param name="func">The function to execute.</param>
        /// <param name="message">The error message.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public static void ThrowExceptionIfFuncIsFalse(ExceptionType exceptionType, Func<bool> func,
            string message, string parameterName)
        {
            var result = func.Invoke();
            if (result.IsFalse())
                InternalThrowException(exceptionType, null, parameterName, message);
        }

        /// <summary>
        ///     Throw argument exception if function result is true.
        /// </summary>
        /// <param name="exceptionType">The type of exception.</param>
        /// <param name="func">The function to execute.</param>
        /// <param name="message">The error message.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public static void ThrowExceptionIfFuncIsTrue(ExceptionType exceptionType, Func<bool> func, 
            string message, string parameterName)
        {
            var result = func.Invoke();
            if (result.IsTrue())
                InternalThrowException(exceptionType, null, parameterName, message);
        }

        /// <summary>
        ///     Throw exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="exceptionType">(Optional) Type of the exception.</param>
        /// <param name="innerException">(Optional) The inner exception.</param>
        public static void ThrowException(string message, ExceptionType exceptionType = ExceptionType.Exception,
            Exception innerException = null)
        {
            InternalThrowException(exceptionType, null, null, message, innerException);
        }

        /// <summary>
        ///     Internal throw exception.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <exception cref="ArgumentException">
        ///     Thrown when one or more arguments have unsupported or illegal values.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when one or more arguments are outside the required range.
        /// </exception>
        /// <exception cref="DivideByZeroException">
        ///     Thrown when an attempt is made to divide a number by zero.
        /// </exception>
        /// <exception cref="FileNotFoundException">
        ///     Thrown when the requested file is not present.
        /// </exception>
        /// <exception cref="FormatException">
        ///     Thrown when the format of an input is incorrect.
        /// </exception>
        /// <exception cref="IndexOutOfRangeException">
        ///     Thrown when the index is outside the required range.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown when the requested operation is invalid.
        /// </exception>
        /// <exception cref="KeyNotFoundException">
        ///     Thrown when a Key Not Found error condition occurs.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     Thrown when the requested operation is not supported.
        /// </exception>
        /// <exception cref="NullReferenceException">
        ///     Thrown when a value was unexpectedly null.
        /// </exception>
        /// <exception cref="OverflowException">Thrown when an arithmetic overflow occurs.</exception>
        /// <exception cref="OutOfMemoryException">Thrown when a low memory situation occurs.</exception>
        /// <exception cref="StackOverflowException">Thrown when a stack overflow occurs.</exception>
        /// <exception cref="TimeoutException">Thrown when a Timeout error condition occurs.</exception>
        /// <exception cref="InvalidCastException">
        ///     Thrown when an object cannot be cast to a required type.
        /// </exception>
        /// <exception cref="MissingMemberException">
        ///     Thrown when a Missing Member error condition occurs.
        /// </exception>
        /// <param name="exceptionType">Type of the exception.</param>
        /// <param name="sourceObject">Source object.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="errorMessage">Message describing the error.</param>
        /// <param name="innerException">(Optional) The inner exception.</param>
        internal static void InternalThrowException(ExceptionType exceptionType,
            object sourceObject, string parameterName, string errorMessage, Exception innerException = null)
        {
            switch (exceptionType)
            {
                case ExceptionType.Exception:
                    throw new Exception(errorMessage.IfIsNull(parameterName), innerException);
                case ExceptionType.ArgumentException:
                    if (parameterName.IsPresent() && errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new ArgumentException(errorMessage, parameterName, innerException);
                    else if (parameterName.IsPresent() && errorMessage.IsPresent())
                        throw new ArgumentException(errorMessage, parameterName);
                    else if (parameterName.IsMissing() && innerException.IsNotNull() && errorMessage.IsPresent())
                        throw new ArgumentException(errorMessage, innerException);
                    else if (errorMessage.IsPresent())
                        throw new ArgumentException(errorMessage);
                    else throw new ArgumentException();
                case ExceptionType.ArgumentNullException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new ArgumentNullException(errorMessage, innerException);
                    else if (parameterName.IsPresent() && errorMessage.IsPresent() && innerException.IsNull())
                        throw new ArgumentNullException(parameterName, errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new ArgumentNullException(parameterName);
                    else throw new ArgumentNullException();
                case ExceptionType.ArgumentOutOfRangeException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new ArgumentOutOfRangeException(errorMessage, innerException);
                    else if (parameterName.IsPresent() && errorMessage.IsPresent() && innerException.IsNull())
                        throw new ArgumentOutOfRangeException(parameterName, errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsPresent() && sourceObject.IsNotNull())
                        throw new ArgumentOutOfRangeException(parameterName, sourceObject, errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new ArgumentOutOfRangeException(parameterName);
                    else throw new ArgumentOutOfRangeException();
                case ExceptionType.DivideByZeroException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new DivideByZeroException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new DivideByZeroException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new DivideByZeroException(parameterName);
                    else throw new DivideByZeroException();
                case ExceptionType.FileNotFoundException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new FileNotFoundException(errorMessage, innerException);
                    else if (parameterName.IsPresent() && errorMessage.IsPresent() && innerException.IsNull())
                        throw new FileNotFoundException(errorMessage, parameterName);
                    else if (errorMessage.IsPresent() && parameterName.IsMissing() && innerException.IsNull())
                        throw new FileNotFoundException(errorMessage);
                    else throw new FileNotFoundException();
                case ExceptionType.FormatException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new FormatException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new FormatException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new FormatException(parameterName);
                    else throw new FormatException();
                case ExceptionType.IndexOutOfRangeException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new IndexOutOfRangeException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new IndexOutOfRangeException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new IndexOutOfRangeException(parameterName);
                    else throw new IndexOutOfRangeException();
                case ExceptionType.InvalidOperationException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new InvalidOperationException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new InvalidOperationException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new InvalidOperationException(parameterName);
                    else throw new InvalidOperationException();
                case ExceptionType.KeyNotFoundException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new KeyNotFoundException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new KeyNotFoundException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new KeyNotFoundException(parameterName);
                    else throw new KeyNotFoundException();
                case ExceptionType.NotSupportedException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new NotSupportedException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new NotSupportedException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new NotSupportedException(parameterName);
                    else throw new NotSupportedException();
                case ExceptionType.NullReferenceException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new NullReferenceException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new NullReferenceException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new NullReferenceException(parameterName);
                    else throw new NullReferenceException();
                case ExceptionType.OverflowException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new OverflowException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new OverflowException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new OverflowException(parameterName);
                    else throw new OverflowException();
                case ExceptionType.OutOfMemoryException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new OutOfMemoryException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new OutOfMemoryException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new OutOfMemoryException(parameterName);
                    else throw new OutOfMemoryException();
                case ExceptionType.StackOverflowException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new StackOverflowException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new StackOverflowException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new StackOverflowException(parameterName);
                    else throw new StackOverflowException();
                case ExceptionType.TimeoutException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new TimeoutException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new TimeoutException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new TimeoutException(parameterName);
                    else throw new TimeoutException();
                case ExceptionType.InvalidCastException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new InvalidCastException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new InvalidCastException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsMissing() && innerException.IsNull())
                        throw new InvalidCastException(parameterName);
                    else throw new InvalidCastException();
                case ExceptionType.MissingMemberException:
                    if (errorMessage.IsPresent() && innerException.IsNotNull())
                        throw new MissingMemberException(errorMessage, innerException);
                    else if (errorMessage.IsPresent() && innerException.IsNull())
                        throw new MissingMemberException(errorMessage);
                    else if (parameterName.IsPresent() && errorMessage.IsPresent() && innerException.IsNull())
                        throw new MissingMemberException(errorMessage, parameterName);
                    else throw new MissingMemberException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(exceptionType), exceptionType, null);
            }
        }
    }
}