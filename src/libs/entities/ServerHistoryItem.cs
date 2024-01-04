using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;

public class ServerHistoryItem : Auditable
{
    #region Properties
    public long Id { get; set; }
    public int? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public int OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public OperatingSystemItem? OperatingSystemItem { get; set; }

    public string ServiceNowKey { get; set; } = "";
    public ServerItem? ServerItem { get; set; }

    /// <summary>
    /// get/set - This key provides a way to map to the matching record in the history table.
    /// This is required to update the calculated values.
    /// </summary>
    public Guid? HistoryKey { get; set; }

    #region ServiceNow Properties
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");
    public JsonDocument RawDataCI { get; set; } = JsonDocument.Parse("{}");
    public string ClassName { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Subcategory { get; set; } = "";
    public string DnsDomain { get; set; } = "";
    public string Platform { get; set; } = "";
    public string IPAddress { get; set; } = "";
    public string FQDN { get; set; } = "";
    public float? DiskSpace { get; set; }
    #endregion

    #region ServiceNow File System Item Summary Properties
    public float? Capacity { get; set; }
    public float? AvailableSpace { get; set; }
    #endregion
    #endregion

    #region Constructors
    protected ServerHistoryItem() { }

    public ServerHistoryItem(Tenant? tenant, Organization organization, OperatingSystemItem? operatingSystemItem, JsonDocument serverData, JsonDocument configurationData)
        : this(tenant?.Id ?? 0, organization.Id, operatingSystemItem?.Id, serverData, configurationData)
    {
        this.Tenant = tenant;
        this.Tenant = tenant;
        this.OperatingSystemItem = operatingSystemItem;
    }

    public ServerHistoryItem(int? tenantId, int organizationId, int? operatingSystemItemId, JsonDocument serverData, JsonDocument configurationData)
    {
        this.TenantId = tenantId;
        this.OrganizationId = organizationId;
        this.OperatingSystemItemId = operatingSystemItemId;

        this.RawData = serverData;
        this.RawDataCI = configurationData;

        this.ServiceNowKey = serverData.GetElementValue<string>(".sys_id") ?? "";
        this.ClassName = serverData.GetElementValue<string>(".sys_class_name") ?? "";
        this.Name = serverData.GetElementValue<string>(".name") ?? "";
        this.Category = serverData.GetElementValue<string>(".category") ?? "";
        this.Subcategory = serverData.GetElementValue<string>(".subcategory") ?? "";
        this.DnsDomain = serverData.GetElementValue<string>(".dns_domain") ?? "";
        this.Platform = serverData.GetElementValue<string>(".u_platform") ?? "";
        this.IPAddress = serverData.GetElementValue<string>(".ip_address") ?? "";
        this.FQDN = serverData.GetElementValue<string>(".fqdn") ?? "";
        this.DiskSpace = serverData.GetElementValue<float?>(".disk_space");
    }

    public ServerHistoryItem(ServerItem entity)
    {
        this.ServiceNowKey = entity.ServiceNowKey;
        this.HistoryKey = entity.HistoryKey;

        this.TenantId = entity.TenantId;
        this.OrganizationId = entity.OrganizationId;
        this.OperatingSystemItemId = entity.OperatingSystemItemId;

        this.RawData = entity.RawData;
        this.RawDataCI = entity.RawDataCI;

        this.ClassName = entity.ClassName;
        this.Name = entity.Name;
        this.Category = entity.Category;
        this.Subcategory = entity.Subcategory;
        this.DnsDomain = entity.DnsDomain;
        this.Platform = entity.Platform;
        this.IPAddress = entity.IPAddress;
        this.FQDN = entity.FQDN;
        this.DiskSpace = entity.DiskSpace;

        this.Capacity = entity.Capacity;
        this.AvailableSpace = entity.AvailableSpace;
    }
    #endregion
}
