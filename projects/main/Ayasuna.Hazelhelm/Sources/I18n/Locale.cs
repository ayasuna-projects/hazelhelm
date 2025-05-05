namespace Ayasuna.Hazelhelm.I18n;

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

/// <summary>
/// Represents a locale which is essentially just a lightweight wrapper around <see cref="CultureInfo"/>
/// </summary>
public sealed class Locale : IEquatable<Locale>, IComparable<Locale>
{
    private static readonly ConcurrentDictionary<string, Locale> Locales = new();

    /// <summary>
    /// Provides access to the ID of the locale
    /// </summary>
    public LocaleId Id { get; }

    /// <summary>
    /// Provides access to the culture of the locale
    /// </summary>
    public CultureInfo Culture { get; }

    /// <summary>
    /// Constructs a new <see cref="Locale"/> object
    /// </summary>
    /// <param name="id">The ID of the locale</param>
    /// <param name="culture">The culture of the locale</param>
    private Locale(LocaleId id, CultureInfo culture)
    {
        Id = id;
        Culture = culture;
    }

    /// <summary>
    /// Converts this <see cref="Locale"/> to its string representation. <br/>
    /// </summary>
    /// <remarks>
    /// The string representation is always in the following format <c>ISO_639_ALPHA_2-ISO_3166_ALPHA_2</c> e.g. <c>en-US</c>
    /// </remarks>
    /// <returns>The string representation of this <see cref="Locale"/></returns>
    public override string ToString()
    {
        return Id.ToString();
    }

    /// <inheritdoc />
    public bool Equals(Locale? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Id.Equals(other.Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Locale other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <inheritdoc />
    public int CompareTo(Locale? other)
    {
        return Id.CompareTo(other?.Id);
    }

    /// <summary>
    /// Parses the given <paramref name="value"/> into a locale. <br/>
    /// The string must conform to the following format <c>ISO_639_ALPHA_2-ISO_3166_ALPHA_2</c> e.g. <c>en-US</c>.
    /// </summary>
    /// <param name="value">The string to parse</param>
    /// <returns>The parsed locale</returns>
    public static Locale Parse(string value)
    {
        if (!TryParse(value, out var result))
        {
            throw new ArgumentException($"The given string '{value}' does not represent a valid locale", nameof(value));
        }
    
        return result;
    }
    
    /// <summary>
    /// Tries to parse the given <paramref name="value"/> into a locale. <br/>
    /// Only strings with the following format <c>ISO_639_ALPHA_2-ISO_3166_ALPHA_2</c> e.g. <c>en-US</c> can be parsed.
    /// </summary>
    /// <param name="value">The string to parse</param>
    /// <param name="result">The variable to store the parse result into</param>
    /// <returns><c>true</c> if the locale could be parsed, <c>false</c> otherwise</returns>
    public static bool TryParse(string? value, [NotNullWhen(true)] out Locale? result)
    {
        while (true)
        {
            if (string.IsNullOrEmpty(value) || value.Length != 5)
            {
                result = null;
                return false;
            }

            if (Locales.TryGetValue(value, out var found))
            {
                result = found;
                return true;
            }

            var parts = value.Split('-');

            if (parts.Length != 2 ||
                parts[0].Length != 2 ||
                parts[1].Length != 2 ||
                !parts[0].ToLowerInvariant().Equals(parts[0], StringComparison.Ordinal) ||
                !parts[1].ToUpperInvariant().Equals(parts[1], StringComparison.Ordinal))
            {
                result = null;
                return false;
            }

            var localeId = new LocaleId(parts[0], parts[1]);
            var culture = CultureInfo.GetCultureInfo(value, true);

            found = new Locale(localeId, culture);

            if (Locales.TryAdd(value, found))
            {
                result = found;
                return true;
            }
        }
    }
    
    /// <summary>
    /// Gets the <see cref="Locale"/> for the given <paramref name="culture"/>. <br/>
    /// </summary>
    /// <remarks>
    /// The <see cref="Culture"/> of the returned <see cref="Locale"/> <b>might</b> differ from the given <paramref name="culture"/>
    /// as the locale is internally resolved by using the <see cref="CultureInfo.Name"/> of the given <paramref name="culture"/>.  
    /// </remarks>
    /// <param name="culture">The culture to get the locale for</param>
    /// <returns>The locale for the given <paramref name="culture"/></returns>
    public static Locale Get(CultureInfo culture)
    {
        return Parse(culture.Name);
    }
    
    /// <summary>
    /// Gets the <see cref="Locale"/> with the given <paramref name="id"/>
    /// </summary>
    /// <param name="id">The ID of the locale to return</param>
    /// <returns>The locale with the given <paramref name="id"/></returns>
    public static Locale Get(LocaleId id)
    {
        return Parse(id.ToString());
    }
    
    /// <summary>
    /// Compares the given <paramref name="left"/> locale with the given <paramref name="right"/> locale for equality
    /// </summary>
    /// <param name="left">The left locale</param>
    /// <param name="right">The right locale</param>
    /// <returns><c>true</c> if both locales are equal, <c>false</c> otherwise</returns>
    public static bool operator ==(Locale left, Locale right)
    {
        return left.Equals(right);
    }
    
    /// <summary>
    /// Compares the given <paramref name="left"/> locale with the given <paramref name="right"/> locale for inequality
    /// </summary>
    /// <param name="left">The left locale</param>
    /// <param name="right">The right locale</param>
    /// <returns><c>true</c> if both locales are not equal, <c>false</c> otherwise</returns>
    public static bool operator !=(Locale left, Locale right)
    {
        return !(left == right);
    }
    
    /// <summary>
    /// Converts the given <paramref name="locale"/> to a <see cref="CultureInfo"/>. <br/>
    /// </summary>
    /// <param name="locale">The locale</param>
    /// <returns>The culture</returns>
    public static implicit operator CultureInfo(Locale locale)
    {
        return locale.Culture;
    }
    
    /// <summary>
    /// Converts the given <paramref name="culture"/> to a <see cref="Locale"/>. <br/>
    /// </summary>
    /// <param name="culture">The culture</param>
    /// <returns>The locale</returns>
    public static implicit operator Locale(CultureInfo culture)
    {
        return Get(culture);
    }
}