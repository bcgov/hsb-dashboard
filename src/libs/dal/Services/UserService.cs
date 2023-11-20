using System.Security.Claims;
using HSB.Entities;
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
    public IEnumerable<User> FindAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> FindByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public User? FindByKey(Guid key)
    {
        throw new NotImplementedException();
    }

    public User? FindByUsername(string username)
    {
        throw new NotImplementedException();
    }
    #endregion
}
