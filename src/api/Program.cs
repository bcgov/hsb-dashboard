using System.Text.Json.Serialization;
using HSB.API.Middleware;
using HSB.Core.Extensions;
using HSB.DAL.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace HSB.API;

/// <summary>
/// Program class, provides a way to setup and run the API.
/// </summary>
public class Program
{
    /// <summary>
    /// The entrypoint to start the API.
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        DotNetEnv.Env.Load();
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var config = builder.Configuration;
        var env = builder.Environment;

        var jsonSerializerOptions = config.GetSerializerOptions();
        builder.Services.AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;
        })
          .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = jsonSerializerOptions.DefaultIgnoreCondition;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = jsonSerializerOptions.PropertyNameCaseInsensitive;
                options.JsonSerializerOptions.PropertyNamingPolicy = jsonSerializerOptions.PropertyNamingPolicy;
                options.JsonSerializerOptions.WriteIndented = jsonSerializerOptions.WriteIndented;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                // options.JsonSerializerOptions.Converters.Add(new Int32ToStringJsonConverter());
            });

        builder.Services.AddOptions<KestrelServerOptions>().Bind(config.GetSection("Kestrel"));
        builder.Services.AddOptions<FormOptions>().Bind(config.GetSection("Form"));
        builder.Services
            .Configure<RouteOptions>(options => options.LowercaseUrls = true)
            .Configure<ForwardedHeadersOptions>(options =>
              {
                  options.ForwardedHeaders = ForwardedHeaders.All;
                  options.AllowedHosts = config.GetValue<string>("AllowedHosts")?.Split(';').ToList() ?? new List<string>();
              })
            .AddSerializerOptions(config)
            .AddOpenAPI(config)
            .AddKeycloak(builder)
            .AddHSBServices(builder)
            .AddScoped<IAuthorizationHelper, AuthorizationHelper>()
            .AddScoped<IXlsExporter, XlsExporter>()
            .AddCors(options =>
            {
                var withOrigins = config.GetSection("Cors:WithOrigins").Value?.Split(" ") ?? Array.Empty<string>();
                if (withOrigins.Length != 0)
                {
                    options.AddPolicy(
                    name: "allowedOrigins",
                    builder =>
                    {
                        builder
                        .WithOrigins(withOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod(); ;
                    });
                }
            })
            .AddResponseCaching();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UsePathBase(config.GetValue<string>("BaseUrl"));
        app.UseOpenAPI();
        app.UseForwardedHeaders();

        app.UseMiddleware(typeof(ErrorHandlingMiddleware));
        app.UseMiddleware(typeof(ResponseTimeMiddleware));

        // app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("CorsPolicy");

        app.UseMiddleware(typeof(LogRequestMiddleware));
        app.UseResponseCaching();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
