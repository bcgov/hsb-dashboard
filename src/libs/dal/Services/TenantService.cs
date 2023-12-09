using System.Security.Claims;
using HSB.Entities;
using LinqKit;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
