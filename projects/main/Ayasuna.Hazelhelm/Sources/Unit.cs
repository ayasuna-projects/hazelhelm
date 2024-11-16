namespace Ayasuna.Hazelhelm;

using System;

/// <summary>
/// The <see cref="Unit"/> type is a type that has only one value.
/// </summary>
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>
{
    /// <summary>
    /// Provides access to the value of the unit type. <br/>
    /// This is intended to be the default way to access the value of the unit type even though <c>new Unit()</c> creates an instance with the same value. 
    /// </summary>
    public static readonly Unit Value = new();

    /// <summary>
    /// Creates a new <see cref="Unit"/> instance. 
    /// </summary>
    public Unit()
    {
    }

    /// <inheritdoc />
    public bool Equals(Unit other)
    {
        return true;
    }

    /// <inheritdoc />
    public int CompareTo(Unit other)
    {
        return 0;
    }
    
    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Unit;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return 0;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return "()";
    }

    /// <summary>
    /// Compares the given left hand side (<paramref name="lh"/>) unit value with the given right hand side (<paramref name="rh"/>) unit value for equality, which will always return <c>true</c> as all instances of the unit type share the same value. 
    /// </summary>
    /// <param name="lh">The left hand side</param>
    /// <param name="rh">The right hand side</param>
    /// <returns><c>true</c></returns>
    public static bool operator ==(Unit lh, Unit rh)
    {
        return true;
    }
    
    /// <summary>
    /// Compares the given left hand side (<paramref name="lh"/>) unit value with the given right hand side (<paramref name="rh"/>) unit value for inequality, which will always return <c>false</c> as all instances of the unit type share the same value. 
    /// </summary>
    /// <param name="lh">The left hand side</param>
    /// <param name="rh">The right hand side</param>
    /// <returns><c>false</c></returns>
    public static bool operator !=(Unit lh, Unit rh)
    {
        return false;
    }
    
    
    
}