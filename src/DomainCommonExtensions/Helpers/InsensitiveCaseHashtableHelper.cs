// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-10 03:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-10 03:05
// ***********************************************************************
//  <copyright file="InsensitiveCaseHashtableHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System.Collections;
using DomainCommonExtensions.CommonExtensions;

namespace DomainCommonExtensions.Helpers
{
    /// <summary>
    ///     Hashtable non sensitive keys collection helper
    /// </summary>
    public class InsensitiveCaseHashtableHelper : Hashtable
    {
        /// <summary>
        ///     Gets or sets key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <value></value>
        /// <remarks></remarks>
        public object this[string key]
        {
            get => base[key.ToLower()];
            set => base[key.ToLower()] = value;
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
                    if (enumerator.Current.IsNull()) continue;
                    var dictionaryEntry = (DictionaryEntry)enumerator.Current;
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
        /// <remarks></remarks>
        public bool ContainsKey(string key) => base.ContainsKey(key.ToLower());

        /// <summary>
        ///     Add new KV to collection
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <remarks></remarks>
        public void Add(string key, object value) => base.Add(key.ToLower(), value);

        /// <summary>
        ///     Remove key from collection
        /// </summary>
        /// <param name="key">Key</param>
        /// <remarks></remarks>
        public void Remove(string key) => base.Remove(key.ToLower());
    }
}