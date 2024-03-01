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
            .Where(filter.GeneratePredicate())
            .Distinct();

        if (filter.Sort != null)
            query = query.OrderByProperty(filter.Sort);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Page > 1 && filter.Quantity.HasValue)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .AsNoTracking()
            .AsSingleQuery()
            .ToArray();
    }

    public IEnumerable<FileSystemHistoryItem> FindForUser(
        int userId,
        FileSystemHistoryItemFilter filter)
    {
        var userOrganizationQuery = from uo in this.Context.UserOrganizations
                                    join o in this.Context.Organizations on uo.OrganizationId equals o.Id
                                    where uo.UserId == userId
                                        && o.IsEnabled
                                    select uo.OrganizationId;
        var userTenants = from ut in this.Context.UserTenants
                          join t in this.Context.Tenants on ut.TenantId equals t.Id
                          where ut.UserId == userId
                            && t.IsEnabled
                          select ut.TenantId;

        var query = (from fsi in this.Context.FileSystemHistoryItems
                     join si in this.Context.ServerItems on fsi.FileSystemItem!.ServerItemServiceNowKey equals si.ServiceNowKey
                     where userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId)
                     select fsi)
            .Where(filter.GeneratePredicate())
            .Distinct();

        if (filter.Sort != null)
            query = query.OrderByProperty(filter.Sort);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Page > 1 && filter.Quantity.HasValue)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .AsNoTracking()
            .AsSplitQuery()
            .ToArray();
    }

    public IEnumerable<FileSystemHistoryItem> FindHistoryByMonth(DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serverServiceKeyNow)
    {
        return this.Context.FindFileSystemHistoryItemsByMonth(start.ToUniversalTime(), end?.ToUniversalTime(), tenantId, organizationId, operatingSystemId, serverServiceKeyNow)
            .AsNoTracking()
            .ToArray();
    }

    public IEnumerable<FileSystemHistoryItem> FindHistoryByMonthForUser(int userId, DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serverServiceKeyNow)
    {
        return this.Context.FindFileSystemHistoryItemsByMonthForUser(userId, start.ToUniversalTime(), end?.ToUniversalTime(), tenantId, organizationId, operatingSystemId, serverServiceKeyNow)
            .AsNoTracking()
            .ToArray();
    }
    #endregion
}
