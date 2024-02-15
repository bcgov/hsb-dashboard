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
        return (from fsi in this.Context.FileSystemItems
                join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
                join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
                join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                where usert.UserId == userId
                   && fsi.ServiceNowKey == key
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
        // Sum up all file system items for the server and update the server.
        var server = this.Context.ServerItems.FirstOrDefault(si => si.ServiceNowKey == entity.ServerItemServiceNowKey);
        if (server != null)
        {
            // TODO: File system items need to be removed otherwise this formula will be invalid over time.
            // Grab all file system items for this server so that space can be calculated.
            // The downside to this implementation is that the calculation will include the prior synced data until all file system items have been synced up.
            var volumes = this.Context.FileSystemItems.AsNoTracking().Where(fsi => fsi.ServerItemServiceNowKey == entity.ServerItemServiceNowKey && fsi.ServiceNowKey != entity.ServiceNowKey).ToArray();
            server.Capacity = volumes.Sum(v => v.SizeBytes) + entity.SizeBytes;
            server.AvailableSpace = volumes.Sum(v => v.FreeSpaceBytes) + entity.FreeSpaceBytes;
            this.Context.Entry(server).State = EntityState.Modified;

            // Update current historical record too.
            // TODO: File system items need to be removed otherwise this formula will be invalid over time.
            var history = this.Context.ServerHistoryItems.FirstOrDefault(shi => shi.ServiceNowKey == server.ServiceNowKey && shi.HistoryKey == server.HistoryKey);
            if (history != null)
            {
                history.Capacity = server.Capacity;
                history.AvailableSpace = server.AvailableSpace;
                this.Context.Entry(history).State = EntityState.Modified;
            }
        }
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
        // Sum up all file system items for the server and update the server.
        var server = this.Context.ServerItems.FirstOrDefault(si => si.ServiceNowKey == entity.ServerItemServiceNowKey);
        if (server != null)
        {
            // TODO: File system items need to be removed otherwise this formula will be invalid over time.
            // Grab all file system items for this server so that space can be calculated.
            // The downside to this implementation is that the calculation will include the prior synced data until all file system items have been synced up.
            var volumes = this.Context.FileSystemItems.AsNoTracking().Where(fsi => fsi.ServerItemServiceNowKey == entity.ServerItemServiceNowKey && fsi.ServiceNowKey != entity.ServiceNowKey).ToArray();
            server.Capacity = volumes.Sum(v => v.SizeBytes) + entity.SizeBytes;
            server.AvailableSpace = volumes.Sum(v => v.FreeSpaceBytes) + entity.FreeSpaceBytes;
            this.Context.Entry(server).State = EntityState.Modified;

            // Update current historical record too.
            // TODO: File system items need to be removed otherwise this formula will be invalid over time.
            var history = this.Context.ServerHistoryItems.FirstOrDefault(shi => shi.ServiceNowKey == server.ServiceNowKey && shi.HistoryKey == server.HistoryKey);
            if (history != null)
            {
                history.Capacity = server.Capacity;
                history.AvailableSpace = server.AvailableSpace;
                this.Context.Entry(history).State = EntityState.Modified;
            }
        }

        // Move original item to history if created more than 12 hours ago.
        var original = this.Context.FileSystemItems.AsNoTracking().FirstOrDefault(fsi => fsi.ServiceNowKey == entity.ServiceNowKey);
        if (original != null && original.CreatedOn.AddHours(12).ToUniversalTime() <= DateTimeOffset.UtcNow)
            this.Context.FileSystemHistoryItems.Add(new FileSystemHistoryItem(original));

        return base.Update(entity);
    }
    #endregion
}
