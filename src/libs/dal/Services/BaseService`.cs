

using System.Security.Claims;
using HSB.DAL.Extensions;
using Microsoft.EntityFrameworkCore;
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
    public virtual TEntity? FindForId(params object?[]? keyValues)
    {
        return this.Context.Find<TEntity>(keyValues);
    }

    /// <summary>
    /// Find the entity for the specified predicate filter.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual IEnumerable<TEntity> Find(
        System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
    {
        var query = this.Context
            .Set<TEntity>()
            .AsNoTracking()
            .Where(predicate);

        return query
            .ToArray();
    }

    /// <summary>
    /// Find the entity for the specified predicate filter.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="sort"></param>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public virtual IEnumerable<TEntity> Find<T>(
        System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate,
        System.Linq.Expressions.Expression<Func<TEntity, T>>? sort = null,
        int? take = null,
        int? skip = null)
    {
        var query = this.Context
            .Set<TEntity>()
            .AsNoTracking()
            .Where(predicate);

        if (sort != null)
            query = query.OrderBy(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .ToArray();
    }

    /// <summary>
    /// Find the entity for the specified predicate filter.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="sort">Array of string pairs ["name asc", "name desc"]</param>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public virtual IEnumerable<TEntity> Find(
        System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null)
    {
        var query = this.Context
            .Set<TEntity>()
            .AsNoTracking()
            .Where(predicate);

        if (sort?.Any() == true)
            query = query.OrderByProperty(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .ToArray();
    }

    /// <summary>
    /// Add the specified 'entity' to the context..
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Add(TEntity entity)
    {
        var entry = this.Context.Entry(entity);
        entry.State = EntityState.Added;
        return entry;
    }

    /// <summary>
    /// Update the specified 'entity' in the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Update(TEntity entity)
    {
        var entry = this.Context.Entry(entity);
        entry.State = EntityState.Modified;
        return entry;
    }

    /// <summary>
    /// Remove the specified 'entity' from the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Remove(TEntity entity)
    {
        var entry = this.Context.Entry(entity);
        entry.State = EntityState.Deleted;
        return entry;
    }
    #endregion
}
