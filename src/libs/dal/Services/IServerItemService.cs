using HSB.Entities;
using HSB.Models;
using HSB.Models.Filters;

namespace HSB.DAL.Services;

public interface IServerItemService : IBaseService<ServerItem>
{
    IEnumerable<ServerItem> Find(ServerItemFilter filter);

    IEnumerable<ServerItem> FindForUser(long userId, ServerItemFilter filter);

    ServerItem? FindForId(string key, bool includeFileSystemItems = false);

    ServerItem? FindForId(string key, long userId, bool includeFileSystemItems = false);

    IEnumerable<ServerItemSmallModel> FindSimple(ServerItemFilter filter);

    IEnumerable<ServerItemSmallModel> FindSimpleForUser(long userId, ServerItemFilter filter);
}
