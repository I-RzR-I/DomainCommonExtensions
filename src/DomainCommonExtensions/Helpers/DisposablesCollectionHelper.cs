// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2022-12-09 00:58
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-12-09 01:16
// ***********************************************************************
//  <copyright file="DisposablesCollectionHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using DomainCommonExtensions.CommonExtensions;
using DomainCommonExtensions.DataTypeExtensions;

#endregion

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace DomainCommonExtensions.Helpers
{
    /// <summary>
    ///     Disposables collection helper
    /// </summary>
    public sealed class DisposablesCollectionHelper : IDisposable
    {
        private Stack<IDisposable> _disposables = new Stack<IDisposable>();

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_disposables.IsNull())
                while (_disposables.Count.IsGreaterThanZero())
                    _disposables.Pop().Dispose();
        }

        /// <summary>
        ///     Add to stack
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        /// <remarks></remarks>
        public T Add<T>(T resource) where T : IDisposable
        {
            _disposables.Push(resource);

            return resource;
        }
    }
}