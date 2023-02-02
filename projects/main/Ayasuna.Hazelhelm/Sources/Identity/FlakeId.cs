namespace Ayasuna.Hazelhelm.Identity;

using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Threading;
using Codec;

/// <summary>
/// Provides helper methods to create <see cref="FlakeId{TEntity}"/> objects
/// </summary>
public static class FlakeId
{
    private static readonly object Lock = new();

    private static DateTimeOffset _lastTimestamp = DateTimeOffset.MinValue;

    private static ushort _lastSequence;

    /// <summary>
    /// Creates a <see cref="FlakeId{TEntity}"/> from the given BASE62 <paramref name="encodedId"/>
    /// </summary>
    /// <param name="encodedId">The BASE62 encoded flake id to parse</param>
    /// <typeparam name="TEntity">The entity type to create the id for</typeparam>
    /// <returns>The created flake id</returns>
    public static FlakeId<TEntity> FromString<TEntity>(string encodedId) where TEntity : notnull
    {
        // TODO: This implementation current just assumes that 'ToByteArray' returns the bytes in little-endian order as the BigInteger(bytes) constructor also expects the bytes to be in little-endian order
        var bytesInLittleEndianOrder = Integer.FromBase62(encodedId).ToByteArray();

        var sequenceIdBytes = new byte[2];
        var workerId = new byte[6];
        var timestampBytes = new byte[8];

        Buffer.BlockCopy(bytesInLittleEndianOrder, 0, sequenceIdBytes, 0, sequenceIdBytes.Length);
        Buffer.BlockCopy(bytesInLittleEndianOrder, sequenceIdBytes.Length, workerId, 0, workerId.Length);
        Buffer.BlockCopy(bytesInLittleEndianOrder, sequenceIdBytes.Length + workerId.Length, timestampBytes, 0, bytesInLittleEndianOrder.Length - sequenceIdBytes.Length - workerId.Length);

        if (!BitConverter.IsLittleEndian)
        {
            sequenceIdBytes = sequenceIdBytes.Reverse().ToArray();
            timestampBytes = timestampBytes.Reverse().ToArray();
        }

        var sequenceId = BitConverter.ToUInt16(sequenceIdBytes);
        var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(BitConverter.ToInt64(timestampBytes));

        return Create<TEntity>(timestamp, new PhysicalAddress(workerId), sequenceId);
    }

