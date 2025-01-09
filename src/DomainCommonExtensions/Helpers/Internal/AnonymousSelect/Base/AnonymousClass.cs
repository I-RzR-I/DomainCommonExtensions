// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-01-08 13:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-08 18:05
// ***********************************************************************
//  <copyright file="AnonymousClass.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using CodeSource;
using DomainCommonExtensions.CommonExtensions;

#endregion

namespace DomainCommonExtensions.Helpers.Internal.AnonymousSelect.Base
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     The anonymous class.
    /// </summary>
    /// <seealso cref="T:System.Dynamic.DynamicObject"/>
    /// =================================================================================================
    [CodeSource(SourceUrl = "System.Linq.Dynamic.Core.DynamicClass", Version = 1.0D, Comment = "The current implementation has inspiration from a specified source URL.")]
    public abstract class AnonymousClass : DynamicObject
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Dictionary of properties.
        /// </summary>
        /// =================================================================================================
        private Dictionary<string, object> _propertiesDictionary;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the properties.
        /// </summary>
        /// <value>
        ///     The properties.
        /// </value>
        /// =================================================================================================
        private Dictionary<string, object> Properties
        {
            get
            {
                if (_propertiesDictionary.IsNull())
                {
                    _propertiesDictionary = new Dictionary<string, object>();

                    foreach (var pi in GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var parameters = pi.GetIndexParameters().Length;
                        if (parameters > 0)
                            // The property is an indexer, skip this.
                            continue;

                        _propertiesDictionary.Add(pi.Name, pi.GetValue(this, null));
                    }
                }

                return _propertiesDictionary;
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the <see cref="object" /> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="object" />.</returns>
        /// <returns>Value from the property.</returns>
        /// =================================================================================================
        public object this[string name]
        {
            get => Properties.TryGetValue(name, out var result) ? result : null;

            set => Properties[name] = value;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the dynamic property by name.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     T.
        /// </returns>
        /// =================================================================================================
        public T GetDynamicPropertyValue<T>(string propertyName)
        {
            var type = GetType();
            var propInfo = type.GetProperty(propertyName);

            return (T)propInfo?.GetValue(this, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the dynamic property value by name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        ///     value.
        /// </returns>
        /// =================================================================================================
        public object GetDynamicPropertyValue(string propertyName) => GetDynamicPropertyValue<object>(propertyName);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets the dynamic property value by name.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// =================================================================================================
        public void SetDynamicPropertyValue<T>(string propertyName, T value)
        {
            var type = GetType();
            var propInfo = type.GetProperty(propertyName);

            propInfo?.SetValue(this, value, null);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets the dynamic property value by name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// =================================================================================================
        public void SetDynamicPropertyValue(string propertyName, object value)
        {
            SetDynamicPropertyValue<object>(propertyName, value);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns the enumeration of all dynamic member names.
        /// </summary>
        /// <returns>
        ///     A sequence that contains dynamic member names.
        /// </returns>
        /// =================================================================================================
        public override IEnumerable<string> GetDynamicMemberNames() => Properties.Keys;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Provides the implementation for operations that get member values. Classes derived from
        ///     the
        ///     <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify
        ///     dynamic behavior for
        ///     operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">
        ///     Provides information about the object that called the dynamic operation. The binder.Name
        ///     property provides the name of the member on which the dynamic operation is performed. For
        ///     example, for the Console.WriteLine(sampleObject.SampleProperty) statement, where
        ///     sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" />
        ///     class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies
        ///     whether the member name is case-sensitive.
        /// </param>
        /// <param name="result">
        ///     [out] The result of the get operation. For example, if the method is called for a
        ///     property, you can assign the property value to <paramref name="result" />.
        /// </param>
        /// <returns>
        ///     true if the operation is successful; otherwise, false. If this method returns false, the
        ///     run-time binder of the language determines the behavior. (In most cases, a run-time
        ///     exception is thrown.)
        /// </returns>
        /// =================================================================================================
        public override bool TryGetMember(GetMemberBinder binder, out object result) => Properties.TryGetValue(binder.Name, out result);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Provides the implementation for operations that set member values. Classes derived from
        ///     the
        ///     <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify
        ///     dynamic behavior for
        ///     operations such as setting a value for a property.
        /// </summary>
        /// <param name="binder">
        ///     Provides information about the object that called the dynamic operation. The binder.Name
        ///     property provides the name of the member to which the value is being assigned. For
        ///     example, for the statement sampleObject.SampleProperty = "Test", where sampleObject is an
        ///     instance of the class derived from the
        ///     <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns
        ///     "SampleProperty". The binder.IgnoreCase
        ///     property specifies whether the member name is case-sensitive.
        /// </param>
        /// <param name="value">
        ///     The value to set to the member. For example, for sampleObject.SampleProperty = "Test",
        ///     where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" />
        ///     class, the
        ///     <paramref name="value" /> is "Test".
        /// </param>
        /// <returns>
        ///     true if the operation is successful; otherwise, false. If this method returns false, the
        ///     run-time binder of the language determines the behavior. (In most cases, a language-
        ///     specific run-time exception is thrown.)
        /// </returns>
        /// =================================================================================================
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var name = binder.Name;
            Properties[name] = value;

            return true;
        }
    }
}