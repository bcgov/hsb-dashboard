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
    }
    #endregion
}
