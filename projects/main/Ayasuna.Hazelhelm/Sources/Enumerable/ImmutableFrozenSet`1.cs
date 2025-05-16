namespace Ayasuna.Hazelhelm.Enumerable;

using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

/// <summary>
/// Unordered <see cref="IImmutableSet{TValue}"/> that is optimized for read operations (lookup and enumeration) at the cost of write operation performance and memory. <br/>
/// Meant to be used in cases where the content of the set is rarely (if ever) updated. <br/>
/// Every write operation is implemented by copying the <b>whole</b> set, similar to how <see cref="ImmutableArray"/> behaves in comparison to <see cref="ImmutableList"/>. <br/>
/// </summary>
/// <typeparam name="TValue">The value type</typeparam>
[CollectionBuilder(typeof(ImmutableFrozenSet), nameof(ImmutableFrozenSet.Create))]
public sealed class ImmutableFrozenSet<TValue> : IImmutableSet<TValue>
{
    /// <summary>
    /// Provides access to the empty set
    /// </summary>
    public static readonly ImmutableFrozenSet<TValue> Empty = [];

    /// <inheritdoc />
    public int Count => _set.Count;

    /// <summary>
    /// Provides access to the comparer used by this set
    /// </summary>
    public IEqualityComparer<TValue> Comparer => _set.Comparer;

    private readonly FrozenSet<TValue> _set;

    private readonly IEqualityComparer<TValue>? _comparer;

    /// <summary>
    /// Creates a new <see cref="ImmutableFrozenSet{TValue}"/> object
    /// </summary>
    /// <param name="values">The values to create the set with</param>
    /// <param name="comparer">The comparer to use, if <c>null</c> <see cref="EqualityComparer{T}.Default"/> is used</param>
    public ImmutableFrozenSet(IEnumerable<TValue> values, IEqualityComparer<TValue>? comparer = null)
    {
        _set = values.ToFrozenSet(comparer);
        _comparer = comparer;
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public IEnumerator<TValue> GetEnumerator()
    {
        return _set.GetEnumerator();
    }

    /// <inheritdoc />
    public bool Contains(TValue value)
    {
        return _set.Contains(value);
    }

    /// <inheritdoc />
    public bool SetEquals(IEnumerable<TValue> other)
    {
        return _set.SetEquals(other);
    }

    /// <inheritdoc />
    public bool IsProperSubsetOf(IEnumerable<TValue> other)
    {
        return _set.IsProperSubsetOf(other);
    }

    /// <inheritdoc />
    public bool IsProperSupersetOf(IEnumerable<TValue> other)
    {
        return _set.IsProperSupersetOf(other);
    }

    /// <inheritdoc />
    public bool IsSubsetOf(IEnumerable<TValue> other)
    {
        return _set.IsSubsetOf(other);
    }

    /// <inheritdoc />
    public bool IsSupersetOf(IEnumerable<TValue> other)
    {
        return _set.IsSupersetOf(other);
    }

    /// <inheritdoc />
    public bool Overlaps(IEnumerable<TValue> other)
    {
        return _set.Overlaps(other);
    }

    /// <inheritdoc />
    public bool TryGetValue(TValue equalValue, out TValue actualValue)
    {
        if (_set.TryGetValue(equalValue, out var found))
        {
            actualValue = found;

            return true;
        }

        actualValue = equalValue;

        return false;
    }

    /// <inheritdoc />
    public IImmutableSet<TValue> Add(TValue value)
    {
        if (_set.Contains(value))
        {
            return this;
        }

        return new ImmutableFrozenSet<TValue>
        (
            _set.Concat([value]),
            _comparer
        );
    }

    /// <inheritdoc />
    public IImmutableSet<TValue> Clear()
    {
        return new ImmutableFrozenSet<TValue>([], _comparer);
    }

    /// <inheritdoc />
    public IImmutableSet<TValue> Remove(TValue value)
    {
        if (_set.Contains(value))
        {
            var mutable = _set.ToHashSet(_set.Comparer);
            mutable.Remove(value);

            return new ImmutableFrozenSet<TValue>
            (
                mutable,
                _comparer
            );
        }

        return this;
    }

    /// <inheritdoc />
    public IImmutableSet<TValue> Intersect(IEnumerable<TValue> other)
    {
        return new ImmutableFrozenSet<TValue>
        (
            _set.Intersect(other, _set.Comparer),
            _comparer
        );
    }

    /// <inheritdoc />
    public IImmutableSet<TValue> SymmetricExcept(IEnumerable<TValue> other)
    {
        return new ImmutableFrozenSet<TValue>
        (
            _set.ToImmutableHashSet(_set.Comparer).SymmetricExcept(other),
            _comparer
        );
    }

    /// <inheritdoc />
    public IImmutableSet<TValue> Except(IEnumerable<TValue> other)
    {
        var except = _set.ToImmutableHashSet(_set.Comparer).Except(other);

        if (except.Count == Count)
        {
            return this;
        }

        return new ImmutableFrozenSet<TValue>
        (
            except,
            _comparer
        );
    }

    /// <inheritdoc />
    public IImmutableSet<TValue> Union(IEnumerable<TValue> other)
    {
        var union = _set.ToImmutableHashSet(_set.Comparer).Union(other);

        if (union.Count == Count)
        {
            return this;
        }

        return new ImmutableFrozenSet<TValue>
        (
            union,
            _comparer
        );
    }
}