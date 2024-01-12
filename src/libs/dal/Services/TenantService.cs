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
    public IEnumerable<Tenant> FindForUser<T>(
        long userId,
        System.Linq.Expressions.Expression<Func<Tenant, bool>> predicate,
        System.Linq.Expressions.Expression<Func<Tenant, T>>? sort = null,
        int? take = null,
        int? skip = null)
    {
        var query = (from t in this.Context.Tenants
                     join ut in this.Context.UserTenants on t.Id equals ut.TenantId
                     where ut.UserId == userId
                     select t)
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

    public IEnumerable<Tenant> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<Tenant, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null)
    {
        var query = (from t in this.Context.Tenants
                     join ut in this.Context.UserTenants on t.Id equals ut.TenantId
                     where ut.UserId == userId
                     select t)
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
