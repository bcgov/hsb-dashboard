using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class ServerItemService : BaseService<ServerItem>, IServerItemService
{
    #region Constructors
    public ServerItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<ServerItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    public IEnumerable<ServerItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<ServerItem, bool>> predicate,
        System.Linq.Expressions.Expression<Func<ServerItem, ServerItem>>? sort = null,
        int? take = null,
        int? skip = null)
    {
        var query = (from si in this.Context.ServerItems
                     join ci in this.Context.ConfigurationItems on si.ConfigurationItemId equals ci.Id
                     join tenant in this.Context.Tenants on ci.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select si)
            .Where(predicate);

        if (sort != null)
            query = query.OrderBy(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .AsNoTracking()
            .ToArray();
    }

    public IEnumerable<ServerItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<ServerItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null)
    {
        var query = (from si in this.Context.ServerItems
                     join ci in this.Context.ConfigurationItems on si.ConfigurationItemId equals ci.Id
                     join tenant in this.Context.Tenants on ci.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select si)
            .Where(predicate);

        if (sort?.Any() == true)
            query = query.OrderByProperty(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .AsNoTracking()
            .ToArray();
    }
    #endregion
}
