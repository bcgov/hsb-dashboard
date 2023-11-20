using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public abstract class BaseService : IBaseService
{
    #region Properties

    /// <summary>
    /// get - The datasource context object.
    /// </summary>
    protected HSBContext Context { get; }

    /// <summary>
    /// get - The user principal claim.
    /// </summary>
    public ClaimsPrincipal Principal { get; }

    /// <summary>
    /// get - The service provider.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// get - The logger.
    /// </summary>
    protected ILogger<BaseService> Logger { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a BaseService object, initializes with specified parameters.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="principal"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="logger"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public BaseService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<BaseService> logger)
    {
        this.Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.Principal = principal ?? throw new ArgumentNullException(nameof(principal));
        this.Services = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    #endregion

    #region Methods
    /// <summary>
    /// Stops tracking all currently tracked entities.
    /// Microsoft.EntityFrameworkCore.DbContext is designed to have a short lifetime where a new instance is created for each unit-of-work. This manner means all tracked entities are discarded when the context is disposed at the end of each unit-of-work. However, clearing all tracked entities using this method may be useful in situations where creating a new context instance is not practical.
    /// This method should always be preferred over detaching every tracked entity. Detaching entities is a slow process that may have side effects. This method is much more efficient at clearing all tracked entities from the context.
    /// Note that this method does not generate Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.StateChanged events since entities are not individually detached.
    /// </summary>
    public void ClearChangeTracker()
    {
        this.Context.ChangeTracker.Clear();
    }

    /// <summary>
    /// Commit the transaction and update the database.
    /// </summary>
    /// <returns></returns>
    public int CommitTransaction()
    {
        return this.Context.CommitTransaction();
    }

    /// <summary>
    /// Saves changes to the database.
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess"></param>
    /// <returns></returns>
    public int SaveChanges(bool acceptAllChangesOnSuccess = true)
    {
        return this.Context.SaveChanges(acceptAllChangesOnSuccess);
    }
    #endregion
}
