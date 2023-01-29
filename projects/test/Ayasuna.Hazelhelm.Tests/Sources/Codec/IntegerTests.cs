namespace Ayasuna.Hazelhelm.Tests.Codec;

using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Hazelhelm.Codec;
using Xunit;

public class IntegerTests
{
    /// <summary>
    /// Tests whether the <see cref="Integer.ToBase2"/> method correctly converts integers to their string representation
    /// </summary>
    [Fact]
    public void The_ToBase2_method_should_be_able_convert_integers_correctly_to_their_base2_representation()
    {
        var integersToTest = Enumerable.Range(0, 1000000);
        
        foreach (var integer in integersToTest)
        {
            Assert.Equal(Convert.ToString(integer, 2), Integer.ToBase2(integer));
        }
    }

    /// <summary>
    /// Tests whether the <see cref="Integer.FromBase2"/> method correctly converts strings which contain integers in their BASE2 representation back to integers
    /// </summary>
    [Fact]
    public void The_FromBase2_method_should_be_able_convert_strings_which_contain_integers_in_their_base2_representation_to_integers()
    {
        var integersToTest = Enumerable.Range(0, 1000000);

        foreach (var integer in integersToTest)
        {
            Assert.Equal(integer, Integer.FromBase2(Convert.ToString(integer, 2)));
        }
    }
    
    /// <summary>
    /// Tests whether the <see cref="Integer.ToBase8"/> method correctly converts integers to their string representation
    /// </summary>
    [Fact]
    public void The_ToBase8_method_should_be_able_convert_integers_correctly_to_their_base8_representation()
    {
        var integersToTest = Enumerable.Range(0, 1000000);

        foreach (var integer in integersToTest)
        {
            Assert.Equal(Convert.ToString(integer, 8), Integer.ToBase8(integer));
        }
    }

    /// <summary>
    /// Tests whether the <see cref="Integer.FromBase8"/> method correctly converts strings which contain integers in their BASE8 representation back to integers
    /// </summary>
    [Fact]
    public void The_FromBase8_method_should_be_able_convert_strings_which_contain_integers_in_their_base8_representation_to_integers()
    {
        var integersToTest = Enumerable.Range(0, 1000000);

        foreach (var integer in integersToTest)
        {
            Assert.Equal(integer, Integer.FromBase8(Convert.ToString(integer, 8)));
        }
    }

    /// <summary>
    /// Tests whether the <see cref="Integer.ToBase10"/> method correctly converts integers to their string representation
    /// </summary>
    [Fact]
    public void The_ToBase10_method_should_be_able_convert_integers_correctly_to_their_base8_representation()
    {
        var integersToTest = Enumerable.Range(0, 1000000);

        foreach (var integer in integersToTest)
        {
            Assert.Equal(Convert.ToString(integer, 10), Integer.ToBase10(integer));
        }
    }

    /// <summary>
    /// Tests whether the <see cref="Integer.FromBase10"/> method correctly converts strings which contain integers in their BASE10 representation back to integers
    /// </summary>
    [Fact]
    public void The_FromBase10_method_should_be_able_convert_strings_which_contain_integers_in_their_base8_representation_to_integers()
    {
        var integersToTest = Enumerable.Range(0, 1000000);

        foreach (var integer in integersToTest)
        {
            Assert.Equal(integer, Integer.FromBase10(Convert.ToString(integer, 10)));
        }
    }

    
    /// <summary>
    /// Tests whether the <see cref="Integer.ToBase16"/> method correctly converts integers to their string representation
    /// </summary>
    [Fact]
    public void The_ToBase16_method_should_be_able_convert_integers_correctly_to_their_base16_representation()
    {
        var integersToTest = Enumerable.Range(0, 1000000);

        foreach (var integer in integersToTest)
        {
            Assert.Equal(Convert.ToString(integer, 16), Integer.ToBase16(integer));
        }
    }

    /// <summary>
    /// Tests whether the <see cref="Integer.FromBase16"/> method correctly converts strings which contain integers in their BASE16 representation back to integers
    /// </summary>
    [Fact]
    public void The_FromBase16_method_should_be_able_convert_strings_which_contain_integers_in_their_base16_representation_to_integers()
    {
        var integersToTest = Enumerable.Range(0, 1000000);

        foreach (var integer in integersToTest)
        {
            Assert.Equal(integer, Integer.FromBase16(Convert.ToString(integer, 16)));
        }
    }
    
    /// <summary>
    /// Tests whether the <see cref="Integer.ToBase62"/> method correctly converts integers to their string representation
    /// </summary>
    [Fact]
    public void The_ToBase62_method_should_be_able_convert_integers_correctly_to_their_base62_representation()
    {
        Assert.Equal("0", Integer.ToBase62(BigInteger.Zero));
        Assert.Equal("1", Integer.ToBase62(BigInteger.One));

        Assert.Equal("m", Integer.ToBase62(new BigInteger("0"u8.ToArray().Reverse().ToArray())));
        Assert.Equal("trBLg", Integer.ToBase62(new BigInteger("1234"u8.ToArray().Reverse().ToArray())));
        Assert.Equal("5ZoxFWcU2WF6W5z", Integer.ToBase62(new BigInteger("98745678943"u8.ToArray().Reverse().ToArray())));
    }

    /// <summary>
    /// Tests whether the <see cref="Integer.FromBase62"/> method correctly converts strings which contain integers in their BASE62 representation back to integers
    /// </summary>
    [Fact]
    public void The_FromBase62_method_should_be_able_convert_strings_which_contain_integers_in_their_base62_representation_to_integers()
    {
        Assert.Equal(BigInteger.Zero, Integer.FromBase62("0"));
        Assert.Equal(BigInteger.One, Integer.FromBase62("1"));

        Assert.Equal(48, Integer.FromBase62("m"));
        Assert.Equal(825373492, Integer.FromBase62("trBLg"));
        Assert.Equal(BigInteger.Parse("69174242573971291647849523", CultureInfo.InvariantCulture), Integer.FromBase62("5ZoxFWcU2WF6W5z"));
    }
}