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
        var userOrganizationQuery = from uo in this.Context.UserOrganizations
                                    where uo.UserId == userId
                                    select uo.OrganizationId;
        var userTenants = from ut in this.Context.UserTenants
                          where ut.UserId == userId
                          select ut.TenantId;

        var query = (from si in this.Context.ServerHistoryItems
                     where userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId)
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
