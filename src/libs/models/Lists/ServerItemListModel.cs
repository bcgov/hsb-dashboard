using HSB.Entities;
using System.Text.Json.Serialization;

namespace HSB.Models;
public class ServerItemListModel
{
    #region Properties
    public string ServiceNowKey { get; set; } = "";

    public int? TenantId { get; set; }
    public TenantListModel? Tenant { get; set; }
    public int OrganizationId { get; set; }
    public OrganizationListModel? Organization { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public OperatingSystemItemListModel? OperatingSystem { get; set; }

    #region ServiceNow Properties
    public string ClassName { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Subcategory { get; set; } = "";
    public string DnsDomain { get; set; } = "";
    public string Platform { get; set; } = "";

    [JsonPropertyName("ipAddress")]
    public string IPAddress { get; set; } = "";

    [JsonPropertyName("fqdn")]
    public string FQDN { get; set; } = "";
    public float? DiskSpace { get; set; }
    #endregion

    #region ServiceNow File System Item Summary Properties
    public float? Capacity { get; set; }
    public float? AvailableSpace { get; set; }
    #endregion
    #endregion

    #region Constructors
    public ServerItemListModel() { }

    public ServerItemListModel(ServerItem entity)
    {
        this.ServiceNowKey = entity.ServiceNowKey;

        this.TenantId = entity.TenantId;
        this.OrganizationId = entity.OrganizationId;
        this.OperatingSystemItemId = entity.OperatingSystemItemId;

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

    public ServerItemListModel(
        int? tenantId,
        int organizationId,
        int? operatingSystemItemId,
        ServiceNow.ResultModel<ServiceNow.BaseItemModel> serverModel,
        ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> ciModel)
    {
        if (serverModel.Data == null) throw new InvalidOperationException("Server data cannot be null");
        if (ciModel.Data == null) throw new InvalidOperationException("Configuration item data cannot be null");

        this.ServiceNowKey = serverModel.Data.Id;

        this.TenantId = tenantId;
        this.OrganizationId = organizationId;
        this.OperatingSystemItemId = operatingSystemItemId;

        this.ClassName = serverModel.Data.ClassName ?? "";
        this.Name = serverModel.Data.Name ?? "";
        this.Category = serverModel.Data.Category ?? "";
        this.Subcategory = serverModel.Data.Subcategory ?? "";
        this.DnsDomain = serverModel.Data.DnsDomain ?? "";
        this.Platform = serverModel.Data.Platform ?? "";
        this.IPAddress = serverModel.Data.IPAddress ?? "";
        this.FQDN = serverModel.Data.FQDN ?? "";
        this.DiskSpace = !String.IsNullOrWhiteSpace(serverModel.Data.DiskSpace) ? float.Parse(serverModel.Data.DiskSpace) : null;
    }
    #endregion
}
