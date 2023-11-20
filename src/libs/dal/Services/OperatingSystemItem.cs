using System.Security.Claims;
using HSB.Entities;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class OperatingSystemItemService : BaseService<OperatingSystemItem>, IOperatingSystemItemService
{
    #region Constructors
    public OperatingSystemItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<OperatingSystemItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    #endregion
}
