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

    /// <summary>
    /// get/set - An array of tenant names to exclude from importing from Service Now.
    /// </summary>
    public string[] ExcludeTenants { get; set; } = Array.Empty<string>();

    /// <summary>
    /// get/set - An array of organization names to exclude from importing from Service Now.
    /// </summary>
    public string[] ExcludeOrganizations { get; set; } = Array.Empty<string>();

    /// <summary>
    /// get/set - An array of operating system item names to exclude from importing from Service Now.
    /// </summary>
    public string[] ExcludeOperatingSystemItems { get; set; } = Array.Empty<string>();

    /// <summary>
    /// get/set - An array of file system item names to exclude from importing from Service Now.
    /// </summary>
    public string[] ExcludeFileSystemItems { get; set; } = Array.Empty<string>();

    /// <summary>
    /// get/set - Number of hours before an update will be allowed to be applied.  This is used to reduce the number or changes that can occur when running the process multiple times.
    /// </summary>
    public int AllowUpdateAfterXHours { get; set; } = 12;

    /// <summary>
    /// get/set - An array of actions to perform.  Leave empty to perform all actions. [sync, clean-servers, clean-organizations]
    /// </summary>
    public string[] Actions { get; set; } = Array.Empty<string>();

    /// <summary>
    /// get/set - Whether to send an email when the service completes successfully.
    /// </summary>
    public bool SendSuccessEmail { get; set; }

    /// <summary>
    /// get/set - Whether to send an email when the service fails to complete.
    /// </summary>
    public bool SendFailureEmail { get; set; } = true;

    /// <summary>
    /// get/set - Number of sequential failures that are allowed to occur before service stops (default = 3).
    /// </summary>
    public int RetryLimit { get; set; } = 3;

    /// <summary>
    /// get/set - Number of milliseconds to wait before proceeding after a failure (default = 10,000).
    /// </summary>
    public int DelayAfterFailureMS { get; set; } = 10000;
    #endregion
}
