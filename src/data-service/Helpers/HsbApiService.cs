using System.Text.Json;
using HSB.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HSB.Core.Http;
using System.Net.Http.Json;

namespace HSB.DataService;

/// <summary>
/// HsbApiService class, provides endpoints for the HSB API.
/// </summary>
public class HsbApiService : IHsbApiService
{
    #region Variables
    #endregion

    #region Properties
    /// <summary>
    /// get - The logger;
    /// </summary>
    protected ILogger<IHsbApiService> Logger { get; }

    /// <summary>
    /// get - The configuration options for the service.
    /// </summary>
    protected ServiceOptions Options { get; }

    /// <summary>
    /// get - The HTTP client to communicate with HSB API.
    /// </summary>
    protected IOpenIdConnectRequestClient ApiClient { get; }

    /// <summary>
    /// get - The serializer options.
    /// </summary>
    protected JsonSerializerOptions SerializerOptions { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a HsbApiService object, initializes with specified parameters.
    /// </summary>
    /// <param name="apiClient"></param>
    /// <param name="serviceOptions"></param>
    /// <param name="serializerOptions"></param>
    /// <param name="logger"></param>
    public HsbApiService(
        IOpenIdConnectRequestClient apiClient,
        IOptions<ServiceOptions> serviceOptions,
        IOptions<JsonSerializerOptions> serializerOptions,
        ILogger<IHsbApiService> logger)
    {
        this.ApiClient = apiClient;
        this.Options = serviceOptions.Value;
        this.SerializerOptions = serializerOptions.Value;
        this.Logger = logger;

        this.ApiClient.Client.BaseAddress = new Uri(this.Options.ApiUrl);
    }
    #endregion

    #region methods
    /// <summary>
    /// Make request to HSB API.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    private async Task<T?> HsbSendAsync<T>(HttpMethod method, Uri uri, HttpContent? content = null)
    {
        try
        {
            var response = await this.ApiClient.SendAsync(uri, method, content);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var model = JsonSerializer.Deserialize<T>(json, this.SerializerOptions);
            return model;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to send request to API");
            throw;
        }
    }

    #region Data Sync
    /// <summary>
    /// Get the data sync for the specified name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<DataSyncModel?> GetDataSync(string name)
    {
        this.Logger.LogDebug("HSB - Fetching data sync");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = $"{this.Options.Endpoints.DataSync}/{name}"
        };
        var results = await HsbSendAsync<DataSyncModel>(HttpMethod.Get, builder.Uri);
        return results;
    }

