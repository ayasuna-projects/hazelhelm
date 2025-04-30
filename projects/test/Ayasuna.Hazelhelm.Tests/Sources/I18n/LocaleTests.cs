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
    /// Tests whether the default locale is equal to en-US
    /// </summary>
    [Fact]
    public void The_default_locale_should_be_equal_to_en_US()
    {
        Locale toTest = default;

        Assert.Equal(EnUs, toTest.ToString());
        Assert.Equal(EnUs, toTest.Culture.Name);
        Assert.Equal(toTest, Locale.Parse(EnUs));
        Assert.Equal(toTest, Locale.Get(CultureInfo.GetCultureInfo(EnUs)));
        Assert.Equal(toTest, new Locale());
    }

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
        
        Assert.Same(first.Culture, second.Culture);
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
        
        Assert.NotSame(first.Culture, second.Culture);
    }
    
    /// <summary>
    /// Tests whether two <see cref="Locale.TryParse"/> is not able to parse invalid locale strings
    /// </summary>
    [Fact]
    public void TryParse_should_not_be_able_to_parse_invalid_locale_strings()
    {
        Assert.False(Locale.TryParse("INVALID", out _));
        Assert.False(Locale.TryParse("invalid", out _));
        Assert.False(Locale.TryParse("IN-va", out _));
        Assert.False(Locale.TryParse("de_DE", out _));
        Assert.False(Locale.TryParse("en_US", out _));
    }
    
    /// <summary>
    /// Tests whether two <see cref="Locale.TryParse"/> is able to parse valid locale strings
    /// </summary>
    [Fact]
    public void TryParse_should_not_be_able_to_parse_valid_locale_strings()
    {
        Assert.True(Locale.TryParse(EnUs, out _));
        Assert.True(Locale.TryParse(DeDe, out _));
        Assert.True(Locale.TryParse("en-GB", out _));
        Assert.True(Locale.TryParse("de-AT", out _));
        Assert.True(Locale.TryParse("ja-JP", out _));
    }
    
    /// <summary>
    /// Tests whether it is possible to convert a <see cref="Locale"/> to a <see cref="CultureInfo"/>
    /// </summary>
    [Fact]
    public void It_should_be_possible_to_convert_a_Locale_to_a_CultureInfo()
    {
        var l1 = default(Locale);
        var l2 = new Locale();
        var l3 = Locale.Parse(DeDe);

        CultureInfo c1 = l1;
        CultureInfo c2 = l2;
        CultureInfo c3 = l3;

        Assert.NotNull(c1);
        Assert.NotNull(c2);
        Assert.NotNull(c3);
    }
    
    /// <summary>
    /// Tests whether it is possible to convert a <see cref="CultureInfo"/> to a <see cref="Locale"/>
    /// </summary>
    [Fact]
    public void It_should_be_possible_to_convert_a_CultureInfo_to_a_Locale()
    {
        var c1 = CultureInfo.GetCultureInfo(EnUs);
        var c2 = CultureInfo.GetCultureInfo(DeDe);

        Locale l1 = c1;
        Locale l2 = c2;

        Assert.NotNull(l1.Culture);
        Assert.NotNull(l2.Culture);
    }
}