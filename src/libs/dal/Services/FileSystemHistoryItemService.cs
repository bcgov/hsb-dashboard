using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using HSB.Models.Filters;
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
    public IEnumerable<FileSystemHistoryItem> Find(FileSystemHistoryItemFilter filter)
    {
        var query = (from fsi in this.Context.FileSystemHistoryItems
                     select fsi)
            .Where(filter.GeneratePredicate());

        if (filter.Sort != null)
            query = query.OrderByProperty(filter.Sort);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Page > 1 && filter.Quantity.HasValue)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .AsNoTracking()
            .ToArray();
    }

    public IEnumerable<FileSystemHistoryItem> FindForUser(
        long userId,
        FileSystemHistoryItemFilter filter)
    {
        var query = (from fsi in this.Context.FileSystemHistoryItems
                     join si in this.Context.ServerItems on fsi.FileSystemItem!.ServerItemServiceNowKey equals si.ServiceNowKey
                     join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select fsi)
            .Where(filter.GeneratePredicate());

        if (filter.Sort != null)
            query = query.OrderByProperty(filter.Sort);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Page > 1 && filter.Quantity.HasValue)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .AsNoTracking()
            .ToArray();
    }

    public IEnumerable<FileSystemHistoryItem> FindHistoryByMonth(DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serverServiceKeyNow)
    {
        return this.Context.FindFileSystemHistoryItemsByMonth(start.ToUniversalTime(), end?.ToUniversalTime(), tenantId, organizationId, operatingSystemId, serverServiceKeyNow).ToArray();
    }
    #endregion
}
