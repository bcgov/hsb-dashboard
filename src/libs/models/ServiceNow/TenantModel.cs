using System.Text.Json.Serialization;

namespace HSB.Models.ServiceNow;

public class TenantModel
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
    [JsonPropertyName("sys_name")]
    public string? SysName { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_type")]
    public string? Type { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_client_organizations")]
    public string? ClientOrganizations { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("sys_class_name")]
    public string? ClassName { get; set; }

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
    #endregion
}
