using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HSB.API.Swagger;

/// <summary>
/// ConfigureSwaggerOptions class, provides a way to configure swagger.
/// </summary>
public class ConfigureSwaggerOptions
    : IConfigureNamedOptions<SwaggerGenOptions>
{
    #region variables
    private readonly IApiVersionDescriptionProvider _provider;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a ConfigureSwaggerOptions object, initializes with specified parameters.
    /// </summary>
    /// <param name="provider"></param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Configure each API discovered for Swagger Documentation
    /// </summary>
    /// <param name="options"></param>
    public void Configure(SwaggerGenOptions options)
    {
        // add swagger document for every API version discovered
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }
    }

    /// <summary>
    /// Configure Swagger Options. Inherited from the Interface
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    /// <summary>
    /// Create information about the version of the API
    /// </summary>
    /// <param name="description"></param>
    /// <returns>Information about the API</returns>
    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = ".NET Core (.NET 7) Web API",
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
        }

        return info;
    }
    #endregion
}
