using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using HSB.Models.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            .Where(filter.GeneratePredicate())
            .Distinct();

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
            .Where(filter.GeneratePredicate())
            .Distinct();

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

    public override EntityEntry<ServerItem> Add(ServerItem entity)
    {
        // This key provides a way to link only the current history record.
        var key = Guid.NewGuid();
        entity.HistoryKey = key;

        var result = base.Add(entity);

        // Add item to history.
        // This ensures we have a matching record when querying history.
        this.Context.ServerHistoryItems.Add(new ServerHistoryItem(entity));

        return result;
    }

    public override EntityEntry<ServerItem> Update(ServerItem entity)
    {
        // This key provides a way to link only the current history record.
        var key = Guid.NewGuid();
        entity.HistoryKey = key;

        // Move original to history.
        // An unsafe assumption occurs here, where we copy the calculated capacity and available space.
        // If the original calculations are off, the historical values will be to.
        // The calculation occurs when a file system item is added or updated.
        // All file system items should be added/updated before a new server item record is updated.
        var original = this.Context.ServerItems.AsNoTracking().FirstOrDefault(si => si.ServiceNowKey == entity.ServiceNowKey);
        if (original != null)
        {
            original.HistoryKey = key;
            this.Context.ServerHistoryItems.Add(new ServerHistoryItem(original));
        }

        return base.Update(entity);
    }
    #endregion
}
