using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using HSB.Models;
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
            .AsNoTracking()
            .AsSingleQuery()
            .ToArray();
    }

    public IEnumerable<ServerItem> FindForUser(long userId, ServerItemFilter filter)
    {
        var userOrganizationQuery = from uo in this.Context.UserOrganizations
                                    join o in this.Context.Organizations on uo.OrganizationId equals o.Id
                                    where uo.UserId == userId
                                        && o.IsEnabled
                                    select uo.OrganizationId;
        var userTenants = from ut in this.Context.UserTenants
                          join t in this.Context.Tenants on ut.TenantId equals t.Id
                          where ut.UserId == userId
                            && t.IsEnabled
                          select ut.TenantId;

        var query = (from si in this.Context.ServerItems
                     where si.TenantId != null
                        && (userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId))
                     select si)
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
            .AsNoTracking()
            .AsSplitQuery()
            .ToArray();
    }

    public IEnumerable<ServerItemSmallModel> FindSimple(ServerItemFilter filter)
    {
        var query = this.Context.ServerItems
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
            .AsNoTracking()
            .AsSplitQuery()
            .Select(si => new ServerItemSmallModel(si))
            .ToArray();
    }

    public IEnumerable<ServerItemSmallModel> FindSimpleForUser(long userId, ServerItemFilter filter)
    {
        var userOrganizationQuery = from uo in this.Context.UserOrganizations
                                    join o in this.Context.Organizations on uo.OrganizationId equals o.Id
                                    where uo.UserId == userId
                                        && o.IsEnabled
                                    select uo.OrganizationId;
        var userTenants = from ut in this.Context.UserTenants
                          join t in this.Context.Tenants on ut.TenantId equals t.Id
                          where ut.UserId == userId
                            && t.IsEnabled
                          select ut.TenantId;

        var query = (from si in this.Context.ServerItems
                     where si.TenantId != null
                        && (userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId))
                     select si)
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
            .AsNoTracking()
            .AsSplitQuery()
            .Select(si => new ServerItemSmallModel(si))
            .ToArray();
    }

    public ServerItem? FindForId(string key, bool includeFileSystemItems = false)
    {
        var query = from si in this.Context.ServerItems
                    join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
                    where si.ServiceNowKey == key
                    select si;

        if (includeFileSystemItems)
            query = query.Include(m => m.FileSystemItems);

        return query
            .AsSingleQuery()
            .FirstOrDefault();
    }

    public ServerItem? FindForId(string key, long userId, bool includeFileSystemItems = false)
    {
        var query = from si in this.Context.ServerItems
                    join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
                    join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                    where si.ServiceNowKey == key
                       && usert.UserId == userId
                    select si;

        if (includeFileSystemItems)
            query = query.Include(m => m.FileSystemItems);

        return query
            .AsSingleQuery()
            .FirstOrDefault();
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
        // Move original to history if created more than 12 hours ago.
        var original = this.Context.ServerItems.AsNoTracking().FirstOrDefault(si => si.ServiceNowKey == entity.ServiceNowKey);
        if (original != null && original.CreatedOn.AddHours(12).ToUniversalTime() <= DateTimeOffset.UtcNow)
        {
            // This key provides a way to link the current server item record with the one in history.
            var key = Guid.NewGuid();
            entity.HistoryKey = key;
            original.HistoryKey = key;
            this.Context.ServerHistoryItems.Add(new ServerHistoryItem(original));
        }
        else
        {
            entity.HistoryKey = original?.HistoryKey ?? Guid.NewGuid();
        }

        return base.Update(entity);
    }
    #endregion
}
