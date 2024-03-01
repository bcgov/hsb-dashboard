using HSB.Entities;
using HSB.Models;

namespace HSB.DAL.Services;

public interface IOperatingSystemItemService : IBaseService<OperatingSystemItem>
{
    IEnumerable<OperatingSystemItem> Find(
        Models.Filters.OperatingSystemItemFilter filter);

    IEnumerable<OperatingSystemItem> FindForUser(
        long userId,
        Models.Filters.OperatingSystemItemFilter filter);

    IEnumerable<OperatingSystemItemListModel> FindList(Models.Filters.OperatingSystemItemFilter filter);

    IEnumerable<OperatingSystemItemListModel> FindListForUser(long userId, Models.Filters.OperatingSystemItemFilter filter);
}
