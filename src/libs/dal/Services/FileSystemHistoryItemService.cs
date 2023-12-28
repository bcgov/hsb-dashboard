using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class FileSystemHistoryItemService : BaseService<FileSystemHistoryItem>, IFileSystemHistoryItemService
{
    #region Constructors
    public FileSystemHistoryItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<FileSystemHistoryItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    #endregion
}
