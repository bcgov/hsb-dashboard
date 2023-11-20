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
    /// <param name="config"></param>
    /// <param name="options"></param>
    /// <param name="env"></param>
    /// <returns></returns>
    public static IServiceCollection AddHSBContext(this IServiceCollection services, IConfiguration config, Action<DbContextOptionsBuilder>? options = null, IHostEnvironment? env = null)
    {
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

            if (options == null)
            {
                var debugLoggerFactory = LoggerFactory.Create(builder => { builder.AddDebug(); });
                sql.UseLoggerFactory(debugLoggerFactory);
            }
            opt.EnableSensitiveDataLogging(env?.IsDevelopment() ?? false);
            opt.EnableDetailedErrors(env?.IsDevelopment() ?? false);

            options?.Invoke(opt);
        });
    }
    #endregion
}
