using System.Security.Claims;
using HSB.Entities;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class FileSystemItemService : BaseService<FileSystemItem>, IFileSystemItemService
{
    #region Constructors
    public FileSystemItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<FileSystemItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    #endregion
}
