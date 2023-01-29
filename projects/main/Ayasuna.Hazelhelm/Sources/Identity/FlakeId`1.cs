namespace Ayasuna.Hazelhelm.Identity;

using System;

/// <summary>
/// Represents a flake id which is a 128 bit k-ordered id that consist of three parts (ordered from most significant to least significant): <br/>
/// - Timestamp (64 bits) <br/>
/// - Worker Id (48 bits; normally a MAC address) <br/>
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
    /// Always exactly 48 bits. 
    /// </summary>
    public byte[] WorkerId { get; }

    /// <summary>
    /// The sequence id component of the flake id
    /// </summary>
    public ushort Sequence { get; }

    internal FlakeId(DateTimeOffset timestamp, byte[] workerId, ushort sequence, string encoded)
        : base(encoded)
    {
        Timestamp = timestamp;
        WorkerId = workerId;
        Sequence = sequence;
    }
}