namespace Ayasuna.Hazelhelm.Enumerable;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

/// <summary>
/// Contains helper methods for the <see cref="ImmutableFrozenDictionary{TKey,TValue}"/> type
/// </summary>
public static class ImmutableFrozenDictionary
{
    /// <summary>
    /// Creates a new <see cref="ImmutableFrozenDictionary{TKey,TValue}"/> that contains the given <paramref name="pairs"/>
    /// </summary>
    /// <param name="pairs">The key-value pairs to create the dictionary with</param>
    /// <param name="keyComparer">The key comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <param name="valueComparer">The value comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>The created <see cref="ImmutableFrozenDictionary{TKey,TValue}"/></returns>
    public static ImmutableFrozenDictionary<TKey, TValue> Create<TKey, TValue>
    (
        IEnumerable<KeyValuePair<TKey, TValue>> pairs,
        IEqualityComparer<TKey>? keyComparer = null,
        IEqualityComparer<TValue>? valueComparer = null
    ) where TKey : notnull
    {
        return new ImmutableFrozenDictionary<TKey, TValue>(pairs, keyComparer, valueComparer);
    }

    /// <summary>
    /// Creates an empty <see cref="ImmutableFrozenDictionary{TKey,TValue}"/>
    /// </summary>
    /// <param name="keyComparer">The key comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <param name="valueComparer">The value comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>An empty <see cref="ImmutableFrozenDictionary{TKey,TValue}"/></returns>
    public static ImmutableFrozenDictionary<TKey, TValue> Create<TKey, TValue>
    (
        IEqualityComparer<TKey>? keyComparer = null,
        IEqualityComparer<TValue>? valueComparer = null
    ) where TKey : notnull
    {
        if (keyComparer == null && valueComparer == null)
        {
            return ImmutableFrozenDictionary<TKey, TValue>.Empty;
        }

        return Create([], keyComparer, valueComparer);
    }

    /// <summary>
    /// Creates a new <see cref="ImmutableFrozenDictionary{TKey,TValue}"/> that contains the given <paramref name="pairs"/>
    /// </summary>
    /// <param name="pairs">The key-value pairs to create the dictionary with</param>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>The created <see cref="ImmutableFrozenDictionary{TKey,TValue}"/></returns>
    public static ImmutableFrozenDictionary<TKey, TValue> Create<TKey, TValue>(ReadOnlySpan<KeyValuePair<TKey, TValue>> pairs) where TKey : notnull
    {
        return Create(pairs.ToArray());
    }
}