namespace Ayasuna.Hazelhelm.Tests.Identity;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Hazelhelm.Identity;
using Xunit;

public class FlakeIdTests : IdTests<FlakeId<string>, string>
{
    private static readonly PhysicalAddress WorkerId1 = PhysicalAddress.Parse("00:00:00:00:00:00");

    private static readonly PhysicalAddress WorkerId2 = PhysicalAddress.Parse("8D:88:D1:27:46:7B");

    private static readonly DateTimeOffset Timestamp1 = DateTimeOffset.UnixEpoch.AddMilliseconds(1);

    private static readonly DateTimeOffset Timestamp2 = DateTimeOffset.UnixEpoch.AddYears(52).AddMonths(3).AddDays(12).AddHours(4).AddMinutes(36).AddSeconds(15).AddMilliseconds(5);

    protected override FlakeId<string> FirstId => FlakeId.Create<string>(Timestamp1, WorkerId1, 0);
    protected override FlakeId<string> SecondId => FlakeId.Create<string>(Timestamp2, WorkerId1, 0);

    /// <summary>
    /// Tests whether the <see cref="FlakeId.Create{TEntity}(PhysicalAddress)"/> is able to create flake ids
    /// </summary>
    [Fact]
    public void It_should_be_possible_to_create_flake_ids()
    {
        foreach (var _ in Enumerable.Range(0, 10000))
        {
            Assert.NotNull(FlakeId.Create<string>(WorkerId1));
        }
    }

    /// <summary>
    /// Tests whether the created flake ids are k-ordered
    /// </summary>
    [Fact]
    public void FlakeIds_that_are_created_after_each_other_should_be_ordered_after_each_other()
    {
        var createdIdsInExpectedOrder = new List<FlakeId<string>>
        {
            FlakeId.Create<string>(Timestamp2, WorkerId1, 0),
            FlakeId.Create<string>(Timestamp2, WorkerId1, 1),
            FlakeId.Create<string>(Timestamp2, WorkerId1, 2),
            FlakeId.Create<string>(Timestamp2, WorkerId1, 3),
            FlakeId.Create<string>(Timestamp2, WorkerId2, 0),
            FlakeId.Create<string>(Timestamp2, WorkerId2, 1),
            FlakeId.Create<string>(Timestamp2, WorkerId2, 2),
            FlakeId.Create<string>(Timestamp2, WorkerId2, 3),
            FlakeId.Create<string>(Timestamp2.AddMilliseconds(1), WorkerId1, 0),
            FlakeId.Create<string>(Timestamp2.AddMilliseconds(1), WorkerId1, 1),
            FlakeId.Create<string>(Timestamp2.AddMilliseconds(1), WorkerId1, 2),
            FlakeId.Create<string>(Timestamp2.AddMilliseconds(1), WorkerId1, 3)
        };

        // NOTE: We're using Reverse here to ensure that the sorted set does not simply follow insertion order but really orders the ids
        var inSortedSet = createdIdsInExpectedOrder.Select(e => e).Reverse().ToImmutableSortedSet();


#pragma warning disable xUnit2027
        Assert.Equal(createdIdsInExpectedOrder, inSortedSet);
#pragma warning restore xUnit2027
    }

    /// <summary>
    /// Tests whether <see cref="FlakeId.Create{TEntity}(DateTimeOffset,PhysicalAddress,ushort)"/> correctly BASE62 encodes flake ids
    /// </summary>
    [Fact]
    public void Create_should_correctly_base62_encode_flake_ids()
    {
        var flakeId1 = FlakeId.Create<string>(Timestamp1, WorkerId1, 0);

        Assert.Equal(Timestamp1, flakeId1.Timestamp);
        Assert.Equal(WorkerId1, flakeId1.WorkerId);
        Assert.Equal(0, flakeId1.Sequence);
        // This is the smallest id that the current implementation can generate
        Assert.Equal("LygHa16AHYG", flakeId1.ToString());

        var flakeId2 = FlakeId.Create<string>(Timestamp2, WorkerId2, 123);

        Assert.Equal(Timestamp2, flakeId2.Timestamp);
        Assert.Equal(WorkerId2, flakeId2.WorkerId);
        Assert.Equal(123, flakeId2.Sequence);
        Assert.Equal("AIOZva1J1jQXuOMSOh", flakeId2.ToString());
    }

    /// <summary>
    /// Tests whether <see cref="FlakeId.Create{TEntity}(PhysicalAddress)"/> correctly throws an exception if the provided worker id is not exactly 48-bits long
    /// </summary>
    [Fact]
    public void Create_should_throw_an_exception_if_the_worker_id_is_not_exactly_48_bits_long()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => FlakeId.Create<string>(PhysicalAddress.None));
    }

    /// <summary>
    /// Tests whether <see cref="FlakeId.Create{TEntity}(PhysicalAddress)"/> correctly throws an exception if the provided timestamp is less than or equal to the start of the unix epoch
    /// </summary>
    [Fact]
    public void Create_should_throw_an_exception_if_the_timestamp_is_less_than_or_equal_to_the_start_of_the_unix_epoch()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => FlakeId.Create<string>(DateTimeOffset.UnixEpoch, WorkerId1, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => FlakeId.Create<string>(DateTimeOffset.UnixEpoch.AddYears(-100), WorkerId1, 0));
    }

    /// <summary>
    /// Tests whether <see cref="FlakeId.FromString{TEntity}(string)"/> is able to (re)create flake ids from their base62 encoded representation
    /// </summary>
    [Fact]
    public void FromString_should_be_able_to_create_flake_ids_from_their_base62_encoded_representation()
    {
        var originalFlakeId1 = FlakeId.Create<string>(Timestamp1, WorkerId1, 0);
        var recreatedFlakeId1 = FlakeId.FromString<string>(originalFlakeId1.ToString());

        Assert.Equal(originalFlakeId1.Sequence, recreatedFlakeId1.Sequence);
        Assert.Equal(originalFlakeId1.WorkerId, recreatedFlakeId1.WorkerId);
        Assert.Equal(originalFlakeId1.Timestamp, recreatedFlakeId1.Timestamp);
        Assert.Equal(originalFlakeId1, recreatedFlakeId1);

        var originalFlakeId2 = FlakeId.Create<string>(Timestamp2, WorkerId1, 0);
        var recreatedFlakeId2 = FlakeId.FromString<string>(originalFlakeId2.ToString());

        Assert.Equal(originalFlakeId2.Sequence, recreatedFlakeId2.Sequence);
        Assert.Equal(originalFlakeId2.WorkerId, recreatedFlakeId2.WorkerId);
        Assert.Equal(originalFlakeId2.Timestamp, recreatedFlakeId2.Timestamp);
        Assert.Equal(originalFlakeId2, recreatedFlakeId2);
    }
}