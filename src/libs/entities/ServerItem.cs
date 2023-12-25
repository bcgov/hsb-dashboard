using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;

public class ServerItem : Auditable
{
    #region Properties
    /// <summary>
    /// get/set - Primary key for HSB, and foreign key to the ServiceNow API.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";

    /// <summary>
    /// get/set - Foreign key to tenant.
    /// </summary>
    public int? TenantId { get; set; }

    /// <summary>
    /// get/set - The tenant that owns this server.
    /// </summary>
    public Tenant? Tenant { get; set; }

    /// <summary>
    /// get/set - Foreign key to the organization
    /// </summary>
    public int OrganizationId { get; set; }

    /// <summary>
    /// get/set - The organization that owns this server.
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// get/set - Foreign key to the operating system.
    /// </summary>
    public int? OperatingSystemItemId { get; set; }

    /// <summary>
    /// get/set - The operating system for this server.
    /// </summary>
    public OperatingSystemItem? OperatingSystemItem { get; set; }

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

    /// <summary>
    /// get - File system items that belong to this server.
    /// </summary>
    public List<FileSystemItem> FileSystemItems { get; } = new List<FileSystemItem>();

    /// <summary>
    /// get - All server item history.
    /// </summary>
    public List<ServerHistoryItem> History { get; } = new List<ServerHistoryItem>();
    #endregion

    #region Constructors
    protected ServerItem() { }

    public ServerItem(Tenant? tenant, Organization organization, OperatingSystemItem? operatingSystemItem, JsonDocument serverData, JsonDocument configurationData)
        : this(tenant?.Id ?? 0, organization.Id, operatingSystemItem?.Id, serverData, configurationData)
    {
        this.Tenant = tenant;
        this.Tenant = tenant;
        this.OperatingSystemItem = operatingSystemItem;
    }

    public ServerItem(int? tenantId, int organizationId, int? operatingSystemItemId, JsonDocument serverData, JsonDocument configurationData)
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
