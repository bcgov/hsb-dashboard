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
    Task<IEnumerable<DataSyncModel>> FetchDataSyncAsync();

    /// <summary>
    /// Get the data sync for the specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<DataSyncModel?> GetDataSyncAsync(string name);

    /// <summary>
    /// Update the data sync.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<DataSyncModel?> UpdateDataSyncAsync(DataSyncModel model);
    #endregion

    #region Operating System Items
    /// <summary>
    /// Fetch all operating system items from HSB.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<OperatingSystemItemModel>> FetchOperatingSystemItemsAsync();

    /// <summary>
    /// Add operating system to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<OperatingSystemItemModel?> AddOperatingSystemItemAsync(OperatingSystemItemModel model);

    /// <summary>
    /// Update operating system in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<OperatingSystemItemModel?> UpdateOperatingSystemItemAsync(OperatingSystemItemModel model);
    #endregion

    #region Tenants
    /// <summary>
    /// Fetch all tenants from HSB.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TenantModel>> FetchTenantsAsync();

    /// <summary>
    /// Add tenant to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<TenantModel?> AddTenantAsync(TenantModel model);

    /// <summary>
    /// Update tenant in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<TenantModel?> UpdateTenantAsync(TenantModel model);
    #endregion

    #region Organizations
    /// <summary>
    /// Fetch all organizations from HSB.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<OrganizationModel>> FetchOrganizationsAsync();

    /// <summary>
    /// Add organization to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<OrganizationModel?> AddOrganizationAsync(OrganizationModel model);

    /// <summary>
    /// Update organization in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<OrganizationModel?> UpdateOrganizationAsync(OrganizationModel model);
    #endregion

    #region Servers
    /// <summary>
    /// Fetch all server items from HSB.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<IEnumerable<ServerItemModel>> FetchServerItemsAsync(Models.Filters.ServerItemFilter? filter = null);

    /// <summary>
    /// Get server item from HSB.
    /// </summary>
    /// <param name="serviceNowKey"></param>
    /// <returns></returns>
    Task<ServerItemModel?> GetServerItemAsync(string serviceNowKey);

    /// <summary>
    /// Add server item to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ServerItemModel?> AddServerItemAsync(ServerItemModel model);

    /// <summary>
    /// Update server item in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="updateTotals"></param>
    /// <returns></returns>
    Task<ServerItemModel?> UpdateServerItemAsync(ServerItemModel model, bool updateTotals = false);

    /// <summary>
    /// Delete server item in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<ServerItemModel> DeleteServerItemAsync(ServerItemModel model);
    #endregion

    #region File System Items
    /// <summary>
    /// Fetch all file system items from HSB.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<FileSystemItemModel>> FetchFileSystemItemsAsync();

    /// <summary>
    ///Get the file system item from HSB for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="serverItemId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<FileSystemItemModel?> GetFileSystemItemAsync(string id, string? serverItemId, string? name = null);

    /// <summary>
    /// Add file system item to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<FileSystemItemModel?> AddFileSystemItemAsync(FileSystemItemModel model);

    /// <summary>
    /// Update file system item in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<FileSystemItemModel?> UpdateFileSystemItemAsync(FileSystemItemModel model);

    /// <summary>
    /// Delete file system item in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<FileSystemItemModel?> DeleteFileSystemItemAsync(FileSystemItemModel model);
    #endregion
}
