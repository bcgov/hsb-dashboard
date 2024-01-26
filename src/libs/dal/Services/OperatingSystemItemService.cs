using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class OperatingSystemItemService : BaseService<OperatingSystemItem>, IOperatingSystemItemService
{
    #region Constructors
    public OperatingSystemItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<OperatingSystemItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    public IEnumerable<OperatingSystemItem> FindForUser<T>(
        long userId,
        System.Linq.Expressions.Expression<Func<OperatingSystemItem, bool>> predicate,
        System.Linq.Expressions.Expression<Func<OperatingSystemItem, T>>? sort = null,
        int? take = null,
        int? skip = null)
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

        var query = (from osi in this.Context.OperatingSystemItems
                     join si in this.Context.ServerItems on osi.Id equals si.OperatingSystemItemId
                     where userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId)
                     select osi)
            .Where(predicate)
            .AsNoTracking()
            .AsSplitQuery()
            .Distinct();

        if (sort != null)
            query = query.OrderBy(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .ToArray();
    }

    public IEnumerable<OperatingSystemItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<OperatingSystemItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null)
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

        var query = (from osi in this.Context.OperatingSystemItems
                     join si in this.Context.ServerItems on osi.Id equals si.OperatingSystemItemId
                     where userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId)
                     select osi)
            .Where(predicate)
            .Distinct();

        if (sort?.Any() == true)
            query = query.OrderByProperty(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .AsNoTracking()
            .AsSplitQuery()
            .ToArray();
    }
    #endregion
}
