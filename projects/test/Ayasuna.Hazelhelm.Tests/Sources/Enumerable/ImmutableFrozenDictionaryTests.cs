namespace Ayasuna.Hazelhelm.Tests.Enumerable;

using System.Collections.Generic;
using Hazelhelm.Enumerable;
using Xunit;

/// <summary>
/// Contains the unit tests for the <see cref="ImmutableFrozenDictionary{TKey,TValue}"/> type
/// </summary>
public sealed class ImmutableFrozenDictionaryTests
{
    private const string Foo = "foo";

    private const string Bar = "bar";

    private const string Baz = "baz";

    private readonly ImmutableFrozenDictionary<string, string> _dictionary;

    /// <summary>
    /// Constructs a new <see cref="ImmutableFrozenDictionaryTests"/> object
    /// </summary>
    public ImmutableFrozenDictionaryTests()
    {
        _dictionary = [new KeyValuePair<string, string>(Foo, Baz), new KeyValuePair<string, string>(Bar, Foo)];
    }

    /// <summary>
    /// Checks whether <see cref="ImmutableFrozenDictionary{TKey,TValue}.SetItem"/> returns a new dictionary
    /// </summary>
    [Fact]
    public void SetItem_should_return_a_new_dictionary()
    {
        var expectedCount = _dictionary.Count;

        Assert.Equal(expectedCount, _dictionary.Count);

        var newDictionary = _dictionary.SetItem(Baz, Bar);

        Assert.NotSame(_dictionary, newDictionary);
        Assert.Equal(expectedCount, _dictionary.Count);

        Assert.Equal(expectedCount + 1, newDictionary.Count);

        Assert.True(newDictionary.ContainsKey(Foo));
        Assert.True(newDictionary.ContainsKey(Bar));
        Assert.True(newDictionary.ContainsKey(Baz));
    }
}