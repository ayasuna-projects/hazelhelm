namespace Ayasuna.Hazelhelm.Identity;

using System;

/// <summary>
/// Abstract <see cref="Id"/> implementation that uses the given type parameter <typeparamref name="TEntity"/> as <see cref="Type"/> for the id. 
/// </summary>
/// <typeparam name="TEntity">The <see cref="Type"/> of the id</typeparam>
public abstract class Id<TEntity> : Id where TEntity : notnull
{
    /// <summary>
    /// Constructs a new <see cref="Id{TEntity}"/> object
    /// </summary>
    /// <param name="value">The value of the id</param>
    protected Id(string value)
        : base(typeof(TEntity), value)
    {
    }
}