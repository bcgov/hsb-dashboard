using DotNetEnv.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using HSB.Core.Extensions;

namespace HSB.DAL;

/// <summary>
/// HSBContextFactory class, provides a way to use the design time tools (i.e. dotnet ef database update).
/// </summary>
[ExcludeFromCodeCoverage]
public class HSBContextFactory : IDesignTimeDbContextFactory<HSBContext>
{
    #region Variables
    private readonly ILogger<HSBContextFactory> _logger;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a HSBContextFactory class.
    /// </summary>
    public HSBContextFactory()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("Site", LogLevel.Debug)
                .AddConsole();
        });
        _logger = loggerFactory.CreateLogger<HSBContextFactory>();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Create the database context so that the design time tools can connect to it.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public HSBContext CreateDbContext(string[] args)
    {
        string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        // As per Microsoft documentation, a typical sequence of configuration providers is:
        //   1. appsettings.json
        //   2. appsettings.{Environment}.json
        //   3. Secret Manager (if in development)
        //   4. Environment variables using the Environment Variables configuration provider.
        //   5. Command - line arguments using the Command-line configuration provider.
        // source: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#configuration-providers
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddDotNetEnv($"{Environment.CurrentDirectory}/.env")
            .AddJsonFile("connectionstrings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"connectionstrings.{environment}.json", optional: true, reloadOnChange: true);

        if (!environment.IsProduction())
        {
            builder.AddUserSecrets<HSBContext>();
        }

        builder.AddEnvironmentVariables();

        _logger.LogInformation("Context Factory Started");

        var config = builder.Build();
        var cs = config.GetConnectionString("Default");
        var sqlBuilder = new NpgsqlConnectionStringBuilder(cs)
        {
            Username = config["POSTGRES_USER"],
            Password = config["POSTGRES_PASSWORD"]
        };
        var optionsBuilder = new DbContextOptionsBuilder<HSBContext>();
        optionsBuilder.UseNpgsql(sqlBuilder.ConnectionString, options =>
        {
            options.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
        });

        var serializerOptions = new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        var optionsSerializer = Microsoft.Extensions.Options.Options.Create(serializerOptions);
        return new HSBContext(optionsBuilder.Options, null, optionsSerializer);
    }
    #endregion
}
