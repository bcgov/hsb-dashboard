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

    public IEnumerable<ServerHistoryItem> Find(ServerHistoryItemFilter filter)
    {
        var query = this.Context.ServerHistoryItems
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

    public IEnumerable<ServerHistoryItem> FindForUser(int userId, ServerHistoryItemFilter filter)
    {
        var query = (from si in this.Context.ServerHistoryItems
                     join org in this.Context.Organizations on si.OrganizationId equals org.Id
                     join tOrg in this.Context.TenantOrganizations on org.Id equals tOrg.OrganizationId
                     join tenant in this.Context.Tenants on tOrg.TenantId equals tenant.Id
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

    public IEnumerable<ServerHistoryItem> FindHistoryByMonth(DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serviceKeyNow)
    {
        return this.Context.FindServerHistoryItemsByMonth(start.ToUniversalTime(), end?.ToUniversalTime(), tenantId, organizationId, operatingSystemId, serviceKeyNow).ToArray();
    }

    public IEnumerable<ServerHistoryItem> FindHistoryByMonthForUser(int userId, DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serviceKeyNow)
    {
        return this.Context.FindServerHistoryItemsByMonthForUser(userId, start.ToUniversalTime(), end?.ToUniversalTime(), tenantId, organizationId, operatingSystemId, serviceKeyNow).ToArray();
    }
    #endregion
}
