

using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public abstract class BaseService<TEntity> : BaseService, IBaseService<TEntity>
    where TEntity : class
{
    #region Constructors
    public BaseService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<BaseService<TEntity>> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    /// <summary>
    /// Find the entity for the specified `keyValues`.
    /// </summary>
    /// <param name="keyValues"></param>
    /// <returns></returns>
    public TEntity? FindForId(params object?[]? keyValues)
    {
        return this.Context.Find<TEntity>(keyValues);
    }

    /// <summary>
    /// Add the specified 'entity' to the context..
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Add(TEntity entity)
    {
        return this.Context.Add(entity);
    }

    /// <summary>
    /// Update the specified 'entity' in the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Update(TEntity entity)
    {
        return this.Context.Update(entity);
    }

    /// <summary>
    /// Remove the specified 'entity' from the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Remove(TEntity entity)
    {
        return this.Context.Remove(entity);
    }
    #endregion
}
