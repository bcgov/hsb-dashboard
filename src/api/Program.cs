using System.Security.Claims;
using System.Text.Json.Serialization;
using HSB.API.Swagger;
using HSB.Core.Extensions;
using HSB.DAL.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;

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
        builder.Services.AddControllers()
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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services
            .AddSerializerOptions(config)
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("api-version"),
                new MediaTypeApiVersionReader("api-version"));
            })
            .AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            })
            .ConfigureOptions<ConfigureSwaggerOptions>()
            .Configure<RouteOptions>(options => options.LowercaseUrls = true)
            .AddHttpContextAccessor()
            .AddTransient(s => s.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal())
            .Configure<ForwardedHeadersOptions>(options =>
              {
                  options.ForwardedHeaders = ForwardedHeaders.All;
                  options.AllowedHosts = config.GetValue<string>("AllowedHosts")?.Split(';').ToList() ?? new List<string>();
              })
            .AddCors(options =>
            {
                var withOrigins = config.GetSection("Cors:WithOrigins").Value?.Split(" ") ?? Array.Empty<string>();
                if (withOrigins.Any())
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
            .AddHSBContext(config);

        var app = builder.Build();

        app.UsePathBase(config.GetValue<string>("BaseUrl"));
        app.UseForwardedHeaders();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse())
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                  description.GroupName.ToUpperInvariant());
                }
            });
        }

        // app.UseHttpsRedirection();
        app.UseCors("allowedOrigins");
        app.UseStaticFiles();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
