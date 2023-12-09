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
    public string ClassName { get; set; } = "";
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

    public FileSystemItem(int configurationItemId, JsonDocument data)
    {
        this.ConfigurationItemId = configurationItemId;
        this.RawData = data;

        this.ServiceNowKey = data.GetElementValue<string>(".sys_id") ?? "";
        this.Name = data.GetElementValue<string>(".name") ?? "";
        this.Label = data.GetElementValue<string>(".label") ?? "";
        this.Category = data.GetElementValue<string>(".category") ?? "";
        this.SubCategory = data.GetElementValue<string>(".subcategory") ?? "";
        this.StorageType = data.GetElementValue<string>(".u_platform") ?? "";
        this.MediaType = data.GetElementValue<string>(".dns_domain") ?? "";
        this.ClassName = data.GetElementValue<string>(".sys_class_name") ?? "";
        this.VolumeId = data.GetElementValue<string>(".volume_id") ?? "";
        this.Capacity = data.GetElementValue<string>(".capacity") ?? "";
        this.DiskSpace = data.GetElementValue<string>(".disk_space") ?? "";
        this.Size = data.GetElementValue<string>(".size") ?? "";
        this.SizeBytes = data.GetElementValue<string>(".size_bytes") ?? "";
        this.UsedSizeBytes = data.GetElementValue<string>(".used_size_bytes") ?? "";
        this.AvailableSpace = data.GetElementValue<string>(".available_space") ?? "";
        this.FreeSpace = data.GetElementValue<string>(".free_space") ?? "";
        this.FreeSpaceBytes = data.GetElementValue<string>(".free_space_bytes") ?? "";
    }
    #endregion
}
