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

            // Treat a 404 like a 204 because so many API developers do this...  It's annoying because it results in the same error if the endpoint doesn't exist.
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

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
    /// Fetch all items from the service now API.
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async Task<IEnumerable<ResultModel<T>>> FetchTableItemsAsync<T>(string tableName, int limit, int offset, string filter = "")
    {
        this.Logger.LogDebug("Service Now - Fetching {tableName} items", tableName);

        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", tableName)
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query.Add("sysparm_offset", $"{offset}");
        query.Add("sysparm_limit", $"{limit}");
        if (!String.IsNullOrWhiteSpace(filter))
        {
            var filterQuery = HttpUtility.ParseQueryString($"sysparm_query={filter}");
            query.Add(filterQuery);
        }
        builder.Query = query.ToString();

        var result = await ServiceNowArraySendAsync<T>(HttpMethod.Get, builder.Uri);
        return result.ToArray();
    }

    /// <summary>
    /// Get the item for the specified 'tableName' and 'id'.
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ResultModel<T>?> GetTableItemAsync<T>(string tableName, string id)
    {
        this.Logger.LogDebug("Service Now - Fetch '{tableName}' item '{id}'", tableName, id);
        var builder = new UriBuilder($"{this.SNClient.Client.BaseAddress}")
        {
            Path = this.Options.Endpoints.TablePath.Replace("{name}", $"{tableName}/{id}")
        };
        return await ServiceNowSendAsync<T>(HttpMethod.Get, builder.Uri);
    }
    #endregion

}