    /// <summary>
    /// Update the data sync.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<DataSyncModel?> UpdateDataSync(DataSyncModel model)
    {
        this.Logger.LogDebug("HSB - Update data sync");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = $"{this.Options.Endpoints.DataSync}/{model.Id}"
        };
        var results = await HsbSendAsync<DataSyncModel>(HttpMethod.Put, builder.Uri, JsonContent.Create(model));
        return results;
    }
    #endregion

    #region Operating System Items
    /// <summary>
    /// Fetch all operating system items from HSB.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<OperatingSystemItemModel>> FetchOperatingSystemItemsAsync()
    {
        this.Logger.LogDebug("HSB - Fetching all operating system items");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.OperatingSystemItems
        };
        var results = await HsbSendAsync<IEnumerable<OperatingSystemItemModel>>(HttpMethod.Get, builder.Uri);
        return results ?? Array.Empty<OperatingSystemItemModel>();
    }

    /// <summary>
    /// Add operating system to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<OperatingSystemItemModel?> AddOperatingSystemItemAsync(OperatingSystemItemModel model)
    {
        this.Logger.LogDebug("HSB - Add operating system item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.OperatingSystemItems
        };
        var results = await HsbSendAsync<OperatingSystemItemModel>(HttpMethod.Post, builder.Uri, JsonContent.Create(model));
        return results;
    }

    /// <summary>
    /// Update operating system in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<OperatingSystemItemModel?> UpdateOperatingSystemItemAsync(OperatingSystemItemModel model)
    {
        this.Logger.LogDebug("HSB - Update operating system item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = $"{this.Options.Endpoints.OperatingSystemItems}/{model.Id}"
        };
        var results = await HsbSendAsync<OperatingSystemItemModel>(HttpMethod.Put, builder.Uri, JsonContent.Create(model));
        return results;
    }
    #endregion

    #region Tenants
    /// <summary>
    /// Fetch all tenants from HSB.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TenantModel>> FetchTenantsAsync()
    {
        this.Logger.LogDebug("HSB - Fetching all tenants");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.Tenants
        };
        var results = await HsbSendAsync<IEnumerable<TenantModel>>(HttpMethod.Get, builder.Uri);
        return results ?? Array.Empty<TenantModel>();
    }

    /// <summary>
    /// Add tenant to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<TenantModel?> AddTenantAsync(TenantModel model)
    {
        this.Logger.LogDebug("HSB - Add tenant item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.Tenants
        };
        var results = await HsbSendAsync<TenantModel>(HttpMethod.Post, builder.Uri, JsonContent.Create(model));
        return results;
    }

    /// <summary>
    /// Update tenant in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<TenantModel?> UpdateTenantAsync(TenantModel model)
    {
        this.Logger.LogDebug("HSB - Update tenant item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = $"{this.Options.Endpoints.Tenants}/{model.Id}"
        };
        var results = await HsbSendAsync<TenantModel>(HttpMethod.Put, builder.Uri, JsonContent.Create(model));
        return results;
    }
    #endregion

    #region Organizations
    /// <summary>
    /// Fetch all organizations from HSB.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<OrganizationModel>> FetchOrganizationsAsync()
    {
        this.Logger.LogDebug("HSB - Fetching all organizations");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.Organizations
        };
        var results = await HsbSendAsync<IEnumerable<OrganizationModel>>(HttpMethod.Get, builder.Uri);
        return results ?? Array.Empty<OrganizationModel>();
    }

    /// <summary>
    /// Add organization to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<OrganizationModel?> AddOrganizationAsync(OrganizationModel model)
    {
        this.Logger.LogDebug("HSB - Add organization item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.Organizations
        };
        var results = await HsbSendAsync<OrganizationModel>(HttpMethod.Post, builder.Uri, JsonContent.Create(model));
        return results;
    }

    /// <summary>
    /// Update organization in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<OrganizationModel?> UpdateOrganizationAsync(OrganizationModel model)
    {
        this.Logger.LogDebug("HSB - Update organization item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = $"{this.Options.Endpoints.Organizations}/{model.Id}"
        };
        var results = await HsbSendAsync<OrganizationModel>(HttpMethod.Put, builder.Uri, JsonContent.Create(model));
        return results;
    }
    #endregion

    #region Servers
    /// <summary>
    /// Fetch all server items from HSB.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<ServerItemModel>> FetchServerItemsAsync()
    {
        this.Logger.LogDebug("HSB - Fetching all server items");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.ServerItems
        };
        var results = await HsbSendAsync<IEnumerable<ServerItemModel>>(HttpMethod.Get, builder.Uri);
        return results ?? Array.Empty<ServerItemModel>();
    }

    /// <summary>
    /// Add server item to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<ServerItemModel?> AddServerItemAsync(ServerItemModel model)
    {
        this.Logger.LogDebug("HSB - Add server item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.ServerItems
        };
        var results = await HsbSendAsync<ServerItemModel>(HttpMethod.Post, builder.Uri, JsonContent.Create(model));
        return results;
    }

    /// <summary>
    /// Update server item in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<ServerItemModel?> UpdateServerItemAsync(ServerItemModel model)
    {
        this.Logger.LogDebug("HSB - Update server item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = $"{this.Options.Endpoints.ServerItems}/{model.Id}"
        };
        var results = await HsbSendAsync<ServerItemModel>(HttpMethod.Put, builder.Uri, JsonContent.Create(model));
        return results;
    }
    #endregion

    #region File System Items
    /// <summary>
    /// Fetch all organizations from HSB.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<FileSystemItemModel>> FetchFileSystemItemsAsync()
    {
        this.Logger.LogDebug("HSB - Fetching all file system items");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.FileSystemItems
        };
        var results = await HsbSendAsync<IEnumerable<FileSystemItemModel>>(HttpMethod.Get, builder.Uri);
        return results ?? Array.Empty<FileSystemItemModel>();
    }

    /// <summary>
    /// Add configuration item to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<FileSystemItemModel?> AddFileSystemItemAsync(FileSystemItemModel model)
    {
        this.Logger.LogDebug("HSB - Add file system item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.FileSystemItems
        };
        var results = await HsbSendAsync<FileSystemItemModel>(HttpMethod.Post, builder.Uri, JsonContent.Create(model));
        return results;
    }

    /// <summary>
    /// Update file system item in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<FileSystemItemModel?> UpdateFileSystemItemAsync(FileSystemItemModel model)
    {
        this.Logger.LogDebug("HSB - Update file system item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = $"{this.Options.Endpoints.FileSystemItems}/{model.Id}"
        };
        var results = await HsbSendAsync<FileSystemItemModel>(HttpMethod.Put, builder.Uri, JsonContent.Create(model));
        return results;
    }
    #endregion

    #region Configuration Items
    /// <summary>
    /// Add configuration item to HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<ConfigurationItemModel?> AddConfigurationItemAsync(ConfigurationItemModel model)
    {
        this.Logger.LogDebug("HSB - Add configuration item");
        var builder = new UriBuilder($"{this.ApiClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.ConfigurationItems
        };
        var results = await HsbSendAsync<ConfigurationItemModel>(HttpMethod.Post, builder.Uri, JsonContent.Create(model));
        return results;
    }
    #endregion
    #endregion
}
