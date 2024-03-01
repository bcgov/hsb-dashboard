using System.Security.Claims;
using HSB.DAL.Extensions;
using HSB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HSB.DAL.Services;

public class OperatingSystemItemService : BaseService<OperatingSystemItem>, IOperatingSystemItemService
{
    #region Constructors
    public OperatingSystemItemService(HSBContext dbContext, ClaimsPrincipal principal, IServiceProvider serviceProvider, ILogger<OperatingSystemItemService> logger)
        : base(dbContext, principal, serviceProvider, logger)
    {
    }
    #endregion

    #region Methods
    public IEnumerable<OperatingSystemItem> Find(
        Models.Filters.OperatingSystemItemFilter filter)
    {
        var query = from tenant in this.Context.OperatingSystemItems
                    select tenant;

        query = query
            .Where(filter.GeneratePredicate());

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

    public IEnumerable<OperatingSystemItem> FindForUser(
        long userId,
        Models.Filters.OperatingSystemItemFilter filter)
    {
        var userOrganizationQuery = from uo in this.Context.UserOrganizations
                                    join o in this.Context.Organizations on uo.OrganizationId equals o.Id
                                    where uo.UserId == userId
                                        && o.IsEnabled
                                    select uo.OrganizationId;
        var userTenantQuery = from ut in this.Context.UserTenants
                              join t in this.Context.Tenants on ut.TenantId equals t.Id
                              where ut.UserId == userId
                                && t.IsEnabled
                              select ut.TenantId;

        var query = from osi in this.Context.OperatingSystemItems
                    join si in this.Context.ServerItems on osi.Id equals si.OperatingSystemItemId
                    where userOrganizationQuery.Contains(si.OrganizationId) || userTenantQuery.Contains(si.TenantId!.Value)
                    select osi;

        query = query
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

    public IEnumerable<Models.OperatingSystemItemListModel> FindList(
        Models.Filters.OperatingSystemItemFilter filter)
    {
        var query = from osi in this.Context.OperatingSystemItems
                    select osi;

        query = query
            .Where(filter.GeneratePredicate());

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
            .Select(t => new Models.OperatingSystemItemListModel(t))
            .ToArray();
    }

    public IEnumerable<Models.OperatingSystemItemListModel> FindListForUser(
        long userId,
        Models.Filters.OperatingSystemItemFilter filter)
    {
        var userOrganizationQuery = from uo in this.Context.UserOrganizations
                                    join o in this.Context.Organizations on uo.OrganizationId equals o.Id
                                    where uo.UserId == userId
                                        && o.IsEnabled
                                    select uo.OrganizationId;
        var userTenantQuery = from ut in this.Context.UserTenants
                              join t in this.Context.Tenants on ut.TenantId equals t.Id
                              where ut.UserId == userId
                                && t.IsEnabled
                              select ut.TenantId;

        var query = from osi in this.Context.OperatingSystemItems
                    join si in this.Context.ServerItems on osi.Id equals si.OperatingSystemItemId
                    where userOrganizationQuery.Contains(si.OrganizationId) || userTenantQuery.Contains(si.TenantId!.Value)
                    select osi;

        query = query
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
            .Select(t => new Models.OperatingSystemItemListModel(t))
            .ToArray();
    }
    #endregion
}
