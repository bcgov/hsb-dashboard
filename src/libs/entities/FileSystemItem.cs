using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;
public class FileSystemItem : Auditable
{
    #region Properties
    public int Id { get; set; }
    public int ConfigurationItemId { get; set; }
    public ConfigurationItem? ConfigurationItem { get; set; }
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string Name { get; set; } = "";
    public string Label { get; set; } = "";
    public string Category { get; set; } = "";
    public string SubCategory { get; set; } = "";
    public string StorageType { get; set; } = "";
    public string MediaType { get; set; } = "";
    public string VolumeId { get; set; } = "";
    public string SysClassName { get; set; } = "";
    public string Capacity { get; set; } = "";
    public string DiskSpace { get; set; } = "";
    public string Size { get; set; } = "";
    public string SizeBytes { get; set; } = "";
    public string UsedSizeBytes { get; set; } = "";
    public string AvailableSpace { get; set; } = "";
    public string FreeSpace { get; set; } = "";
    public string FreeSpaceBytes { get; set; } = "";
    #endregion
    #endregion

    #region Constructors
    protected FileSystemItem() { }

    public FileSystemItem(ConfigurationItem configurationItem, JsonDocument serviceNowJson)
        : this(configurationItem?.Id ?? 0, serviceNowJson)
    {
        this.ConfigurationItem = configurationItem ?? throw new ArgumentNullException(nameof(configurationItem));
    }

    public FileSystemItem(int configurationItemId, JsonDocument serviceNowJson)
    {
        this.ConfigurationItemId = configurationItemId;
        this.RawData = serviceNowJson;

        var values = JsonSerializer.Deserialize<Dictionary<string, object>>(serviceNowJson);
        InitServiceNowProperties(values);
    }
    #endregion

    #region Methods
    private void InitServiceNowProperties(Dictionary<string, object>? values)
    {
        this.ServiceNowKey = values?.GetDictionaryJsonValue<string>("sys_id") ?? "";
        this.Name = values?.GetDictionaryJsonValue<string>("name") ?? "";
        this.Label = values?.GetDictionaryJsonValue<string>("label") ?? "";
        this.Category = values?.GetDictionaryJsonValue<string>("category") ?? "";
        this.SubCategory = values?.GetDictionaryJsonValue<string>("subcategory") ?? "";
        this.StorageType = values?.GetDictionaryJsonValue<string>("u_platform") ?? "";
        this.MediaType = values?.GetDictionaryJsonValue<string>("dns_domain") ?? "";
        this.SysClassName = values?.GetDictionaryJsonValue<string>("sys_class_name") ?? "";
        this.VolumeId = values?.GetDictionaryJsonValue<string>("volume_id") ?? "";
        this.Capacity = values?.GetDictionaryJsonValue<string>("capacity") ?? "";
        this.DiskSpace = values?.GetDictionaryJsonValue<string>("disk_space") ?? "";
        this.Size = values?.GetDictionaryJsonValue<string>("size") ?? "";
        this.SizeBytes = values?.GetDictionaryJsonValue<string>("size_bytes") ?? "";
        this.UsedSizeBytes = values?.GetDictionaryJsonValue<string>("used_size_bytes") ?? "";
        this.AvailableSpace = values?.GetDictionaryJsonValue<string>("available_space") ?? "";
        this.FreeSpace = values?.GetDictionaryJsonValue<string>("free_space") ?? "";
        this.FreeSpaceBytes = values?.GetDictionaryJsonValue<string>("free_space_bytes") ?? "";
    }
    #endregion
}
