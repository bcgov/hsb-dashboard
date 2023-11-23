using System.Text;
using System.Text.Json;
using HSB.Core.Extensions;
using HSB.DAL.Configuration;
using HSB.DAL.Extensions;
using HSB.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HSB.DAL;
public class HSBContext : DbContext
{
    #region Variables
    private readonly ILogger? _logger;
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly JsonSerializerOptions? _serializerOptions;
    #endregion

    #region Properties
    public DbSet<ConfigurationItem> ConfigurationItems => Set<ConfigurationItem>();
    public DbSet<OperatingSystemItem> OperatingSystemItems => Set<OperatingSystemItem>();
    public DbSet<FileSystemItem> FileSystemItems => Set<FileSystemItem>();
    public DbSet<ServerItem> ServerItems => Set<ServerItem>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<Role> Groups => Set<Role>();
    public DbSet<GroupRole> GroupRoles => Set<GroupRole>();
    public DbSet<Role> Roles => Set<Role>();
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a SiteContext object, initializes with specified parameters.
    /// </summary>
    /// <param name="logger"></param>
    protected HSBContext(ILogger<HSBContext> logger)
    {
        _logger = logger;
        this.ChangeTracker.LazyLoadingEnabled = false;
    }

    /// <summary>
    /// Creates a new instance of a SiteContext object, initializes with specified parameters.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="serializerOptions"></param>
    /// <param name="logger"></param>
    public HSBContext(DbContextOptions<HSBContext> options, IHttpContextAccessor? httpContextAccessor = null, IOptions<JsonSerializerOptions>? serializerOptions = null, ILogger<HSBContext>? logger = null)
      : base(options)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _serializerOptions = serializerOptions?.Value;
        this.ChangeTracker.LazyLoadingEnabled = false;
    }
    #endregion

    #region Methods

    /// <summary>
    /// Configures the DbContext with the specified options.
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        base.OnConfiguring(optionsBuilder);
    }

    /// <summary>
    /// Control the database conventions.
    /// </summary>
    /// <param name="configurationBuilder"></param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        // configurationBuilder.Conventions.Remove(typeof(PluralizingTableNameConvention));
    }

    /// <summary>
    /// Apply all the configuration settings to the model.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAllConfigurations(typeof(AuditableConfiguration<>), this);
        modelBuilder.RemovePluralizingTableNameConvention();
    }

    /// <summary>
    /// Save the entities with who created them or updated them.
    /// </summary>
    /// <returns></returns>
    public override int SaveChanges(bool acceptAllChangesOnSuccess = true)
    {
        // get entries that are being Added or Updated
        var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

        var user = _httpContextAccessor?.HttpContext?.User;
        foreach (var entry in modifiedEntries)
        {
            if (entry.Entity is Auditable entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = user?.GetUsername() ?? "";
                    entity.CreatedOn = new DateTimeOffset();
                    entity.UpdatedBy = entity.CreatedBy;
                    entity.UpdatedOn = entity.CreatedOn;
                    entity.Version = 0;
                }
                else if (entry.State != EntityState.Deleted)
                {
                    // These values will only be correct if you first load the entity before updating it.
                    entity.CreatedBy = entry.GetOriginalValue(nameof(Auditable.CreatedBy), entity.CreatedBy);
                    entity.CreatedOn = entry.GetOriginalValue(nameof(Auditable.CreatedOn), entity.CreatedOn);
                    entity.UpdatedBy = user?.GetUsername() ?? "";
                    entity.UpdatedOn = new DateTimeOffset();
                    entity.Version++;
                }
            }
        }

        return base.SaveChanges();
    }

    /// <summary>
    /// Wrap the save changes in a transaction for rollback.
    /// </summary>
    /// <returns></returns>
    public int CommitTransaction()
    {
        using var transaction = Database.BeginTransaction();
        try
        {
            var result = SaveChanges();
            transaction.Commit();
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                var metadataName = entry.Metadata.Name;
                var dbValues = entry.GetDatabaseValues();
                var currentValues = entry.CurrentValues;
                var originalValues = entry.OriginalValues;
                var sb = new StringBuilder();

                foreach (var property in currentValues.Properties)
                {
                    var dbValue = dbValues?[property];
                    var currentValue = currentValues[property];
                    var originalValue = originalValues[property];

                    if (dbValue?.ToString() != originalValue?.ToString() ||
                        dbValue?.ToString() != currentValue?.ToString())
                    {
                        sb.Append($"[{property.Name} - Current: {currentValue}; DB: {dbValue}; Original: {originalValue}]");
                    }
                }

                _logger?.LogError("{metadataName}: {sb}", metadataName, sb);
            }
            throw;
        }
        catch (DbUpdateException)
        {
            transaction.Rollback();
            throw;
        }
    }

    /// <summary>
    /// Deserialize the specified 'json' to the specified type of 'T'.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _serializerOptions);
    }

    /// <summary>
    /// Serialize the specified 'item'.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="item"></param>
    /// <returns></returns>
    public string Serialize<T>(T item)
    {
        return JsonSerializer.Serialize(item, _serializerOptions);
    }
    #endregion
}
