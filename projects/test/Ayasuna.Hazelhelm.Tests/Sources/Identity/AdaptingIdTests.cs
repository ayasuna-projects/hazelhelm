namespace Ayasuna.Hazelhelm.Tests.Identity;

using Hazelhelm.Identity;
using Xunit;

/// <inheritdoc />
public sealed class AdaptingIdTests : IdTests<AdaptingId<AdaptingIdTests, string>, AdaptingIdTests>
{
    private const string First = nameof(First);
    
    private const string Second = nameof(Second);
    
    /// <inheritdoc />
    protected override AdaptingId<AdaptingIdTests, string> FirstId => new(First);

    /// <inheritdoc />
    protected override AdaptingId<AdaptingIdTests, string> SecondId => new(Second);
    
    /// <summary>
    /// Checks whether the <see cref="AdaptingId{TEntity,TExternalId}.Adaptee"/> is equal to the value/adaptee that was provided when constructing the <see cref="AdaptingId{TEntity,TExternalId}"/> 
    /// </summary>
    [Fact]
    public void The_adaptee_property_should_be_equal_to_the_value_that_was_provided_to_the_constructor()
    {
        Assert.Equal(First, FirstId.Adaptee);
        Assert.Equal(Second, SecondId.Adaptee);
    }
}