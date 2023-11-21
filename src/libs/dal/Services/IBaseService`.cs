namespace HSB.DAL.Services;

public interface IBaseService<TEntity> : IBaseService
    where TEntity : class
{
    /// <summary>
    /// Find the entity for the specified `keyValues`.
    /// </summary>
    /// <param name="keyValues"></param>
    /// <returns></returns>
    TEntity? FindForId(params object?[]? keyValues);

    /// <summary>
    /// Find the entity for the specified predicate filter.
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="sort"></param>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    IEnumerable<TEntity> Find(
        System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate,
        System.Linq.Expressions.Expression<Func<TEntity, bool>>? sort = null,
        int? take = null,
        int? skip = null);

    /// <summary>
    /// Add the specified 'entity' to the context..
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Add(TEntity entity);

    /// <summary>
    /// Update the specified 'entity' in the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Update(TEntity entity);

    /// <summary>
    /// Remove the specified 'entity' from the context.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Remove(TEntity entity);
}
