using System.Security.Claims;
using HSB.Entities;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class OrganizationService : BaseService<Organization>, IOrganizationService
{
    #region Constructors
    public OrganizationService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<OrganizationService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    #endregion
}
