namespace HSB.DataService;

/// <summary>
/// IDataService interface, provides methods to manage the lifecycle of the service.
/// </summary>
public interface IDataService
{
    #region Methods
    /// <summary>
    /// Run the service manager.
    /// </summary>
    /// <returns></returns>
    public Task RunAsync();
    #endregion
}
