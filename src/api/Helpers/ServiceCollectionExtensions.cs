using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using HSB.Keycloak;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace HSB.API;

/// <summary>
/// ServiceCollectionExtensions static class, provides extension methods for IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddOpenAPI(this IServiceCollection services, IConfiguration config)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services
            .ConfigureOptions<Config.ConfigureSwaggerOptions>()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                options.EnableAnnotations(false, true);
                options.CustomSchemaIds(o => o.FullName);
                options.OperationFilter<Config.SwaggerDefaultValues>();
                options.DocumentFilter<Config.SwaggerDocumentFilter>();
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            })
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                // options.ApiVersionReader = new HeaderApiVersionReader("api-version");
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new HeaderApiVersionReader("api-version"),
                    new MediaTypeApiVersionReader("api-version"));
            })
            .AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

        return services;
    }

    /// <summary>
    /// Add Keycloak authentication and authorization.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <returns></returns>
    /// <exception cref="AuthenticationException"></exception>
    public static IServiceCollection AddKeycloak(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // The following dependencies provide dynamic authorization based on keycloak client roles.
        services.AddOptions<Config.KeycloakOptions>().Bind(builder.Configuration.GetSection("Keycloak"));
        services.AddSingleton<IAuthorizationHandler, KeycloakClientRoleHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, ClientRoleAuthorizationPolicyProvider>();
        services.AddAuthorization(options =>
        {
            // options.AddPolicy("administrator", policy => policy.Requirements.Add(new KeycloakClientRoleRequirement("administrator")));
        });

        IdentityModelEventSource.ShowPII = true;
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var section = builder.Configuration.GetSection("Keycloak");
            var requireHttpsMetadata = section.GetValue<bool?>("RequireHttpsMetadata");
            options.RequireHttpsMetadata = requireHttpsMetadata ?? !builder.Environment.IsDevelopment();
            options.Authority = section.GetValue<string>("Authority");
            options.Audience = section.GetValue<string>("Audience");
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                // ValidIssuer = section.GetValue<string>("Issuer"),
                ValidIssuers = section.GetValue<string>("Issuer")?.Split(",") ?? [],
                ValidateIssuer = section.GetValue<bool>("ValidateIssuer"),
                // ValidAudience = section.GetValue<string>("Audience"),
                ValidAudiences = section.GetValue<string>("Audience")?.Split(",") ?? [],
                ValidateAudience = section.GetValue<bool>("ValidateAudience"),
                ValidateLifetime = true
            };
            var secret = builder.Configuration["Keycloak:Secret"];
            if (!String.IsNullOrWhiteSpace(secret))
            {
                var key = Encoding.ASCII.GetBytes(secret);
                if (key.Length > 0) options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(key);
            }
            options.Events = new JwtBearerEvents()
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    context.NoResult();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    throw new AuthenticationException("Failed to authenticate", context.Exception);
                },
                OnForbidden = context =>
                {
                    return Task.CompletedTask;
                }
            };
        });
        return services;
    }

}
