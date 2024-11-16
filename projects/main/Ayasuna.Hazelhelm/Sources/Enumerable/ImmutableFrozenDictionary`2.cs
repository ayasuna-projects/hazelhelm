namespace Ayasuna.Hazelhelm.Enumerable;

using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

/// <summary>
/// <see cref="IImmutableDictionary{TKey,TValue}"/> that is optimized for read operations (lookup and enumeration) at the cost of write operation performance and memory. <br/>
/// Meant to be used in cases where the content of the dictionary is rarely (if ever) updated. <br/>
/// Every write operation is implemented by copying the <b>whole</b> dictionary, similar to how <see cref="ImmutableArray"/> behaves in comparison to <see cref="ImmutableList"/>. <br/>
/// </summary>
/// <typeparam name="TKey">The key type</typeparam>
/// <typeparam name="TValue">The value type</typeparam>
[CollectionBuilder(typeof(ImmutableFrozenDictionary), nameof(ImmutableFrozenDictionary.Create))]
public sealed class ImmutableFrozenDictionary<TKey, TValue> : IImmutableDictionary<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// Provides access to the empty dictionary
    /// </summary>
    public static readonly ImmutableFrozenDictionary<TKey, TValue> Empty = [];

    /// <inheritdoc />
    public int Count => _dictionary.Count;

    /// <inheritdoc />
    public IEnumerable<TKey> Keys => _dictionary.Keys;

    /// <inheritdoc />
    public IEnumerable<TValue> Values => _dictionary.Values;

    /// <inheritdoc />
    public TValue this[TKey key] => _dictionary[key];

    private readonly FrozenDictionary<TKey, TValue> _dictionary;

    private readonly ImmutableFrozenSet<TKey> _keys;

    private readonly IEqualityComparer<TKey>? _keyComparer;

    private readonly IEqualityComparer<TValue>? _valueComparer;
    
    /// <summary>
    /// Constructs a new <see cref="ImmutableFrozenDictionary{TKey,TValue}"/> object
    /// </summary>
    /// <param name="pairs">The key-value pairs to create the dictionary with</param>
    /// <param name="keyComparer">The key comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    /// <param name="valueComparer">The value comparer to use, if null <see cref="EqualityComparer{T}.Default"/> is used</param>
    public ImmutableFrozenDictionary
    (
        IEnumerable<KeyValuePair<TKey, TValue>> pairs,
        IEqualityComparer<TKey>? keyComparer = null,
        IEqualityComparer<TValue>? valueComparer = null
    )
    {
        _dictionary = pairs.ToFrozenDictionary(keyComparer);
        _keys = _dictionary.Keys.ToImmutableFrozenSet(_dictionary.Comparer);
        _keyComparer = keyComparer;
        _valueComparer = valueComparer;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    /// <inheritdoc />
    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    /// <inheritdoc />
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    /// <inheritdoc />
    public IImmutableDictionary<TKey, TValue> Clear()
    {
        return new ImmutableFrozenDictionary<TKey, TValue>([], _keyComparer, _valueComparer);
    }

    /// <inheritdoc />
    public bool Contains(KeyValuePair<TKey, TValue> pair)
    {
        if (TryGetValue(pair.Key, out var value))
        {
            return _valueComparer?.Equals(pair.Value, value) ?? EqualityComparer<TValue>.Default.Equals(pair.Value, value);
        }

        return false;
    }

    /// <inheritdoc />
    public IImmutableDictionary<TKey, TValue> Add(TKey key, TValue value)
    {
        return AddRange([new KeyValuePair<TKey, TValue>(key, value)]);
    }

    /// <inheritdoc />
    public IImmutableDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
        return new ImmutableFrozenDictionary<TKey, TValue>
        (
            _dictionary.Concat(pairs),
            _keyComparer,
            _valueComparer
        );
    }

    /// <inheritdoc />
    public IImmutableDictionary<TKey, TValue> Remove(TKey key)
    {
        return RemoveRange([key]);
    }

    /// <inheritdoc />
    public IImmutableDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys)
    {
        var toRemoveKeys = keys.ToImmutableHashSet(_dictionary.Comparer);

        return new ImmutableFrozenDictionary<TKey, TValue>
        (
            _dictionary.Where(e => !toRemoveKeys.Contains(e.Key)),
            _keyComparer,
            _valueComparer
        );
    }

    /// <inheritdoc />
    public IImmutableDictionary<TKey, TValue> SetItem(TKey key, TValue value)
    {
        return SetItems([new KeyValuePair<TKey, TValue>(key, value)]);
    }

    /// <inheritdoc />
    public IImmutableDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        var asMutable = _dictionary.ToDictionary(_dictionary.Comparer);

        foreach (var (key, value) in items)
        {
            asMutable[key] = value;
        }

        return new ImmutableFrozenDictionary<TKey, TValue>(asMutable, _keyComparer, _valueComparer);
    }

    /// <inheritdoc />
    public bool TryGetKey(TKey equalKey, out TKey actualKey)
    {
        return _keys.TryGetValue(equalKey, out actualKey);
    }
}