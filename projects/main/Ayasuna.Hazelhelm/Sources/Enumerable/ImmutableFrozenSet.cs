namespace Ayasuna.Hazelhelm.Enumerable;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

/// <summary>
/// Contains helper methods for the <see cref="ImmutableFrozenSet{TValue}"/> type
/// </summary>
public static class ImmutableFrozenSet
{
    /// <summary>
    /// Creates a new <see cref="ImmutableFrozenSet{TValue}"/> that contains the given <paramref name="values"/>
    /// </summary>
    /// <param name="values">The values to create the set with</param>
    /// <param name="comparer">The comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>The created <see cref="ImmutableFrozenSet{TValue}"/></returns>
    public static ImmutableFrozenSet<TValue> Create<TValue>
    (
        IEnumerable<TValue> values,
        IEqualityComparer<TValue>? comparer = null
    )
    {
        return new ImmutableFrozenSet<TValue>(values, comparer);
    }

    /// <summary>
    /// Creates an empty <see cref="ImmutableFrozenSet{TValue}"/>
    /// </summary>
    /// <param name="comparer">The comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>An empty <see cref="ImmutableFrozenSet{TValue}"/></returns>
    public static ImmutableFrozenSet<TValue> Create<TValue>(IEqualityComparer<TValue>? comparer = null)
    {
        return comparer == null ? ImmutableFrozenSet<TValue>.Empty : Create([], comparer);
    }

    /// <summary>
    /// Creates a new <see cref="ImmutableFrozenSet{TValue}"/> that contains the given <paramref name="values"/>
    /// </summary>
    /// <param name="values">The values to create the set with</param>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>The created <see cref="ImmutableFrozenSet{TValue}"/></returns>
    public static ImmutableFrozenSet<TValue> Create<TValue>(ReadOnlySpan<TValue> values)
    {
        return Create(values.ToArray());
    }
}