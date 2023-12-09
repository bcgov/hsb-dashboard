namespace HSB.DataService;

/// <summary>
/// ServiceNowOptions class, provides configuration settings for the service.
/// </summary>
public class ServiceNowOptions
{
    #region Properties
    /// <summary>
    /// get/set - Number of items to request for.
    /// </summary>
    public int Limit { get; set; } = 100;

    /// <summary>
    /// get/set - The username.
    /// </summary>
    public string Username { get; set; } = "";

    /// <summary>
    /// get/set - The password.
    /// </summary>
    public string Password { get; set; } = "";

    /// <summary>
    /// get/set - The Service Now instance name.
    /// </summary>
    public string Instance { get; set; } = "";

    /// <summary>
    /// get/set - The API URL
    /// </summary>
    public string ApiUrl { get; set; } = "";

    /// <summary>
    /// get/set - Service Now API endpoints.
    /// </summary>
    public ServiceNowEndpointOptions Endpoints { get; set; } = new();

    /// <summary>
    /// get/set - Names of tables.
    /// </summary>
    public ServiceNowTableNameOptions TableNames { get; set; } = new();
    #endregion
}
