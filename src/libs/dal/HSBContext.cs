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
    private readonly ILoggerFactory? _loggerFactory;
    private readonly ILogger? _logger;
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly JsonSerializerOptions? _serializerOptions;
    #endregion

    #region Properties
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<TenantOrganization> TenantOrganizations => Set<TenantOrganization>();
    public DbSet<OperatingSystemItem> OperatingSystemItems => Set<OperatingSystemItem>();
    public DbSet<FileSystemItem> FileSystemItems => Set<FileSystemItem>();
    public DbSet<FileSystemHistoryItem> FileSystemHistoryItems => Set<FileSystemHistoryItem>();
    public DbSet<ServerItem> ServerItems => Set<ServerItem>();
    public DbSet<ServerHistoryItem> ServerHistoryItems => Set<ServerHistoryItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<UserTenant> UserTenants => Set<UserTenant>();
    public DbSet<UserOrganization> UserOrganizations => Set<UserOrganization>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<GroupRole> GroupRoles => Set<GroupRole>();
    public DbSet<Role> Roles => Set<Role>();
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a SiteContext object, initializes with specified parameters.
    /// </summary>
    /// <param name="loggerFactory"></param>
    protected HSBContext(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger<HSBContext>();
        this.ChangeTracker.LazyLoadingEnabled = false;
    }

    /// <summary>
    /// Creates a new instance of a SiteContext object, initializes with specified parameters.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="serializerOptions"></param>
    /// <param name="loggerFactory"></param>
    public HSBContext(
        DbContextOptions<HSBContext> options,
        IHttpContextAccessor? httpContextAccessor = null,
        IOptions<JsonSerializerOptions>? serializerOptions = null,
        ILoggerFactory? loggerFactory = null)
      : base(options)
    {
        _loggerFactory = loggerFactory;
        _logger = loggerFactory?.CreateLogger<HSBContext>();
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
        base.OnConfiguring(optionsBuilder);
    }

    /// <summary>
    /// Control the database conventions.
    /// </summary>
    /// <param name="configurationBuilder"></param>
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
    }

    /// <summary>
    /// Apply all the configuration settings to the model.
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyAllConfigurations(typeof(AuditableConfiguration<>), this);
        modelBuilder.RemovePluralizingTableNameConvention();

        modelBuilder.HasDbFunction(typeof(HSBContext)
            .GetMethod(
                nameof(FindServerHistoryItemsByMonth),
                [typeof(DateTime), typeof(DateTime?), typeof(int?), typeof(int?), typeof(int?), typeof(string)])!
            ).HasName(nameof(FindServerHistoryItemsByMonth));
        modelBuilder.HasDbFunction(typeof(HSBContext)
            .GetMethod(
                nameof(FindServerHistoryItemsByMonthForUser),
                [typeof(int), typeof(DateTime), typeof(DateTime?), typeof(int?), typeof(int?), typeof(int?), typeof(string)])!
            ).HasName(nameof(FindServerHistoryItemsByMonthForUser));
        modelBuilder.HasDbFunction(typeof(HSBContext)
            .GetMethod(
                nameof(FindFileSystemHistoryItemsByMonth),
                [typeof(DateTime), typeof(DateTime?), typeof(int?), typeof(int?), typeof(int?), typeof(string)])!
            ).HasName(nameof(FindFileSystemHistoryItemsByMonth));
        modelBuilder.HasDbFunction(typeof(HSBContext)
            .GetMethod(
                nameof(FindFileSystemHistoryItemsByMonthForUser),
                [typeof(int), typeof(DateTime), typeof(DateTime?), typeof(int?), typeof(int?), typeof(int?), typeof(string)])!
            ).HasName(nameof(FindFileSystemHistoryItemsByMonthForUser));

        // modelBuilder.Entity<ServerHistoryItemsByMonth>().ToTable((string?)null);
        // modelBuilder.Entity<ServerHistoryItemsByMonthForUser>().ToTable((string?)null);
        // modelBuilder.Entity<FileSystemHistoryItemsByMonth>().ToTable((string?)null);
        // modelBuilder.Entity<FileSystemHistoryItemsByMonthForUser>().ToTable((string?)null);

        // modelBuilder.Ignore<ServerHistoryItemsByMonth>();
        // modelBuilder.Ignore<ServerHistoryItemsByMonthForUser>();
        // modelBuilder.Ignore<FileSystemHistoryItemsByMonth>();
        // modelBuilder.Ignore<FileSystemHistoryItemsByMonthForUser>();

        // modelBuilder.Entity<ServerHistoryItemSmall>().ToView("vServerHistoryItem").HasKey(m => m.Id);
        // modelBuilder.Entity<ServerHistoryItemsByMonthForUser>().ToView("vServerHistoryItem").HasKey(m => m.Id);
        // modelBuilder.Entity<FileSystemHistoryItemsByMonth>().ToView("vFileSystemHistoryItem").HasKey(m => m.Id);
        // modelBuilder.Entity<FileSystemHistoryItemSmall>().ToView("vFileSystemHistoryItem").HasKey(m => m.Id);

        // modelBuilder.Entity<ServerHistoryItemsByMonth>().HasNoKey().ToSqlQuery("SELECT * FROM public.vServerHistoryItem");
        // modelBuilder.Entity<ServerHistoryItemsByMonthForUser>().HasNoKey().ToSqlQuery("SELECT * FROM public.vServerHistoryItem");
        // modelBuilder.Entity<FileSystemHistoryItemsByMonth>().HasNoKey().ToSqlQuery("SELECT * FROM public.vFileSystemHistoryItem");
        // modelBuilder.Entity<FileSystemHistoryItemsByMonthForUser>().HasNoKey().ToSqlQuery("SELECT * FROM public.vFileSystemHistoryItem");

        // modelBuilder.Entity<ServerHistoryItemsByMonth>().ToFunction(nameof(FindServerHistoryItemsByMonth));
        // modelBuilder.Entity<ServerHistoryItemsByMonthForUser>().ToFunction(nameof(FindServerHistoryItemsByMonthForUser));
        // modelBuilder.Entity<FileSystemHistoryItemsByMonth>().ToFunction(nameof(FindFileSystemHistoryItemsByMonth));
        // modelBuilder.Entity<FileSystemHistoryItemsByMonthForUser>().ToFunction(nameof(FindFileSystemHistoryItemsByMonthForUser));
    }

    /// <summary>
    /// Function to find server history items by month.
    /// This will only return a single row for each month.
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="tenantId"></param>
    /// <param name="organizationId"></param>
    /// <param name="operatingSystemId"></param>
    /// <param name="serviceKeyNow"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public IQueryable<ServerHistoryItem> FindServerHistoryItemsByMonth(DateTime startDate, DateTime? endDate, int? tenantId, int? organizationId, int? operatingSystemItemId, string? serviceNowKey)
        => FromExpression(() => FindServerHistoryItemsByMonth(startDate, endDate, tenantId, organizationId, operatingSystemItemId, serviceNowKey));

    /// <summary>
    /// Function to find server history items by month.
    /// This will only return a single row for each month.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="tenantId"></param>
    /// <param name="organizationId"></param>
    /// <param name="operatingSystemId"></param>
    /// <param name="serviceKeyNow"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public IQueryable<ServerHistoryItem> FindServerHistoryItemsByMonthForUser(int userId, DateTime startDate, DateTime? endDate, int? tenantId, int? organizationId, int? operatingSystemItemId, string? serviceNowKey)
        => FromExpression(() => FindServerHistoryItemsByMonthForUser(userId, startDate, endDate, tenantId, organizationId, operatingSystemItemId, serviceNowKey));

    /// <summary>
    /// Function to find file system history items by month.
    /// This will only return a single row for each month.
    /// </summary>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="tenantId"></param>
    /// <param name="organizationId"></param>
    /// <param name="operatingSystemId"></param>
    /// <param name="serverServiceKeyNow"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public IQueryable<FileSystemHistoryItem> FindFileSystemHistoryItemsByMonth(DateTime startDate, DateTime? endDate, int? tenantId, int? organizationId, int? operatingSystemItemId, string? serverServiceNowKey)
        => FromExpression(() => FindFileSystemHistoryItemsByMonth(startDate, endDate, tenantId, organizationId, operatingSystemItemId, serverServiceNowKey));

    /// <summary>
    /// Function to find file system history items by month.
    /// This will only return a single row for each month.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="tenantId"></param>
    /// <param name="organizationId"></param>
    /// <param name="operatingSystemId"></param>
    /// <param name="serverServiceKeyNow"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public IQueryable<FileSystemHistoryItem> FindFileSystemHistoryItemsByMonthForUser(int userId, DateTime startDate, DateTime? endDate, int? tenantId, int? organizationId, int? operatingSystemItemId, string? serverServiceNowKey)
        => FromExpression(() => FindFileSystemHistoryItemsByMonthForUser(userId, startDate, endDate, tenantId, organizationId, operatingSystemItemId, serverServiceNowKey));

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
                    entity.CreatedOn = DateTimeOffset.UtcNow;
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
                    entity.UpdatedOn = DateTimeOffset.UtcNow;
                    entity.Version++;
                }
            }
        }

        return base.SaveChanges(acceptAllChangesOnSuccess);
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
