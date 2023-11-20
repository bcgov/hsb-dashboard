using System.Security.Claims;

namespace HSB.DAL.Services;

public interface IBaseService
{
    #region Properties
    ClaimsPrincipal Principal { get; }

    IServiceProvider Services { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Save all the changes in the context to the database in a single transaction.
    /// </summary>
    /// <returns></returns>
    int CommitTransaction();

    /// <summary>
    /// Stops tracking all currently tracked entities.
    /// Microsoft.EntityFrameworkCore.DbContext is designed to have a short lifetime where a new instance is created for each unit-of-work. This manner means all tracked entities are discarded when the context is disposed at the end of each unit-of-work. However, clearing all tracked entities using this method may be useful in situations where creating a new context instance is not practical.
    /// This method should always be preferred over detaching every tracked entity. Detaching entities is a slow process that may have side effects. This method is much more efficient at clearing all tracked entities from the context.
    /// Note that this method does not generate Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.StateChanged events since entities are not individually detached.
    /// </summary>
    void ClearChangeTracker();

    /// <summary>
    /// Saves changes to the database.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <returns></returns>
    int SaveChanges(bool acceptAllChangesOnSuccess = true);
    #endregion
}
