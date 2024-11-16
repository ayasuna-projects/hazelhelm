namespace Ayasuna.Hazelhelm.Tests.Identity;

using Hazelhelm.Identity;
using Xunit;

/// <summary>
/// Base class for all <see cref="FirstId"/> tests, which tests if the <see cref="TConcreteId"/> implementation follow the general Id contract. 
/// </summary>
/// <remarks>
/// With the current <see cref="Id{TEntity}"/> implementation it is somewhat redundant to tests these cases for each implementation as <see cref="Id{TEntity}"/> implementations can't overwrite the members that are tested here. <br/>
/// We are performing these tests nonetheless as this <b>might</b> change in the future.
/// </remarks>
/// <typeparam name="TConcreteId">The concrete id type</typeparam>
/// <typeparam name="TEntity">The entity type of the id</typeparam>
public abstract class IdTests<TConcreteId, TEntity> where TConcreteId : Id<TEntity> where TEntity : notnull
{
    /// <summary>
    /// The id to test
    /// </summary>
    protected abstract TConcreteId FirstId { get; }

    /// <summary>
    /// The second id, <b>must</b> differ from the <see cref="FirstId"/>
    /// </summary>
    protected abstract TConcreteId SecondId { get; }

    /// <summary>
    /// Checks whether the type returned by <see cref="Id.Type"/> is equal to type <see cref="TEntity"/>
    /// </summary>
    [Fact]
    public void The_type_property_should_be_equal_to_the_entity_type()
    {
        Assert.Equal(typeof(TEntity), FirstId.Type);
        Assert.Equal(typeof(TEntity), SecondId.Type);
    }

    /// <summary>
    /// Checks whether the value returned by <see cref="Id.Value"/> is equal to the the value returned by <see cref="Id.ToString"/>
    /// </summary>
    [Fact]
    public void The_value_property_should_be_equal_to_the_string_representation_of_the_id()
    {
        Assert.Equal(FirstId.Value, FirstId.ToString());
        Assert.Equal(SecondId.Value, SecondId.ToString());
    }

    /// <summary>
    /// Checks whether an id is considered equal with itself
    /// </summary>
    [Fact]
    public void When_comparing_an_id_with_itself_it_should_be_considered_equal()
    {
        Assert.True(FirstId.Equals(FirstId));
        Assert.True(((object)FirstId).Equals(FirstId));

        Assert.True(SecondId.Equals(SecondId));
        Assert.True(((object)SecondId).Equals(SecondId));

        Assert.Equal(0, FirstId.CompareTo(FirstId));
    }

    /// <summary>
    /// Checks whether two ids are considered unequal if their <see cref="Id.Type"/> and/or <see cref="Id.Value"/> is not the same
    /// </summary>
    [Fact]
    public void When_comparing_an_id_with_a_different_id_they_should_be_considered_unequal()
    {
        Assert.False(FirstId.Equals(SecondId));
        Assert.False(((object)FirstId).Equals(SecondId));

        Assert.NotEqual(0, FirstId.CompareTo(SecondId));
    }
}