using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;
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
    public IEnumerable<FileSystemItem> FindForUser<T>(
        long userId,
        System.Linq.Expressions.Expression<Func<FileSystemItem, bool>> predicate,
        System.Linq.Expressions.Expression<Func<FileSystemItem, T>>? sort = null,
        int? take = null,
        int? skip = null)
    {
        var query = (from fsi in this.Context.FileSystemItems
                     join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
                     join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select fsi)
            .Where(predicate);

        if (sort != null)
            query = query.OrderBy(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .AsNoTracking()
            .ToArray();
    }

    public IEnumerable<FileSystemItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<FileSystemItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null)
    {
        var query = (from fsi in this.Context.FileSystemItems
                     join si in this.Context.ServerItems on fsi.ServerItemServiceNowKey equals si.ServiceNowKey
                     join tenant in this.Context.Tenants on si.TenantId equals tenant.Id
                     join usert in this.Context.UserTenants on tenant.Id equals usert.TenantId
                     where usert.UserId == userId
                     select fsi)
            .Where(predicate);

        if (sort?.Any() == true)
            query = query.OrderByProperty(sort);
        if (take.HasValue)
            query = query.Take(take.Value);
        if (skip.HasValue)
            query = query.Skip(skip.Value);

        return query
            .AsNoTracking()
            .ToArray();
    }
    #endregion
}
