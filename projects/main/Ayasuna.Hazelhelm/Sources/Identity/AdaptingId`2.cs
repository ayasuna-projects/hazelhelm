namespace Ayasuna.Hazelhelm.Identity;

using System;

/// <summary>
/// Adapts an external id like <see cref="Guid"/> as <see cref="Id"/> 
/// </summary>
/// <typeparam name="TEntity">The entity type of the id</typeparam>
/// <typeparam name="TExternalId">The external id type</typeparam>
public class AdaptingId<TEntity, TExternalId> : Id<TEntity> where TEntity : notnull where TExternalId : notnull
{
    /// <summary>
    /// The adapted/external id
    /// </summary>
    public TExternalId Adaptee { get; }

    /// <summary>
    /// Constructs a new <see cref="AdaptingId{TEntity,TExternalId}"/> object
    /// </summary>
    /// <param name="adaptee">The external id to adapt, it's string representation will be obtained by calling <c>ToString</c> on it</param>
    public AdaptingId(TExternalId adaptee)
        : this(adaptee, e => e.ToString())
    {
    }

    /// <summary>
    /// Constructs a new <see cref="AdaptingId{TEntity,TExternalId}"/> object
    /// </summary>
    /// <param name="adaptee">The external id to adapt</param>
    /// <param name="toString">The function to call to convert the external id to it's string representation</param>
    public AdaptingId(TExternalId adaptee, Func<TExternalId, string> toString)
        : base(toString(adaptee))
    {
        Adaptee = adaptee;
    }
}