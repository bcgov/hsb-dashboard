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
    /// Update the owning server with the volume space information.
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
            var volumes = this.Context.FileSystemItems.AsNoTracking().Where(fsi => fsi.ServerItemServiceNowKey == entity.ServerItemServiceNowKey).ToArray();
            server.Capacity = volumes.Sum(v => v.SizeBytes / 1024 / 1024);
            server.AvailableSpace = volumes.Sum(v => v.FreeSpaceBytes / 1024 / 1024);
            this.Context.Entry(server).State = EntityState.Modified;

            // Update current historical record too.
            // TODO: File system items need to be removed otherwise this formula will be invalid over time.
            var history = this.Context.ServerHistoryItems.FirstOrDefault(shi => shi.ServiceNowKey == server.ServiceNowKey && shi.HistoryKey == server.HistoryKey);
            if (history != null)
            {
                history.Capacity = volumes.Sum(v => v.SizeBytes / 1024 / 1024);
                history.AvailableSpace = volumes.Sum(v => v.FreeSpaceBytes / 1024 / 1024);
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
    /// Update the owning server with the volume space information.
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
            var volumes = this.Context.FileSystemItems.AsNoTracking().Where(fsi => fsi.ServerItemServiceNowKey == entity.ServerItemServiceNowKey).ToArray();
            server.Capacity = volumes.Sum(v => v.SizeBytes / 1024 / 1024);
            server.AvailableSpace = volumes.Sum(v => v.FreeSpaceBytes / 1024 / 1024);
            this.Context.Entry(server).State = EntityState.Modified;

            // Update current historical record too.
            // TODO: File system items need to be removed otherwise this formula will be invalid over time.
            var history = this.Context.ServerHistoryItems.FirstOrDefault(shi => shi.ServiceNowKey == server.ServiceNowKey && shi.HistoryKey == server.HistoryKey);
            if (history != null)
            {
                history.Capacity = volumes.Sum(v => v.SizeBytes / 1024 / 1024);
                history.AvailableSpace = volumes.Sum(v => v.FreeSpaceBytes / 1024 / 1024);
                this.Context.Entry(history).State = EntityState.Modified;
            }
        }

        // Add original item to history before making updates.
        var original = this.Context.FileSystemItems.AsNoTracking().FirstOrDefault(fsi => fsi.ServiceNowKey == entity.ServiceNowKey);
        if (original != null)
            this.Context.FileSystemHistoryItems.Add(new FileSystemHistoryItem(original));

        return base.Update(entity);
    }
    #endregion
}
