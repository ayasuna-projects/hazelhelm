namespace Ayasuna.Hazelhelm.Tests;

using System;
using Xunit;

public sealed class ObjectExtensionsTests
{
    /// <summary>
    /// Test whether <see cref="ObjectExtensions.CastOrThrow{T}"/> throws an <see cref="InvalidCastException"/>
    /// if <c>this</c> object is not of the requested type
    /// </summary>
    [Fact]
    public void CastOrThrow_should_throw_if_this_object_is_not_of_the_requested_type()
    {
        var value = "test";

        Assert.Throws<InvalidCastException>(() => value.CastOrThrow<Unit>());
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.CastOrThrow{T}"/> does not throw an <see cref="InvalidCastException"/>
    /// if <c>this</c> object is of the requested type
    /// </summary>
    [Fact]
    public void CastOrThrow_should_not_throw_if_this_object_is_of_the_requested_type()
    {
        {
            const string expected = "test";

            Assert.Same(expected, expected.CastOrThrow<object>());
            Assert.Same(expected, expected.CastOrThrow<string>());
        }

        {
            const int expected = 123;

            Assert.Equal(expected, expected.CastOrThrow<object>());
            Assert.Equal(expected, expected.CastOrThrow<int>());
            Assert.Equal(expected, expected.CastOrThrow<int?>());
        }

        {
            Unit? expected = Unit.Value;

            Assert.Equal(expected, expected.CastOrThrow<object>());
            Assert.Equal(expected, expected.CastOrThrow<Unit>());
            Assert.Equal(expected, expected.CastOrThrow<Unit?>());
        }
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.CastOrThrow{T}"/> does not throw an <see cref="InvalidCastException"/>
    /// if <c>this</c> object is <c>null</c>
    /// </summary>
    [Fact]
    public void CastOrThrow_should_not_throw_if_this_object_is_null()
    {
        string? expected = null;

        Assert.Null(expected.CastOrThrow<int?>());
        Assert.Null(expected.CastOrThrow<bool?>());
        Assert.Null(expected.CastOrThrow<Unit?>());
        Assert.Null(expected.CastOrThrow<object>());
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.CastOrDefault{T}"/> returns the default value if <c>this</c>
    /// object is not of the requested type
    /// </summary>
    [Fact]
    public void CastOrDefault_should_return_the_default_value_if_this_object_is_not_of_the_requested_type()
    {
        {
            var expected = "test";
            var value = 123;

            Assert.Same(expected, value.CastOrDefault(expected));
        }

        {
            var expected = 0.1d;
            var value = 123;

            Assert.Equal(expected, value.CastOrDefault(expected));
        }

        {
            double? expected = null;
            var value = 123;

            Assert.Null(value.CastOrDefault(expected));
        }
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.CastOrDefault{T}"/> returns <c>this</c>
    /// object if <c>this</c> object is of the requested type
    /// </summary>
    [Fact]
    public void CastOrDefault_should_return_this_object_if_this_object_is_of_the_requested_type()
    {
        {
            var expected = "test";

            Assert.Same(expected, expected.CastOrDefault("other"));
            Assert.Same(expected, expected.CastOrDefault(new object()));
        }

        {
            var expected = 123;

            Assert.Equal(expected, expected.CastOrDefault(456));
            Assert.Equal(expected, expected.CastOrDefault(new object()));
            Assert.Equal(expected, expected.CastOrDefault<int?>());
        }
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.Cast{T}"/> throws an <see cref="InvalidCastException"/>
    /// if <c>this</c> object is not of the requested type
    /// </summary>
    [Fact]
    public void Cast_should_throw_if_this_object_is_not_of_the_requested_type()
    {
        var value = "test";

        Assert.Throws<InvalidCastException>(() => value.Cast<Unit>());
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.NotNull{T}"/> throws an <see cref="ArgumentNullException"/> if <c>this</c>
    /// object is null
    /// </summary>
    [Fact]
    public void NotNull_should_throw_if_this_object_is_null()
    {
        string? value = null;

        Assert.Throws<ArgumentNullException>(() => value.NotNull());
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.NotNullOrThrow{T}"/> throws an <see cref="ArgumentNullException"/> if <c>this</c>
    /// object is <c>null</c>
    /// </summary>
    [Fact]
    public void NotNullOrThrow_should_throw_if_this_object_is_null()
    {
        string? value = null;

        Assert.Throws<ArgumentNullException>(() => value.NotNullOrThrow());
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.NotNullOrDefault{T}"/> returns the given default object if <c>this</c> object is <c>null</c>
    /// </summary>
    [Fact]
    public void NotNullOrDefault_should_return_the_default_object_if_this_object_is_null()
    {
        string? value = null;
        var expected = "test";

        Assert.Same(expected, value.NotNullOrDefault(expected));
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.NotNull{T}"/> returns <c>this</c> object if <c>this</c>
    /// object is not <c>null</c>
    /// </summary>
    [Fact]
    public void NotNull_should_return_this_object_if_this_object_is_not_null()
    {
        var expected = "test";

        Assert.Same(expected, expected.NotNull());
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.NotNullOrThrow{T}"/> returns <c>this</c> object if <c>this</c>
    /// object is not <c>null</c>
    /// </summary>
    [Fact]
    public void NotNullOrThrow_should_return_this_object_if_this_object_is_not_null()
    {
        var expected = "test";

        Assert.Same(expected, expected.NotNullOrThrow());
    }

    /// <summary>
    /// Test whether <see cref="ObjectExtensions.NotNullOrDefault{T}"/> returns <c>this</c> object if <c>this</c>
    /// object is not <c>null</c>
    /// </summary>
    [Fact]
    public void NotNullOrDefault_should_return_this_object_if_this_object_is_not_null()
    {
        var expected = "test";

        Assert.Same(expected, expected.NotNullOrDefault("other"));
    }
}