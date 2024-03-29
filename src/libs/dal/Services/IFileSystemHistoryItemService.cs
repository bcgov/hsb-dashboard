using HSB.Entities;
using HSB.Models.Filters;

namespace HSB.DAL.Services;

public interface IFileSystemHistoryItemService : IBaseService<FileSystemHistoryItem>
{
    IEnumerable<FileSystemHistoryItem> Find(FileSystemHistoryItemFilter filter);

    IEnumerable<FileSystemHistoryItem> FindForUser(int userId, FileSystemHistoryItemFilter filter);

    IEnumerable<FileSystemHistoryItem> FindHistoryByMonth(DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serverServiceKeyNow);

    IEnumerable<FileSystemHistoryItem> FindHistoryByMonthForUser(int userId, DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serverServiceKeyNow);
}
