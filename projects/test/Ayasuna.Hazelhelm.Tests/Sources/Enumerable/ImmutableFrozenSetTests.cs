namespace Ayasuna.Hazelhelm.Tests.Enumerable;

using Hazelhelm.Enumerable;
using Xunit;

/// <summary>
/// Contains the unit tests for the <see cref="ImmutableFrozenSet{TValue}"/> type
/// </summary>
public sealed class ImmutableFrozenSetTests
{
    private const string Foo = "foo";

    private const string Bar = "bar";

    private const string Baz = "baz";

    private readonly ImmutableFrozenSet<string> _set;

    /// <summary>
    /// Constructs a new <see cref="ImmutableFrozenDictionaryTests"/> object
    /// </summary>
    public ImmutableFrozenSetTests()
    {
        _set = [Foo, Bar];
    }

    /// <summary>
    /// Checks whether <see cref="ImmutableFrozenSet{TValue}.Add"/> returns the set itself if
    /// the value is already part of the set
    /// </summary>
    [Fact]
    public void Add_should_return_the_set_itself_if_it_is_already_part_of_the_set()
    {
        var expectedCount = _set.Count;

        Assert.Equal(expectedCount, _set.Count);
        Assert.Same(_set, _set.Add(Foo));
        Assert.Equal(expectedCount, _set.Count);
        Assert.Same(_set, _set.Add(Bar));
        Assert.Equal(expectedCount, _set.Count);
    }

    /// <summary>
    /// Checks whether <see cref="ImmutableFrozenSet{TValue}.Add"/> returns a new set if the added value
    /// is not yet part of the set
    /// </summary>
    [Fact]
    public void Add_should_return_a_new_set_if_the_values_is_not_yet_part_of_the_set()
    {
        var newSet = _set.Add(Baz);

        Assert.NotSame(_set, newSet);
        Assert.Equal(_set.Count + 1, newSet.Count);
        
        Assert.True(newSet.Contains(Foo));
        Assert.True(newSet.Contains(Bar));
        Assert.True(newSet.Contains(Baz));
    }
}