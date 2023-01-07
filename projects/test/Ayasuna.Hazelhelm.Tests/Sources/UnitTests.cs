namespace Ayasuna.Hazelhelm.Tests;

using System.Diagnostics.CodeAnalysis;
using Xunit;

#pragma warning disable CS1718

public sealed class UnitTests
{
    /// <summary>
    /// Test whether all unit instances are considered equal to one another
    /// </summary>
    [Fact]
    [SuppressMessage("Assertions", "xUnit2000:Constants and literals should be the expected argument")]
    [SuppressMessage("ReSharper", "EqualExpressionComparison")]
    public void Unit_instances_should_all_be_equal_to_one_another()
    {
        Assert.Equal(Unit.Value, Unit.Value);
        Assert.Equal(Unit.Value, new Unit());
        Assert.Equal(Unit.Value, default);
        Assert.Equal(Unit.Value, (object)new Unit());

        Assert.True(Unit.Value.CompareTo(Unit.Value) == 0);
        Assert.True(Unit.Value.CompareTo(new Unit()) == 0);
        Assert.True(Unit.Value.CompareTo(default) == 0);

        Assert.True(Unit.Value == Unit.Value);
        Assert.True(Unit.Value == new Unit());
        Assert.True(Unit.Value == default);
        
        Assert.False(Unit.Value != Unit.Value);
        Assert.False(Unit.Value != new Unit());
        Assert.False(Unit.Value != default);
    }

    /// <summary>
    /// Test whether <see cref="Unit.Equals(object)"/> returns <c>false</c> for non <see cref="Unit"/> objects
    /// </summary>
    [Fact]
    public void Unit_instances_should_not_be_equal_to_instances_of_other_types()
    {
        var instance = (object)Unit.Value;

        Assert.False(instance.Equals("test"));
        Assert.False(instance.Equals(0));
        Assert.False(instance.Equals(null));
    }

    /// <summary>
    /// Test whether <see cref="Unit.ToString"/> returns <c>()</c>
    /// </summary>
    [Fact]
    public void The_string_representation_of_a_unit_instance_should_always_be_the_same()
    {
        Assert.Equal("()", Unit.Value.ToString());
    }
}

#pragma warning restore