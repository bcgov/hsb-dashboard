using HSB.Entities;

namespace HSB.DAL.Services;

public interface IServerItemService : IBaseService<ServerItem>
{
    IEnumerable<ServerItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<ServerItem, bool>> predicate,
        System.Linq.Expressions.Expression<Func<ServerItem, ServerItem>>? sort = null,
        int? take = null,
        int? skip = null);

    public IEnumerable<ServerItem> FindForUser(
        long userId,
        System.Linq.Expressions.Expression<Func<ServerItem, bool>> predicate,
        string[] sort,
        int? take = null,
        int? skip = null);
}
