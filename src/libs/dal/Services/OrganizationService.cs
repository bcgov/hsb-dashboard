using System.Security.Claims;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using HSB.Entities;
using HSB.DAL.Extensions;
using HSB.Models.Lists;

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
    /// <summary>
    /// Find the entity for the specified `keyValues`.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Organization? FindForIdAsNoTracking(int id)
    {
        return this.Context.Organizations.AsNoTracking()
            .FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Organization> Find(Models.Filters.OrganizationFilter filter)
    {
        var query = from org in this.Context.Organizations
                    select org;

        if (filter.IncludeTenants == true)
            query = query.Include(o => o.TenantsManyToMany).ThenInclude(t => t.Tenant);

        query = query
            .Where(filter.GeneratePredicate());

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

    public IEnumerable<Organization> FindForUser(
        long userId,
        Models.Filters.OrganizationFilter filter)
    {
        var userOrganizationQuery = from uo in this.Context.UserOrganizations
                                    where uo.UserId == userId
                                    select uo.OrganizationId;
        var tenantOrganizationQuery = from tOrg in this.Context.TenantOrganizations
                                      join ut in this.Context.UserTenants on tOrg.TenantId equals ut.TenantId
                                      where ut.UserId == userId
                                      select tOrg.OrganizationId;

        var query = from org in this.Context.Organizations
                    where userOrganizationQuery.Contains(org.Id) || tenantOrganizationQuery.Contains(org.Id)
                    select org;

        if (filter.IncludeTenants == true)
            query = query.Include(o => o.TenantsManyToMany).ThenInclude(t => t.Tenant);

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
    public IEnumerable<OrganizationListModel> FindList(Models.Filters.OrganizationFilter filter)
    {
        var query = from org in this.Context.Organizations
                    select org;

        if (filter.IncludeTenants == true)
            query = query.Include(o => o.TenantsManyToMany).ThenInclude(t => t.Tenant);

        query = query
            .Where(filter.GeneratePredicate());

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
            .Select(o => new OrganizationListModel(o, true, false))
            .ToArray();
    }

    public IEnumerable<OrganizationListModel> FindListForUser(
        long userId,
        Models.Filters.OrganizationFilter filter)
    {
        var userOrganizationQuery = from uo in this.Context.UserOrganizations
                                    where uo.UserId == userId
                                    select uo.OrganizationId;
        var tenantOrganizationQuery = from tOrg in this.Context.TenantOrganizations
                                      join ut in this.Context.UserTenants on tOrg.TenantId equals ut.TenantId
                                      where ut.UserId == userId
                                      select tOrg.OrganizationId;

        var query = from org in this.Context.Organizations
                    where userOrganizationQuery.Contains(org.Id) || tenantOrganizationQuery.Contains(org.Id)
                    select org;

        if (filter.IncludeTenants == true)
            query = query.Include(o => o.TenantsManyToMany).ThenInclude(t => t.Tenant);

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
            .Select(o => new OrganizationListModel(o, true, false))
            .ToArray();
    }

    public Organization? FindForId(int id, bool includeTenants)
    {
        var query = this.Context.Organizations.Where(u => u.Id == id);

        if (includeTenants)
            query = query
                .Include(m => m.TenantsManyToMany).ThenInclude(g => g.Tenant);

        return query.FirstOrDefault();
    }

    public IEnumerable<Organization> Cleanup()
    {
        var organizations = this.Context.Organizations.Where(o => !o.ServerItems.Any()).ToArray();
        this.Context.Organizations.RemoveRange(organizations);
        this.CommitTransaction();
        return organizations;
    }
    #endregion
}
