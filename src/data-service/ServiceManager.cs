using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using HSB.Core.Http;
using HSB.Core.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HSB.DataService;

/// <summary>
/// ServiceManager class, provides a console application that pulls Service Now data and sends it to the HSB API.
/// </summary>
public class ServiceManager
{
    #region Variables
    /// <summary>
    /// The logger for the service.
    /// </summary>
    private readonly ILogger _logger;
    #endregion

    #region Properties
    /// <summary>
    /// get - The environment.
    /// </summary>
    public IHostEnvironment Environment { get; private set; }

    /// <summary>
    /// get - Program configuration.
    /// </summary>
    public IConfiguration Configuration { get; private set; }

    /// <summary>
    /// get - The application.
    /// </summary>
    public WebApplication App { get; private set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a ServiceManager object, initializes with arguments.
    /// </summary>
    /// <param name="args"></param>
    public ServiceManager(string[] args)
    {
        DotNetEnv.Env.Load($"{System.Environment.CurrentDirectory}{Path.DirectorySeparatorChar}.env");
        var builder = WebApplication.CreateBuilder(args);
        this.Environment = builder.Environment;
        this.Configuration = Configure(builder, args)
            .Build();
        ConfigureServices(builder.Services);
        this.App = builder.Build();
        Console.OutputEncoding = Encoding.UTF8;
        _logger = this.App.Services.GetRequiredService<ILogger<DataService>>();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Configure application.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    protected virtual IConfigurationBuilder Configure(WebApplicationBuilder builder, string[]? args)
    {
        string? environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string urls = System.Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://+:5000";
        builder.WebHost.UseUrls(urls);
        return builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .AddCommandLine(args ?? Array.Empty<string>());
    }

    /// <summary>
    /// Configure dependency injection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    protected virtual IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var jsonSerializerOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = this.Configuration.GetValue<JsonIgnoreCondition>("Serialization:Json:DefaultIgnoreCondition", JsonIgnoreCondition.WhenWritingNull),
            PropertyNameCaseInsensitive = this.Configuration.GetValue<bool>("Serialization:Json:PropertyNameCaseInsensitive", false),
            PropertyNamingPolicy = this.Configuration["Serialization:Json:PropertyNamingPolicy"] == "CamelCase" ? JsonNamingPolicy.CamelCase : null,
            WriteIndented = this.Configuration.GetValue<bool>("Serialization:Json:WriteIndented", true)
        };

        services
            .AddSingleton<IConfiguration>(this.Configuration)
            .Configure<ServiceOptions>(this.Configuration.GetSection("Service"))
            .Configure<ServiceNowOptions>(this.Configuration.GetSection("ServiceNow"))
            .Configure<JsonSerializerOptions>(options =>
            {
                options.DefaultIgnoreCondition = jsonSerializerOptions.DefaultIgnoreCondition;
                options.PropertyNameCaseInsensitive = jsonSerializerOptions.PropertyNameCaseInsensitive;
                options.PropertyNamingPolicy = jsonSerializerOptions.PropertyNamingPolicy;
                options.WriteIndented = jsonSerializerOptions.WriteIndented;
                options.Converters.Add(new JsonStringEnumConverter());
            })
            .AddLogging(options =>
            {
                options.AddConfiguration(this.Configuration.GetSection("Logging"));
                options.AddConsole();
            })
            .AddTransient<JwtSecurityTokenHandler>()
            .AddSingleton(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, "") })))
            .Configure<AuthClientOptions>(this.Configuration.GetSection("Keycloak"))
            .Configure<OpenIdConnectOptions>(this.Configuration.GetSection("OIDC"))
            .AddTransient<IHttpRequestClient, HttpRequestClient>()
            .AddTransient<IOpenIdConnectRequestClient, OpenIdConnectRequestClient>()
            .AddScoped<IDataService, DataService>()
            .AddTransient<IServiceNowApiService, ServiceNowApiService>()
            .AddScoped<IHsbApiService, HsbApiService>();

        services.AddHttpClient(typeof(DataService).FullName ?? nameof(DataService), client => { });
        return services;
    }

    /// <summary>
    /// Run the service.
    /// </summary>
    /// <returns></returns>
    private async Task RunServiceAsync()
    {
        using var scope = this.App.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IDataService>();
        await service.RunAsync();
    }

    /// <summary>
    /// Run the ingestion service.
    /// </summary>
    /// <returns></returns>
    public async Task<int> RunAsync()
    {
        try
        {
            _logger.LogInformation("Service started");
            var tasks = new[] { RunServiceAsync() };
            var task = await Task.WhenAny(tasks);
            if (task.Status == TaskStatus.Faulted) throw task.Exception ?? new Exception("An unhandled exception has occurred.");
            _logger.LogInformation("Service stopped");
            return 0;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "An unhandled error has occurred.");
            return 1;
        }
    }
    #endregion
}
