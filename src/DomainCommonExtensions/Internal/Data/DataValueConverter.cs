// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2026-10-08 10:00
//
//  Last Modified By : RzR
//  Last Modified On : 2026-10-08 10:00
// ***********************************************************************
//  <copyright file="DataValueConverter.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
//
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Data;
using System.Globalization;

#endregion

namespace RzR.Extensions.Domain.Internal.Data
{
    /// <summary>
    ///     Reads a column value once and converts it to a CLR type for the DataReader <c>To*</c>
    ///     extensions.
    /// </summary>
    internal static class DataValueConverter
    {
        private static readonly string[] IsoDateTimeFormats =
        {
            "o",
            "yyyy-MM-dd",
            "yyyy-MM-dd HH:mm:ss.FFFFFFF",
            "yyyy-MM-ddTHH:mm:ss.FFFFFFFK"
        };

        /// <summary>
        ///     Reads the column value with a single lookup and reports whether it holds a value.
        /// </summary>
        /// <remarks>
        ///     DBNull, null, an empty or whitespace-only string and a whitespace char mean "no value". A
        ///     missing column throws the provider's exception unchanged.
        /// </remarks>
        /// <param name="record">The data record to read from.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="value">[out] The raw column value.</param>
        /// <returns>
        ///     <see langword="true"/> when the column holds a value; otherwise <see langword="false"/>.
        /// </returns>
        internal static bool TryRead(IDataRecord record, string columnName, out object value)
        {
            value = record[columnName];

            return !IsNoValue(value);
        }

        /// <summary>
        ///     Converts a column value to <see cref="bool"/>.
        /// </summary>
        /// <remarks>
        ///     Numbers map 0 to <see langword="false"/> and any other value to <see langword="true"/>.
        ///     The strings "1", "0", "true" and "false" are accepted (case-insensitive, trimmed).
        /// 
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static bool ToBoolean(object value, string columnName)
        {
            if (value is bool typed)
                return typed;

            if (IsNumber(value) && !IsNaN(value))
                return Convert.ToBoolean(value, CultureInfo.InvariantCulture);

            var text = AsText(value).Trim();
            if (text == "1")
                return true;

            if (text == "0")
                return false;
            
            if (bool.TryParse(text, out var result))
                return result;

            throw CreateFormatException(value, columnName, typeof(bool), null);
        }

        /// <summary>
        ///     Converts a column value to <see cref="byte"/>.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static byte ToByte(object value, string columnName)
            => ToIntegral<byte>(value, columnName);

        /// <summary>
        ///     Converts a column value to <see cref="sbyte"/>.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static sbyte ToSByte(object value, string columnName)
            => ToIntegral<sbyte>(value, columnName);

        /// <summary>
        ///     Converts a column value to <see cref="short"/>.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static short ToInt16(object value, string columnName)
            => ToIntegral<short>(value, columnName);

        /// <summary>
        ///     Converts a column value to <see cref="ushort"/>.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static ushort ToUInt16(object value, string columnName)
            => ToIntegral<ushort>(value, columnName);

        /// <summary>
        ///     Converts a column value to <see cref="int"/>.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static int ToInt32(object value, string columnName)
            => ToIntegral<int>(value, columnName);

        /// <summary>
        ///     Converts a column value to <see cref="uint"/>.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static uint ToUInt32(object value, string columnName)
            => ToIntegral<uint>(value, columnName);

        /// <summary>
        ///     Converts a column value to <see cref="long"/>.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static long ToInt64(object value, string columnName)
            => ToIntegral<long>(value, columnName);

        /// <summary>
        ///     Converts a column value to <see cref="ulong"/>.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static ulong ToUInt64(object value, string columnName)
            => ToIntegral<ulong>(value, columnName);

