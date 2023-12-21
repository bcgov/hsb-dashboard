using System.Security.Claims;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using HSB.Entities;
using HSB.DAL.Extensions;

namespace HSB.DAL.Services;

public class OrganizationService : BaseService<Organization>, IOrganizationService
{
    #region Constructors
    public OrganizationService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<OrganizationService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    public IEnumerable<Organization> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<Organization, bool>> predicate,
        System.Linq.Expressions.Expression<Func<Organization, Organization>>? sort = null,
        int? take = null,
        int? skip = null)
    {
        var query = (from org in this.Context.Organizations
                     join torg in this.Context.TenantOrganizations on org.Id equals torg.OrganizationId
                     join tenant in this.Context.Tenants on torg.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select org)
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

    public IEnumerable<Organization> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<Organization, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null)
    {
        var query = (from org in this.Context.Organizations
                     join torg in this.Context.TenantOrganizations on org.Id equals torg.OrganizationId
                     join tenant in this.Context.Tenants on torg.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select org)
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
