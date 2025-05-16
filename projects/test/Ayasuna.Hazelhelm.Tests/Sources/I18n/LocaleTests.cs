namespace Ayasuna.Hazelhelm.Tests.I18n;

using System.Globalization;
using Hazelhelm.I18n;
using Xunit;

/// <summary>
/// Defines the unit tests for the <see cref="Locale"/> type
/// </summary>
public sealed class LocaleTests
{
    private const string EnUs = "en-US";

    private const string DeDe = "de-DE";

    /// <summary>
    /// Tests whether two <see cref="Locale">locales</see> are equal to one another if they were created from the same <see cref="CultureInfo"/>
    /// </summary>
    [Fact]
    public void Two_locales_should_be_equal_if_they_were_created_from_the_same_culture()
    {
        var culture = CultureInfo.GetCultureInfo(DeDe);

        var first = Locale.Get(culture);
        var second = Locale.Get(culture);

        Assert.Equal(first, second);
        Assert.Equal(second, first);

        Assert.True(first == second);
        Assert.True(second == first);

        Assert.False(first != second);
        Assert.False(second != first);

        // The locales be the *same* as the locales are cached
        Assert.Same(first, second);
        // While generally not guaranteed that the culture of the locale is the same as the culture for which the locale was looked up they should be the same in this case
        // as the culture we used for the lookup is not a custom culture.
        Assert.Same(culture, first.Culture);
    }

    /// <summary>
    /// Tests whether two <see cref="Locale">locales</see> are not equal to one another if the culture from which they were created was different
    /// </summary>
    [Fact]
    public void Two_locales_should_not_be_equal_if_they_were_created_from_different_cultures()
    {
        var firstCulture = CultureInfo.GetCultureInfo(DeDe);
        var secondCulture = CultureInfo.GetCultureInfo(EnUs);

        var first = Locale.Get(firstCulture);
        var second = Locale.Get(secondCulture);

        Assert.NotEqual(first, second);
        Assert.NotEqual(second, first);

        Assert.False(first == second);
        Assert.False(second == first);

        Assert.True(first != second);
        Assert.True(second != first);

        Assert.NotSame(first, second);
        Assert.NotSame(first.Id, second.Id);
        Assert.NotSame(first.Culture, second.Culture);
    }

    /// <summary>
    /// Tests whether two <see cref="Locale.TryParse"/> is not able to parse invalid locale strings
    /// </summary>
    /// <param name="localeId">The locale ID</param>
    [Theory]
    [InlineData("INVALID")]
    [InlineData("invalid")]
    [InlineData("IN-va")]
    [InlineData("de_DE")]
    [InlineData("en_US")]
    public void TryParse_should_not_be_able_to_parse_invalid_locale_strings(string localeId)
    {
        Assert.False(Locale.TryParse(localeId, out _));
    }

    /// <summary>
    /// Tests whether two <see cref="Locale.TryParse"/> is able to parse valid locale strings
    /// </summary>
    /// <param name="localeId">The locale ID</param>
    [Theory]
    [InlineData(DeDe)]
    [InlineData(EnUs)]
    [InlineData("en-GB")]
    [InlineData("de-AT")]
    [InlineData("ja-JP")]
    public void TryParse_should_be_able_to_parse_valid_locale_strings(string localeId)
    {
        Assert.True(Locale.TryParse(localeId, out _));
    }

    /// <summary>
    /// Tests whether it is possible to convert a <see cref="Locale"/> to a <see cref="CultureInfo"/>
    /// </summary>
    /// <param name="localeId">The locale ID</param>
    [Theory]
    [InlineData(DeDe)]
    [InlineData(EnUs)]
    public void It_should_be_possible_to_convert_a_Locale_to_a_CultureInfo(string localeId)
    {
        var parsed = Locale.Parse(localeId);

        CultureInfo culture = parsed;

        Assert.NotNull(culture);
        Assert.NotNull(culture.Name);
    }

    /// <summary>
    /// Tests whether it is possible to convert a <see cref="CultureInfo"/> to a <see cref="Locale"/>
    /// </summary>
    /// <param name="localeId">The locale ID</param>
    [Theory]
    [InlineData(DeDe)]
    [InlineData(EnUs)]
    public void It_should_be_possible_to_convert_a_CultureInfo_to_a_Locale(string localeId)
    {
        var culture = CultureInfo.GetCultureInfo(localeId);

        Locale locale = culture;

        Assert.NotNull(locale);
        Assert.NotNull(locale.Culture);
    }


    /// <summary>
    /// Tests whether the string representation of a <see cref="Locale"/> is equal to the <paramref name="localeId"/>
    /// </summary>
    /// <param name="localeId">The locale ID</param>
    [Theory]
    [InlineData(DeDe)]
    [InlineData(EnUs)]
    [InlineData("en-GB")]
    [InlineData("de-AT")]
    [InlineData("ja-JP")]
    public void The_string_representation_of_a_locale_should_be_equal_to_the_locale_id(string localeId)
    {
        var locale = Locale.Parse(localeId);

        Assert.Equal(localeId, locale.ToString());
        Assert.Equal(locale.Id.ToString(), locale.ToString());
    }
}