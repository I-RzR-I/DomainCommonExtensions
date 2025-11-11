// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.DomainCommonExtensions
//  Author           : RzR
//  Created On       : 2025-11-10 20:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-11 22:22
// ***********************************************************************
//  <copyright file="TupleResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace DomainCommonExtensions.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a tuple.
    /// </summary>
    /// =================================================================================================
    public static class TupleResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new TupleResult&lt;T1&gt;
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <param name="item1">The first item.</param>
        /// <returns>
        ///     A TupleResult&lt;T1&gt;
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1> Create<T1>(T1 item1)
            => new TupleResult<T1>(item1);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new TupleResult&lt;T1&gt;
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <returns>
        ///     A TupleResult&lt;T1&gt;
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
            => new TupleResult<T1, T2>(item1, item2);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new TupleResult&lt;T1&gt;
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <typeparam name="T3">Generic type parameter.</typeparam>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <returns>
        ///     A TupleResult&lt;T1&gt;
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
            => new TupleResult<T1, T2, T3>(item1, item2, item3);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new TupleResult&lt;T1&gt;
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <typeparam name="T3">Generic type parameter.</typeparam>
        /// <typeparam name="T4">Generic type parameter.</typeparam>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <param name="item4">The fourth item.</param>
        /// <returns>
        ///     A TupleResult&lt;T1&gt;
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
            => new TupleResult<T1, T2, T3, T4>(item1, item2, item3, item4);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new TupleResult&lt;T1&gt;
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <typeparam name="T3">Generic type parameter.</typeparam>
        /// <typeparam name="T4">Generic type parameter.</typeparam>
        /// <typeparam name="T5">Generic type parameter.</typeparam>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <param name="item4">The fourth item.</param>
        /// <param name="item5">The fourth item.</param>
        /// <returns>
        ///     A TupleResult&lt;T1&gt;
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
            => new TupleResult<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a tuple.
    /// </summary>
    /// <typeparam name="T1">Generic type parameter.</typeparam>
    /// =================================================================================================
    public struct TupleResult<T1>
    {
        private readonly T1 _item1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 1.
        /// </summary>
        /// <value>
        ///     The item 1.
        /// </value>
        /// =================================================================================================
        public T1 Item1 => _item1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TupleResult{T1}"/> struct.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// =================================================================================================
        public TupleResult(T1 item1)
        {
            _item1 = item1;
        }
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a tuple.
    /// </summary>
    /// <typeparam name="T1">Generic type parameter.</typeparam>
    /// <typeparam name="T2">Generic type parameter.</typeparam>
    /// =================================================================================================
    public struct TupleResult<T1, T2>
    {
        private readonly T1 _item1;
        private readonly T2 _item2;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 1.
        /// </summary>
        /// <value>
        ///     The item 1.
        /// </value>
        /// =================================================================================================
        public T1 Item1 => _item1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 2.
        /// </summary>
        /// <value>
        ///     The item 2.
        /// </value>
        /// =================================================================================================
        public T2 Item2 => _item2;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TupleResult{T1, T2}"/> struct.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// =================================================================================================
        public TupleResult(T1 item1, T2 item2)
        {
            _item1 = item1;
            _item2 = item2;
        }
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a tuple.
    /// </summary>
    /// <typeparam name="T1">Generic type parameter.</typeparam>
    /// <typeparam name="T2">Generic type parameter.</typeparam>
    /// <typeparam name="T3">Generic type parameter.</typeparam>
    /// =================================================================================================
    public struct TupleResult<T1, T2, T3>
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 1.
        /// </summary>
        /// <value>
        ///     The item 1.
        /// </value>
        /// =================================================================================================
        public T1 Item1 => _item1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 2.
        /// </summary>
        /// <value>
        ///     The item 2.
        /// </value>
        /// =================================================================================================
        public T2 Item2 => _item2;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 3.
        /// </summary>
        /// <value>
        ///     The item 3.
        /// </value>
        /// =================================================================================================
        public T3 Item3 => _item3;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TupleResult{T1, T2, T3}"/> struct.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// =================================================================================================
        public TupleResult(T1 item1, T2 item2, T3 item3)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
        }
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a tuple.
    /// </summary>
    /// <typeparam name="T1">Generic type parameter.</typeparam>
    /// <typeparam name="T2">Generic type parameter.</typeparam>
    /// <typeparam name="T3">Generic type parameter.</typeparam>
    /// <typeparam name="T4">Generic type parameter.</typeparam>
    /// =================================================================================================
    public struct TupleResult<T1, T2, T3, T4>
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 1.
        /// </summary>
        /// <value>
        ///     The item 1.
        /// </value>
        /// =================================================================================================
        public T1 Item1 => _item1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 2.
        /// </summary>
        /// <value>
        ///     The item 2.
        /// </value>
        /// =================================================================================================
        public T2 Item2 => _item2;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 3.
        /// </summary>
        /// <value>
        ///     The item 3.
        /// </value>
        /// =================================================================================================
        public T3 Item3 => _item3;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 4.
        /// </summary>
        /// <value>
        ///     The item 4.
        /// </value>
        /// =================================================================================================
        public T4 Item4 => _item4;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TupleResult{T1, T2, T3, T4}"/> struct.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <param name="item4">The fourth item.</param>
        /// =================================================================================================
        public TupleResult(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
        }
    }
    
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a tuple.
    /// </summary>
    /// <typeparam name="T1">Generic type parameter.</typeparam>
    /// <typeparam name="T2">Generic type parameter.</typeparam>
    /// <typeparam name="T3">Generic type parameter.</typeparam>
    /// <typeparam name="T4">Generic type parameter.</typeparam>
    /// <typeparam name="T5">Generic type parameter.</typeparam>
    /// =================================================================================================
    public struct TupleResult<T1, T2, T3, T4, T5>
    {
        private readonly T1 _item1;
        private readonly T2 _item2;
        private readonly T3 _item3;
        private readonly T4 _item4;
        private readonly T5 _item5;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 1.
        /// </summary>
        /// <value>
        ///     The item 1.
        /// </value>
        /// =================================================================================================
        public T1 Item1 => _item1;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 2.
        /// </summary>
        /// <value>
        ///     The item 2.
        /// </value>
        /// =================================================================================================
        public T2 Item2 => _item2;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 3.
        /// </summary>
        /// <value>
        ///     The item 3.
        /// </value>
        /// =================================================================================================
        public T3 Item3 => _item3;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 4.
        /// </summary>
        /// <value>
        ///     The item 4.
        /// </value>
        /// =================================================================================================
        public T4 Item4 => _item4;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the item 5.
        /// </summary>
        /// <value>
        ///     The item 5.
        /// </value>
        /// =================================================================================================
        public T5 Item5 => _item5;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TupleResult{T1, T2, T3, T4, T5}"/> struct.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <param name="item4">The fourth item.</param>
        /// <param name="item5">The fifth item.</param>
        /// =================================================================================================
        public TupleResult(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
        }
    }
}