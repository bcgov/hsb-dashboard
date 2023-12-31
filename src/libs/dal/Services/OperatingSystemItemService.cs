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
        var query = (from osi in this.Context.OperatingSystemItems
                     join si in this.Context.ServerItems on osi.Id equals si.OperatingSystemItemId
                     join org in this.Context.Organizations on si.OrganizationId equals org.Id
                     join tOrg in this.Context.TenantOrganizations on org.Id equals tOrg.OrganizationId
                     join tenant in this.Context.Tenants on tOrg.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select osi)
            .Where(predicate)
            .Distinct();

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

    public IEnumerable<OperatingSystemItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<OperatingSystemItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null)
    {
        var query = (from osi in this.Context.OperatingSystemItems
                     join si in this.Context.ServerItems on osi.Id equals si.OperatingSystemItemId
                     join org in this.Context.Organizations on si.OrganizationId equals org.Id
                     join tOrg in this.Context.TenantOrganizations on org.Id equals tOrg.OrganizationId
                     join tenant in this.Context.Tenants on tOrg.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
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
            .ToArray();
    }
    #endregion
}
