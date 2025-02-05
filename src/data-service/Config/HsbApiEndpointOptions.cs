namespace HSB.DataService;

/// <summary>
/// HsbApiEndpointOptions class, provides configuration settings for the service.
/// </summary>
public class HsbApiEndpointOptions
{
    #region Properties
    /// <summary>
    /// get/set -
    /// </summary>
    public string DataSync { get; set; } = "/v1/services/data-syncs";

    /// <summary>
    /// get/set -
    /// </summary>
    public string ConfigurationItems { get; set; } = "/v1/services/configuration-items";

    /// <summary>
    /// get/set -
    /// </summary>
    public string OperatingSystemItems { get; set; } = "/v1/services/operating-system-items";

    /// <summary>
    /// get/set -
    /// </summary>
    public string Organizations { get; set; } = "/v1/services/organizations";

    /// <summary>
    /// get/set -
    /// </summary>
    public string Tenants { get; set; } = "/v1/services/tenants";

    /// <summary>
    /// get/set -
    /// </summary>
    public string ServerItems { get; set; } = "/v1/services/server-items";

    /// <summary>
    /// get/set -
    /// </summary>
    public string ServerHistoryItems { get; set; } = "/v1/services/server-items/history";

    /// <summary>
    /// get/set -
    /// </summary>
    public string FileSystemItems { get; set; } = "/v1/services/file-system-items";

    /// <summary>
    /// get/set -
    /// </summary>
    public string FileSystemHistoryItems { get; set; } = "/v1/services/file-system-items/history";

    /// <summary>
    /// get/set -
    /// </summary>
    public string RefreshMaterializedViews { get; set; } = "/v1/services/refresh-materialized-views";
    #endregion
}
