using HSB.Entities;

namespace HSB.DAL.Services;

public interface IOperatingSystemItemService : IBaseService<OperatingSystemItem>
{
    IEnumerable<OperatingSystemItem> FindForUser<T>(
        long userId,
        System.Linq.Expressions.Expression<Func<OperatingSystemItem, bool>> predicate,
        System.Linq.Expressions.Expression<Func<OperatingSystemItem, T>>? sort = null,
        int? take = null,
        int? skip = null);

    public IEnumerable<OperatingSystemItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<OperatingSystemItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null);
}
