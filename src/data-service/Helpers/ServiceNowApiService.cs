using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HSB.Core.Http;
using HSB.Models.ServiceNow;

namespace HSB.DataService;

/// <summary>
/// ServiceNowApiService class, provides a way to communicate with the Service Now API.
/// </summary>
public class ServiceNowApiService : IServiceNowApiService
{
    #region Properties
    /// <summary>
    /// get - The logger;
    /// </summary>
    protected ILogger<IServiceNowApiService> Logger { get; }

    /// <summary>
    /// get - The configuration options for Service Now API.
    /// </summary>
    public ServiceNowOptions Options { get; }

    /// <summary>
    /// get - The HTTP client to communicate with Service Now API.
    /// </summary>
    protected IHttpRequestClient SNClient { get; }

    /// <summary>
    /// get - The serializer options.
    /// </summary>
    protected JsonSerializerOptions SerializerOptions { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a DataService object, initializes with specified parameters.
    /// </summary>
    /// <param name="serviceNowClient"></param>
    /// <param name="serviceNowOptions"></param>
    /// <param name="serializerOptions"></param>
    /// <param name="logger"></param>
    public ServiceNowApiService(
        IHttpRequestClient serviceNowClient,
        IOptions<ServiceNowOptions> serviceNowOptions,
        IOptions<JsonSerializerOptions> serializerOptions,
        ILogger<IServiceNowApiService> logger)
    {
        this.SNClient = serviceNowClient;
        this.Options = serviceNowOptions.Value;
        this.SerializerOptions = serializerOptions.Value;
        this.Logger = logger;

        var authenticationString = $"{this.Options.Username}:{this.Options.Password}";
        var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
        this.SNClient.Client.BaseAddress = new Uri($"{this.Options.ApiUrl.Replace("{instance}", this.Options.Instance)}");
        this.SNClient.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Make a request to Service Now API.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    private async Task<ResultModel<T>?> ServiceNowSendAsync<T>(HttpMethod method, Uri uri)
    {
        try
        {
            var response = await this.SNClient.SendAsync(uri, method);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (String.IsNullOrWhiteSpace(json)) throw new InvalidOperationException("Response contained invalid JSON");

            var model = JsonSerializer.Deserialize<ResponseModel<T>>(json, this.SerializerOptions) ?? throw new InvalidOperationException("Response contained invalid JSON");
            return new ResultModel<T>(model.Result, JsonDocument.Parse(json));
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to send request to API");
            throw;
        }
    }

    /// <summary>
    /// Make a request to Service Now API.
    /// Iterate through array to return an array of ResultModel.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    private async Task<ResultModel<T>[]> ServiceNowArraySendAsync<T>(HttpMethod method, Uri uri)
    {
        try
        {
            var response = await this.SNClient.SendAsync(uri, method);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            if (String.IsNullOrWhiteSpace(json)) throw new InvalidOperationException("Response contained invalid JSON");

            var document = JsonDocument.Parse(json) ?? throw new InvalidOperationException("Response contained invalid JSON");
            var resultElement = document.RootElement.GetProperty("result");

            var results = resultElement.EnumerateArray().Select(i => new ResultModel<T>(i.Deserialize<T>(this.SerializerOptions), JsonDocument.Parse(i.ToString())));
            return results.ToArray();
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Failed to send request to API");
            throw;
        }
    }

    /// <summary>
    /// Fetch all configuration items from service now.
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ResultModel<ConfigurationItemModel>>> FetchConfigurationItemsAsync(int limit, int offset, string filter = "")
    {
        this.Logger.LogDebug("Service Now - Fetching configuration items");

        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", this.Options.TableNames.ConfigurationItem)
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query.Add("sysparm_offset", $"{offset}");
        query.Add("sysparm_limit", $"{limit}");
        if (!String.IsNullOrWhiteSpace(filter))
        {
            var filterQuery = HttpUtility.ParseQueryString(filter);
            query.Add(filterQuery);
        }
        builder.Query = query.ToString();

        var result = await ServiceNowArraySendAsync<ConfigurationItemModel>(HttpMethod.Get, builder.Uri);
        return result.ToArray();
    }

    /// <summary>
    /// Fetch server items from service now.
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ResultModel<ServerModel>>> FetchServerItemsAsync(int limit, int offset, string filter = "")
    {
        this.Logger.LogDebug("Service Now - Fetching server items");

        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", this.Options.TableNames.Server)
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query.Add("sysparm_offset", $"{offset}");
        query.Add("sysparm_limit", $"{limit}");
        if (!String.IsNullOrWhiteSpace(filter))
        {
            var filterQuery = HttpUtility.ParseQueryString(filter);
            query.Add(filterQuery);
        }
        builder.Query = query.ToString();

        var result = await ServiceNowArraySendAsync<ServerModel>(HttpMethod.Get, builder.Uri);
        return result.ToArray();
    }

    /// <summary>
    /// Get the configuration item for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResultModel<ConfigurationItemModel>?> GetConfigurationItemAsync(string id)
    {
        this.Logger.LogDebug("Service Now - Fetch operating system '{id}'", id);
        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", $"{this.Options.TableNames.ConfigurationItem}/{id}")
        };
        return await ServiceNowSendAsync<ConfigurationItemModel>(HttpMethod.Get, builder.Uri);
    }

    /// <summary>
    /// Get the operating system for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResultModel<OperatingSystemModel>?> GetOperatingSystemAsync(string id)
    {
        this.Logger.LogDebug("Service Now - Fetch operating system '{id}'", id);
        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", $"{this.Options.TableNames.OperatingSystem}/{id}")
        };
        return await ServiceNowSendAsync<OperatingSystemModel>(HttpMethod.Get, builder.Uri);
    }

    /// <summary>
    /// Get the client organization for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResultModel<ClientOrganizationModel>?> GetClientOrganizationAsync(string id)
    {
        this.Logger.LogDebug("Service Now - Fetch client organization '{id}'", id);
        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", $"{this.Options.TableNames.ClientOrganization}/{id}")
        };
        return await ServiceNowSendAsync<ClientOrganizationModel>(HttpMethod.Get, builder.Uri);
    }

    /// <summary>
    /// Get the tenant for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResultModel<TenantModel>?> GetTenantAsync(string id)
    {
        this.Logger.LogDebug("Service Now - Fetch tenant '{id}'", id);
        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", $"{this.Options.TableNames.Tenant}/{id}")
        };
        return await ServiceNowSendAsync<TenantModel>(HttpMethod.Get, builder.Uri);
    }

    /// <summary>
    /// Get the server for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResultModel<ServerModel>?> GetServerAsync(string id)
    {
        this.Logger.LogDebug("Service Now - Fetch server '{id}'", id);
        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", $"{this.Options.TableNames.Server}/{id}")
        };
        return await ServiceNowSendAsync<ServerModel>(HttpMethod.Get, builder.Uri);
    }

    /// <summary>
    /// Get the file system for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResultModel<FileSystemModel>?> GetFileSystemAsync(string id)
    {
        this.Logger.LogDebug("Service Now - Fetch file system '{id}'", id);
        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", $"{this.Options.TableNames.FileSystem}/{id}")
        };
        return await ServiceNowSendAsync<FileSystemModel>(HttpMethod.Get, builder.Uri);
    }
    #endregion

}
