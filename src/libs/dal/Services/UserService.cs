using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class UserService : BaseService<User>, IUserService
{
    #region Constructors
    public UserService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<UserService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    public IEnumerable<User> Find(Models.Filters.UserFilter filter)
    {
        var query = this.Context
            .Users
            .AsNoTracking();

        if (filter.IncludePermissions == true)
            query = query
                .Include(u => u.GroupsManyToMany).ThenInclude(g => g.Group)
                .Include(u => u.OrganizationsManyToMany).ThenInclude(g => g.Organization)
                .Include(u => u.TenantsManyToMany).ThenInclude(g => g.Tenant);

        query = query.Where(filter.GeneratePredicate());

        if (filter.Sort?.Any() == true)
            query = query.OrderByProperty(filter.Sort);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Page > 1 && filter.Quantity.HasValue)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .ToArray();
    }

    public IEnumerable<User> FindByEmail(string email)
    {
        return this.Context.Users
            .Include(u => u.Groups).ThenInclude(g => g.Roles)
            .Where(u => EF.Functions.Like(u.Email, email))
            .AsSingleQuery()
            .ToArray();
    }

    public User? FindForId(int id, bool includePermissions)
    {
        var query = this.Context.Users.Where(u => u.Id == id);

        if (includePermissions)
            query = query
                .Include(m => m.Groups).ThenInclude(g => g.Roles)
                .Include(m => m.Organizations)
                .Include(m => m.Tenants);

        return query.FirstOrDefault();
    }

    public User? FindByKey(string key)
    {
        return this.Context.Users
            .Include(u => u.Groups).ThenInclude(g => g.Roles)
            .AsSingleQuery()
            .FirstOrDefault(u => u.Key == key);
    }

    public User? FindByUsername(string username)
    {
        return this.Context.Users
            .Include(u => u.Groups).ThenInclude(g => g.Roles)
            .AsSingleQuery()
            .FirstOrDefault(u => EF.Functions.Like(u.Username, username));
    }

    public override EntityEntry<User> Update(User entity)
    {
        // Update groups
        var originalGroups = this.Context.UserGroups.Where(ug => ug.UserId == entity.Id).ToArray();
        originalGroups.Except(entity.GroupsManyToMany).ForEach((group) =>
        {
            this.Context.Entry(group).State = EntityState.Deleted;
        });
        entity.GroupsManyToMany.ForEach((group) =>
        {
            var originalGroup = originalGroups.FirstOrDefault(s => s.GroupId == group.GroupId);
            if (originalGroup == null)
            {
                group.UserId = entity.Id;
                this.Context.Entry(group).State = EntityState.Added;
            }
        });

        // Update organizations
        var originalOrganizations = this.Context.UserOrganizations.Where(ut => ut.UserId == entity.Id).ToArray();
        originalOrganizations.Except(entity.OrganizationsManyToMany).ForEach((organization) =>
        {
            this.Context.Entry(organization).State = EntityState.Deleted;
        });
        entity.OrganizationsManyToMany.ForEach((organization) =>
        {
            var originalOrganization = originalOrganizations.FirstOrDefault(s => s.OrganizationId == organization.OrganizationId);
            if (originalOrganization == null)
            {
                organization.UserId = entity.Id;
                this.Context.Entry(organization).State = EntityState.Added;
            }
        });

        // Update tenants
        var originalTenants = this.Context.UserTenants.Where(ut => ut.UserId == entity.Id).ToArray();
        originalTenants.Except(entity.TenantsManyToMany).ForEach((tenant) =>
        {
            this.Context.Entry(tenant).State = EntityState.Deleted;
        });
        entity.TenantsManyToMany.ForEach((tenant) =>
        {
            var originalTenant = originalTenants.FirstOrDefault(s => s.TenantId == tenant.TenantId);
            if (originalTenant == null)
            {
                tenant.UserId = entity.Id;
                this.Context.Entry(tenant).State = EntityState.Added;
            }
        });

        return base.Update(entity);
    }
    #endregion
}
