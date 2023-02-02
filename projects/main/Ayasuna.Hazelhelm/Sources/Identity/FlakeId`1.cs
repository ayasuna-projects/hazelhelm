namespace Ayasuna.Hazelhelm.Identity;

using System;
using System.Net.NetworkInformation;

/// <summary>
/// Represents a flake id which is a 128 bit k-ordered id that consist of three parts (ordered from most significant to least significant): <br/>
/// - Timestamp (64 bits) <br/>
/// - Worker Id (48 bits; MAC address) <br/>
/// - Sequence Id (16 bits; incremented each time more than one id is created at the same time and reset to zero when the time moves forward) <br/>
/// Flake ids are normally also BASE62 encoded. 
/// </summary>
/// <typeparam name="TEntity">The <see cref="Type"/> of the id</typeparam>
public sealed class FlakeId<TEntity> : Id<TEntity> where TEntity : notnull
{
    /// <summary>
    /// The timestamp component of the flake id
    /// </summary>
    public DateTimeOffset Timestamp { get; }

    /// <summary>
    /// The worker id component of the flake id. <br/>
    /// </summary>
    public PhysicalAddress WorkerId { get; }

    /// <summary>
    /// The sequence id component of the flake id
    /// </summary>
    public ushort Sequence { get; }

    /// <summary>
    /// Constructs a new <see cref="FlakeId{TEntity}"/> object
    /// </summary>
    /// <param name="timestamp">The timestamp component of the flake id</param>
    /// <param name="workerId">The worker id component of the flake id</param>
    /// <param name="sequence">The sequence component of the flake id</param>
    /// <param name="encoded">The encoded flake id</param>
    internal FlakeId(DateTimeOffset timestamp, PhysicalAddress workerId, ushort sequence, string encoded)
        : base(encoded)
    {
        Timestamp = timestamp;
        WorkerId = workerId;
        Sequence = sequence;
    }
}