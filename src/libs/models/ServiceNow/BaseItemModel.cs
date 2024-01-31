using System.Text.Json.Serialization;

namespace HSB.Models.ServiceNow;

public class BaseItemModel
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
    [JsonPropertyName("sys_class_name")]
    public string? ClassName { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("install_status")]
    public string? InstallStatus { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("host_name")]
    public string? HostName { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_client_ci_name")]
    public string? ClientName { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("fqdn")]
    public string? FQDN { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_primary_node_name")]
    public string? PrimaryNodeName { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_platform")]
    public string? Platform { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_sla_tier_numbers")]
    public string? SlaTierNumbers { get; set; }

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
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("subcategory")]
    public string? Subcategory { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("used_for")]
    public string? UsedFor { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("ram")]
    public string? Ram { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("cpu_name")]
    public string? CpuName { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("cpu_type")]
    public string? CpuType { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("cpu_speed")]
    public string? CpuSpeed { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("cpu_count")]
    public string? CpuCount { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("disk_space")]
    public string? DiskSpace { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("sys_domain_path")]
    public string? DomainPath { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("dns_domain")]
    public string? DnsDomain { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_pcm_group")]
    public string? PcmGroup { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("short_description")]
    public string? ShortDescription { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("os_domain")]
    public string? OsDomain { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("os_version")]
    public string? OsVersion { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("serial_number")]
    public string? SerialNumber { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("comments")]
    public string? Comments { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("ip_address")]
    public string? IPAddress { get; set; }
    #endregion
}
