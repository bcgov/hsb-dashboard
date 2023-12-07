using System.Security.Claims;
using HSB.Entities;
using Microsoft.Extensions.Logging;

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
    #endregion
}
