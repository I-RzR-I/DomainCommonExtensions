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

namespace RzR.Extensions.Domain.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A static factory that creates <see cref="TupleResult{T1}"/> value instances using
    ///     type inference, avoiding the need to specify generic type arguments explicitly.
    /// </summary>
    /// <remarks>
    ///     <see cref="TupleResult"/> structs are immutable, stack-allocated value types that support
    ///     C# deconstruction syntax. They serve as a self-describing alternative to
    ///     <see cref="System.ValueTuple"/> when named type parameters are desired in public APIs.
    ///     <example>
    ///     <code>
    ///     var city = TupleResult.Create("New York", 8_000_000, 468.9);
    ///     var (name, pop, area) = city;     // deconstruction syntax
    ///     string n = city.Item1;             // named property access
    ///     </code>
    ///     </example>
    /// </remarks>
    /// =================================================================================================
    public static class TupleResult
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="TupleResult{T1}"/> holding a single component.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <param name="item1">The first component value.</param>
        /// <returns>
        ///     A <see cref="TupleResult{T1}"/> containing <paramref name="item1"/>.
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1> Create<T1>(T1 item1)
            => new TupleResult<T1>(item1);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="TupleResult{T1, T2}"/> holding two components.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <typeparam name="T2">The type of the second component.</typeparam>
        /// <param name="item1">The first component value.</param>
        /// <param name="item2">The second component value.</param>
        /// <returns>
        ///     A <see cref="TupleResult{T1, T2}"/> containing <paramref name="item1"/> and
        ///     <paramref name="item2"/>.
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
            => new TupleResult<T1, T2>(item1, item2);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="TupleResult{T1, T2, T3}"/> holding three components.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <typeparam name="T2">The type of the second component.</typeparam>
        /// <typeparam name="T3">The type of the third component.</typeparam>
        /// <param name="item1">The first component value.</param>
        /// <param name="item2">The second component value.</param>
        /// <param name="item3">The third component value.</param>
        /// <returns>
        ///     A <see cref="TupleResult{T1, T2, T3}"/> containing <paramref name="item1"/>,
        ///     <paramref name="item2"/>, and <paramref name="item3"/>.
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
            => new TupleResult<T1, T2, T3>(item1, item2, item3);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="TupleResult{T1, T2, T3, T4}"/> holding four components.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <typeparam name="T2">The type of the second component.</typeparam>
        /// <typeparam name="T3">The type of the third component.</typeparam>
        /// <typeparam name="T4">The type of the fourth component.</typeparam>
        /// <param name="item1">The first component value.</param>
        /// <param name="item2">The second component value.</param>
        /// <param name="item3">The third component value.</param>
        /// <param name="item4">The fourth component value.</param>
        /// <returns>
        ///     A <see cref="TupleResult{T1, T2, T3, T4}"/> containing <paramref name="item1"/>,
        ///     <paramref name="item2"/>, <paramref name="item3"/>, and <paramref name="item4"/>.
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
            => new TupleResult<T1, T2, T3, T4>(item1, item2, item3, item4);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a <see cref="TupleResult{T1, T2, T3, T4, T5}"/> holding five components.
        /// </summary>
        /// <typeparam name="T1">The type of the first component.</typeparam>
        /// <typeparam name="T2">The type of the second component.</typeparam>
        /// <typeparam name="T3">The type of the third component.</typeparam>
        /// <typeparam name="T4">The type of the fourth component.</typeparam>
        /// <typeparam name="T5">The type of the fifth component.</typeparam>
        /// <param name="item1">The first component value.</param>
        /// <param name="item2">The second component value.</param>
        /// <param name="item3">The third component value.</param>
        /// <param name="item4">The fourth component value.</param>
        /// <param name="item5">The fifth component value.</param>
        /// <returns>
        ///     A <see cref="TupleResult{T1, T2, T3, T4, T5}"/> containing <paramref name="item1"/>,
        ///     <paramref name="item2"/>, <paramref name="item3"/>, <paramref name="item4"/>, and
        ///     <paramref name="item5"/>.
        /// </returns>
        /// =================================================================================================
        public static TupleResult<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
            => new TupleResult<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An immutable value type holding a single strongly-typed component, accessible via
    ///     <see cref="Item1"/> or C# deconstruction syntax (<c>var (a) = result</c>).
    /// </summary>
    /// <typeparam name="T1">The type of the first (and only) component.</typeparam>
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

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deconstructs this instance into its component, enabling C# tuple-deconstruction
        ///     syntax: <c>var (a) = result;</c>
        /// </summary>
        /// <param name="item1">Receives the value of <see cref="Item1"/>.</param>
        /// =================================================================================================
        public void Deconstruct(out T1 item1)
        {
            item1 = _item1;
        }
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An immutable value type holding two strongly-typed components, accessible via
    ///     <see cref="Item1"/>/<see cref="Item2"/> or C# deconstruction syntax
    ///     (<c>var (a, b) = result</c>).
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
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

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deconstructs this instance into its two components, enabling C# tuple-deconstruction
        ///     syntax: <c>var (a, b) = result;</c>
        /// </summary>
        /// <param name="item1">Receives the value of <see cref="Item1"/>.</param>
        /// <param name="item2">Receives the value of <see cref="Item2"/>.</param>
        /// =================================================================================================
        public void Deconstruct(out T1 item1, out T2 item2)
        {
            item1 = _item1;
            item2 = _item2;
        }
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An immutable value type holding three strongly-typed components, accessible via
    ///     <see cref="Item1"/>/<see cref="Item2"/>/<see cref="Item3"/> or C# deconstruction
    ///     syntax (<c>var (a, b, c) = result</c>).
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
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

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deconstructs this instance into its three components, enabling C# tuple-deconstruction
        ///     syntax: <c>var (a, b, c) = result;</c>
        /// </summary>
        /// <param name="item1">Receives the value of <see cref="Item1"/>.</param>
        /// <param name="item2">Receives the value of <see cref="Item2"/>.</param>
        /// <param name="item3">Receives the value of <see cref="Item3"/>.</param>
        /// =================================================================================================
        public void Deconstruct(out T1 item1, out T2 item2, out T3 item3)
        {
            item1 = _item1;
            item2 = _item2;
            item3 = _item3;
        }
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An immutable value type holding four strongly-typed components, accessible via
    ///     <see cref="Item1"/>/<see cref="Item2"/>/<see cref="Item3"/>/<see cref="Item4"/> or
    ///     C# deconstruction syntax (<c>var (a, b, c, d) = result</c>).
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    /// <typeparam name="T4">The type of the fourth component.</typeparam>
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

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deconstructs this instance into its four components, enabling C# tuple-deconstruction
        ///     syntax: <c>var (a, b, c, d) = result;</c>
        /// </summary>
        /// <param name="item1">Receives the value of <see cref="Item1"/>.</param>
        /// <param name="item2">Receives the value of <see cref="Item2"/>.</param>
        /// <param name="item3">Receives the value of <see cref="Item3"/>.</param>
        /// <param name="item4">Receives the value of <see cref="Item4"/>.</param>
        /// =================================================================================================
        public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4)
        {
            item1 = _item1;
            item2 = _item2;
            item3 = _item3;
            item4 = _item4;
        }
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An immutable value type holding five strongly-typed components, accessible via
    ///     <see cref="Item1"/> through <see cref="Item5"/> or C# deconstruction syntax
    ///     (<c>var (a, b, c, d, e) = result</c>).
    /// </summary>
    /// <typeparam name="T1">The type of the first component.</typeparam>
    /// <typeparam name="T2">The type of the second component.</typeparam>
    /// <typeparam name="T3">The type of the third component.</typeparam>
    /// <typeparam name="T4">The type of the fourth component.</typeparam>
    /// <typeparam name="T5">The type of the fifth component.</typeparam>
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

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Deconstructs this instance into its five components, enabling C# tuple-deconstruction
        ///     syntax: <c>var (a, b, c, d, e) = result;</c>
        /// </summary>
        /// <param name="item1">Receives the value of <see cref="Item1"/>.</param>
        /// <param name="item2">Receives the value of <see cref="Item2"/>.</param>
        /// <param name="item3">Receives the value of <see cref="Item3"/>.</param>
        /// <param name="item4">Receives the value of <see cref="Item4"/>.</param>
        /// <param name="item5">Receives the value of <see cref="Item5"/>.</param>
        /// =================================================================================================
        public void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5)
        {
            item1 = _item1;
            item2 = _item2;
            item3 = _item3;
            item4 = _item4;
            item5 = _item5;
        }
    }
}