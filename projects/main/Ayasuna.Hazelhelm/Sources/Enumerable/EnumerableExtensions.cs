namespace Ayasuna.Hazelhelm.Enumerable;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

/// <summary>
/// Adds extension methods to the <see cref="IEnumerable{T}"/> interface
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Adds the given <paramref name="key"/> with the given <paramref name="value"/> to a copy of <paramref name="this"/> dictionary, possibly overwriting
    /// the existing value for the key.
    /// </summary>
    /// <param name="this">This dictionary</param>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    /// <returns>The new immutable dictionary that contains the given <paramref name="key"/> and <paramref name="value"/></returns>
    public static IImmutableDictionary<TKey, TValue> Put<TKey, TValue>(this IImmutableDictionary<TKey, TValue> @this, TKey key, TValue value)
    {
        return @this.SetItem(key, value);
    }

    /// <summary>
    /// Adds the given key-value <paramref name="pairs"/> to a copy of <paramref name="this"/> dictionary, possibly overwriting
    /// any existing values for the keys.
    /// </summary>
    /// <param name="this">This dictionary</param>
    /// <param name="pairs">The key value pairs</param>
    /// <returns>The new immutable dictionary that contains the given <paramref name="pairs"/></returns>
    public static IImmutableDictionary<TKey, TValue> PutAll<TKey, TValue>(this IImmutableDictionary<TKey, TValue> @this, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
        return @this.SetItems(pairs);
    }

    /// <summary>
    /// Adds the given <paramref name="key"/> with the given <paramref name="value"/> to <paramref name="this"/> dictionary, possibly overwriting
    /// the existing value for the key.
    /// </summary>
    /// <param name="this">This dictionary</param>
    /// <param name="key">The key</param>
    /// <param name="value">The value</param>
    /// <returns>This dictionary after the key has been added</returns>
    public static IDictionary<TKey, TValue> Put<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue value)
    {
        @this[key] = value;

        return @this;
    }

    /// <summary>
    /// Adds the given key-value <paramref name="pairs"/> to this <paramref name="this"/> dictionary, possibly overwriting
    /// any existing values for the keys.
    /// </summary>
    /// <param name="this">This dictionary</param>
    /// <param name="pairs">The key value pairs</param>
    /// <returns>This dictionary after the keys have been added</returns>
    public static IDictionary<TKey, TValue> PutAll<TKey, TValue>(this IDictionary<TKey, TValue> @this, IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
        foreach (var (key, value) in pairs)
        {
            @this[key] = value;
        }

        return @this;
    }

    
    /// <summary>
    /// Creates a new <see cref="ImmutableFrozenDictionary{TKey,TValue}"/> from <paramref name="this"/> enumerable
    /// </summary>
    /// <param name="this">The enumerable to create the dictionary from</param>
    /// <param name="keySelector">The function to call to get the key for a <see cref="TValue"/></param>
    /// <param name="keyComparer">The key comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <param name="valueComparer">The value comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>The created <see cref="ImmutableFrozenDictionary{TKey,TValue}"/></returns>
    public static ImmutableFrozenDictionary<TKey, TValue> ToImmutableFrozenDictionary<TKey, TValue>
    (
        this IEnumerable<TValue> @this,
        Func<TValue, TKey> keySelector,
        IEqualityComparer<TKey>? keyComparer = null,
        IEqualityComparer<TValue>? valueComparer = null
    ) where TKey : notnull
    {
        return @this
            .Select(e => new KeyValuePair<TKey,TValue>(keySelector(e), e))
            .ToImmutableFrozenDictionary(keyComparer, valueComparer);
    }
    
    /// <summary>
    /// Creates a new <see cref="ImmutableFrozenDictionary{TKey,TValue}"/> from <paramref name="this"/> enumerable
    /// </summary>
    /// <param name="this">The enumerable to create the dictionary from</param>
    /// <param name="keySelector">The function to call to get the key for a <see cref="TElement"/></param>
    /// <param name="valueSelector">The function to call to get the value for a <see cref="TElement"/></param>
    /// <param name="keyComparer">The key comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <param name="valueComparer">The value comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <typeparam name="TElement">The element type of <paramref name="this"/> enumerable</typeparam>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>The created <see cref="ImmutableFrozenDictionary{TKey,TValue}"/></returns>
    public static ImmutableFrozenDictionary<TKey, TValue> ToImmutableFrozenDictionary<TElement, TKey, TValue>
    (
        this IEnumerable<TElement> @this,
        Func<TElement, TKey> keySelector,
        Func<TElement, TValue> valueSelector,
        IEqualityComparer<TKey>? keyComparer = null,
        IEqualityComparer<TValue>? valueComparer = null
    ) where TKey : notnull
    {
        return @this
            .Select(e => new KeyValuePair<TKey,TValue>(keySelector(e), valueSelector(e)))
            .ToImmutableFrozenDictionary(keyComparer, valueComparer);
    }

    /// <summary>
    /// Creates a new <see cref="ImmutableFrozenDictionary{TKey,TValue}"/> from <paramref name="this"/> enumerable
    /// </summary>
    /// <param name="this">The key-value pairs to create the dictionary with</param>
    /// <param name="keyComparer">The key comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <param name="valueComparer">The value comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>The created <see cref="ImmutableFrozenDictionary{TKey,TValue}"/></returns>
    public static ImmutableFrozenDictionary<TKey, TValue> ToImmutableFrozenDictionary<TKey, TValue>
    (
        this IEnumerable<KeyValuePair<TKey, TValue>> @this,
        IEqualityComparer<TKey>? keyComparer = null,
        IEqualityComparer<TValue>? valueComparer = null
    ) where TKey : notnull
    {
        return ImmutableFrozenDictionary.Create(@this, keyComparer, valueComparer);
    }

    /// <summary>
    /// Creates a new <see cref="ImmutableFrozenSet{TValue}"/> from <paramref name="this"/> enumerable
    /// </summary>
    /// <param name="this">The values to create the set with</param>
    /// <param name="comparer">The comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <typeparam name="TValue">The value type</typeparam>
    /// <returns>The created <see cref="ImmutableFrozenSet{TValue}"/></returns>
    public static ImmutableFrozenSet<TValue> ToImmutableFrozenSet<TValue>
    (
        this IEnumerable<TValue> @this,
        IEqualityComparer<TValue>? comparer = null
    )
    {
        return ImmutableFrozenSet.Create(@this, comparer);
    }
}