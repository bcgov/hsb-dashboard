using HSB.Entities;
using HSB.Models.Filters;

namespace HSB.DAL.Services;

public interface IServerHistoryItemService : IBaseService<ServerHistoryItem>
{
    IEnumerable<ServerHistoryItem> Find(ServerHistoryItemFilter filter);

    IEnumerable<ServerHistoryItem> FindForUser(int userId, ServerHistoryItemFilter filter);

    IEnumerable<ServerHistoryItem> FindHistoryByMonth(DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serviceKeyNow);

    IEnumerable<ServerHistoryItem> FindHistoryByMonthForUser(int userId, DateTime start, DateTime? end, int? tenantId, int? organizationId, int? operatingSystemId, string? serviceKeyNow);
}
