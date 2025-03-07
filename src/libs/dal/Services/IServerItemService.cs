using HSB.Entities;
using HSB.Models.Filters;
using HSB.Models.Lists;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HSB.DAL.Services;

public interface IServerItemService : IBaseService<ServerItem>
{
    IEnumerable<ServerItem> Find(ServerItemFilter filter, bool includeRelated = false);

    IEnumerable<ServerItem> FindForUser(long userId, ServerItemFilter filter, bool includeRelated = false);

    ServerItem? FindForId(string key, bool includeFileSystemItems = false);

    ServerItem? FindForId(string key, long userId, bool includeFileSystemItems = false);

    IEnumerable<ServerItemListModel> FindList(ServerItemFilter filter);

    IEnumerable<ServerItemListModel> FindListForUser(long userId, ServerItemFilter filter);

    EntityEntry<ServerItem> Update(ServerItem entity, bool updateTotals);
}
