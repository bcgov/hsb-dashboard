using System.Text.Json.Serialization;

namespace HSB.Models.ServiceNow;

public class OperatingSystemModel
{
    #region Properties
    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("sys_id")]
    public string Id { get; set; } = "";

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_name")]
    public string? Name { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_end_of_life")]
    public string? EndOfLife { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("sys_created_on")]
    public string? CreatedOn { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("sys_updated_on")]
    public string? UpdatedOn { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_class")]
    public string? Class { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_active")]
    public string? Active { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_discovered_os_sn")]
    public string? DiscoveredOsSn { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_discovered_os_version")]
    public string? DiscoveredOsVersion { get; set; }
    #endregion
}
