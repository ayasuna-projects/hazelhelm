namespace Ayasuna.Hazelhelm.I18n;

using System;
using System.Globalization;

/// <summary>
/// Represents a locale which is essentially just a lightweight wrapper around <see cref="CultureInfo"/>
/// </summary>
public readonly struct Locale : IEquatable<Locale>, IComparable<Locale>
{
    private static readonly CultureInfo DefaultCulture = CultureInfo.GetCultureInfo("en-US", true);

    /// <summary>
    /// Provides access to the culture of the locale
    /// </summary>
    public CultureInfo Culture => _culture ?? DefaultCulture;

    private readonly CultureInfo? _culture;

    /// <summary>
    /// Constructs a new <see cref="Locale"/> object
    /// </summary>
    public Locale()
        : this(null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="Locale"/>
    /// </summary>
    /// <param name="culture">The culture</param>
    private Locale(CultureInfo? culture)
    {
        _culture = culture;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Culture.Name;
    }

    /// <inheritdoc />
    public bool Equals(Locale other)
    {
        return Culture.Name.Equals(other.Culture.Name);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Locale other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Culture.Name.GetHashCode();
    }

    /// <inheritdoc />
    public int CompareTo(Locale other)
    {
        return string.Compare(Culture.Name, other.Culture.Name, StringComparison.Ordinal);
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
    /// Tries to parse the given <paramref name="value"/> into a locale. <br/>
    /// Only strings with the following format <c>ISO_639_ALPHA_2-ISO_3166_ALPHA_2</c> e.g. <c>en-US</c> can be parsed.
    /// </summary>
    /// <param name="value">The string to parse</param>
    /// <param name="result">The variable to store the parse result into</param>
    /// <returns><c>true</c> if the locale could be parsed, <c>false</c> otherwise</returns>
    public static bool TryParse(string? value, out Locale result)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        var parts = value.Split('-');

        if (parts.Length != 2 ||
            parts[0].Length != 2 ||
            parts[1].Length != 2 ||
            !parts[0].ToLowerInvariant().Equals(parts[0]) ||
            !parts[1].ToUpperInvariant().Equals(parts[1]))
        {
            result = default;
            return false;
        }

        result = new Locale(CultureInfo.GetCultureInfo(value, true));

        return true;
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