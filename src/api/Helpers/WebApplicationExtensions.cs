using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace HSB.API;

/// <summary>
/// WebApplicationExtensions static class, provides extension methods for WebApplication.
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Use Swagger endpoint in application.
    /// </summary>
    /// <param name="app"></param>
    public static void UseOpenAPI(this WebApplication app)
    {
        var config = app.Configuration;

        app.UseSwagger(options =>
        {
            options.RouteTemplate = config.GetValue<string>("Swagger:RouteTemplate");
        });
        app.UseSwaggerUI(options =>
        {
            var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in apiVersionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(String.Format(config.GetValue<string>("Swagger:EndpointPath") ?? "", description.GroupName), description.GroupName);
            }
            options.RoutePrefix = config.GetValue<string>("Swagger:RoutePrefix");
        });
    }
}
