using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class ConfigurationItemService : BaseService<ConfigurationItem>, IConfigurationItemService
{
    #region Constructors
    public ConfigurationItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<ConfigurationItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    public IEnumerable<ConfigurationItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<ConfigurationItem, bool>> predicate,
        System.Linq.Expressions.Expression<Func<ConfigurationItem, ConfigurationItem>>? sort = null,
        int? take = null,
        int? skip = null)
    {
        var query = (from ci in this.Context.ConfigurationItems
                     join tenant in this.Context.Tenants on ci.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select ci)
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

    public IEnumerable<ConfigurationItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<ConfigurationItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null)
    {
        var query = (from ci in this.Context.ConfigurationItems
                     join tenant in this.Context.Tenants on ci.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select ci)
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
