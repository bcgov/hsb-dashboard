using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;
public class FileSystemHistoryItem : Auditable
{
    #region Properties
    /// <summary>
    /// get/set - Primary key (identity).
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// get/set - Foreign key to the file system this is a history of.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";

    /// <summary>
    /// get/set - The file system this is a history of.
    /// </summary>
    public FileSystemItem? FileSystemItem { get; set; }

    #region ServiceNow Properties
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");
    public JsonDocument RawDataCI { get; set; } = JsonDocument.Parse("{}");
    public string Name { get; set; } = "";
    public string Label { get; set; } = "";
    public string Category { get; set; } = "";
    public string Subcategory { get; set; } = "";
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
    protected FileSystemHistoryItem() { }

    public FileSystemHistoryItem(JsonDocument fileSystemItemData, JsonDocument configurationItemData)
    {
        this.RawData = fileSystemItemData;
        this.RawDataCI = configurationItemData;

        this.ServiceNowKey = fileSystemItemData.GetElementValue<string>(".sys_id") ?? "";
        this.ClassName = fileSystemItemData.GetElementValue<string>(".sys_class_name") ?? "";
        this.Name = fileSystemItemData.GetElementValue<string>(".name") ?? "";
        this.Label = fileSystemItemData.GetElementValue<string>(".label") ?? "";
        this.Category = fileSystemItemData.GetElementValue<string>(".category") ?? "";
        this.Subcategory = fileSystemItemData.GetElementValue<string>(".subcategory") ?? "";
        this.StorageType = fileSystemItemData.GetElementValue<string>(".u_platform") ?? "";
        this.MediaType = fileSystemItemData.GetElementValue<string>(".dns_domain") ?? "";
        this.VolumeId = fileSystemItemData.GetElementValue<string>(".volume_id") ?? "";
        this.Capacity = fileSystemItemData.GetElementValue<string>(".capacity") ?? "";
        this.DiskSpace = fileSystemItemData.GetElementValue<string>(".disk_space") ?? "";
        this.Size = fileSystemItemData.GetElementValue<string>(".size") ?? "";
        this.SizeBytes = fileSystemItemData.GetElementValue<string>(".size_bytes") ?? "";
        this.UsedSizeBytes = fileSystemItemData.GetElementValue<string>(".used_size_bytes") ?? "";
        this.AvailableSpace = fileSystemItemData.GetElementValue<string>(".available_space") ?? "";
        this.FreeSpace = fileSystemItemData.GetElementValue<string>(".free_space") ?? "";
        this.FreeSpaceBytes = fileSystemItemData.GetElementValue<string>(".free_space_bytes") ?? "";
    }
    #endregion
}
