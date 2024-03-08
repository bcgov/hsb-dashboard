using System.Security.Claims;
using HSB.DAL.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HSB.DAL.Extensions;

/// <summary>
/// ServiceCollectionExtensions static class, provides extension methods for IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    #region Methods
    /// <summary>
    /// Add HSBContext to services collection.
    /// Configure the default database.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddHSBContext(this IServiceCollection services, WebApplicationBuilder builder, Action<DbContextOptionsBuilder, DbContextOptionsBuilder>? options = null)
    {
        var config = builder.Configuration;
        var env = builder.Environment;

        return services.AddDbContext<HSBContext>(opt =>
        {
            // Generate the database connection string.
            var builder = new NpgsqlConnectionStringBuilder(config.GetConnectionString("Default"));
            if (String.IsNullOrWhiteSpace(builder.Host) && !String.IsNullOrWhiteSpace(config["DB_ADDR"]))
                builder.Host = String.IsNullOrWhiteSpace(builder.Host) ? config["DB_ADDR"] : builder.Host;
            builder.Database = String.IsNullOrWhiteSpace(builder.Database) ? config["DB_NAME"] : builder.Database;
            builder.Username = String.IsNullOrWhiteSpace(builder.Username) ? config["DB_USER"] : builder.Username;
            builder.Password = String.IsNullOrWhiteSpace(builder.Password) ? config["DB_PASSWORD"] : builder.Password;

            var sql = opt.UseNpgsql(builder.ConnectionString, sqlOptions =>
            {
                sqlOptions.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds);
            });

            if (options == null && env.IsDevelopment())
            {
                var debugLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });
                sql.UseLoggerFactory(debugLoggerFactory);
                sql.EnableDetailedErrors(env.IsDevelopment());
                sql.EnableSensitiveDataLogging(env.IsDevelopment());
                sql.LogTo(Console.Write);
            }

            options?.Invoke(opt, sql);
        });
    }

    /// <summary>
    /// Add all required services to support DAL layer.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IServiceCollection AddHSBServices(this IServiceCollection services, WebApplicationBuilder builder, Action<DbContextOptionsBuilder, DbContextOptionsBuilder>? options = null)
    {
        // Find all the configuration classes.
        var assembly = typeof(BaseService).Assembly;
        var type = typeof(IBaseService);
        var tnoServiceTypes = assembly.GetTypes().Where(t => !t.IsAbstract && t.IsClass && t.GetInterfaces().Any(i => i.Name.Equals(type.Name)));
        foreach (var serviceType in tnoServiceTypes)
        {
            var sInterface = serviceType.GetInterface($"I{serviceType.Name}") ?? throw new InvalidOperationException($"Service type '{serviceType.Name}' is missing its interface.");
            services.AddScoped(sInterface, serviceType);
        }

        return services
            .AddHSBContext(builder, options)
            .AddHttpContextAccessor()
            .AddTransient(s => s.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? new ClaimsPrincipal());
    }
    #endregion
}
