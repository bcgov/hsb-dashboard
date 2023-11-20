using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;
public class ServerItem : Auditable
{
    #region Properties
    public int Id { get; set; }
    public int ConfigurationItemId { get; set; }
    public ConfigurationItem? ConfigurationItem { get; set; }
    public int OperatingSystemItemId { get; set; }
    public OperatingSystemItem? OperatingSystemItem { get; set; }
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string SubCategory { get; set; } = "";
    public string DiskSpace { get; set; } = "";
    public string DnsDomain { get; set; } = "";
    public string SysClassName { get; set; } = "";
    public string Platform { get; set; } = "";
    public string IPAddress { get; set; } = "";

    #endregion
    #endregion

    #region Constructors
    protected ServerItem() { }

    public ServerItem(ConfigurationItem configurationItem, OperatingSystemItem operatingSystemItem, JsonDocument serviceNowJson)
        : this(configurationItem?.Id ?? 0, operatingSystemItem?.Id ?? 0, serviceNowJson)
    {
        this.ConfigurationItem = configurationItem ?? throw new ArgumentNullException(nameof(configurationItem));
        this.OperatingSystemItem = operatingSystemItem ?? throw new ArgumentNullException(nameof(operatingSystemItem));
    }

    public ServerItem(int configurationItemId, int operatingSystemItemId, JsonDocument serviceNowJson)
    {
        this.ConfigurationItemId = configurationItemId;
        this.OperatingSystemItemId = operatingSystemItemId;
        this.RawData = serviceNowJson;

        var values = JsonSerializer.Deserialize<Dictionary<string, object>>(serviceNowJson);
        InitServiceNowProperties(values);
    }

    public ServerItem(ConfigurationItem configurationItem, OperatingSystemItem operatingSystemItem, string serviceNowJson)
        : this(configurationItem?.Id ?? 0, operatingSystemItem?.Id ?? 0, serviceNowJson)
    {
        this.ConfigurationItem = configurationItem ?? throw new ArgumentNullException(nameof(configurationItem));
        this.OperatingSystemItem = operatingSystemItem ?? throw new ArgumentNullException(nameof(operatingSystemItem));
    }

    public ServerItem(int configurationItemId, int operatingSystemItemId, string serviceNowJson)
    {
        this.ConfigurationItemId = configurationItemId;
        this.OperatingSystemItemId = operatingSystemItemId;
        this.RawData = JsonDocument.Parse(serviceNowJson);

        var values = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonDocument.Parse(serviceNowJson));
        InitServiceNowProperties(values);
    }
    #endregion

    #region Methods
    private void InitServiceNowProperties(Dictionary<string, object>? values)
    {
        this.ServiceNowKey = values?.GetDictionaryJsonValue<string>("sys_id") ?? "";
        this.Name = values?.GetDictionaryJsonValue<string>("name") ?? "";
        this.Category = values?.GetDictionaryJsonValue<string>("category") ?? "";
        this.SubCategory = values?.GetDictionaryJsonValue<string>("subcategory") ?? "";
        this.DiskSpace = values?.GetDictionaryJsonValue<string>("disk_space") ?? "";
        this.DnsDomain = values?.GetDictionaryJsonValue<string>("dns_domain") ?? "";
        this.SysClassName = values?.GetDictionaryJsonValue<string>("sys_class_name") ?? "";
        this.Platform = values?.GetDictionaryJsonValue<string>("u_platform") ?? "";
        this.IPAddress = values?.GetDictionaryJsonValue<string>("ip_address") ?? "";
    }
    #endregion
}
