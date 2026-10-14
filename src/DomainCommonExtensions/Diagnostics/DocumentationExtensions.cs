// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-08-10 21:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-12 23:39
// ***********************************************************************
//  <copyright file="DocumentationExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using RzR.Extensions.Domain.Primitives;
using RzR.Extensions.Domain.Text;

#endregion

namespace RzR.Extensions.Domain.Diagnostics
{
    /// <summary>
    ///     Utility class to provide documentation for various types where available with the assembly
    /// </summary>
    public static class DocumentationExtensions
    {
        /// <summary>
        ///     A cache used to remember Xml documentation for assemblies
        /// </summary>
        private static readonly Dictionary<Assembly, XmlDocument> Cache = new Dictionary<Assembly, XmlDocument>();

        /// <summary>
        ///     A cache used to store failure exceptions for assembly lookups
        /// </summary>
        private static readonly Dictionary<Assembly, Exception> FailCache = new Dictionary<Assembly, Exception>();

        /// <summary>
        ///     Lock guarding every access to <see cref="Cache"/> and <see cref="FailCache"/>
        /// </summary>
        private static readonly object CacheLock = new object();

        /// <summary>
        ///     Provides the documentation comments for a specific method
        /// </summary>
        /// <param name="methodInfo">The MethodInfo (reflection data ) of the member to find documentation for</param>
        /// <returns>The XML fragment describing the method</returns>
        public static XmlElement GetDocumentation(this MethodInfo methodInfo)
        {
            // Calculate the parameter string as this is in the member name in the XML
            var parametersString = "";
            foreach (var parameterInfo in methodInfo.GetParameters())
            {
                if (parametersString.Length.IsGreaterThanZero()) parametersString += ",";

                // Nested types are "Outer+Inner" in reflection but "Outer.Inner" in the XML
                parametersString += parameterInfo.ParameterType.FullName?.Replace('+', '.');
            }

            //AL: 15.04.2008 ==> BUG-FIX remove “()” if parametersString is empty
            if (parametersString.Length.IsGreaterThanZero())
                return XmlFromName(methodInfo.DeclaringType, 'M', methodInfo.Name + "(" + parametersString + ")");

            return XmlFromName(methodInfo.DeclaringType, 'M', methodInfo.Name);
        }

        /// <summary>
        ///     Provides the documentation comments for a specific member
        /// </summary>
        /// <param name="memberInfo">The MemberInfo (reflection data) or the member to find documentation for</param>
        /// <returns>The XML fragment describing the member</returns>
        public static XmlElement GetDocumentation(this MemberInfo memberInfo)
        {
            if (memberInfo.IsNull()) return null;

            // First character [0] of member type is prefix character in the name in the XML
            return XmlFromName(memberInfo.DeclaringType, memberInfo.MemberType.ToString()[0], memberInfo.Name);
        }

        /// <summary>
        ///     Returns the Xml documentation summary comment for this member
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static string GetSummary(this MemberInfo memberInfo)
        {
            var element = memberInfo.GetDocumentation();
            var summaryElm = element?.SelectSingleNode("summary");
            if (summaryElm.IsNull()) return "";

            return summaryElm?.InnerText.Trim();
        }

        /// <summary>
        ///     Provides the documentation comments for a specific type
        /// </summary>
        /// <param name="type">Type to find the documentation for</param>
        /// <returns>The XML fragment that describes the type</returns>
        public static XmlElement GetDocumentation(this Type type)
        {
            // Prefix in type names is T
            return XmlFromName(type, 'T', "");
        }

        /// <summary>
        ///     Gets the summary portion of a type's documentation or returns an empty string if not available
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSummary(this Type type)
        {
            var element = type.GetDocumentation();
            var summaryElm = element?.SelectSingleNode("summary");
            if (summaryElm.IsNull()) return "";

            return summaryElm?.InnerText.Trim();
        }

