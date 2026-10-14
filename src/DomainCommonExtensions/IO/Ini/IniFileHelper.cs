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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using RzR.Extensions.Domain.Collections;
using RzR.Extensions.Domain.Reflection.TypeParam;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;
using RzR.Extensions.Domain.Models;
using RzR.Extensions.Domain.Validation;

// ReSharper disable MethodOverloadWithOptionalParameter

#endregion

namespace RzR.Extensions.Domain.IO.Ini
{
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
    ///     The data is parsed line by line: lines starting with ';' or '#' are comments, and a line
    ///     starting with '[' that is not a valid section header ends the current section, so the
    ///     entries after it are returned for no section, including the empty one. Sections and keys
    ///     are compared ordinal, ignoring case, so culture-specific equivalences (e.g. "ß" and "ss")
    ///     and ignorable characters (e.g. a soft hyphen) are not treated as equal.
    /// </remarks>
    public class IniFileHelper
    {
        /// <summary>
        ///     (Immutable) a pattern matching a single ini line: a section header, an invalid section
        ///     header (any other line starting with '[') or a key/value entry. The section, key and
        ///     value are captured already trimmed. Comment lines and other lines produce no match.
        /// </summary>
        /// <remarks>
        ///     The pattern is anchored to the line start and has no nested repetitions, so the matching
        ///     time grows linearly with the line length.
        /// </remarks>
        private const string LinePattern =
            @"^\uFEFF?[^\S\r\n]*(?:" +
            @"\[[ \t]*(?<section>[^\]\s](?:[^\]\r\n]*[^\]\s])?)[ \t]*\]" +
            @"|(?<invalidSection>\[)" +
            @"|(?<key>[^=;#\[\]\s](?:[^=;#\[\]\r\n]*[^=;#\[\]\s])?)[ \t]*=[ \t]*(?<value>(?:\S(?:.*\S)?)?)" +
            @")";

        /// <summary>
        ///     (Immutable) the Regular Expression instance shared by all instances.
        /// </summary>
        private static readonly Regex IniLineRegex = new Regex(LinePattern,
            RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.CultureInvariant);

        /// <summary>
        ///     (Immutable) type of the string comparison.
        /// </summary>
        private readonly StringComparison _stringComparisonType;

        /// <summary>
        ///     Collection of initialize file matches.
        /// </summary>
        private MatchCollection _iniFileMatchCollection;

        /// <summary>
        ///     (Immutable)
        ///     The class instance.
        /// </summary>
        public static readonly IniFileHelper Instance = new IniFileHelper();

        /// <summary>
        ///     Prevents a default instance of the <see cref="IniFileHelper"/> class from being created.
        ///     The instance holds no data and behaves as an empty file until it is loaded.
        /// </summary>
        private IniFileHelper()
            : this(ParseMatches(string.Empty))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IniFileHelper"/> class holding already
        ///     parsed data.
        /// </summary>
        /// <param name="iniFileMatchCollection">
        ///     The fully evaluated collection of ini file matches.
        /// </param>
        private IniFileHelper(MatchCollection iniFileMatchCollection)
        {
            _stringComparisonType = StringComparison.OrdinalIgnoreCase;
            _iniFileMatchCollection = iniFileMatchCollection;
        }

        /// <summary>
        ///     Loads ini file data from the raw ini text.
        /// </summary>
        /// <remarks>
        ///     This overload parses the supplied string as INI text, not as a file path. A call with a
        ///     single path argument binds to this overload and parses the path itself as INI text; to
        ///     read a file call <see cref="LoadFile(string, Encoding)"/> (recommended) or
        ///     <see cref="Load(string, Encoding)"/> with the encoding argument (null for UTF-8).
        ///     The loaded data is kept by this instance and returned as a new independent instance. 
        /// </remarks>
        /// <param name="rawFileData">The raw file data to load.</param>
        /// <returns>
        ///     A new independent instance holding the loaded data.
        /// </returns>
        public IniFileHelper Load(string rawFileData)
        {
            return LoadContent(rawFileData);
        }

        /// <summary>
        ///     Loads ini file data.
        /// </summary>
        /// <remarks>
        ///     The loaded data is kept by this instance and returned as a new independent instance.
        /// </remarks>
        /// <param name="reader">The reader to load.</param>
        /// <returns>
        ///     A new independent instance holding the loaded data.
        /// </returns>
        public IniFileHelper Load(TextReader reader)
        {
            var textData = reader.ReadToEnd();

            return LoadContent(textData);
        }

        /// <summary>
        ///     Loads ini file data.
        /// </summary>
        /// <remarks>
        ///     The loaded data is kept by this instance and returned as a new independent instance.
        /// </remarks>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">(Optional) The encoding.</param>
        /// <returns>
        ///     A new independent instance holding the loaded data.
        /// </returns>
        public IniFileHelper Load(Stream stream, Encoding encoding = null)
        {
            using var reader = new StreamReader(stream, encoding.IfIsNull(Encoding.UTF8));

            return LoadContent(reader.ReadToEnd());
        }

