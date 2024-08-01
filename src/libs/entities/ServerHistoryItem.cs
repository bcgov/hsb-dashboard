using System.Text.Json;

namespace HSB.Entities;

public class ServerHistoryItem : ServerHistoryItemSmall
{
    #region Properties
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");
    public JsonDocument RawDataCI { get; set; } = JsonDocument.Parse("{}");
    #endregion

    #region Constructors
    protected ServerHistoryItem() { }

    public ServerHistoryItem(Tenant? tenant, Organization organization, OperatingSystemItem? operatingSystemItem, JsonDocument serverData, JsonDocument serverItemData)
        : this(tenant?.Id ?? 0, organization.Id, operatingSystemItem?.Id, serverData, serverItemData)
    {
    }

    public ServerHistoryItem(int? tenantId, int organizationId, int? operatingSystemItemId, JsonDocument serverData, JsonDocument serverItemData)
        : base(tenantId, organizationId, operatingSystemItemId, serverData, serverItemData)
    {
    }

    public ServerHistoryItem(ServerItem entity)
        : base(entity)
    {
    }
    #endregion
}