        /// <summary>
        ///     Converts a column value to <see cref="decimal"/>.
        /// </summary>
        /// <remarks>
        ///     Strings are parsed with <see cref="NumberStyles.Float"/> and the invariant culture first,
        ///     then with the current culture and <see cref="NumberStyles.Number"/>.
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <exception cref="CreateOverflowException">
        ///     Thrown when a Create Overflow error condition occurs.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static decimal ToDecimal(object value, string columnName)
        {
            if (value is decimal typed)
                return typed;

            try
            {
                if (value is double || value is float)
                    return decimal.Parse(((IFormattable)value).ToString(null, CultureInfo.InvariantCulture),
                        NumberStyles.Float, CultureInfo.InvariantCulture);
                
                if (IsNumber(value))
                    return Convert.ToDecimal(value, CultureInfo.InvariantCulture);

                var text = AsText(value);

                return decimal.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
                    ? result
                    : decimal.Parse(text, NumberStyles.Number, CultureInfo.CurrentCulture);
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(value, columnName, typeof(decimal), exception);
            }
            catch (OverflowException exception)
            {
                throw CreateOverflowException(value, columnName, typeof(decimal), exception);
            }
        }

        /// <summary>
        ///     Converts a column value to <see cref="double"/>.
        /// </summary>
        /// <remarks>
        ///     Strings are parsed with <see cref="NumberStyles.Float"/> and the invariant culture first,
        ///     then with the current culture and <see cref="NumberStyles.Float"/> plus
        ///     <see cref="NumberStyles.AllowThousands"/>.
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <exception cref="CreateOverflowException">
        ///     Thrown when a Create Overflow error condition occurs.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static double ToDouble(object value, string columnName)
        {
            if (value is double typed)
                return typed;

            try
            {
                // A widened float shows its binary error (0.1f becomes 0.100000001490116), so the
                // shortest round-trip text of the float is parsed instead, which keeps the value seen today.
                if (value is float single)
                    return double.Parse(single.ToString("R", CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
                if (IsNumber(value))
                    return Convert.ToDouble(value, CultureInfo.InvariantCulture);

                var text = AsText(value);

                return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
                    ? result
                    : double.Parse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture);
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(value, columnName, typeof(double), exception);
            }
            catch (OverflowException exception)
            {
                throw CreateOverflowException(value, columnName, typeof(double), exception);
            }
        }

        /// <summary>
        ///     Converts a column value to <see cref="float"/>.
        /// </summary>
        /// <remarks>
        ///     A typed number outside the <see cref="float"/> range becomes infinity. Strings are parsed
        ///     like <see cref="ToDouble"/>.
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <exception cref="CreateOverflowException">
        ///     Thrown when a Create Overflow error condition occurs.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static float ToSingle(object value, string columnName)
        {
            if (value is float typed)
                return typed;

            try
            {
                if (IsNumber(value))
                    return Convert.ToSingle(value, CultureInfo.InvariantCulture);

                var text = AsText(value);

                return float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
                    ? result
                    : float.Parse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.CurrentCulture);
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(value, columnName, typeof(float), exception);
            }
            catch (OverflowException exception)
            {
                throw CreateOverflowException(value, columnName, typeof(float), exception);
            }
        }

        /// <summary>
        ///     Converts a column value to <see cref="char"/>.
        /// </summary>
        /// <remarks>
        ///     The value text must be exactly one character long; it is not trimmed.
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static char ToChar(object value, string columnName)
        {
            if (value is char typed)
                return typed;

            var text = AsText(value);
            if (text.Length == 1)
                return text[0];

            throw CreateFormatException(value, columnName, typeof(char), null);
        }

        /// <summary>
        ///     Converts a column value to <see cref="DateTime"/>.
        /// </summary>
        /// <remarks>
        ///     A <see cref="DateTime"/> is returned as is (milliseconds and kind kept) and a
        ///     <see cref="DateTimeOffset"/> as its <see cref="DateTimeOffset.LocalDateTime"/>. Strings
        ///     are
        ///     parsed with the ISO 8601 formats and the invariant culture first, then with the current
        ///     culture;
        ///     a string with a zone designator (Z or an offset) is converted to local time, as before.
        /// 
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static DateTime ToDateTime(object value, string columnName)
        {
            if (TryGetDateTime(value, out var typed))
                return typed;

            var text = AsText(value);
            if (DateTime.TryParseExact(text, IsoDateTimeFormats, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var result))
                return result;

            try
            {
                return DateTime.Parse(text, CultureInfo.CurrentCulture);
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(value, columnName, typeof(DateTime), exception);
            }
        }

