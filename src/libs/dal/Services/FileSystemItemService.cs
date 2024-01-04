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
                     join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
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

    public IEnumerable<FileSystemItem> FindForUser(
        long userId,
        FileSystemItemFilter filter)
    {
        var query = (from fsi in this.Context.FileSystemItems
                     join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
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

    public FileSystemItem? FindForId(string key, long userId)
    {
        return (from fsi in this.Context.FileSystemItems
                join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
                join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
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
        }
        return base.Add(entity);
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
        }
        return base.Update(entity);
    }
    #endregion
}
