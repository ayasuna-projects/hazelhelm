namespace Ayasuna.Hazelhelm;

using System;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Adds extension methods to the <see cref="object"/> type
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Returns <paramref name="this"/> object if it is not <c>null</c> otherwise throws an exception
    /// </summary>
    /// <param name="this">This object</param>
    /// <typeparam name="T">The object type</typeparam>
    /// <returns>This object</returns>
    public static T NotNull<T>(this T? @this)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(@this);

        return @this;
    }

    /// <summary>
    /// Casts <paramref name="this"/> object to an object of type <typeparamref name="T"/>
    /// </summary>
    /// <param name="this">This object</param>
    /// <typeparam name="T">The target type</typeparam>
    /// <returns>This object as an object of type <typeparamref name="T"/></returns>
    /// <exception cref="InvalidCastException">If <paramref name="this"/> object is not of type <typeparamref name="T"/></exception>
    [return: NotNullIfNotNull(nameof(@this))]
    public static T? Cast<T>(this object? @this)
    {
        return @this.CastOrThrow<T>();
    }

    /// <summary>
    /// Casts <paramref name="this"/> object to an object of type <typeparamref name="T"/>
    /// </summary>
    /// <param name="this">This object</param>
    /// <typeparam name="T">The target type</typeparam>
    /// <returns>This object as an object of type <typeparamref name="T"/></returns>
    /// <exception cref="InvalidCastException">If <paramref name="this"/> object is not of type <typeparamref name="T"/></exception>
    [return: NotNullIfNotNull(nameof(@this))]
    public static T? CastOrThrow<T>(this object? @this)
    {
        return (T?)@this;
    }

    /// <summary>
    /// Casts <paramref name="this"/> object to an object of type <typeparamref name="T"/> or returns the given <paramref name="defaultValue"/>
    /// if <paramref name="this"/> object is not of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="this">This object</param>
    /// <param name="defaultValue">The default value</param>
    /// <typeparam name="T">The target type</typeparam>
    /// <returns>This object as an object of type <typeparamref name="T"/> or the given <paramref name="defaultValue"/></returns>
    [return: NotNullIfNotNull(nameof(defaultValue))]
    public static T? CastOrDefault<T>(this object? @this, T? defaultValue = default)
    {
        if (@this is T casted)
        {
            return casted;
        }

        return defaultValue;
    }
}