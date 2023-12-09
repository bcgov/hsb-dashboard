using System.Text.Json.Serialization;

namespace HSB.Models.ServiceNow;

public class ClientOrganizationModel
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
    [JsonPropertyName("sys_updated_on")]
    public string? UpdatedOn { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("sys_created_on")]
    public string? CreatedOn { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_active")]
    public string? Active { get; set; }

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("u_org_code")]
    public string? OrganizationCode { get; set; }
    #endregion
}
