namespace HSB.DataService;

/// <summary>
/// ServiceNowEndpointOptions class, provides configuration settings for the service.
/// </summary>
public class ServiceNowEndpointOptions
{
    #region Properties
    /// <summary>
    /// get/set - The path to the table endpoint.
    /// </summary>
    public string TablePath { get; set; } = "/api/now/table/{name}";
    #endregion
}
