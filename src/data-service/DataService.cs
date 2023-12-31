using HSB.Core.Extensions;
using ServiceNow = HSB.Models.ServiceNow;
using Hsb = HSB.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace HSB.DataService;

/// <summary>
/// DataService class, provides the service which will connect to ServiceNow
/// </summary>
public class DataService : IDataService
{
    #region Variables
    private readonly Dictionary<string, Hsb.OrganizationModel> _organizations = new();
    private readonly Dictionary<string, Hsb.TenantModel> _tenants = new();
    private readonly Dictionary<string, Hsb.OperatingSystemItemModel> _operatingSystemItems = new();
    private readonly Dictionary<string, Hsb.ServerItemModel> _serverItems = new();
    private readonly List<Hsb.DataSyncModel> _dataSync = new();
    #endregion

    #region Properties
    /// <summary>
    /// get - The logger;
    /// </summary>
    protected ILogger<IDataService> Logger { get; }

    /// <summary>
    /// get - The configuration options for the service.
    /// </summary>
    protected ServiceOptions Options { get; }

    /// <summary>
    /// get - HSB API.
    /// </summary>
    protected IHsbApiService HsbApi { get; }

    /// <summary>
    /// get - Service now API.
    /// </summary>
    protected IServiceNowApiService ServiceNowApi { get; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a DataService object, initializes with specified parameters.
    /// </summary>
    /// <param name="hsbApi"></param>
    /// <param name="serviceNowApi"></param>
    /// <param name="serviceOptions"></param>
    /// <param name="logger"></param>
    public DataService(
        IHsbApiService hsbApi,
        IServiceNowApiService serviceNowApi,
        IOptions<ServiceOptions> serviceOptions,
        ILogger<IDataService> logger)
    {
        this.HsbApi = hsbApi;
        this.ServiceNowApi = serviceNowApi;
        this.Options = serviceOptions.Value;
        this.Logger = logger;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Run the data service.
    /// Fetch records from HSB to determine whether they need to be updated.
    /// Process all configuration items for file systems and servers.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task RunAsync()
    {
        this.Logger.LogInformation("Data Sync Service Started");

        await GetConfiguration();
        await InitLookups();

        // If there is an active data sync, start with it and continue where it left off.
        var dataSyncItems = this.Options.DataSync.Where(o => o.IsEnabled).OrderBy(o => o.SortOrder).ThenBy(o => o.Id).ToList();
        var index = dataSyncItems.FindIndex(o => o.IsActive);
        if (index == -1) index = 0;

        for (var i = index; i < dataSyncItems.Count; i++)
        {
            var dataSync = dataSyncItems[i];
            if (dataSync.Id != 0)
            {
                // Make this data sync active.
                dataSync.IsActive = true;
                var updated = await this.HsbApi.UpdateDataSyncAsync(dataSync) ?? throw new InvalidOperationException($"Failed to return data sync from HSB: {dataSync.Name}");
                dataSync.Version = updated.Version;
            }

            await ProcessConfigurationItemsAsync(dataSync);

            if (dataSync.Id != 0)
            {
                // Reset the current offset.
                dataSync.IsActive = false;
                dataSync.Offset = 0;
                var updated = await this.HsbApi.UpdateDataSyncAsync(dataSync) ?? throw new InvalidOperationException($"Failed to return data sync from HSB: {dataSync.Name}");
                dataSync.Version = updated.Version;
            }
        }

        this.Logger.LogInformation("Data Sync Service Completed");
    }

    private async Task GetConfiguration()
    {
        // If the configuration specifies which data syncs to run, use them.
        // Otherwise fetch all enable data syncs.
        if (this.Options.DataSync.Any())
        {
            await this.Options.DataSync.ForEachAsync(async (option) =>
            {
                if (!String.IsNullOrWhiteSpace(option.Name))
                {
                    var ds = await this.HsbApi.GetDataSyncAsync(option.Name);
                    if (ds != null)
                    {
                        _dataSync.Add(ds);
                        option.Id = ds.Id;
                        option.Name = ds.Name;
                        option.Offset = ds.Offset;
                        option.Query = ds.Query;
                        option.IsEnabled = ds.IsEnabled;
                        option.IsActive = ds.IsActive;
                        option.Version = ds.Version;
                    }
                }
            });
        }
        else
        {
            var options = await this.HsbApi.FetchDataSyncAsync();
            this.Options.DataSync = options.ToArray();
        }
    }

    private async Task InitLookups()
    {
        var tenants = await this.HsbApi.FetchTenantsAsync();
        tenants.ForEach(t => _tenants.Add(t.ServiceNowKey, t));

        var organizations = await this.HsbApi.FetchOrganizationsAsync();
        organizations.ForEach(o => _organizations.Add(o.ServiceNowKey, o));

        var operatingSystems = await this.HsbApi.FetchOperatingSystemItemsAsync();
        operatingSystems.ForEach(os => _operatingSystemItems.Add(os.ServiceNowKey, os));

        var servers = await this.HsbApi.FetchServerItemsAsync();
        servers.ForEach(s => _serverItems.Add(s.ServiceNowKey, s));
    }

    /// <summary>
    /// Add a new configuration item and related data to HSB.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    private async Task ProcessConfigurationItemsAsync(Models.DataSyncModel option)
    {
        var limit = this.ServiceNowApi.Options.Limit;
        var offset = option.Offset;
        var query = !String.IsNullOrWhiteSpace(option.Query) ? option.Query : "";
        var keepGoing = true;

        while (keepGoing)
        {
            var configurationItems = await this.ServiceNowApi.FetchTableItemsAsync<ServiceNow.ConfigurationItemModel>(this.ServiceNowApi.Options.TableNames.ConfigurationItem, limit, offset, query);

            // Iterate over configurations items and send them to HSB API.
            foreach (var configurationItemSN in configurationItems)
            {
                if (configurationItemSN.Data == null) continue;

                // Extract the tableName from the configuration item.
                var tableName = configurationItemSN.Data.ClassName;
                if (String.IsNullOrWhiteSpace(tableName))
                {
                    this.Logger.LogError("Configuration class name is missing: {id}", configurationItemSN.Data.Id);
                    continue;
                }

                if (tableName == this.ServiceNowApi.Options.TableNames.FileSystem)
                {
                    // Get the specific type of item this configuration is for.
                    var itemSN = await this.ServiceNowApi.GetTableItemAsync<ServiceNow.FileSystemModel>(tableName, configurationItemSN.Data.Id);
                    if (itemSN == null)
                    {
                        this.Logger.LogError("Configuration file system item is missing: {tableName}:{id}", tableName, configurationItemSN.Data.Id);
                        continue;
                    }

                    await ProcessFileSystemItemAsync(itemSN, configurationItemSN);
                }
                else
                {
                    // Get the specific type of item this configuration is for.
                    var itemSN = await this.ServiceNowApi.GetTableItemAsync<ServiceNow.BaseItemModel>(tableName, configurationItemSN.Data.Id);
                    if (itemSN == null)
                    {
                        this.Logger.LogError("Configuration table item is missing: {tableName}:{id}", tableName, configurationItemSN.Data.Id);
                        continue;
                    }

                    await ProcessServerItemAsync(itemSN, configurationItemSN);
                }

                // Update the current offset
                if (option.Id != 0)
                {
                    // TODO: This is a really noisy update.
                    option.Offset++;
                    var update = await this.HsbApi.UpdateDataSyncAsync(option) ?? throw new InvalidOperationException($"Failed to return data sync from HSB: {option.Name}");
                    option.Version = update.Version;
                }
            }

            // We assume that if the results contain the limit, we need to make another request for more.
            if (configurationItems.Count() < limit) keepGoing = false;
            offset += limit;
        }
    }

    /// <summary>
    /// Add the server item to HSB if it hasn't been added already, or update if required.
    /// Also add a server history item.
    /// </summary>
    /// <param name="serverItemSN"></param>
    /// <param name="configurationItemSN"></param>
    /// <returns></returns>
    private async Task<Hsb.ServerItemModel?> ProcessServerItemAsync(
        ServiceNow.ResultModel<ServiceNow.BaseItemModel> serverItemSN,
        ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> configurationItemSN)
    {
        if (serverItemSN.Data == null) throw new ArgumentNullException(nameof(serverItemSN));
        if (configurationItemSN.Data == null) throw new ArgumentNullException(nameof(configurationItemSN));

        var serviceNowKey = serverItemSN.Data.Id;
        var tenant = await ProcessTenantAsync(configurationItemSN.RawData, serverItemSN.RawData);
        var organization = await ProcessOrganizationAsync(configurationItemSN.RawData, serverItemSN.RawData);
        if (organization == null)
        {
            this.Logger.LogError("Server item is missing organization: {key}", serviceNowKey);
            return null;
        }
        var operatingSystem = await ProcessOperatingSystemAsync(configurationItemSN.RawData, serverItemSN.RawData);

        if (!_serverItems.TryGetValue(serviceNowKey, out Hsb.ServerItemModel? serverItem))
        {
            // Add the server item to HSB.
            this.Logger.LogDebug("Add Server Item: '{id}'", configurationItemSN.Data?.Id);
            serverItem = await this.HsbApi.AddServerItemAsync(new Hsb.ServerItemModel(tenant?.Id, organization.Id, operatingSystem?.Id, serverItemSN, configurationItemSN));
            if (serverItem == null)
            {
                this.Logger.LogError("Server Item was not returned from HSB: {id}", serviceNowKey);
                return null;
            }
            _serverItems.Add(serverItem.ServiceNowKey, serverItem);
        }
        else if (serverItem.UpdatedOn.AddDays(1) < DateTime.UtcNow) // Don't update records less than a day old.
        {
            // Update the server item in HSB.
            this.Logger.LogDebug("Update Server Item: '{id}'", configurationItemSN.Data?.Id);
            serverItem.TenantId = tenant?.Id;
            serverItem.OrganizationId = organization.Id;
            serverItem.OperatingSystemItemId = operatingSystem?.Id;

            serverItem.RawData = serverItemSN.RawData;
            serverItem.RawDataCI = configurationItemSN.RawData;

            serverItem.ClassName = serverItemSN.Data.ClassName ?? "";
            serverItem.Name = serverItemSN.Data.Name ?? "";
            serverItem.Category = serverItemSN.Data.Category ?? "";
            serverItem.Subcategory = serverItemSN.Data.Subcategory ?? "";
            serverItem.DnsDomain = serverItemSN.Data.DnsDomain ?? "";
            serverItem.Platform = serverItemSN.Data.Platform ?? "";
            serverItem.IPAddress = serverItemSN.Data.IPAddress ?? "";
            serverItem.FQDN = serverItemSN.Data.FQDN ?? "";

            serverItem = await this.HsbApi.UpdateServerItemAsync(serverItem);
            if (serverItem == null)
            {
                this.Logger.LogError("Server Item was not returned from HSB: {id}", serviceNowKey);
                return null;
            }
            _serverItems[serverItem.ServiceNowKey] = serverItem;
        }

        return serverItem;
    }

    /// <summary>
    /// Add the file system item in HSB, or update it if required.
    /// Also add the file system history item to HSB.
    /// Also add server item if it doesn't already exist.
    /// </summary>
    /// <param name="fileSystemItemSN"></param>
    /// <param name="configurationItemSN"></param>
    /// <returns></returns>
    private async Task<Hsb.FileSystemItemModel?> ProcessFileSystemItemAsync(
        ServiceNow.ResultModel<ServiceNow.FileSystemModel> fileSystemItemSN,
        ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> configurationItemSN)
    {
        if (fileSystemItemSN.Data == null) throw new ArgumentNullException(nameof(fileSystemItemSN));
        if (configurationItemSN.Data == null) throw new ArgumentNullException(nameof(configurationItemSN));

        var serviceNowKey = fileSystemItemSN.Data.Id;

        // Server does not currently exist, add it.
        if (!_serverItems.TryGetValue(serviceNowKey, out Hsb.ServerItemModel? serverItem))
        {
            var computerKey = configurationItemSN.RawData.GetElementValue<string>(".computer.value")
                ?? configurationItemSN.RawData.GetElementValue<string>(".computer")
                ?? fileSystemItemSN.RawData.GetElementValue<string>(".computer.value")
                ?? fileSystemItemSN.RawData.GetElementValue<string>(".computer");
            if (String.IsNullOrWhiteSpace(computerKey))
            {
                this.Logger.LogWarning("File System Item does not have a Computer: '{id}'", serviceNowKey);
                return null;
            }

            // Get the configuration item for this linked computer.
            var serverConfigurationItemSN = await this.ServiceNowApi.GetTableItemAsync<ServiceNow.ConfigurationItemModel>(this.ServiceNowApi.Options.TableNames.ConfigurationItem, computerKey);
            if (serverConfigurationItemSN == null)
            {
                this.Logger.LogError("Configuration item does not exist for file system: {id}", serviceNowKey);
                return null;
            }

            var tableName = serverConfigurationItemSN.RawData.GetElementValue<string>(".sys_class_name");
            if (String.IsNullOrWhiteSpace(tableName))
            {
                this.Logger.LogError("Configuration class name is missing: {id}", serverConfigurationItemSN);
                return null;
            }

            // Get the server item for the current file system from ServiceNow.
            var serverItemSN = await this.ServiceNowApi.GetTableItemAsync<ServiceNow.BaseItemModel>(tableName, computerKey);
            if (serverItemSN == null)
            {
                this.Logger.LogError("Server Item was not returned from ServiceNow: {id}", computerKey);
                return null;
            }

            // Add the server item to HSB and also process tenant/organization/operating system.
            serverItem = await ProcessServerItemAsync(serverItemSN, configurationItemSN);
            if (serverItem == null)
            {
                this.Logger.LogError("Server Item was not returned from HSB: {id}", computerKey);
                return null;
            }
        }

        // Check if file system item exists in HSB.
        // TODO: This is noisy, but we don't want to keep all of them in memory.
        var fileSystemItem = await this.HsbApi.GetFileSystemItemAsync(serviceNowKey);
        if (fileSystemItem == null)
        {
            // Add the server item to HSB.
            this.Logger.LogDebug("Add File System Item: '{id}'", configurationItemSN.Data?.Id);
            fileSystemItem = await this.HsbApi.AddFileSystemItemAsync(new Hsb.FileSystemItemModel(serverItem.ServiceNowKey, fileSystemItemSN, configurationItemSN));
        }
        else if (fileSystemItem.UpdatedOn.AddDays(1) < DateTime.UtcNow) // Don't update records less than a day old.
        {
            // Update the server item to HSB.
            this.Logger.LogDebug("Update File System Item: '{id}'", configurationItemSN.Data?.Id);

            fileSystemItem.RawData = fileSystemItemSN.RawData;
            fileSystemItem.RawDataCI = configurationItemSN.RawData;

            fileSystemItem.ServerItemServiceNowKey = serverItem.ServiceNowKey;

            fileSystemItem.ClassName = fileSystemItemSN.Data.ClassName ?? "";
            fileSystemItem.Name = fileSystemItemSN.Data.Name ?? "";
            fileSystemItem.Label = fileSystemItemSN.Data.Label ?? "";
            fileSystemItem.Category = fileSystemItemSN.Data.Category ?? "";
            fileSystemItem.Subcategory = fileSystemItemSN.Data.Subcategory ?? "";
            fileSystemItem.StorageType = fileSystemItemSN.Data.StorageType ?? "";
            fileSystemItem.MediaType = fileSystemItemSN.Data.MediaType ?? "";
            fileSystemItem.VolumeId = fileSystemItemSN.Data.VolumeId ?? "";
            fileSystemItem.Capacity = !String.IsNullOrWhiteSpace(fileSystemItemSN.Data.Capacity) ? Int32.Parse(fileSystemItemSN.Data.Capacity) : 0;
            fileSystemItem.DiskSpace = !String.IsNullOrWhiteSpace(fileSystemItemSN.Data.DiskSpace) ? float.Parse(fileSystemItemSN.Data.DiskSpace) : 0;
            fileSystemItem.Size = fileSystemItemSN.Data.Size ?? "";
            fileSystemItem.SizeBytes = !String.IsNullOrWhiteSpace(fileSystemItemSN.Data.SizeBytes) ? long.Parse(fileSystemItemSN.Data.SizeBytes) : 0;
            fileSystemItem.UsedSizeBytes = !String.IsNullOrWhiteSpace(fileSystemItemSN.Data.UsedSizeBytes) ? long.Parse(fileSystemItemSN.Data.UsedSizeBytes) : 0;
            fileSystemItem.AvailableSpace = !String.IsNullOrWhiteSpace(fileSystemItemSN.Data.AvailableSpace) ? Int32.Parse(fileSystemItemSN.Data.AvailableSpace) : 0;
            fileSystemItem.FreeSpace = fileSystemItemSN.Data.FreeSpace ?? "";
            fileSystemItem.FreeSpaceBytes = !String.IsNullOrWhiteSpace(fileSystemItemSN.Data.FreeSpaceBytes) ? long.Parse(fileSystemItemSN.Data.FreeSpaceBytes) : 0;

            fileSystemItem = await this.HsbApi.UpdateFileSystemItemAsync(fileSystemItem);
        }

        return fileSystemItem;
    }

    /// <summary>
    /// Add or update a tenant in HSB.
    /// </summary>
    /// <param name="configurationItemSN"></param>
    /// <param name="itemSN"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.TenantModel?> ProcessTenantAsync(
        JsonDocument configurationItemSN,
        JsonDocument itemSN)
    {
        var serviceNowKey = configurationItemSN.GetElementValue<string>(".sys_id");
        this.Logger.LogDebug("Processing Tenant for Configuration Item: '{id}'", serviceNowKey);

        var tenantKey = configurationItemSN.GetElementValue<string>(".u_tenant.value")
            ?? configurationItemSN.GetElementValue<string>(".u_tenant")
            ?? itemSN.GetElementValue<string>(".u_tenant.value")
            ?? itemSN.GetElementValue<string>(".u_tenant");
        if (String.IsNullOrWhiteSpace(tenantKey))
        {
            this.Logger.LogWarning("Configuration Item does not have a Tenant: '{id}'", serviceNowKey);
            return null;
        }

        // If HSB doesn't have the tenant, add it, otherwise update
        if (!_tenants.TryGetValue(tenantKey, out Hsb.TenantModel? tenant))
        {
            this.Logger.LogInformation("Adding tenant '{id}'", tenantKey);

            tenant = await GetTenantAsync(tenantKey);
            if (tenant == null)
            {
                this.Logger.LogError("Tenant was not returned from Servicenow: {id}", tenantKey);
                return null;
            }

            tenant = await this.HsbApi.AddTenantAsync(tenant);
            if (tenant == null)
            {
                this.Logger.LogError("Tenant was not returned from HSB: {id}", tenantKey);
                return null;
            }
            _tenants.Add(tenantKey, tenant);
        }
        else if (tenant.UpdatedOn.AddDays(1) < DateTime.UtcNow)
        {
            this.Logger.LogInformation("Updating tenant '{id}'", tenant.ServiceNowKey);

            var update = await GetTenantAsync(tenantKey);
            if (update == null)
            {
                this.Logger.LogError("Tenant was not returned from Servicenow: {id}", tenantKey);
                return null;
            }
            update.Id = tenant.Id;
            update.Version = tenant.Version;
            tenant = await this.HsbApi.UpdateTenantAsync(update);
            if (tenant == null)
            {
                this.Logger.LogError("Tenant was not returned from HSB: {id}", tenantKey);
                return null;
            }
            _tenants[tenantKey] = tenant;
        }
        else
            this.Logger.LogDebug("Tenant data was updated less than a day ago: {id}", tenantKey);

        return tenant;
    }

    /// <summary>
    /// Get the tenant from ServiceNow and extract the related organizations.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.TenantModel?> GetTenantAsync(string key)
    {
        // Fetch tenant information.
        var tenantSN = await this.ServiceNowApi.GetTableItemAsync<ServiceNow.TenantModel>(this.ServiceNowApi.Options.TableNames.Tenant, key);
        if (tenantSN == null || tenantSN.Data == null)
            return null;

        var organizations = new List<Hsb.OrganizationModel>();

        // Tenants can be related to more than one organization.
        if (!String.IsNullOrWhiteSpace(tenantSN.Data.ClientOrganizations))
        {
            var values = tenantSN.Data.ClientOrganizations.Split(",");
            foreach (var organizationKey in values)
            {
                // If HSB has the organization already, use it, otherwise fetch it from Service Now.
                if (!_organizations.TryGetValue(organizationKey, out Hsb.OrganizationModel? organization))
                {
                    var organizationSN = await this.ServiceNowApi.GetTableItemAsync<ServiceNow.ClientOrganizationModel>(this.ServiceNowApi.Options.TableNames.ClientOrganization, organizationKey);
                    if (organizationSN == null || organizationSN.Data == null) throw new InvalidOperationException($"Organization cannot be null: '{organizationKey}'");
                    organization = await this.HsbApi.AddOrganizationAsync(new Hsb.OrganizationModel(organizationSN));
                    if (organization == null)
                    {
                        this.Logger.LogError("Organization was not returned from HSB: {id}", organizationKey);
                        return null;
                    }
                    _organizations.Add(organizationKey, organization);
                }
                organizations.Add(organization);
            }
        }

        return new Hsb.TenantModel(tenantSN, organizations);
    }

    /// <summary>
    /// Add or update an organization in HSB.
    /// </summary>
    /// <param name="configurationItemSN"></param>
    /// <param name="itemSN"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.OrganizationModel?> ProcessOrganizationAsync(
        JsonDocument configurationItemSN,
        JsonDocument itemSN)
    {
        var serviceNowKey = configurationItemSN.GetElementValue<string>(".sys_id");
        this.Logger.LogDebug("Processing Organization for Configuration Item: '{id}'", serviceNowKey);

        var organizationKey = configurationItemSN.GetElementValue<string>(".u_client_organization.value")
            ?? configurationItemSN.GetElementValue<string>(".u_client_organization")
            ?? itemSN.GetElementValue<string>(".u_client_organization.value")
            ?? itemSN.GetElementValue<string>(".u_client_organization");
        if (String.IsNullOrWhiteSpace(organizationKey))
        {
            this.Logger.LogWarning("Configuration Item does not have an Organization: '{id}'", serviceNowKey);
            return null;
        }

        // If HSB doesn't have the organization, add it, otherwise update
        if (!_organizations.TryGetValue(organizationKey, out Hsb.OrganizationModel? organization))
        {
            this.Logger.LogInformation("Adding organization '{id}'", organizationKey);

            // Fetch organization information.
            var organizationSN = await this.ServiceNowApi.GetTableItemAsync<ServiceNow.ClientOrganizationModel>(this.ServiceNowApi.Options.TableNames.ClientOrganization, organizationKey);
            if (organizationSN == null || organizationSN.Data == null)
            {
                this.Logger.LogError("Organization was not returned from Servicenow: {id}", organizationKey);
                return null;
            }

            organization = await this.HsbApi.AddOrganizationAsync(new Hsb.OrganizationModel(organizationSN));
            if (organization == null)
            {
                this.Logger.LogError("Organization was not returned from HSB: {id}", organizationKey);
                return null;
            }

            _organizations.Add(organizationKey, organization);
        }
        else if (organization.UpdatedOn.AddDays(1) < DateTime.UtcNow)
        {
            this.Logger.LogInformation("Updating organization '{id}'", organization.ServiceNowKey);

            // Fetch organization information.
            var organizationSN = await this.ServiceNowApi.GetTableItemAsync<ServiceNow.ClientOrganizationModel>(this.ServiceNowApi.Options.TableNames.ClientOrganization, organizationKey);
            if (organizationSN == null || organizationSN.Data == null)
            {
                this.Logger.LogError("Organization was not returned from Servicenow: {id}", organizationKey);
                return null;
            }

            var update = new Hsb.OrganizationModel(organizationSN)
            {
                Id = organization.Id,
                Version = organization.Version,
            };
            organization = await this.HsbApi.UpdateOrganizationAsync(update);
            if (organization == null)
            {
                this.Logger.LogError("Organization was not returned from HSB: {id}", organizationKey);
                return null;
            }
            _organizations[organizationKey] = organization;
        }
        else
            this.Logger.LogDebug("Organization data has not changed '{id}", organization.ServiceNowKey);

        return organization;
    }

