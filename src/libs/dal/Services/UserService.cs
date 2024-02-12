using System.Security.Claims;
using HSB.Core.Exceptions;
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

    public IEnumerable<User> FindByEmail(string email, bool includePermissions)
    {
        var query = this.Context.Users.AsQueryable();

        if (includePermissions)
            query = query
                .Include(m => m.Groups).ThenInclude(g => g.Roles)
                .Include(m => m.OrganizationsManyToMany).ThenInclude(m => m.Organization)
                .Include(m => m.TenantsManyToMany).ThenInclude(m => m.Tenant);

        return query
            .Where(u => EF.Functions.Like(u.Email, email))
            .AsSingleQuery()
            .ToArray();
    }

    public User? FindForId(int id, bool includePermissions)
    {
        var query = this.Context.Users.AsQueryable();

        if (includePermissions)
            query = query
                .Include(m => m.Groups).ThenInclude(g => g.Roles)
                .Include(m => m.OrganizationsManyToMany).ThenInclude(m => m.Organization)
                .Include(m => m.TenantsManyToMany).ThenInclude(m => m.Tenant);

        return query
            .Where(u => u.Id == id)
            .AsSingleQuery()
            .FirstOrDefault();
    }

    public User? FindByKey(string key, bool includePermissions)
    {
        var query = this.Context.Users.AsQueryable();

        if (includePermissions)
            query = query
                .Include(m => m.Groups).ThenInclude(g => g.Roles)
                .Include(m => m.OrganizationsManyToMany).ThenInclude(m => m.Organization)
                .Include(m => m.TenantsManyToMany).ThenInclude(m => m.Tenant);

        return query
            .AsSingleQuery()
            .FirstOrDefault(u => u.Key == key);
    }

    public User? FindByUsername(string username, bool includePermissions)
    {
        var query = this.Context.Users.AsQueryable();

        if (includePermissions)
            query = query
                .Include(m => m.Groups).ThenInclude(g => g.Roles)
                .Include(m => m.OrganizationsManyToMany).ThenInclude(m => m.Organization)
                .Include(m => m.TenantsManyToMany).ThenInclude(m => m.Tenant);

        return query
            .AsSingleQuery()
            .FirstOrDefault(u => EF.Functions.Like(u.Username, username));
    }

    public override EntityEntry<User> Update(User entity)
    {
        var isSystemAdmin = Keycloak.Extensions.IdentityExtensions.HasClientRole(this.Principal, HSB.Keycloak.ClientRole.SystemAdministrator);
        var isOrganizationAdmin = Keycloak.Extensions.IdentityExtensions.HasClientRole(this.Principal, HSB.Keycloak.ClientRole.OrganizationAdministrator);
        var organizationAdminGroup = this.Context.Groups.FirstOrDefault(g => g.Name == Core.Extensions.EnumExtensions.GetName(HSB.Keycloak.ClientRole.OrganizationAdministrator));

        // A organization admin can only add the Organization Admin and Client role to users within the tenant or organization they belong to.
        var username = Core.Extensions.IdentityExtensions.GetUsername(this.Principal) ?? throw new NotAuthorizedException("Username is missing");
        var user = Find(new HSB.Models.Filters.UserFilter() { Username = username, IncludePermissions = true }).FirstOrDefault() ?? throw new NotAuthorizedException($"User [{username}] does not exist");
        var allowedTenants = user.TenantsManyToMany.Select(t => t.TenantId).ToArray();
        var allowedOrganizations = user.OrganizationsManyToMany.Select(o => o.OrganizationId).ToArray().Concat(this.Context.TenantOrganizations.Where(to => allowedTenants.Contains(to.TenantId)).Select(to => to.OrganizationId).ToArray()).Distinct();

        // Update groups
        var originalGroups = this.Context.UserGroups.Where(ug => ug.UserId == entity.Id).ToArray();
        originalGroups.Except(entity.GroupsManyToMany).ForEach((group) =>
        {
            // Only allowed to remove organization admin if the user performing the action is an organization admin.
            if (isSystemAdmin || (isOrganizationAdmin && group.GroupId == organizationAdminGroup?.Id))
            {
                this.Context.Entry(group).State = EntityState.Deleted;
            }
        });
        entity.GroupsManyToMany.ForEach((group) =>
        {
            var originalGroup = originalGroups.FirstOrDefault(s => s.GroupId == group.GroupId);
            // Only allowed to add organization admin if the user performing the action is an organization admin.
            if (originalGroup == null && (isSystemAdmin || (isOrganizationAdmin && group.GroupId == organizationAdminGroup?.Id)))
            {
                group.UserId = entity.Id;
                this.Context.Entry(group).State = EntityState.Added;
            }
        });

        // Update organizations
        var originalOrganizations = this.Context.UserOrganizations.Where(ut => ut.UserId == entity.Id).ToArray();
        originalOrganizations.Except(entity.OrganizationsManyToMany).ForEach((organization) =>
        {
            // Only allow to remove organization if the user performing the action is member of this organization.
            if (isSystemAdmin || (isOrganizationAdmin && allowedOrganizations.Contains(organization.OrganizationId)))
            {
                this.Context.Entry(organization).State = EntityState.Deleted;
            }
        });
        entity.OrganizationsManyToMany.ForEach((organization) =>
        {
            var originalOrganization = originalOrganizations.FirstOrDefault(s => s.OrganizationId == organization.OrganizationId);
            // Only allowed to add organization if the user performing the action is an organization admin and is a member of this organization.
            if (originalOrganization == null && (isSystemAdmin || (isOrganizationAdmin && allowedOrganizations.Contains(organization.OrganizationId))))
            {
                organization.UserId = entity.Id;
                this.Context.Entry(organization).State = EntityState.Added;
            }
        });

        // Update tenants
        var originalTenants = this.Context.UserTenants.Where(ut => ut.UserId == entity.Id).ToArray();
        originalTenants.Except(entity.TenantsManyToMany).ForEach((tenant) =>
        {
            // Only allow to remove tenant if the user performing the action is member of this tenant.
            if (isSystemAdmin || (isOrganizationAdmin && allowedTenants.Contains(tenant.TenantId)))
            {
                this.Context.Entry(tenant).State = EntityState.Deleted;
            }
        });
        entity.TenantsManyToMany.ForEach((tenant) =>
        {
            var originalTenant = originalTenants.FirstOrDefault(s => s.TenantId == tenant.TenantId);
            // Only allowed to add tenant if the user performing the action is an organization admin and is a member of this tenant.
            if (originalTenant == null && (isSystemAdmin || (isOrganizationAdmin && allowedTenants.Contains(tenant.TenantId))))
            {
                tenant.UserId = entity.Id;
                this.Context.Entry(tenant).State = EntityState.Added;
            }
        });

        return base.Update(entity);
    }
    #endregion
}
