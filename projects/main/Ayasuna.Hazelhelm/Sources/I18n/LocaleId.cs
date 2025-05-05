namespace Ayasuna.Hazelhelm.I18n;

using System;

/// <summary>
/// Represents the ID of a <see cref="Locale"/>
/// </summary>
public sealed class LocaleId : IEquatable<LocaleId>, IComparable<LocaleId>
{
    /// <summary>
    /// Provides access to the language part of the ID
    /// </summary>
    public string Language { get; }

    /// <summary>
    /// Provides access to the country part of the ID
    /// </summary>
    public string Country { get; }

    /// <summary>
    /// Constructs a new <see cref="LocaleId"/> object
    /// </summary>
    /// <param name="language">The language</param>
    /// <param name="country">The country</param>
    internal LocaleId(string language, string country)
    {
        Language = language;
        Country = country;
    }

    /// <summary>
    /// Converts this <see cref="LocaleId"/> to its string representation. <br/>
    /// Always in the format <c>Language-Country</c>
    /// </summary>
    /// <returns>The string representation of this <see cref="LocaleId"/></returns>
    public override string ToString()
    {
        return $"{Language}-{Country}";
    }

    /// <inheritdoc />
    public bool Equals(LocaleId? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return string.Equals(Language, other.Language, StringComparison.Ordinal) &&
               string.Equals(Country, other.Country, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is LocaleId other && Equals(other);
    }

    /// <inheritdoc />
    public int CompareTo(LocaleId? other)
    {
        return string.Compare(ToString(), other?.ToString(), StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Language, Country);
    }
}