using System.Security.Claims;
using HSB.Entities;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class GroupService : BaseService<Group>, IGroupService
{
    #region Constructors
    public GroupService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<GroupService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    #endregion
}
