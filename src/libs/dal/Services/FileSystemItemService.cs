using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using HSB.Models.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    public IEnumerable<FileSystemItem> Find(FileSystemItemFilter filter)
    {
        var query = (from fsi in this.Context.FileSystemItems
                     join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
                     select fsi)
            .Where(fsi => fsi.InstallStatus == 1)
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

    public IEnumerable<FileSystemItem> FindForUser(
        long userId,
        FileSystemItemFilter filter)
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

        var query = (from fsi in this.Context.FileSystemItems
                     join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
                     where userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId)
                     select fsi)
            .Where(fsi => fsi.InstallStatus == 1)
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

    public FileSystemItem? FindForId(string key, long userId)
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

        return (from fsi in this.Context.FileSystemItems
                join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
                where fsi.ServiceNowKey == key
                   && userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId)
                select fsi)
                    .AsSplitQuery()
                    .FirstOrDefault();
    }

    /// <summary>
    /// Add a new file system item record to the database.
    /// Update the owning server with the volume space information.  We do this so that when the app requests servers it does not also need to request all file systems to get storage totals.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public override EntityEntry<FileSystemItem> Add(FileSystemItem entity)
    {
        var result = base.Add(entity);

        // Add item to history.
        this.Context.FileSystemHistoryItems.Add(new FileSystemHistoryItem(entity));

        return result;
    }

    /// <summary>
    /// Update the file system item record in the database.
    /// Update the owning server with the volume space information.  We do this so that when the app requests servers it does not also need to request all file systems to get storage totals.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public override EntityEntry<FileSystemItem> Update(FileSystemItem entity)
    {
        // Move original item to history if created more than 12 hours ago.
        var original = this.Context.FileSystemItems.AsNoTracking().FirstOrDefault(fsi => fsi.ServiceNowKey == entity.ServiceNowKey);
        if (original != null && original.CreatedOn.ToUniversalTime().AddHours(12) <= DateTimeOffset.UtcNow.ToUniversalTime())
            this.Context.FileSystemHistoryItems.Add(new FileSystemHistoryItem(original));

        return base.Update(entity);
    }
    #endregion
}
