using HSB.Entities;

namespace HSB.DAL.Services;

public interface ITenantService : IBaseService<Tenant>
{
    IEnumerable<Tenant> FindForUser<T>(
        long userId,
        System.Linq.Expressions.Expression<Func<Tenant, bool>> predicate,
        System.Linq.Expressions.Expression<Func<Tenant, T>>? sort = null,
        int? take = null,
        int? skip = null);

    public IEnumerable<Tenant> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<Tenant, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null);
}