        /// <summary>
        ///     Converts a column value to <see cref="DateTime"/> using an exact format for strings.
        /// </summary>
        /// <remarks>
        ///     A <see cref="DateTime"/> or <see cref="DateTimeOffset"/> value is returned like
        ///     <see cref="ToDateTime"/> does and the format is not used.
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <param name="format">The exact format, parsed with the invariant culture.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static DateTime ToDateTimeExact(object value, string columnName, string format)
        {
            if (TryGetDateTime(value, out var typed))
                return typed;

            try
            {
                return DateTime.ParseExact(AsText(value), format, CultureInfo.InvariantCulture);
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(value, columnName, typeof(DateTime), exception);
            }
        }

        /// <summary>
        ///     Converts a column value to <see cref="Guid"/>.
        /// </summary>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static Guid ToGuid(object value, string columnName)
        {
            if (value is Guid typed)
                return typed;

            try
            {
                return Guid.Parse(AsText(value));
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(value, columnName, typeof(Guid), exception);
            }
        }

        /// <summary>
        ///     Converts a column value to the given type, for the DataTable <c>ToList</c> extensions.
        /// </summary>
        /// <param name="value">The column value; must not be null or DBNull.</param>
        /// <param name="type">The target type.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static object ChangeType(object value, Type type, string columnName)
        {
            var targetType = Nullable.GetUnderlyingType(type) ?? type;
            
            if (targetType.IsInstanceOfType(value))
                return value;
            
            if (targetType.IsEnum)
                return ToEnum(value, targetType, columnName);
            
            if (targetType == typeof(Guid))
                return ToGuid(value, columnName);
           
            if (!(value is string))
                return Convert.ChangeType(value, targetType, CultureInfo.CurrentCulture);

            return ConvertByTypeCode(value, targetType, columnName, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Determines whether a column value counts as absent for <see cref="ConvertTo"/>.
        /// </summary>
        /// <remarks>
        ///     DBNull and null are always absent. A value the target type can hold as is (for example
        ///     any string for <see cref="string"/> or <see cref="object"/>) is present, and so are a non-
        ///     empty string for <see cref="char"/> and any char for <see cref="string"/>. Every other
        ///     value follows the "no value" rule of <see cref="TryRead"/>.
        /// </remarks>
        /// <param name="value">The raw column value.</param>
        /// <param name="type">
        ///     The target type; a nullable type is checked as its underlying type.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> when the value means "no value" for the target type; otherwise
        ///     <see langword="false"/>.
        /// </returns>
        internal static bool IsAbsent(object value, Type type)
        {
            if (value is null || value is DBNull)
                return true;

            var targetType = Nullable.GetUnderlyingType(type) ?? type;
           
            if (targetType.IsInstanceOfType(value))
                return false;
           
            if (targetType == typeof(char) && value is string text && text.Length != 0)
                return false;
           
            if (targetType == typeof(string) && value is char)
                return false;

            return IsNoValue(value);
        }

        /// <summary>
        ///     Converts a column value to the given type with invariant-culture rules, for
        ///     <c>GetValueOrDefault</c>.
        /// </summary>
        /// <param name="value">The column value; must not be null or DBNull.</param>
        /// <param name="type">The target type.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static object ConvertTo(object value, Type type, string columnName)
        {
            var targetType = Nullable.GetUnderlyingType(type) ?? type;
            
            if (targetType.IsInstanceOfType(value))
                return value;
           
            if (targetType.IsEnum)
                return ToEnumValue(value, targetType, columnName);
            
            if (targetType == typeof(string))
                return ToText(value, columnName);
           
            if (targetType == typeof(Guid))
                return ToGuid(value, columnName);
            
            if (targetType == typeof(TimeSpan))
                return ToTimeSpan(value, columnName);
           
            if (targetType == typeof(DateTimeOffset))
                return ToDateTimeOffset(value, columnName);
            
            if (targetType == typeof(byte[]))
                return ToBytes(value, columnName);

            return ConvertByTypeCode(value, targetType, columnName, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Converts a column value to its text.
        /// </summary>
        /// <remarks>
        ///     A string is returned as is. <see cref="DateTime"/> and <see cref="DateTimeOffset"/> use
        ///     the round-trip format "o", and <see cref="double"/> and <see cref="float"/> the round-
        ///     trip format "R", so every target framework gives the same text; other formattable values
        ///     use the invariant culture.
        /// </remarks>
        /// <exception cref="CreateInvalidCastException">
        ///     Thrown when a Create Invalid Cast error condition occurs.
        /// </exception>
        /// <exception cref="InvalidCastException">The value is a byte array.</exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static string ToText(object value, string columnName)
        {
            switch (value)
            {
                case string text:
                    return text;
                case DateTime dateTime:
                    return dateTime.ToString("o", CultureInfo.InvariantCulture);
                case DateTimeOffset dateTimeOffset:
                    return dateTimeOffset.ToString("o", CultureInfo.InvariantCulture);
                case byte[] _:
                    throw CreateInvalidCastException(value, columnName, typeof(string));
                case double number:
                    return number.ToString("R", CultureInfo.InvariantCulture);
                case float number:
                    return number.ToString("R", CultureInfo.InvariantCulture);
                case IFormattable formattable:
                    return formattable.ToString(null, CultureInfo.InvariantCulture);
                default:
                    return value.ToString();
            }
        }

        /// <summary>
        ///     Converts a column value to a byte array.
        /// </summary>
        /// <exception cref="InvalidCastException">The value is not a byte array.</exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The byte array itself.
        /// </returns>
        internal static byte[] ToBytes(object value, string columnName)
            => value as byte[] ?? throw CreateInvalidCastException(value, columnName, typeof(byte[]));

        /// <summary>
        ///     Converts a column value to <see cref="TimeSpan"/>.
        /// </summary>
        /// <remarks>
        ///     Strings are parsed with the constant format "c" and the invariant culture first, then with
        ///     <see cref="TimeSpan.Parse(string, IFormatProvider)"/> and the invariant culture. Numbers
        ///     are
        ///     rejected because ticks and seconds cannot be told apart.
        /// </remarks>
        /// <exception cref="CreateInvalidCastException">
        ///     Thrown when a Create Invalid Cast error condition occurs.
        /// </exception>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <exception cref="CreateOverflowException">
        ///     Thrown when a Create Overflow error condition occurs.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     The value is neither a <see cref="TimeSpan"/> nor a string.
        /// </exception>
        /// <exception cref="FormatException">The string is not a valid time interval.</exception>
        /// <exception cref="OverflowException">
        ///     The string is outside the <see cref="TimeSpan"/> range.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static TimeSpan ToTimeSpan(object value, string columnName)
        {
            if (value is TimeSpan typed)
                return typed;
           
            if (!(value is string text))
                throw CreateInvalidCastException(value, columnName, typeof(TimeSpan));
            
            if (TimeSpan.TryParseExact(text, "c", CultureInfo.InvariantCulture, out var result))
                return result;

            try
            {
                return TimeSpan.Parse(text, CultureInfo.InvariantCulture);
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(value, columnName, typeof(TimeSpan), exception);
            }
            catch (OverflowException exception)
            {
                throw CreateOverflowException(value, columnName, typeof(TimeSpan), exception);
            }
        }

        /// <summary>
        ///     Converts a column value to <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <remarks>
        ///     A <see cref="DateTime"/> of kind <see cref="DateTimeKind.Local"/> gets the local offset; a
        ///     <see cref="DateTimeKind.Utc"/> or <see cref="DateTimeKind.Unspecified"/> one gets +00:00,
        ///     so the
        ///     result does not depend on the host time zone. Strings are parsed with the round-trip
        ///     format "o" first, then with the invariant culture; a string without an offset is read as
        ///     UTC.
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <exception cref="FormatException">
        ///     The value is not a date and time or a valid date and time string.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     A local date and time is outside the range in UTC.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        internal static DateTimeOffset ToDateTimeOffset(object value, string columnName)
        {
            switch (value)
            {
                case DateTimeOffset typed:
                    return typed;
                case DateTime dateTime:
                    return FromDateTime(dateTime, columnName);
                case string text:
                    return ParseDateTimeOffset(text, columnName);
                default:
                    throw CreateFormatException(value, columnName, typeof(DateTimeOffset), null);
            }
        }

        /// <summary>
        ///     Converts a column value to an enum type.
        /// </summary>
        /// <remarks>
        ///     A string is parsed by name (case-insensitive, trimmed) or as a number. A typed number
        ///     must be integral-valued and in the range of the enum's underlying type. Undefined values
        ///     are allowed, as with a cast.
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <exception cref="CreateOverflowException">
        ///     Thrown when a Create Overflow error condition occurs.
        /// </exception>
        /// <exception cref="FormatException">
        ///     The value is not a number, a name or a number string.
        /// </exception>
        /// <exception cref="OverflowException">
        ///     The number is outside the range of the underlying type.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="enumType">The enum type.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value, boxed as <paramref name="enumType"/>.
        /// </returns>
        internal static object ToEnumValue(object value, Type enumType, string columnName)
        {
            if (enumType.IsInstanceOfType(value))
                return value;
           
            if (value is string)
                return ToEnum(value, enumType, columnName);
            
            if (!IsNumber(value))
                throw CreateFormatException(value, columnName, enumType, null);

            try
            {
                var underlyingValue = ConvertByTypeCode(value, Enum.GetUnderlyingType(enumType), columnName,
                    CultureInfo.InvariantCulture);

                return Enum.ToObject(enumType, underlyingValue);
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(value, columnName, enumType, exception);
            }
            catch (OverflowException exception)
            {
                throw CreateOverflowException(value, columnName, enumType, exception);
            }
        }

        /// <summary>
        ///     Converts a column value with the converter that matches the target type code.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="targetType">The target type, not nullable.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <param name="provider">
        ///     The culture for <see cref="System.Convert.ChangeType(object, Type, IFormatProvider)"/>
        ///     when no converter matches.
        /// </param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        private static object ConvertByTypeCode(object value, Type targetType, string columnName,
            IFormatProvider provider)
        {
            switch (Type.GetTypeCode(targetType))
            {
                case TypeCode.Boolean:
                    return ToBoolean(value, columnName);
                case TypeCode.Char:
                    return ToChar(value, columnName);
                case TypeCode.SByte:
                    return ToSByte(value, columnName);
                case TypeCode.Byte:
                    return ToByte(value, columnName);
                case TypeCode.Int16:
                    return ToInt16(value, columnName);
                case TypeCode.UInt16:
                    return ToUInt16(value, columnName);
                case TypeCode.Int32:
                    return ToInt32(value, columnName);
                case TypeCode.UInt32:
                    return ToUInt32(value, columnName);
                case TypeCode.Int64:
                    return ToInt64(value, columnName);
                case TypeCode.UInt64:
                    return ToUInt64(value, columnName);
                case TypeCode.Single:
                    return ToSingle(value, columnName);
                case TypeCode.Double:
                    return ToDouble(value, columnName);
                case TypeCode.Decimal:
                    return ToDecimal(value, columnName);
                case TypeCode.DateTime:
                    return ToDateTime(value, columnName);
                default:
                    return Convert.ChangeType(value, targetType, provider);
            }
        }

        /// <summary>
        ///     Converts a column value to an enum type.
        /// </summary>
        /// <remarks>
        ///     A string is parsed by name (case-insensitive, trimmed) or as a number. Any other value is
        ///     converted to the enum's underlying type like <see cref="ChangeType"/> does.
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <exception cref="CreateOverflowException">
        ///     Thrown when a Create Overflow error condition occurs.
        /// </exception>
        /// <param name="value">The column value.</param>
        /// <param name="enumType">The enum type.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        private static object ToEnum(object value, Type enumType, string columnName)
        {
            if (!(value is string text))
                return Enum.ToObject(enumType, ChangeType(value, Enum.GetUnderlyingType(enumType), columnName));

            try
            {
                return Enum.Parse(enumType, text.Trim(), true);
            }
            catch (ArgumentException exception)
            {
                throw CreateFormatException(value, columnName, enumType, exception);
            }
            catch (OverflowException exception)
            {
                throw CreateOverflowException(value, columnName, enumType, exception);
            }
        }

        /// <summary>
        ///     Converts a column value to an integral type with checked arithmetic.
        /// </summary>
        /// <remarks>
        ///     A decimal, double or float value is accepted only when it has no fractional part and is
        ///     never rounded. Strings are parsed with <see cref="NumberStyles.Integer"/> and the
        ///     invariant culture first, then with the current culture.
        /// </remarks>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <exception cref="CreateOverflowException">
        ///     Thrown when a Create Overflow error condition occurs.
        /// </exception>
        /// <typeparam name="T">The integral target type.</typeparam>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The converted value.
        /// </returns>
        private static T ToIntegral<T>(object value, string columnName)
        {
            if (value is T typed)
                return typed;

            if (HasFraction(value))
                throw CreateFormatException(value, columnName, typeof(T), null);

            try
            {
                if (IsNumber(value))
                    return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);

                var text = AsText(value);
                try
                {
                    return (T)Convert.ChangeType(text, typeof(T), CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    return (T)Convert.ChangeType(text, typeof(T), CultureInfo.CurrentCulture);
                }
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(value, columnName, typeof(T), exception);
            }
            catch (OverflowException exception)
            {
                throw CreateOverflowException(value, columnName, typeof(T), exception);
            }
        }

        /// <summary>
        ///     Determines whether a raw column value means "no value".
        /// </summary>
        /// <param name="value">The raw column value.</param>
        /// <returns>
        ///     <see langword="true"/> for DBNull, null, an empty or whitespace-only string or text, and
        ///     a whitespace char; otherwise <see langword="false"/>.
        /// </returns>
        private static bool IsNoValue(object value)
        {
            switch (value)
            {
                case null:
                case DBNull _:
                    return true;
                case string text:
                    return string.IsNullOrWhiteSpace(text);
                case char character:
                    return char.IsWhiteSpace(character);
                case IFormattable _:
                    return false;
                default:
                    return string.IsNullOrWhiteSpace(value.ToString());
            }
        }

        /// <summary>
        ///     Determines whether a value is a typed number (integral, decimal, double or float).
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <returns>
        ///     <see langword="true"/> when the value is a typed number that is not an enum; otherwise
        ///     <see langword="false"/>.
        /// </returns>
        private static bool IsNumber(object value)
        {
            if (value is Enum)
                return false;

            var typeCode = Convert.GetTypeCode(value);

            return typeCode >= TypeCode.SByte && typeCode <= TypeCode.Decimal;
        }

        /// <summary>
        ///     Query if a value is a floating-point NaN.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     True if the value is a double or float NaN.
        /// </returns>
        private static bool IsNaN(object value)
        {
            return value is double d && double.IsNaN(d) || value is float f && float.IsNaN(f);
        }

        /// <summary>
        ///     Query if a numeric value has a fractional part (infinity counts as fractional).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     True if the value is not integral-valued.
        /// </returns>
        private static bool HasFraction(object value)
        {
            switch (value)
            {
                case decimal number:
                    return decimal.Truncate(number) != number;
                case double number:
                    return double.IsInfinity(number) || Math.Floor(number) != number;
                case float number:
                    return float.IsInfinity(number) || Math.Floor(number) != number;
                default:
                    return false;
            }
        }

        /// <summary>
        ///     Gets a typed date and time value.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="result">
        ///     [out] The <see cref="DateTime"/> as is, or the <see cref="DateTimeOffset.LocalDateTime"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> when the value is a <see cref="DateTime"/> or a <see cref="DateTimeOffset"/>
        ///     ;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        private static bool TryGetDateTime(object value, out DateTime result)
        {
            switch (value)
            {
                case DateTime dateTime:
                    result = dateTime;
                    return true;
                case DateTimeOffset dateTimeOffset:
                    result = dateTimeOffset.LocalDateTime;
                    return true;
                default:
                    result = default;
                    return false;
            }
        }

        /// <summary>
        ///     Converts a <see cref="DateTime"/> column value to <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <exception cref="CreateOverflowException">
        ///     Thrown when a Create Overflow error condition occurs.
        /// </exception>
        /// <param name="dateTime">The column value.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The value with the local offset for <see cref="DateTimeKind.Local"/>; otherwise with
        ///     +00:00.
        /// </returns>
        private static DateTimeOffset FromDateTime(DateTime dateTime, string columnName)
        {
            if (dateTime.Kind != DateTimeKind.Local)
                return new DateTimeOffset(dateTime, TimeSpan.Zero);

            try
            {
                return new DateTimeOffset(dateTime);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                throw CreateOverflowException(dateTime, columnName, typeof(DateTimeOffset), exception);
            }
        }

        /// <summary>
        ///     Parses a <see cref="DateTimeOffset"/> column text.
        /// </summary>
        /// <exception cref="CreateFormatException">
        ///     Thrown when a Create Format error condition occurs.
        /// </exception>
        /// <param name="text">The column text.</param>
        /// <param name="columnName">The column name, used in error messages.</param>
        /// <returns>
        ///     The parsed value; a text without an offset is read as UTC.
        /// </returns>
        private static DateTimeOffset ParseDateTimeOffset(string text, string columnName)
        {
            if (DateTimeOffset.TryParseExact(text, "o", CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeUniversal, out var result))
                return result;

            try
            {
                return DateTimeOffset.Parse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            }
            catch (FormatException exception)
            {
                throw CreateFormatException(text, columnName, typeof(DateTimeOffset), exception);
            }
        }

        /// <summary>
        ///     Gets the text of a column value, the same text the extensions parsed before.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <returns>
        ///     The string itself, or the result of <see cref="object.ToString"/>.
        /// </returns>
        private static string AsText(object value)
            => value as string ?? value.ToString();

        /// <summary>
        ///     Creates a <see cref="FormatException"/> that names the column and the source type.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="innerException">The original exception, or null.</param>
        /// <returns>
        ///     The exception to throw.
        /// </returns>
        private static FormatException CreateFormatException(object value, string columnName, Type targetType,
            Exception innerException)
            => new FormatException(CreateMessage(value, columnName, targetType), innerException);

        /// <summary>
        ///     Creates an <see cref="OverflowException"/> that names the column and the source type.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="innerException">The original exception.</param>
        /// <returns>
        ///     The exception to throw.
        /// </returns>
        private static OverflowException CreateOverflowException(object value, string columnName, Type targetType,
            Exception innerException)
            => new OverflowException(CreateMessage(value, columnName, targetType), innerException);

        /// <summary>
        ///     Creates an <see cref="InvalidCastException"/> that names the column and the source type.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>
        ///     The exception to throw.
        /// </returns>
        private static InvalidCastException CreateInvalidCastException(object value, string columnName,
            Type targetType)
            => new InvalidCastException(CreateMessage(value, columnName, targetType));

        /// <summary>
        ///     Creates the conversion error message. The value itself is left out because it may hold
        ///     sensitive data.
        /// </summary>
        /// <param name="value">The column value.</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>
        ///     The message.
        /// </returns>
        private static string CreateMessage(object value, string columnName, Type targetType)
            => $"The value of column '{columnName}' of type '{value.GetType().FullName}' cannot be converted to '{targetType.Name}'.";
    }
}
