namespace HSB.DataService;

/// <summary>
/// ServiceNowTableNameOptions class, provides configuration settings for the service.
/// </summary>
public class ServiceNowTableNameOptions
{
    #region Properties
    /// <summary>
    /// get/set -
    /// </summary>
    public string ConfigurationItem { get; set; } = "cmdb_ci";

    /// <summary>
    /// get/set -
    /// </summary>
    public string Server { get; set; } = "cmdb_ci_server";

    /// <summary>
    /// get/set -
    /// </summary>
    public string FileSystem { get; set; } = "cmdb_ci_file_system";

    /// <summary>
    /// get/set -
    /// </summary>
    public string Tenant { get; set; } = "u_tenant";

    /// <summary>
    /// get/set -
    /// </summary>
    public string ClientOrganization { get; set; } = "u_as_client_organization";

    /// <summary>
    /// get/set -
    /// </summary>
    public string OperatingSystem { get; set; } = "u_operating_system";
    #endregion
}
