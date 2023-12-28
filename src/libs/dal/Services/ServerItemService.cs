using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using HSB.Models.Filters;
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

    public IEnumerable<ServerItem> Find(ServerItemFilter filter)
    {
        var query = this.Context.ServerItems
            .AsNoTracking()
            .Where(filter.GeneratePredicate());

        if (filter.Sort?.Any() == true)
            query = query.OrderByProperty(filter.Sort);
        else query = query.OrderBy(si => si.Name);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Quantity.HasValue && filter.Page > 1)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .ToArray();
    }

    public IEnumerable<ServerItem> FindForUser(long userId, ServerItemFilter filter)
    {
        var query = (from si in this.Context.ServerItems
                     join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select si)
            .AsNoTracking()
            .Where(filter.GeneratePredicate());

        if (filter.Sort?.Any() == true)
            query = query.OrderByProperty(filter.Sort);
        else query = query.OrderBy(si => si.Name);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Quantity.HasValue && filter.Page > 1)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .ToArray();
    }

    public ServerItem? FindForId(string key, long userId)
    {
        var query = from si in this.Context.ServerItems
                    join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
                    join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                    where si.ServiceNowKey == key
                       && usert.UserId == userId
                    select si;

        return query.FirstOrDefault();
    }
    #endregion
}
