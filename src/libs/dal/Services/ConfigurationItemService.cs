using System.Security.Claims;
using HSB.Entities;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class ConfigurationItemService : BaseService<ConfigurationItem>, IConfigurationItemService
{
    #region Constructors
    public ConfigurationItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<ConfigurationItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    #endregion
}
