using System.Security.Claims;
using HSB.Entities;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class DataSyncService : BaseService<DataSync>, IDataSyncService
{
    #region Constructors
    public DataSyncService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<DataSyncService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    #endregion
}
