using System.Text.Json;
using HSB.Entities;
using System.Text.Json.Serialization;

namespace HSB.Models.Dashboard;

public class ServerItemModel : AuditableModel
{
    #region Properties
    public string ServiceNowKey { get; set; } = "";
    public Guid? HistoryKey { get; set; }

    public int? TenantId { get; set; }
    public TenantModel? Tenant { get; set; }
    public int OrganizationId { get; set; }
    public OrganizationModel? Organization { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public OperatingSystemItemModel? OperatingSystem { get; set; }

    public IEnumerable<FileSystemItemModel> FileSystemItems { get; set; } = Array.Empty<FileSystemItemModel>();

    #region ServiceNow Properties
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");
    public JsonDocument RawDataCI { get; set; } = JsonDocument.Parse("{}");
    public string ClassName { get; set; } = "";
    public string Name { get; set; } = "";
    public int InstallStatus { get; set; }
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
    public ServerItemModel() { }

    public ServerItemModel(ServerItem entity) : base(entity)
    {
        this.ServiceNowKey = entity.ServiceNowKey;
        this.HistoryKey = entity.HistoryKey;

        this.TenantId = entity.TenantId;
        this.OrganizationId = entity.OrganizationId;
        this.OperatingSystemItemId = entity.OperatingSystemItemId;

        this.ClassName = entity.ClassName;
        this.Name = entity.Name;
        this.InstallStatus = entity.InstallStatus;
        this.Category = entity.Category;
        this.Subcategory = entity.Subcategory;
        this.DnsDomain = entity.DnsDomain;
        this.Platform = entity.Platform;
        this.IPAddress = entity.IPAddress;
        this.FQDN = entity.FQDN;
        this.DiskSpace = entity.DiskSpace;

        this.Capacity = entity.Capacity;
        this.AvailableSpace = entity.AvailableSpace;

        this.FileSystemItems = entity.FileSystemItems.Select(fsi => new FileSystemItemModel(fsi)).ToArray();
    }

    public ServerItemModel(
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
        this.InstallStatus = int.Parse(serverModel.Data.InstallStatus ?? "0");
        this.Category = serverModel.Data.Category ?? "";
        this.Subcategory = serverModel.Data.Subcategory ?? "";
        this.DnsDomain = serverModel.Data.DnsDomain ?? "";
        this.Platform = serverModel.Data.Platform ?? "";
        this.IPAddress = serverModel.Data.IPAddress ?? "";
        this.FQDN = serverModel.Data.FQDN ?? "";
        this.DiskSpace = !String.IsNullOrWhiteSpace(serverModel.Data.DiskSpace) ? float.Parse(serverModel.Data.DiskSpace) : null;
    }
    #endregion

    #region Methods
    public ServerItem ToEntity()
    {
        return (ServerItem)this;
    }

    public static explicit operator ServerItem(ServerItemModel model)
    {
        if (model.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new ServerItem(model.TenantId, model.OrganizationId, model.OperatingSystemItemId, model.RawData, model.RawDataCI)
        {
            ServiceNowKey = model.ServiceNowKey,
            HistoryKey = model.HistoryKey,
            ClassName = model.ClassName,
            Name = model.Name,
            InstallStatus = model.InstallStatus,
            Category = model.Category,
            Subcategory = model.Subcategory,
            DnsDomain = model.DnsDomain,
            Platform = model.Platform,
            IPAddress = model.IPAddress,
            FQDN = model.FQDN,
            DiskSpace = model.DiskSpace,
            Capacity = model.Capacity,
            AvailableSpace = model.AvailableSpace,
            CreatedOn = model.CreatedOn,
            CreatedBy = model.CreatedBy,
            UpdatedOn = model.UpdatedOn,
            UpdatedBy = model.UpdatedBy,
            Version = model.Version,
        };
    }
    #endregion
}