        /// <summary>
        ///     Loads ini file data.
        /// </summary>
        /// <remarks>
        ///     To read a file pass the encoding argument explicitly (null for UTF-8). A call with only
        ///     the path binds to <see cref="Load(string)"/>, which parses the path itself as INI text.
        ///     <see cref="LoadFile(string, Encoding)"/> is the recommended way to read a file: it behaves
        ///     the same and has no overload trap. The loaded data is kept by this instance and returned
        ///     as a new independent instance.
        /// </remarks>
        /// <param name="fullFileNamePath">Full pathname of the full file name file.</param>
        /// <param name="encoding">(Optional) The encoding.</param>
        /// <returns>
        ///     A new independent instance holding the loaded data.
        /// </returns>
        public IniFileHelper Load(string fullFileNamePath, Encoding encoding = null)
        {
            var flExist = DirectoryHelper.ExistFileInDirectory(fullFileNamePath);
            if (flExist.IsFalse())
                DomainEnsure.InternalThrowException(ExceptionType.FileNotFoundException, fullFileNamePath, nameof(flExist),
                    "Supplied parameter is not a file directory or file was not found!");

            var content = File.ReadAllText(fullFileNamePath, encoding.IfIsNull(Encoding.UTF8));

            return LoadContent(content);
        }

        /// <summary>
        ///     Loads ini file data from a file.
        /// </summary>
        /// <remarks>
        ///     The recommended way to read a file: unlike <see cref="Load(string)"/>, a call with only
        ///     the path always reads the file. It behaves the same as <see cref="Load(string, Encoding)"/>.
        ///     The loaded data is kept by this instance and returned as a new independent instance. 
        /// </remarks>
        /// <exception cref="FileNotFoundException">
        ///     Thrown when <paramref name="fullFileNamePath"/> is null, empty or does not name an
        ///     existing file.
        /// </exception>
        /// <exception cref="IOException">Thrown when the file cannot be read.</exception>
        /// <param name="fullFileNamePath">Full path of the ini file to read.</param>
        /// <param name="encoding">(Optional) The encoding; null for UTF-8.</param>
        /// <returns>
        ///     A new independent instance holding the loaded data.
        /// </returns>
        public IniFileHelper LoadFile(string fullFileNamePath, Encoding encoding = null)
        {
            return Load(fullFileNamePath, encoding);
        }

        /// <summary>
        ///     Returns a single entry specified by sectionName and key, or a default value if no entry
        ///     is found.
        /// </summary>
        /// <remarks>
        ///     As a result of searching will be no duplicate values and only the last defined key/value.
        /// </remarks>
        /// <param name="sectionName">The section name.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">(Optional) The default value.</param>
        /// <returns>
        ///     The entry value.
        /// </returns>
        public string GetEntry(string sectionName, string key, string defaultValue = null)
        {
            key = key.IfNullThenEmpty();
            sectionName = sectionName.IfNullThenEmpty();

            var fileData = GetEntry(sectionName, key, false);

            return fileData.Item1.Equals(key).IsTrue() && fileData.Item2.IsNotNullOrEmptyEnumerable().IsTrue()
                ? fileData.Item2[0]
                : defaultValue.IfNullThenEmpty();
        }

        /// <summary>
        ///     Returns all entries contained in the sectionName. If no entry is found, an empty
        ///     collection will be returned.
        /// </summary>
        /// <remarks>
        ///     As a result of searching will be no duplicate values and only the last defined key/value.
        /// </remarks>
        /// <param name="sectionName">The sectionName name.</param>
        /// <returns>
        ///     The distinct entry value.
        /// </returns>
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

        /// <summary>
        ///     Returns a single entry specified by sectionName and key, or a default value if no entry
        ///     is found.
        /// </summary>
        /// <remarks>
        ///     As a result of searching will be the key with value/s.
        /// </remarks>
        /// <param name="sectionName">The section name.</param>
        /// <param name="key">The section key.</param>
        /// <param name="includeDuplicates">True to include, false to exclude the duplicates.</param>
        /// <returns>
        ///     The entry value.
        /// </returns>
        public TupleResult<string, string[]> GetEntry(string sectionName, string key, bool includeDuplicates)
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

        /// <summary>
        ///     Parses the content, keeps it as the data of this instance and returns a new independent
        ///     instance holding the same data.
        /// </summary>
        /// <param name="content">The raw ini content.</param>
        /// <returns>
        ///     A new independent instance holding the loaded data.
        /// </returns>
        private IniFileHelper LoadContent(string content)
        {
            var matches = ParseMatches(content);
            _iniFileMatchCollection = matches;

            return new IniFileHelper(matches);
        }

        /// <summary>
        ///     Parses the ini content into a fully evaluated collection of matches.
        /// </summary>
        /// <param name="content">The raw ini content.</param>
        /// <returns>
        ///     The ini file matches.
        /// </returns>
        private static MatchCollection ParseMatches(string content)
        {
            var matches = IniLineRegex.Matches(content);

            // A MatchCollection and the groups of each Match are filled lazily; evaluating them now
            // means a collection shared between instances is never mutated by concurrent readers.
            foreach (Match match in matches)
                _ = match.Groups["section"];

            return matches;
        }

        /// <summary>
        ///     Check section.
        /// </summary>
        /// <param name="match">Specifies the match.</param>
        /// <param name="section">The section.</param>
        /// <param name="currentSection">
        ///     [in,out] The current section; null after an invalid section header.
        /// </param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        private bool CheckSection(Match match, string section, ref string currentSection)
        {
            var sectionGroup = match.Groups["section"];
            if (sectionGroup.Success.IsTrue())
                currentSection = sectionGroup.Value;
            else if (match.Groups["invalidSection"].Success.IsTrue())
                currentSection = null;

            return currentSection != null && currentSection.Equals(section, _stringComparisonType);
        }
    }
}
