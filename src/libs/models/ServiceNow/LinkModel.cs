using System.Text.Json.Serialization;

namespace HSB.Models.ServiceNow;

public class LinkModel
{
    #region Properties
    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("link")]
    public string Link { get; set; } = "";

    /// <summary>
    /// get/set -
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; set; } = "";
    #endregion
}
