using System.Security.Claims;
using HSB.Entities;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class RoleService : BaseService<Role>, IRoleService
{
    #region Constructors
    public RoleService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<RoleService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    #endregion
}