    /// <summary>
    /// Creates a new BASE62 encoded <see cref="FlakeId{TEntity}"/> that uses the <see cref="PhysicalAddress"/> of the fastest NIC that is up and has a MAC address which is exactly 48-bits long and is not a loopback device as worker id. <br/>
    /// The current time (<see cref="DateTimeOffset.Now"/>) will be used to determine the <see cref="FlakeId{TEntity}.Timestamp"/> component of the id
    /// </summary>
    /// <remarks>
    /// The current implementation will block if: <br/>
    /// - The next sequence id would overflow, in this case the method call is blocked until the <see cref="DateTimeOffset.Now"/> returns a timestamp which is greater than <see cref="_lastTimestamp"/> <br/>
    /// - <see cref="DateTimeOffset.Now"/> is in the past (compared to <see cref="_lastTimestamp"/>) in this case the call will block until the <see cref="DateTimeOffset.Now"/> is greater than equal to <see cref="_lastTimestamp"/>
    /// </remarks>
    /// <exception cref="InvalidOperationException">If no <see cref="PhysicalAddress"/> with a length of exactly 48-bits could be found</exception>
    /// <typeparam name="TEntity">The entity type to create the id for</typeparam>
    /// <returns>The created flake id</returns>
    public static FlakeId<TEntity> Create<TEntity>() where TEntity : notnull
    {
        return Create<TEntity>
        (
            NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(e => e.OperationalStatus == OperationalStatus.Up && e.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Where(e => e.GetPhysicalAddress().GetAddressBytes().Length == 6)
                .OrderByDescending(e => e.Speed)
                .Select(e => e.GetPhysicalAddress())
                .First()
        );
    }

    /// <summary>
    /// Creates a new BASE62 encoded <see cref="FlakeId{TEntity}"/> with the given <paramref name="workerId"/>. <br/>
    /// The current time (<see cref="DateTimeOffset.Now"/>) will be used to determine the <see cref="FlakeId{TEntity}.Timestamp"/> component of the id
    /// </summary>
    /// <remarks>
    /// The current implementation will block if: <br/>
    /// - The next sequence id would overflow, in this case the method call is blocked until the <see cref="DateTimeOffset.Now"/> returns a timestamp which is greater than <see cref="_lastTimestamp"/> <br/>
    /// - <see cref="DateTimeOffset.Now"/> is in the past (compared to <see cref="_lastTimestamp"/>) in this case the call will block until the <see cref="DateTimeOffset.Now"/> is greater than equal to <see cref="_lastTimestamp"/>
    /// </remarks>
    /// <param name="workerId">The worker id of the flake id</param>
    /// <exception cref="ArgumentOutOfRangeException">If the given <paramref name="workerId"/> is not exactly 48-bits long</exception>
    /// <typeparam name="TEntity">The entity type to create the id for</typeparam>
    /// <returns>The created flake id</returns>
    public static FlakeId<TEntity> Create<TEntity>(PhysicalAddress workerId) where TEntity : notnull
    {
        lock (Lock)
        {
            while (true)
            {
                var currentTimestamp = DateTimeOffset.Now;

                // Handle the easiest case first, the timestamp returned by the timestamp factory is larger than the last timestamp
                if (currentTimestamp > _lastTimestamp)
                {
                    _lastTimestamp = currentTimestamp;
                    _lastSequence = 0;

                    return Create<TEntity>(currentTimestamp, workerId, _lastSequence);
                }

                // Next handle the case in which we have to increase the sequence id
                if (currentTimestamp == _lastTimestamp)
                {
                    // The last sequence id would overflow if we were to increase it again, so block until the timestamp factory returns a timestamp which is larger than the current last timestamp
                    if (_lastSequence == ushort.MaxValue)
                    {
                        SpinWait.SpinUntil(() => DateTimeOffset.Now > _lastTimestamp);
                        continue;
                    }

                    return Create<TEntity>(currentTimestamp, workerId, ++_lastSequence);
                }

                // If we got here the clock ticked backwards so we block here until the clock is greater than equal to the last timestamp
                SpinWait.SpinUntil(() => DateTimeOffset.Now >= _lastTimestamp);
            }
        }
    }


    /// <summary>
    /// Creates a new BASE62 encoded <see cref="FlakeId{TEntity}"/> with the given components
    /// </summary>
    /// <param name="timestamp">The timestamp component</param>
    /// <param name="workerId">The worker id component</param>
    /// <param name="sequenceId">The sequence id component</param>
    /// <exception cref="ArgumentOutOfRangeException">If the given <paramref name="workerId"/> is not exactly 48-bits long or i the given timestamp is less than or equal to the start of the unix epoch</exception>
    /// <typeparam name="TEntity">The entity type to create the id for</typeparam>
    /// <returns>The created flake id</returns>
    public static FlakeId<TEntity> Create<TEntity>(DateTimeOffset timestamp, PhysicalAddress workerId, ushort sequenceId) where TEntity : notnull
    {
        if (timestamp <= DateTimeOffset.UnixEpoch)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp), "The given timestamp is less than or equal to the start of the unix epoch but must be at least one millisecond larger");
        }

        var workerIdBytes = workerId.GetAddressBytes();

        if (workerIdBytes.Length != 6)
        {
            throw new ArgumentOutOfRangeException(nameof(workerId), $"The provided worker id was {workerIdBytes.Length * 8} bits long but must be exactly 48 bits long");
        }

        var timestampBytes = BitConverter.GetBytes(timestamp.ToUnixTimeMilliseconds());
        var sequenceIdBytes = BitConverter.GetBytes(sequenceId);

        if (!BitConverter.IsLittleEndian)
        {
            timestampBytes = timestampBytes.Reverse().ToArray();
            sequenceIdBytes = sequenceIdBytes.Reverse().ToArray();
        }

        var bytes = new byte[timestampBytes.Length + workerIdBytes.Length + sequenceIdBytes.Length];

        Buffer.BlockCopy(sequenceIdBytes, 0, bytes, 0, sequenceIdBytes.Length);
        Buffer.BlockCopy(workerIdBytes, 0, bytes, sequenceIdBytes.Length, workerIdBytes.Length);
        Buffer.BlockCopy(timestampBytes, 0, bytes, sequenceIdBytes.Length + workerIdBytes.Length, timestampBytes.Length);

        return new FlakeId<TEntity>(timestamp, workerId, sequenceId, Integer.ToBase62(new BigInteger(bytes)));
    }
}