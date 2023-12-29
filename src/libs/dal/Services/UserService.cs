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

        if (filter.IncludeGroups == true)
            query = query.Include(u => u.GroupsManyToMany).ThenInclude(g => g.Group);

        if (filter.IncludeTenants == true)
            query = query.Include(u => u.TenantsManyToMany).ThenInclude(g => g.Tenant);

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
            .ToArray();
    }

    public User? FindByKey(string key)
    {
        return this.Context.Users
            .Include(u => u.Groups).ThenInclude(g => g.Roles)
            .FirstOrDefault(u => u.Key == key);
    }

    public User? FindByUsername(string username)
    {
        return this.Context.Users
            .Include(u => u.Groups).ThenInclude(g => g.Roles)
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

        // Update tenants
        var originalTenants = this.Context.UserTenants.Where(ut => ut.UserId == entity.Id).ToArray();
        originalTenants.Except(entity.TenantsManyToMany).ForEach((source) =>
        {
            this.Context.Entry(source).State = EntityState.Deleted;
        });
        entity.TenantsManyToMany.ForEach((group) =>
        {
            var originalTenant = originalTenants.FirstOrDefault(s => s.TenantId == group.TenantId);
            if (originalTenant == null)
            {
                group.UserId = entity.Id;
                this.Context.Entry(group).State = EntityState.Added;
            }
        });

        return base.Update(entity);
    }
    #endregion
}
