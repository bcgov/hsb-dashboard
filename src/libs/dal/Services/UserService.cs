using System.Security.Claims;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;
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
    #endregion
}
