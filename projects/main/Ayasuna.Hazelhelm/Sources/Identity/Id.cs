namespace Ayasuna.Hazelhelm.Identity;

using System;

/// <summary>
/// Uniquely identifies an entity. The scope (e.g. globally, current application run, current node) in which the id uniquely identifies an entity is undefined.
/// </summary>
/// <remarks>
/// <see cref="Id"/> objects are considered equal if their (entity) <see cref="Type"/> and <see cref="Value"/> are equal even if their concrete implementation type differs. 
/// </remarks>
public abstract class Id : IEquatable<Id>, IComparable<Id>
{
    /// <summary>
    /// The type of the entity for which the id is
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// The actual value of the id, which is also the string representation of the id. 
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Constructs a new <see cref="Id"/> object
    /// </summary>
    /// <param name="type">The entity type of the id</param>
    /// <param name="value">The raw value of the id</param>
    protected Id(Type type, string value)
    {
        Type = type;
        Value = value;
    }

    /// <inheritdoc />
    public bool Equals(Id other)
    {
        return Type == other.Type && Value.Equals(other.Value);
    }

    /// <inheritdoc />
    public int CompareTo(Id other)
    {
        if (Type == other.Type)
        {
            return string.Compare(Value, other.Value, StringComparison.Ordinal);
        }

        return string.Compare(Type.FullName, other.Type.FullName, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public sealed override bool Equals(object obj)
    {
        if (obj is Id asId)
        {
            return Equals(asId);
        }

        return false;
    }

    /// <inheritdoc />
    public sealed override int GetHashCode()
    {
        return HashCode.Combine(Type, Value);
    }
    

    /// <summary>
    /// Returns the string representation of this <see cref="Id"/> which is always just the <see cref="Value"/> of the id.
    /// </summary>
    /// <returns>The <see cref="Value"/> of the id</returns>
    public sealed override string ToString()
    {
        return Value;
    }
}