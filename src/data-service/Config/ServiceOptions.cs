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
    #endregion
}
