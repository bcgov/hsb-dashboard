using HSB.Models;

namespace HSB.DataService;

/// <summary>
/// IHsbApiService interface, provides endpoints for the HSB API.
/// </summary>
public interface IHsbApiService
{
    #region Data Sync
    /// <summary>
    /// Fetch all data sync configuration items.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<DataSyncModel>> FetchDataSyncAsync();

    /// <summary>
    /// Get the data sync for the specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Task<DataSyncModel?> GetDataSyncAsync(string name);

    /// <summary>
    /// Update the data sync.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<DataSyncModel?> UpdateDataSyncAsync(DataSyncModel model);
    #endregion

    #region Operating System Items
    /// <summary>
    /// Fetch all operating system items from HSB.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<OperatingSystemItemModel>> FetchOperatingSystemItemsAsync();

    /// <summary>
    /// Add operating system to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<OperatingSystemItemModel?> AddOperatingSystemItemAsync(OperatingSystemItemModel model);

    /// <summary>
    /// Update operating system in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<OperatingSystemItemModel?> UpdateOperatingSystemItemAsync(OperatingSystemItemModel model);
    #endregion

    #region Tenants
    /// <summary>
    /// Fetch all tenants from HSB.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<TenantModel>> FetchTenantsAsync();

    /// <summary>
    /// Add tenant to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<TenantModel?> AddTenantAsync(TenantModel model);

    /// <summary>
    /// Update tenant in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<TenantModel?> UpdateTenantAsync(TenantModel model);
    #endregion

    #region Organizations
    /// <summary>
    /// Fetch all organizations from HSB.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<OrganizationModel>> FetchOrganizationsAsync();

    /// <summary>
    /// Add organization to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<OrganizationModel?> AddOrganizationAsync(OrganizationModel model);

    /// <summary>
    /// Update organization in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<OrganizationModel?> UpdateOrganizationAsync(OrganizationModel model);
    #endregion

    #region Servers
    /// <summary>
    /// Fetch all server items from HSB.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<ServerItemModel>> FetchServerItemsAsync();

    /// <summary>
    /// Add server item to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<ServerItemModel?> AddServerItemAsync(ServerItemModel model);

    /// <summary>
    /// Update server item in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<ServerItemModel?> UpdateServerItemAsync(ServerItemModel model);

    /// <summary>
    /// Add server history item to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<ServerHistoryItemModel?> AddServerHistoryItemAsync(ServerHistoryItemModel model);
    #endregion

    #region File System Items
    /// <summary>
    /// Fetch all file system items from HSB.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<FileSystemItemModel>> FetchFileSystemItemsAsync();

    /// <summary>
    /// Get the file system item from HSB for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<FileSystemItemModel?> GetFileSystemItemAsync(string id);

    /// <summary>
    /// Add file system item to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<FileSystemItemModel?> AddFileSystemItemAsync(FileSystemItemModel model);

    /// <summary>
    /// Update file system item in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<FileSystemItemModel?> UpdateFileSystemItemAsync(FileSystemItemModel model);

    /// <summary>
    /// Add file system history item to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task<FileSystemHistoryItemModel?> AddFileSystemHistoryItemAsync(FileSystemHistoryItemModel model);
    #endregion
}
