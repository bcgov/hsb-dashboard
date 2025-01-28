using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using HSB.Models.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class ServerHistoryItemService : BaseService<ServerHistoryItem>, IServerHistoryItemService
{
    #region Constructors
    public ServerHistoryItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<ServerHistoryItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods

    public IEnumerable<ServerHistoryItem> Find(ServerHistoryItemFilter filter, bool includeRelated = false)
    {
        var query = this.Context.ServerHistoryItems.AsQueryable();

        if (includeRelated)
            query = query
                .Include(shi => shi.Tenant)
                .Include(shi => shi.Organization)
                .Include(shi => shi.OperatingSystemItem);

        query = query
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

    public IEnumerable<ServerHistoryItem> FindForUser(int userId, ServerHistoryItemFilter filter, bool includeRelated = false)
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

        var query = from si in this.Context.ServerHistoryItems
                    where userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId)
                    select si;

        if (includeRelated)
            query = query
                .Include(shi => shi.Tenant)
                .Include(shi => shi.Organization)
                .Include(shi => shi.OperatingSystemItem);

        query = query
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

    public IEnumerable<ServerHistoryItem> FindHistoryByMonth(DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serviceKeyNow, bool includeRelated = false)
    {
        var items = this.Context.FindServerHistoryItemsByMonth(start.ToUniversalTime(), end?.ToUniversalTime(), tenantId, organizationId, operatingSystemId, serviceKeyNow)
            .AsNoTracking()
            .ToArray();

        if (includeRelated)
        {
            var tenantIds = items.Select(i => i.TenantId).Distinct();
            var organizationIds = items.Select(i => i.OrganizationId).Distinct();
            var operatingSystemIds = items.Select(i => i.OperatingSystemItemId).Distinct();

            var tenants = this.Context.Tenants.Where(t => tenantIds.Contains(t.Id)).ToDictionary(t => t.Id);
            var organizations = this.Context.Organizations.Where(o => organizationIds.Contains(o.Id)).ToDictionary(o => o.Id);
            var operatingSystemItems = this.Context.OperatingSystemItems.Where(os => operatingSystemIds.Contains(os.Id)).ToDictionary(os => os.Id);

            foreach (var item in items)
            {
                item.Tenant = item.TenantId.HasValue ? tenants[item.TenantId.Value] : null;
                item.Organization = organizations[item.OrganizationId];
                item.OperatingSystemItem = item.OperatingSystemItemId.HasValue ? operatingSystemItems[item.OperatingSystemItemId.Value] : null;
            }
        }

        return items;
    }

    public IEnumerable<CompactServerHistoryItem> FindCompactHistoryByMonth(DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serviceKeyNow)
    {
        var items = this.Context.FindServerHistoryItemsByMonth(start.ToUniversalTime(), end?.ToUniversalTime(), tenantId, organizationId, operatingSystemId, serviceKeyNow)
            .Select(shi => new CompactServerHistoryItem
            {
                Id = shi.Id,
                Name = shi.Name,
                ServiceNowKey = shi.ServiceNowKey,
                TenantId = shi.TenantId,
                OrganizationId = shi.OrganizationId,
                Capacity = shi.Capacity,
                AvailableSpace = shi.AvailableSpace,
                OperatingSystemItemId = shi.OperatingSystemItemId,
                CreatedOn = shi.CreatedOn,
                UpdatedOn = shi.UpdatedOn
            })
            .AsNoTracking()
            .ToArray();

        return items;
    }

    public IEnumerable<ServerHistoryItem> FindHistoryByMonthForUser(int userId, DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serviceKeyNow, bool includeRelated = false)
    {
        var items = this.Context.FindServerHistoryItemsByMonthForUser(userId, start.ToUniversalTime(), end?.ToUniversalTime(), tenantId, organizationId, operatingSystemId, serviceKeyNow)
        .AsNoTracking()
        .ToArray();

        if (includeRelated)
        {
            var tenantIds = items.Select(i => i.TenantId).Distinct();
            var organizationIds = items.Select(i => i.OrganizationId).Distinct();
            var operatingSystemIds = items.Select(i => i.OperatingSystemItemId).Distinct();

            var tenants = this.Context.Tenants.Where(t => tenantIds.Contains(t.Id)).ToDictionary(t => t.Id);
            var organizations = this.Context.Organizations.Where(o => organizationIds.Contains(o.Id)).ToDictionary(o => o.Id);
            var operatingSystemItems = this.Context.OperatingSystemItems.Where(os => operatingSystemIds.Contains(os.Id)).ToDictionary(os => os.Id);

            foreach (var item in items)
            {
                item.Tenant = item.TenantId.HasValue ? tenants[item.TenantId.Value] : null;
                item.Organization = organizations[item.OrganizationId];
                item.OperatingSystemItem = item.OperatingSystemItemId.HasValue ? operatingSystemItems[item.OperatingSystemItemId.Value] : null;
            }
        }

        return items;
    }
    #endregion
}
