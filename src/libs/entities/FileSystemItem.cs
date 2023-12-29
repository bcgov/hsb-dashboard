using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;
public class FileSystemItem : Auditable
{
    #region Properties
    /// <summary>
    /// get/set - Primary key for HSB, and foreign key to the ServiceNow API.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";

    /// <summary>
    /// get/set - Foreign key to server that owns this file system item.
    /// </summary>
    public string ServerItemServiceNowKey { get; set; } = "";

    /// <summary>
    /// get/set - The server that owns this file system item.
    /// </summary>
    public ServerItem? ServerItem { get; set; }

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
    public int Capacity { get; set; }
    public float DiskSpace { get; set; }
    public string Size { get; set; } = "";
    public long SizeBytes { get; set; }
    public long? UsedSizeBytes { get; set; }
    public int AvailableSpace { get; set; }
    public string FreeSpace { get; set; } = "";
    public long FreeSpaceBytes { get; set; }
    #endregion

    /// <summary>
    /// get - All file system item history.
    /// </summary>
    public List<FileSystemHistoryItem> History { get; } = new List<FileSystemHistoryItem>();
    #endregion

    #region Constructors
    protected FileSystemItem() { }

    public FileSystemItem(ServerItem serverItem, JsonDocument fileSystemItemData, JsonDocument configurationItemData)
        : this(serverItem.ServiceNowKey, fileSystemItemData, configurationItemData)
    {
        this.ServerItem = serverItem ?? throw new ArgumentNullException(nameof(serverItem));
    }

    public FileSystemItem(string serverItemId, JsonDocument fileSystemItemData, JsonDocument configurationItemData)
    {
        this.ServerItemServiceNowKey = serverItemId;

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
        this.Capacity = fileSystemItemData.GetElementValue<int>(".capacity");
        this.DiskSpace = fileSystemItemData.GetElementValue<float>(".disk_space");
        this.Size = fileSystemItemData.GetElementValue<string>(".size") ?? "";
        this.SizeBytes = fileSystemItemData.GetElementValue<long>(".size_bytes");
        this.UsedSizeBytes = fileSystemItemData.GetElementValue<long>(".used_size_bytes");
        this.AvailableSpace = fileSystemItemData.GetElementValue<int>(".available_space");
        this.FreeSpace = fileSystemItemData.GetElementValue<string>(".free_space") ?? "";
        this.FreeSpaceBytes = fileSystemItemData.GetElementValue<long>(".free_space_bytes");
    }
    #endregion
}
