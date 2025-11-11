// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-11-06 14:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-10 21:03
// ***********************************************************************
//  <copyright file="IniFileHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using DomainCommonExtensions.ArraysExtensions;
using DomainCommonExtensions.CommonExtensions.TypeParam;
using DomainCommonExtensions.DataTypeExtensions;
using DomainCommonExtensions.Models;
using DomainCommonExtensions.Resources.Enums;
using DomainCommonExtensions.Utilities.Ensure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable MethodOverloadWithOptionalParameter

#endregion

namespace DomainCommonExtensions.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An initialize (.INI) file read and extract entry helper.
    /// </summary>
    /// <example>
    ///     For example of ini file:
    ///     <![CDATA[
    ///     title = My app init file
    ///
    ///     [main]
    ///     title = The Current App
    ///
    ///     [database]
    ///     ip = 127.0.0.1
    ///     ports = [1111, 5555]
    ///     name = LocalDb
    ///     ]]>
    /// <code>
    ///     IniFileHelper instance = IniFileHelper.Instance.Load(iniFile);
    ///     var info = instance.GetEntry("main", "title", "MyXApp");
    ///     // The result should be: `The Current App`
    /// </code>
    /// </example>
    /// <remarks>
    ///     It supports a standard file with sections and clear entries, and can contain duplicate entries.
    ///     On retrieve entries as result (value) is an array of data, and can choose what values are required.
    ///     Additionally, it can be retrieved only for unique values (the last defined value).
    /// </remarks>
    /// =================================================================================================
    public class IniFileHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) a pattern specifying the comment.
        /// </summary>
        /// =================================================================================================
        private const string CommentPattern = @"(?:(?<=;|#)(?:[ \t]*))(?<comment>.+)(?<=\S)";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) a pattern specifying the section.
        /// </summary>
        /// =================================================================================================
        private const string SectionPattern = @"(?:[ \t]*)(?<=\[)(?:[ \t]*)(?<section>[\w. ]+)(?:[ \t]*?)(?=\])";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) a pattern specifying the entry.
        /// </summary>
        /// =================================================================================================
        private const string EntryPattern = @"(?<entry>(?=\S)(?<key>[\w. ]+\S)(?:[ \t]*)(?==)=(?<==)(?:[ \t]*)(?<value>.*)(?<=\S))";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the Regular Expression instance.
        /// </summary>
        /// =================================================================================================
        private readonly Regex _regex;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) type of the string comparison.
        /// </summary>
        /// =================================================================================================
        private readonly StringComparison _stringComparisonType;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Collection of initialize file matches.
        /// </summary>
        /// =================================================================================================
        private MatchCollection _iniFileMatchCollection;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The class instance.
        /// </summary>
        /// =================================================================================================
        public static IniFileHelper Instance = new IniFileHelper();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Prevents a default instance of the <see cref="IniFileHelper"/> class from being
        ///     created.
        /// </summary>
        /// =================================================================================================
        private IniFileHelper()
        {
            _stringComparisonType = StringComparison.InvariantCultureIgnoreCase;
            _regex = new Regex($"{CommentPattern}|{SectionPattern}|{EntryPattern}",
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Loads ini file data.
        /// </summary>
        /// <param name="rawFileData">The raw file data to load.</param>
        /// =================================================================================================
        public IniFileHelper Load(string rawFileData)
        {
            _iniFileMatchCollection = _regex.Matches(rawFileData);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Loads ini file data.
        /// </summary>
        /// <param name="reader">The reader to load.</param>
        /// =================================================================================================
        public IniFileHelper Load(TextReader reader)
        {
            var textData = reader.ReadToEnd();

            _iniFileMatchCollection = _regex.Matches(textData);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Loads ini file data.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">(Optional) The encoding.</param>
        /// =================================================================================================
        public IniFileHelper Load(Stream stream, Encoding encoding = null)
        {
            using var reader = new StreamReader(stream, encoding.IfIsNull(Encoding.UTF8));
            _iniFileMatchCollection = _regex.Matches(reader.ReadToEnd());

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Loads ini file data.
        /// </summary>
        /// <param name="fullFileNamePath">Full pathname of the full file name file.</param>
        /// <param name="encoding">(Optional) The encoding.</param>
        /// =================================================================================================
        public IniFileHelper Load(string fullFileNamePath, Encoding encoding = null)
        {
            var flExist = DirectoryHelper.ExistFileInDirectory(fullFileNamePath);
            if (flExist.IsFalse())
                DomainEnsure.InternalThrowException(ExceptionType.FileNotFoundException, fullFileNamePath, nameof(flExist),
                    "Supplied parameter is not a file directory or file was not found!");

            var content = File.ReadAllText(fullFileNamePath, encoding.IfIsNull(Encoding.UTF8));
            _iniFileMatchCollection = _regex.Matches(content);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns a single entry specified by sectionName and key, or a default value if no entry is
        ///     found.
        /// </summary>
        /// <param name="sectionName">The section name.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">(Optional) The default value.</param>
        /// <remarks>
        ///     As a result of searching will be no duplicate values and only the last defined key/value.
        /// </remarks>
        /// <returns>
        ///     The entry value.
        /// </returns>
        /// =================================================================================================
        public string GetEntry(string sectionName, string key, string defaultValue = null)
        {
            key = key.IfNullThenEmpty();
            sectionName = sectionName.IfNullThenEmpty();

            var fileData = GetEntry(sectionName, key, false);

            return fileData.Item1.Equals(key).IsTrue() && fileData.Item2.IsNotNullOrEmptyEnumerable().IsTrue()
                ? fileData.Item2[0]
                : defaultValue.IfNullThenEmpty();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns all entries contained in the sectionName. If no entry is found, an empty
        ///     collection will be returned.
        /// </summary>
        /// <param name="sectionName">The sectionName name.</param>
        /// <remarks>
        ///     As a result of searching will be no duplicate values and only the last defined key/value.
        /// </remarks>
        /// <returns>
        ///     The distinct entry value.
        /// </returns>
        /// =================================================================================================
        public IDictionary<string, string> GetDistinctEntryValue(string sectionName)
        {
            var entries = new Dictionary<string, string>();
            var currentSection = string.Empty;

            for (var i = 0; i < _iniFileMatchCollection.Count; i++)
            {
                var match = _iniFileMatchCollection[i];
                if (CheckSection(match, sectionName, ref currentSection).IsFalse())
                    continue;

                var keyGroup = match.Groups["key"];
                var valueGroup = match.Groups["value"];
                if (keyGroup.Success.IsTrue())
                    entries.AddOrUpdate(keyGroup.Value, valueGroup.Value);
            }

            return entries;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns a single entry specified by sectionName and key, or a default value if no entry
        ///     is found.
        /// </summary>
        /// <param name="sectionName">The section name.</param>
        /// <param name="key">The section key.</param>
        /// <param name="includeDuplicates">
        ///     (Optional) True to include, false to exclude the duplicates.
        /// </param>
        /// <remarks>
        ///     As a result of searching will be the key with value/s.
        /// </remarks>
        /// <returns>
        ///     The entry value.
        /// </returns>
        /// =================================================================================================
        public TupleResult<string, string[]> GetEntry(string sectionName, string key, bool includeDuplicates = false)
        {
            var currentSection = string.Empty;
            var currentValues = new string[] { };

            for (var i = 0; i < _iniFileMatchCollection.Count; i++)
            {
                var match = _iniFileMatchCollection[i];
                if (CheckSection(match, sectionName, ref currentSection).IsFalse())
                    continue;

                var keyGroup = match.Groups["key"];
                var valueGroup = match.Groups["value"];
                if (keyGroup.Success.IsTrue() && keyGroup.Value.Equals(key, _stringComparisonType).IsTrue())
                {
                    currentValues = includeDuplicates.IsTrue()
                        ? currentValues.AppendItem(valueGroup.Value)
                        : new[] { valueGroup.Value };
                }
            }

            return TupleResult.Create(key, currentValues);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns all entries contained in the sectionName. If no entry is found, an empty
        ///     collection will be returned.
        /// </summary>
        /// <param name="sectionName">The sectionName name.</param>
        /// <param name="includeDuplicates">
        ///     (Optional) True to include, false to exclude the duplicates.
        /// </param>
        /// <returns>
        ///     The entries.
        /// </returns>
        /// =================================================================================================
        public IDictionary<string, string[]> GetEntries(string sectionName, bool includeDuplicates = false)
        {
            var entries = new Dictionary<string, string[]>();
            var currentSection = string.Empty;

            for (var i = 0; i < _iniFileMatchCollection.Count; i++)
            {
                var match = _iniFileMatchCollection[i];
                if (CheckSection(match, sectionName, ref currentSection).IsFalse())
                    continue;

                var keyGroup = match.Groups["key"];
                var valueGroup = match.Groups["value"];
                if (keyGroup.Success.IsTrue())
                {
                    if (includeDuplicates.IsTrue())
                        entries.AddOrUpdateValues(keyGroup.Value, valueGroup.Value);
                    else
                        entries.AddOrUpdateValue(keyGroup.Value, valueGroup.Value);
                }
            }

            return entries;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Check section.
        /// </summary>
        /// <param name="match">Specifies the match.</param>
        /// <param name="section">The section.</param>
        /// <param name="currentSection">[in,out] The current section.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        private bool CheckSection(Match match, string section, ref string currentSection)
        {
            var sectionGroup = match.Groups["section"];
            if (sectionGroup.Success.IsTrue())
                currentSection = sectionGroup.Value;

            return currentSection.Equals(section, _stringComparisonType);
        }
    }
}