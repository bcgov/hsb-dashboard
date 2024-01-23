namespace HSB.DataService;

/// <summary>
/// ServiceOptions class, provides configuration settings for the service.
/// </summary>
public class ServiceOptions
{
    #region Properties
    /// <summary>
    /// get/set - URL to the HSB API.
    /// </summary>
    public string ApiUrl { get; set; } = "";

    /// <summary>
    /// get/set -
    /// </summary>
    public HsbApiEndpointOptions Endpoints { get; set; } = new HsbApiEndpointOptions();

    /// <summary>
    /// get/set - An array of data sync configuration items to run in this service.
    /// </summary>
    public Models.DataSyncModel[] DataSync { get; set; } = Array.Empty<Models.DataSyncModel>();

    /// <summary>
    /// get/set - An array of table names that represent mapped volumes and drives.
    /// </summary>
    public string[] VolumeTableNames { get; set; } = Array.Empty<string>();

    /// <summary>
    /// get/set - An array of table names that represent servers/devices/computers.
    /// </summary>
    public string[] ServerTableNames { get; set; } = Array.Empty<string>();
    #endregion
}
