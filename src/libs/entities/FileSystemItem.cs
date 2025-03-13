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
    public int InstallStatus { get; set; }
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

    #region Calculated Properties
    /// <summary>
    /// get - Indicates if this is SAN storage or not. This will sometimes involve some guesswork,
    /// based on whether the ServerItem is virtual or physical, the StorageType, and the VolumeId.
    /// </summary>
    public bool IsSAN {
        get {
            // 1. Determine if the server is a physical or virtual server.
            if (this.ServerItem != null && this.ServerItem.IsVirtual.HasValue) {
                bool isVirtual = this.ServerItem.IsVirtual.Value;

                // 2. If the StorageType is set:
                if (this.StorageType != "") {
                    if (isVirtual) {
                        // a. If virtual, StorageType of logical → SAN, otherwise it’s Non-SAN
                        return this.StorageType == "logical";
                    } else {
                        // b. If physical, StorageType of network → SAN, otherwise it's Non-SAN
                        return this.StorageType == "network";
                    }
                }
            }
            // Otherwise, we will do a best guess using the VolumeId. If the VolumeId containing
            // “C:” (or similar) OR containing “*root*” → Non-SAN, otherwise it’s SAN
            return !(
              this.VolumeId.StartsWith("C", StringComparison.OrdinalIgnoreCase) ||
              this.VolumeId.Contains("root", StringComparison.OrdinalIgnoreCase)
            );
        }
    }
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
        this.InstallStatus = fileSystemItemData.GetElementValue<int>(".install_status");
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
