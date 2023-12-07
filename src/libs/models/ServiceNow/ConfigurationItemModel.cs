using System.Text.Json.Serialization;

namespace HSB.Models.ServiceNow;

public class ConfigurationItemModel
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
    [JsonPropertyName("u_tenant_last_changed")]
    public string? TenantLastChanged { get; set; }

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
    [JsonPropertyName("short_description")]
    public string? ShortDescription { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_platform")]
    public string? Platform { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("serial_number")]
    public string? SerialNumber { get; set; }

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
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("category")]
    public string? Category { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("subcategory")]
    public string? SubCategory { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("ip_address")]
    public string? IPAddress { get; set; }
    #endregion
}
