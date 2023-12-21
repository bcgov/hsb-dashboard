using HSB.Entities;

namespace HSB.DAL.Services;

public interface IConfigurationItemService : IBaseService<ConfigurationItem>
{
    IEnumerable<ConfigurationItem> FindForUser<T>(
        long userId,
        System.Linq.Expressions.Expression<Func<ConfigurationItem, bool>> predicate,
        System.Linq.Expressions.Expression<Func<ConfigurationItem, T>>? sort = null,
        int? take = null,
        int? skip = null);

    public IEnumerable<ConfigurationItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<ConfigurationItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null);
}