    /// <summary>
    /// Add or update the operating system in HSB.
    /// </summary>
    /// <param name="configurationItemSN"></param>
    /// <param name="itemSN"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.OperatingSystemItemModel?> ProcessOperatingSystemAsync(
        JsonDocument configurationItemSN,
        JsonDocument itemSN)
    {
        var serviceNowKey = configurationItemSN.GetElementValue<string>(".sys_id");
        this.Logger.LogDebug("Processing Operating System for Configuration Item: '{id}'", serviceNowKey);

        var operatingSystemKey = configurationItemSN.GetElementValue<string>(".u_operating_system.value")
            ?? configurationItemSN.GetElementValue<string>(".u_operating_system")
            ?? itemSN.GetElementValue<string>(".u_operating_system.value")
            ?? itemSN.GetElementValue<string>(".u_operating_system");
        if (String.IsNullOrWhiteSpace(operatingSystemKey))
        {
            this.Logger.LogWarning("Configuration Item does not have an Operating System: '{id}'", serviceNowKey);
            return null;
        }

        // If HSB doesn't have the Operating System, add it.
        if (!_operatingSystemItems.TryGetValue(operatingSystemKey, out Hsb.OperatingSystemItemModel? operatingSystem))
        {
            this.Logger.LogInformation("Adding Operating System '{id}'", operatingSystemKey);

            // Fetch Operating System information.
            var operatingSystemSN = await this.ServiceNowApi.GetTableItemAsync<ServiceNow.OperatingSystemModel>(this.ServiceNowApi.Options.TableNames.OperatingSystem, operatingSystemKey);
            if (operatingSystemSN == null || operatingSystemSN.Data == null)
            {
                this.Logger.LogError("Operating System Item was not returned from Servicenow: {id}", operatingSystemKey);
                return null;
            }

            operatingSystem = await this.HsbApi.AddOperatingSystemItemAsync(new Hsb.OperatingSystemItemModel(operatingSystemSN));
            if (operatingSystem == null)
            {
                this.Logger.LogError("Operating System Item was not returned from HSB: {id}", operatingSystemKey);
                return null;
            }
            _operatingSystemItems.Add(operatingSystemKey, operatingSystem);
        }

        return operatingSystem;
    }

    #endregion
}

