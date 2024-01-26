using System.Security.Claims;
using HSB.Entities;
using LinqKit;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using HSB.DAL.Extensions;

namespace HSB.DAL.Services;

public class TenantService : BaseService<Tenant>, ITenantService
{
    #region Constructors
    public TenantService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<TenantService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    public IEnumerable<Tenant> Find(
        Models.Filters.TenantFilter filter)
    {
        var query = from tenant in this.Context.Tenants
                    select tenant;

        if (filter.IncludeOrganizations == true)
            query = query.Include(o => o.OrganizationsManyToMany).ThenInclude(t => t.Organization);

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

    public IEnumerable<Tenant> FindForUser(
        long userId,
        Models.Filters.TenantFilter filter)
    {
        var userTenantQuery = from uo in this.Context.UserTenants
                              where uo.UserId == userId
                              select uo.TenantId;
        var tenantOrganizationQuery = from tOrg in this.Context.TenantOrganizations
                                      join ut in this.Context.UserTenants on tOrg.TenantId equals ut.TenantId
                                      where ut.UserId == userId
                                      select tOrg.TenantId;

        var query = from tenant in this.Context.Tenants
                    where userTenantQuery.Contains(tenant.Id) || tenantOrganizationQuery.Contains(tenant.Id)
                    select tenant;

        if (filter.IncludeOrganizations == true)
            query = query.Include(o => o.OrganizationsManyToMany).ThenInclude(t => t.Organization);

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

    public Tenant? FindForId(int id, bool includeOrganizations)
    {
        var query = this.Context.Tenants.Where(u => u.Id == id);

        if (includeOrganizations)
            query = query
                .Include(m => m.OrganizationsManyToMany).ThenInclude(g => g.Organization);

        return query.FirstOrDefault();
    }

    public override EntityEntry<Tenant> Add(Tenant entity)
    {
        if (entity.OrganizationsManyToMany.Any())
        {
            // Add relationship to organizations.
            entity.OrganizationsManyToMany.ForEach(o =>
            {
                o.Tenant = entity;
                this.Context.Entry(o).State = EntityState.Added;
            });
        }

        return base.Add(entity);
    }

    public override EntityEntry<Tenant> Update(Tenant entity)
    {
        // Fetch existing organization relationships.
        var currentOrganizations = this.Context.TenantOrganizations.Where(to => to.TenantId == entity.Id).ToArray();
        currentOrganizations.Except(entity.OrganizationsManyToMany).ForEach(remove =>
        {
            this.Context.Entry(remove).State = EntityState.Deleted;
        });
        entity.OrganizationsManyToMany.ForEach(addOrUpdate =>
        {
            var currentOrganization = currentOrganizations.FirstOrDefault(o => o.OrganizationId == addOrUpdate.OrganizationId);
            if (currentOrganization == null)
            {
                this.Context.Entry(addOrUpdate).State = EntityState.Added;
            }
        });
        return base.Update(entity);
    }
    #endregion
}
