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
                     join org in this.Context.Organizations on si.OrganizationId equals org.Id
                     join tOrg in this.Context.TenantOrganizations on org.Id equals tOrg.OrganizationId
                     join tenant in this.Context.Tenants on tOrg.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
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
            .ToArray();
    }

    public IEnumerable<FileSystemItem> FindForUser(
        long userId,
        FileSystemItemFilter filter)
    {
        var query = (from fsi in this.Context.FileSystemItems
                     join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
                     join org in this.Context.Organizations on si.OrganizationId equals org.Id
                     join tOrg in this.Context.TenantOrganizations on org.Id equals tOrg.OrganizationId
                     join tenant in this.Context.Tenants on tOrg.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
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
            .ToArray();
    }

    public FileSystemItem? FindForId(string key, long userId)
    {
        return (from fsi in this.Context.FileSystemItems
                join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
                join org in this.Context.Organizations on si.OrganizationId equals org.Id
                join tOrg in this.Context.TenantOrganizations on org.Id equals tOrg.OrganizationId
                join tenant in this.Context.Tenants on tOrg.TenantId equals tenant.Id
                join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                where usert.UserId == userId
                   && fsi.ServiceNowKey == key
                select fsi).FirstOrDefault();
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
            var volumes = this.Context.FileSystemItems.AsNoTracking().Where(fsi => fsi.ServerItemServiceNowKey == entity.ServerItemServiceNowKey).ToArray();
            server.Capacity = volumes.Sum(v => v.Capacity);
            server.AvailableSpace = volumes.Sum(v => v.AvailableSpace);
            this.Context.Entry(server).State = EntityState.Modified;

            // Update current historical record too.
            var history = this.Context.ServerHistoryItems.FirstOrDefault(shi => shi.ServiceNowKey == server.ServiceNowKey && shi.HistoryKey == server.HistoryKey);
            if (history != null)
            {
                history.Capacity = volumes.Sum(v => v.Capacity);
                history.AvailableSpace = volumes.Sum(v => v.AvailableSpace);
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
            var volumes = this.Context.FileSystemItems.AsNoTracking().Where(fsi => fsi.ServerItemServiceNowKey == entity.ServerItemServiceNowKey).ToArray();
            server.Capacity = volumes.Sum(v => v.Capacity);
            server.AvailableSpace = volumes.Sum(v => v.AvailableSpace);
            this.Context.Entry(server).State = EntityState.Modified;

            // Update current historical record too.
            var history = this.Context.ServerHistoryItems.FirstOrDefault(shi => shi.ServiceNowKey == server.ServiceNowKey && shi.HistoryKey == server.HistoryKey);
            if (history != null)
            {
                history.Capacity = volumes.Sum(v => v.Capacity);
                history.AvailableSpace = volumes.Sum(v => v.AvailableSpace);
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
