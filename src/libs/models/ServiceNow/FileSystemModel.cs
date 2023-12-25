using System.Text.Json.Serialization;

namespace HSB.Models.ServiceNow;

public class FileSystemModel : BaseItemModel
{
    #region Properties

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("volume_id")]
    public string? VolumeId { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("storage_type")]
    public string? StorageType { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("mount_point")]
    public string? MountPoint { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("size")]
    public string? Size { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("size_bytes")]
    public string? SizeBytes { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("capacity")]
    public string? Capacity { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("used_size_bytes")]
    public string? UsedSizeBytes { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("free_space")]
    public string? FreeSpace { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("free_space_bytes")]
    public string? FreeSpaceBytes { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("available_space")]
    public string? AvailableSpace { get; set; }
    #endregion
}
