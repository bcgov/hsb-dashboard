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
    /// Fetch all items from the service now API.
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public Task<IEnumerable<ResultModel<T>>> FetchTableItemsAsync<T>(string tableName, int limit, int offset, string filter = "");

    /// <summary>
    /// Get the item for the specified 'tableName' and 'id'.
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<ResultModel<T>?> GetTableItemAsync<T>(string tableName, string id);
    #endregion
}
