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
    public string WinServer { get; set; } = "cmdb_ci_win_server";

    /// <summary>
    /// get/set -
    /// </summary>
    public string SolarisServer { get; set; } = "cmdb_ci_solaris_server";

    /// <summary>
    /// get/set -
    /// </summary>
    public string LinuxServer { get; set; } = "cmdb_ci_linux_server";

    /// <summary>
    /// get/set -
    /// </summary>
    public string UnixServer { get; set; } = "cmdb_ci_unix_server";

    /// <summary>
    /// get/set -
    /// </summary>
    public string EsxServer { get; set; } = "cmdb_ci_esx_server";

    /// <summary>
    /// get/set -
    /// </summary>
    public string AixServer { get; set; } = "cmdb_ci_aix_server";

    /// <summary>
    /// get/set -
    /// </summary>
    public string Appliance { get; set; } = "u_cmdb_ci_appliance";

    /// <summary>
    /// get/set -
    /// </summary>
    public string PCHardware { get; set; } = "cmdb_ci_pc_hardware";

    /// <summary>
    /// get/set -
    /// </summary>
    public string FileSystem { get; set; } = "cmdb_ci_file_system";

    /// <summary>
    /// get/set -
    /// </summary>
    public string OpenVMS { get; set; } = "u_openvms";

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
