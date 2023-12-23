using HSB.Entities;

namespace HSB.DAL.Services;

public interface IServerItemService : IBaseService<ServerItem>
{
    IEnumerable<ServerItem> FindForUser<T>(
        bool distinct,
        long userId,
        System.Linq.Expressions.Expression<Func<ServerItem, bool>> predicate,
        System.Linq.Expressions.Expression<Func<ServerItem, T>>? sort = null,
        int? take = null,
        int? skip = null);

    public IEnumerable<ServerItem> FindForUser(
        bool distinct,
        long userId,
        System.Linq.Expressions.Expression<Func<ServerItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null);
}
