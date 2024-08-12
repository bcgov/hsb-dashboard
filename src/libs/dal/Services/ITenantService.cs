using HSB.Entities;
using HSB.Models.Lists;

namespace HSB.DAL.Services;

public interface ITenantService : IBaseService<Tenant>
{
    /// <summary>
    /// Find the entity for the specified `keyValues`.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Tenant? FindForIdAsNoTracking(int id);

    IEnumerable<Tenant> Find(
        Models.Filters.TenantFilter filter);

    IEnumerable<Tenant> FindForUser(
        long userId,
        Models.Filters.TenantFilter filter);

    IEnumerable<TenantListModel> FindList(Models.Filters.TenantFilter filter);

    IEnumerable<TenantListModel> FindListForUser(long userId, Models.Filters.TenantFilter filter);

    Tenant? FindForId(int id, bool includeOrganizations);
}
