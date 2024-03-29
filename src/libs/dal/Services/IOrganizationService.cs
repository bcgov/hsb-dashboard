using HSB.Entities;
using HSB.Models;

namespace HSB.DAL.Services;

public interface IOrganizationService : IBaseService<Organization>
{
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
