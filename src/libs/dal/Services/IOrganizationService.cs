using HSB.Entities;
using HSB.Models;

namespace HSB.DAL.Services;

public interface IOrganizationService : IBaseService<Organization>
{
    /// <summary>
    /// Find the entity for the specified `keyValues`.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Organization? FindForIdAsNoTracking(int id);

    IEnumerable<Organization> Find(
        Models.Filters.OrganizationFilter filter);

    IEnumerable<Organization> FindForUser(
        long userId,
        Models.Filters.OrganizationFilter filter);

    IEnumerable<OrganizationListModel> FindList(Models.Filters.OrganizationFilter filter);

    IEnumerable<OrganizationListModel> FindListForUser(long userId, Models.Filters.OrganizationFilter filter);

    Organization? FindForId(int id, bool includeTenants);

    IEnumerable<Organization> Cleanup();
}
