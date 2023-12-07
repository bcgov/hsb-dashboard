using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;

public class ServerItem : Auditable
{
    #region Properties
    public int Id { get; set; }
    public int ConfigurationItemId { get; set; }
    public ConfigurationItem? ConfigurationItem { get; set; }
    public int? OperatingSystemItemId { get; set; }
    public OperatingSystemItem? OperatingSystemItem { get; set; }
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string OperatingSystemKey { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string SubCategory { get; set; } = "";
    public string DiskSpace { get; set; } = "";
    public string DnsDomain { get; set; } = "";
    public string ClassName { get; set; } = "";
    public string Platform { get; set; } = "";
    public string IPAddress { get; set; } = "";
    #endregion
    #endregion

    #region Constructors
    protected ServerItem() { }

    public ServerItem(ConfigurationItem configurationItem, OperatingSystemItem? operatingSystemItem, JsonDocument data)
        : this(configurationItem?.Id ?? 0, operatingSystemItem?.Id, data)
    {
        this.ConfigurationItem = configurationItem ?? throw new ArgumentNullException(nameof(configurationItem));
        this.OperatingSystemItem = operatingSystemItem;
    }

    public ServerItem(int configurationItemId, int? operatingSystemItemId, JsonDocument data)
    {
        this.ConfigurationItemId = configurationItemId;
        this.OperatingSystemItemId = operatingSystemItemId;
        this.RawData = data;

        this.ServiceNowKey = data.GetElementValue<string>(".sys_id") ?? "";
        this.OperatingSystemKey = data.GetElementValue<string>(".u_operating_system.value") ?? "";
        this.Name = data.GetElementValue<string>(".name") ?? "";
        this.Category = data.GetElementValue<string>(".category") ?? "";
        this.SubCategory = data.GetElementValue<string>(".subcategory") ?? "";
        this.DiskSpace = data.GetElementValue<string>(".disk_space") ?? "";
        this.DnsDomain = data.GetElementValue<string>(".dns_domain") ?? "";
        this.ClassName = data.GetElementValue<string>(".sys_class_name") ?? "";
        this.Platform = data.GetElementValue<string>(".u_platform") ?? "";
        this.IPAddress = data.GetElementValue<string>(".ip_address") ?? "";
    }
    #endregion
}
