using HSB.Entities;

namespace HSB.DAL.Services;

public interface IOrganizationService : IBaseService<Organization>
{
    IEnumerable<Organization> FindForUser<T>(
        long userId,
        System.Linq.Expressions.Expression<Func<Organization, bool>> predicate,
        System.Linq.Expressions.Expression<Func<Organization, T>>? sort = null,
        int? take = null,
        int? skip = null);

    public IEnumerable<Organization> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<Organization, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null);
}
