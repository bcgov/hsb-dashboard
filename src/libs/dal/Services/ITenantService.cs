using HSB.Entities;
using HSB.Models;

namespace HSB.DAL.Services;

public interface ITenantService : IBaseService<Tenant>
{
    IEnumerable<Tenant> Find(
        Models.Filters.TenantFilter filter);

    IEnumerable<Tenant> FindForUser(
        long userId,
        Models.Filters.TenantFilter filter);

    IEnumerable<TenantListModel> FindList(Models.Filters.TenantFilter filter);

    IEnumerable<TenantListModel> FindListForUser(long userId, Models.Filters.TenantFilter filter);

    Tenant? FindForId(int id, bool includeOrganizations);
}
