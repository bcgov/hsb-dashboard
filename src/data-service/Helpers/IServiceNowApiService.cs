using HSB.Models.ServiceNow;

namespace HSB.DataService;

/// <summary>
/// IServiceNowApiService interface, provides endpoints to communicate with the Service Now API.
/// </summary>
public interface IServiceNowApiService
{
    #region Properties
    /// <summary>
    /// get - The configuration options for Service Now API.
    /// </summary>
    public ServiceNowOptions Options { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Fetch configuration items from service now.
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task<IEnumerable<ResultModel<ConfigurationItemModel>>> FetchConfigurationItemsAsync(int limit, int offset, string filter = "");

    /// <summary>
    /// Get the operating system for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ResultModel<OperatingSystemModel>?> GetOperatingSystemAsync(string id);

    /// <summary>
    /// Get the client organization for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ResultModel<ClientOrganizationModel>?> GetClientOrganizationAsync(string id);

    /// <summary>
    /// Get the tenant for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ResultModel<TenantModel>?> GetTenantAsync(string id);

    /// <summary>
    /// Get the server for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ResultModel<ServerModel>?> GetServerAsync(string id);

    /// <summary>
    /// Get the file system for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ResultModel<FileSystemModel>?> GetFileSystemAsync(string id);
    #endregion
}
