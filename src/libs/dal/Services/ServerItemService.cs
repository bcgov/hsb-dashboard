using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using HSB.Models;
using HSB.Models.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class ServerItemService : BaseService<ServerItem>, IServerItemService
{
    #region Constructors
    public ServerItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<ServerItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods

    public IEnumerable<ServerItem> Find(ServerItemFilter filter)
    {
        var query = this.Context.ServerItems
            .Where(filter.GeneratePredicate())
            .Distinct();

        if (filter.Sort?.Any() == true)
            query = query.OrderByProperty(filter.Sort);
        else query = query.OrderBy(si => si.Name);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Quantity.HasValue && filter.Page > 1)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .AsNoTracking()
            .AsSingleQuery()
            .ToArray();
    }

    public IEnumerable<ServerItem> FindForUser(long userId, ServerItemFilter filter)
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

        var query = (from si in this.Context.ServerItems
                     where si.TenantId != null
                        && (userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId))
                     select si)
            .Where(filter.GeneratePredicate())
            .Distinct();

        if (filter.Sort?.Any() == true)
            query = query.OrderByProperty(filter.Sort);
        else query = query.OrderBy(si => si.Name);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Quantity.HasValue && filter.Page > 1)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .AsNoTracking()
            .AsSplitQuery()
            .ToArray();
    }

    public IEnumerable<ServerItemListModel> FindList(ServerItemFilter filter)
    {
        var query = this.Context.ServerItems
            .Where(filter.GeneratePredicate())
            .Distinct();

        if (filter.Sort?.Any() == true)
            query = query.OrderByProperty(filter.Sort);
        else query = query.OrderBy(si => si.Name);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Quantity.HasValue && filter.Page > 1)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .AsNoTracking()
            .AsSplitQuery()
            .Select(si => new ServerItemListModel(si))
            .ToArray();
    }

    public IEnumerable<ServerItemListModel> FindListForUser(long userId, ServerItemFilter filter)
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

        var query = (from si in this.Context.ServerItems
                     where si.TenantId != null
                        && (userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId))
                     select si)
            .Where(filter.GeneratePredicate())
            .Distinct();

        if (filter.Sort?.Any() == true)
            query = query.OrderByProperty(filter.Sort);
        else query = query.OrderBy(si => si.Name);
        if (filter.Quantity.HasValue)
            query = query.Take(filter.Quantity.Value);
        if (filter.Page.HasValue && filter.Quantity.HasValue && filter.Page > 1)
            query = query.Skip(filter.Page.Value * filter.Quantity.Value);

        return query
            .AsNoTracking()
            .AsSplitQuery()
            .Select(si => new ServerItemListModel(si))
            .ToArray();
    }

    public ServerItem? FindForId(string key, bool includeFileSystemItems = false)
    {
        var query = from si in this.Context.ServerItems
                    where si.ServiceNowKey == key
                    select si;

        if (includeFileSystemItems)
            query = query.Include(m => m.FileSystemItems);

        return query
            .AsSingleQuery()
            .FirstOrDefault();
    }

    public ServerItem? FindForId(string key, long userId, bool includeFileSystemItems = false)
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

        var query = from si in this.Context.ServerItems
                    where si.ServiceNowKey == key
                        && userTenants.Contains(si.TenantId!.Value) || userOrganizationQuery.Contains(si.OrganizationId)
                    select si;

        if (includeFileSystemItems)
            query = query.Include(m => m.FileSystemItems);

        return query
            .AsSplitQuery()
            .FirstOrDefault();
    }

    public override EntityEntry<ServerItem> Add(ServerItem entity)
    {
        // This key provides a way to link only the current history record.
        var key = Guid.NewGuid();
        entity.HistoryKey = key;

        var result = base.Add(entity);

        // Add item to history.
        // This ensures we have a matching record when querying history.
        this.Context.ServerHistoryItems.Add(new ServerHistoryItem(entity));

        return result;
    }

    public EntityEntry<ServerItem> Update(ServerItem entity, bool updateTotals)
    {
        // If the install status has changed from being installed, also set the same status on all related children.
        if (entity.InstallStatus != 1)
        {
            this.Context.FileSystemHistoryItems.FromSqlRaw("UPDATE public.\"FileSystemHistoryItem\" SET \"InstallStatus\" = {1} WHERE \"ServerItemServiceNowKey\" = '{0}'", entity.ServiceNowKey, entity.InstallStatus);
            this.Context.FileSystemItems.FromSqlRaw("UPDATE public.\"FileSystemItem\" SET \"InstallStatus\" = {1} WHERE \"ServerItemServiceNowKey\" = '{0}'", entity.ServiceNowKey, entity.InstallStatus);
            this.Context.ServerHistoryItems.FromSqlRaw("UPDATE public.\"ServerHistoryItem\" SET \"InstallStatus\" = {1} WHERE \"ServiceNowKey\" = '{0}'", entity.ServiceNowKey, entity.InstallStatus);
            this.Context.CommitTransaction();
        }

        if (updateTotals)
        {
            // TODO: File system items need to be removed otherwise this formula will be invalid over time.
            // Grab all file system items for this server so that space can be calculated.
            // The downside to this implementation is that the calculation will include the prior synced data until all file system items have been synced up.
            var volumes = this.Context.FileSystemItems.AsNoTracking().Where(fsi => fsi.ServerItemServiceNowKey == entity.ServiceNowKey).ToArray();
            entity.Capacity = volumes.Sum(v => v.SizeBytes);
            entity.AvailableSpace = volumes.Sum(v => v.FreeSpaceBytes);
            this.Context.Entry(entity).State = EntityState.Modified;

            // Update current historical record too.
            // TODO: File system items need to be removed otherwise this formula will be invalid over time.
            var history = this.Context.ServerHistoryItems.FirstOrDefault(shi => shi.ServiceNowKey == entity.ServiceNowKey && shi.HistoryKey == entity.HistoryKey);
            if (history != null)
            {
                history.Capacity = entity.Capacity;
                history.AvailableSpace = entity.AvailableSpace;
                this.Context.Entry(history).State = EntityState.Modified;
            }
        }

        // Move original to history if created more than 12 hours ago.
        var original = this.Context.ServerItems.AsNoTracking().FirstOrDefault(si => si.ServiceNowKey == entity.ServiceNowKey);
        if (original != null && original.CreatedOn.ToUniversalTime().AddHours(12) <= DateTimeOffset.UtcNow.ToUniversalTime())
        {
            // This key provides a way to link the current server item record with the one in history.
            var key = Guid.NewGuid();
            entity.HistoryKey = key;
            original.HistoryKey = key;
            this.Context.ServerHistoryItems.Add(new ServerHistoryItem(original));
        }

        return base.Update(entity);
    }

    public override EntityEntry<ServerItem> Update(ServerItem entity)
    {
        return this.Update(entity, false);
    }

    public override EntityEntry<ServerItem> Remove(ServerItem entity)
    {
        this.Context.FileSystemHistoryItems.FromSqlRaw("DELETE FROM public.\"FileSystemHistoryItem\" WHERE \"ServerItemServiceNowKey\" = '{0}'", entity.ServiceNowKey);
        this.Context.FileSystemItems.FromSqlRaw("DELETE FROM public.\"FileSystemItem\" WHERE \"ServerItemServiceNowKey\" = '{0}'", entity.ServiceNowKey);
        this.Context.ServerHistoryItems.FromSqlRaw("DELETE FROM public.\"ServerHistoryItem\" WHERE \"ServiceNowKey\" = '{0}'", entity.ServiceNowKey);
        return base.Remove(entity);
    }
    #endregion
}