        /// <summary>
        ///     Obtains the XML Element that describes a reflection element by searching the
        ///     members for a member that has a name that describes the element.
        /// </summary>
        /// <param name="type">The type or parent type, used to fetch the assembly</param>
        /// <param name="prefix">The prefix as seen in the name attribute in the documentation XML</param>
        /// <param name="name">Where relevant, the full name qualifier for the element</param>
        /// <returns>The member that has a name that describes the specified reflection element</returns>
        /// <remarks>Nested type names ("Outer+Inner") are written as "Outer.Inner", as in the XML documentation IDs.</remarks>
        private static XmlElement XmlFromName(this Type type, char prefix, string name)
        {
            string fullName;
            var typeName = type.FullName?.Replace('+', '.');

            if (name.IsNullOrEmpty())
                fullName = prefix + ":" + typeName;
            else
                fullName = prefix + ":" + typeName + "." + name;

            var xmlDocument = XmlFromAssembly(type.Assembly);
            var matchedElement =
                xmlDocument?["doc"]?["members"]?.SelectSingleNode("member[@name='" + fullName + "']") as XmlElement;

            return matchedElement;
        }

        /// <summary>
        ///     Obtains the documentation file for the specified assembly
        /// </summary>
        /// <param name="assembly">The assembly to find the XML document for</param>
        /// <returns>The XML document</returns>
        /// <remarks>
        ///     This version uses a cache to preserve the assemblies, so that
        ///     the XML file is not loaded and parsed on every single lookup.
        ///     The caches are guarded by a lock, so the XML file is loaded at most once per assembly.
        /// </remarks>
        public static XmlDocument XmlFromAssembly(this Assembly assembly)
        {
            lock (CacheLock)
            {
                if (FailCache.ContainsKey(assembly)) return null;

                try
                {
                    if (!Cache.ContainsKey(assembly))
                        // load the document into the cache
                        Cache[assembly] = XmlFromAssemblyNonCached(assembly);

                    return Cache[assembly];
                }
                catch (Exception exception)
                {
                    FailCache[assembly] = exception;

                    return null;
                }
            }
        }

        /// <summary>
        ///     Loads and parses the documentation file for the specified assembly
        /// </summary>
        /// <param name="assembly">The assembly to find the XML document for</param>
        /// <returns>The XML document</returns>
        /// <remarks>
        ///     The XML file is looked up next to Assembly.Location first, then next to the file URI CodeBase
        ///     (e.g. a shadow-copied assembly). DTD processing and external resolution are disabled when loading the XML.
        /// </remarks>
        private static XmlDocument XmlFromAssemblyNonCached(Assembly assembly)
        {
            var filePath = FindDocumentationFile(assembly.Location);

            if (filePath == null)
            {
                var codeBase = assembly.CodeBase;

                if (codeBase.IsNullOrEmpty()
                    || !Uri.TryCreate(codeBase, UriKind.Absolute, out var codeBaseUri)
                    || !codeBaseUri.IsFile)
                {
                    if (assembly.Location.IsNullOrEmpty())
                        throw new Exception("Could not ascertain assembly filename", null);

                    return null;
                }

                filePath = FindDocumentationFile(codeBaseUri.LocalPath);
                if (filePath == null) return null;
            }

            StreamReader streamReader;

            try
            {
                streamReader = new StreamReader(filePath);
            }
            catch (FileNotFoundException exception)
            {
                throw new Exception(
                    "XML documentation not present (make sure it is turned on in project properties when building)",
                    exception);
            }

            using (streamReader)
            using (var xmlReader = new XmlTextReader(streamReader) { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null })
            {
                var xmlDocument = new XmlDocument { XmlResolver = null };
                xmlDocument.Load(xmlReader);

                return xmlDocument;
            }
        }

        /// <summary>
        ///     Gets the XML documentation file path next to the given assembly file, when it exists
        /// </summary>
        /// <param name="assemblyFilename">The assembly file path</param>
        /// <returns>The XML documentation file path, or null when it does not exist</returns>
        private static string FindDocumentationFile(string assemblyFilename)
        {
            if (assemblyFilename.IsNullOrEmpty()) return null;

            var filePath = Path.ChangeExtension(assemblyFilename, ".xml");

            return File.Exists(filePath) ? filePath : null;
        }
    }
}
