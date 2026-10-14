// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-10 03:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-10 03:05
// ***********************************************************************
//  <copyright file="InsensitiveCaseHashtable.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections;
using RzR.Extensions.Domain.Primitives;

#endregion

namespace RzR.Extensions.Domain.Collections.Types
{
    /// <summary>
    ///     Hashtable non-sensitive keys collection
    /// </summary>
    public class InsensitiveCaseHashtable : Hashtable
    {
        /// <summary>
        ///     Initializes a new instance using the ordinal ignore-case key comparer.
        /// </summary>
        /// <remarks>Keys used through the base (object) members are compared case-insensitively too.</remarks>
        public InsensitiveCaseHashtable() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        ///     Gets or sets key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <value></value>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
        /// <remarks>
        ///     The key is matched case-insensitively; a new key is stored lower-cased with the invariant
        ///     culture, an existing key keeps its original casing.
        /// </remarks>
        public object this[string key]
        {
            get => base[key?.ToLowerInvariant()!];
            set => base[key?.ToLowerInvariant()!] = value;
        }

        /// <summary>
        ///     Gets collection data.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public ArrayList Collection
        {
            get
            {
                var arrayList = new ArrayList();
                IEnumerator enumerator = GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var arg = arrayList;
                    if (enumerator.Current.IsNull())
                        continue;

                    var dictionaryEntry = (DictionaryEntry)enumerator.Current!;
                    arg.Add(dictionaryEntry.Value);
                }

                return arrayList;
            }
        }

        /// <summary>
        ///     Check if collection contains key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
        /// <remarks></remarks>
        public bool ContainsKey(string key) => base.ContainsKey(key?.ToLowerInvariant()!);

        /// <summary>
        ///     Add new KV to collection
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
        /// <remarks>
        ///     The key is stored lower-cased with the invariant culture; a key that already exists in any
        ///     casing throws, as for <see cref="Hashtable.Add(object, object)"/>.
        /// </remarks>
        public void Add(string key, object value) => base.Add(key?.ToLowerInvariant()!, value);

        /// <summary>
        ///     Remove key from collection
        /// </summary>
        /// <param name="key">Key</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="key"/> is null.</exception>
        /// <remarks></remarks>
        public void Remove(string key) => base.Remove(key?.ToLowerInvariant()!);
    }
}
