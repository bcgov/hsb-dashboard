using HSB.Core.Extensions;
using ServiceNow = HSB.Models.ServiceNow;
using Hsb = HSB.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HSB.DataService;

/// <summary>
/// DataService class, provides the service which will connect to ServiceNow
/// </summary>
public class DataService : IDataService
{
    #region Variables
    private readonly List<Hsb.OrganizationModel> _organizations = new();
    private readonly List<Hsb.TenantModel> _tenants = new();
    private readonly List<Hsb.OperatingSystemItemModel> _operatingSystemItems = new();
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
        if (this.Options.DataSync.Any())
        {
            await this.Options.DataSync.ForEachAsync(async (option) =>
            {
                if (!String.IsNullOrWhiteSpace(option.Name))
                {
                    var ds = await this.HsbApi.GetDataSync(option.Name);
                    if (ds != null)
                    {
                        _dataSync.Add(ds);
                        option.DataType = ds.DataType;
                        option.Offset = ds.Offset;
                        option.Query = ds.Query;
                        option.Model = ds;
                    }
                }
            });
        }
        _tenants.AddRange(await this.HsbApi.FetchTenantsAsync());
        _organizations.AddRange(await this.HsbApi.FetchOrganizationsAsync());
        _operatingSystemItems.AddRange(await this.HsbApi.FetchOperatingSystemItemsAsync());

        foreach (var dataSync in this.Options.DataSync)
        {
            if (dataSync.DataType == Entities.ServiceNowDataType.Server)
                await ProcessServersAsync(dataSync);
            else if (dataSync.DataType == Entities.ServiceNowDataType.FileSystem)
                await ProcessFileSystemItemsAsync(dataSync);

            // Reset the current offset.
            if (dataSync.Model != null)
            {
                dataSync.Model.Offset = 0;
                dataSync.Model = await this.HsbApi.UpdateDataSync(dataSync.Model);
            }
        }
    }

    /// <summary>
    /// Add a new configuration item and server in HSB.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    private async Task ProcessServersAsync(DataSyncOptions option)
    {
        var limit = this.ServiceNowApi.Options.Limit;
        var offset = option.Offset;
        var query = !String.IsNullOrWhiteSpace(option.Query) ? option.Query : "";
        var keepGoing = true;

        while (keepGoing)
        {
            var serverItems = await this.ServiceNowApi.FetchServerItemsAsync(limit, offset, query);
            // var configurationItems = await this.ServiceNowApi.FetchConfigurationItemsAsync(limit, offset, query);

            // Iterate over server items and send them to HSB API.
            foreach (var model in serverItems)
            {
                if (model.Data == null) continue;

                var configurationItemSN = await this.ServiceNowApi.GetConfigurationItemAsync(model.Data.Id);
                if (configurationItemSN == null)
                    this.Logger.LogWarning("Configuration Item is missing: '{id}'", model.Data.Id);

                var tenant = await ProcessTenantAsync(model) ?? (configurationItemSN != null ? await ProcessTenantAsync(configurationItemSN) : null);
                var organization = await ProcessOrganizationAsync(model) ?? (configurationItemSN != null ? await ProcessOrganizationAsync(configurationItemSN) : null);
                if (tenant == null && organization == null)
                    this.Logger.LogWarning("Configuration Item is missing Tenant and Organization information: '{id}'", model.Data.Id);

                var configurationItem = configurationItemSN != null ? await AddConfigurationItemAsync(configurationItemSN, tenant, organization) : null;

                var server = await AddServerAsync(model, configurationItem?.Id);
                if (!String.IsNullOrWhiteSpace(server.OperatingSystemKey))
                    await ProcessOperatingSystemAsync(server);
                else
                    this.Logger.LogWarning("Server does not reference an Operating System: '{id}'", server.ServiceNowKey);

                // Update the current offset
                if (option.Model != null)
                {
                    // TODO: This is a really noisy update.
                    option.Model.Offset = option.Model.Offset + 1;
                    option.Model = await this.HsbApi.UpdateDataSync(option.Model);
                }
            }

            // We assume that if the results contain the limit, we need to make another request for more.
            if (serverItems == null || serverItems.Count() < limit) keepGoing = false;
            offset += limit;
        }
    }

    /// <summary>
    /// Add a new configuration item and file system in HSB.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    private async Task ProcessFileSystemItemsAsync(DataSyncOptions option)
    {
        var limit = this.ServiceNowApi.Options.Limit;
        var offset = option.Offset;
        var query = !String.IsNullOrWhiteSpace(option.Query) ? option.Query : $"sys_class_name={this.ServiceNowApi.Options.TableNames.FileSystem}";
        var keepGoing = true;

        while (keepGoing)
        {
            var configurationItems = await this.ServiceNowApi.FetchConfigurationItemsAsync(limit, offset, query);

            // Iterate over configuration items and send them to HSB API.
            foreach (var model in configurationItems)
            {
                var tenant = await ProcessTenantAsync(model);
                var organization = await ProcessOrganizationAsync(model);
                if (tenant != null || organization != null)
                {
                    var ci = await AddConfigurationItemAsync(model, tenant, organization);
                    await AddFileSystemAsync(ci);
                }
                else
                    this.Logger.LogWarning("Configuration Item is missing Tenant and Organization information: '{id}'", model.Data?.Id);

                // Update the current offset
                if (option.Model != null)
                {
                    // TODO: This is a really noisy update.
                    option.Model.Offset = option.Model.Offset + 1;
                    option.Model = await this.HsbApi.UpdateDataSync(option.Model);
                }
            }

            // We assume that if the results contain the limit, we need to make another request for more.
            if (configurationItems == null || configurationItems.Count() < limit) keepGoing = false;
            offset += limit;
        }
    }

    /// <summary>
    /// Add or update a tenant in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.TenantModel?> ProcessTenantAsync(ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> model)
    {
        this.Logger.LogDebug("Processing Tenant for Configuration Item: '{id}'", model.Data?.Id);

        if (model.Data == null) throw new InvalidOperationException("Configuration Item information cannot be null");

        var tenantKey = model.RawData.GetElementValue<string>(".u_tenant.value") ?? model.RawData.GetElementValue<string>(".u_tenant");
        if (String.IsNullOrWhiteSpace(tenantKey))
        {
            this.Logger.LogWarning("Configuration Item does not have a Tenant: '{id}'", model.Data.Id);
            return null;
        }

        // Fetch tenant information.
        var tenantSN = await this.ServiceNowApi.GetTenantAsync(tenantKey);
        if (tenantSN == null || tenantSN.Data == null) throw new InvalidOperationException($"Tenant cannot be null: '{tenantKey}'");

        var organizations = new List<Hsb.OrganizationModel>();
        var index = _tenants.FindIndex(t => t.ServiceNowKey == tenantSN.Data.Id);

        // Tenants can be related to more than one organization.
        if (!String.IsNullOrWhiteSpace(tenantSN.Data.ClientOrganizations))
        {
            var values = tenantSN.Data.ClientOrganizations.Split(",");
            foreach (var organizationKey in values)
            {
                // If HSB has the organization already, use it, otherwise fetch it from Service Now.
                var organization = _organizations.FirstOrDefault(o => o.ServiceNowKey == organizationKey);
                if (organization == null)
                {
                    var organizationSN = await this.ServiceNowApi.GetClientOrganizationAsync(organizationKey);
                    if (organizationSN == null || organizationSN.Data == null) throw new InvalidOperationException($"Organization cannot be null: '{organizationKey}'");
                    organization = await this.HsbApi.AddOrganizationAsync(new Hsb.OrganizationModel(organizationSN)) ?? throw new InvalidOperationException($"Failed to return a new Organization: '{organizationKey}'");
                    _organizations.Add(organization);
                }
                organizations.Add(organization);
            }
        }

        var tenant = index > -1 ? _tenants[index] : new Hsb.TenantModel(tenantSN, organizations);

        // If HSB doesn't have the tenant, add it, otherwise update
        if (tenant.Id == 0)
        {
            this.Logger.LogInformation("Adding tenant '{id}'", tenant.ServiceNowKey);
            tenant = await this.HsbApi.AddTenantAsync(tenant) ?? throw new InvalidOperationException($"Failed to return a new Tenant: '{tenant.ServiceNowKey}'");
            _tenants.Add(tenant);
        }
        else if (tenantSN.Data.UpdatedOn != tenant.RawData?.GetElementValue<string>(".sys_updated_on"))
        {
            this.Logger.LogInformation("Updating tenant '{id}'", tenant.ServiceNowKey);
            var update = new Hsb.TenantModel(tenantSN, organizations)
            {
                Id = tenant.Id,
                Version = tenant.Version
            };
            tenant = await this.HsbApi.UpdateTenantAsync(update) ?? throw new InvalidOperationException($"Failed to return an updated Tenant: '{tenant.ServiceNowKey}'");
            _tenants[index] = tenant;
        }
        else
            this.Logger.LogDebug("Tenant data has not changed '{id}", tenant.ServiceNowKey);

        return tenant;
    }
    /// <summary>
    /// Add or update a tenant in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.TenantModel?> ProcessTenantAsync(ServiceNow.ResultModel<ServiceNow.ServerModel> model)
    {
        this.Logger.LogDebug("Processing Tenant for Server Item: '{id}'", model.Data?.Id);

        if (model.Data == null) throw new InvalidOperationException("Server Item information cannot be null");

        var tenantKey = model.RawData.GetElementValue<string>(".u_tenant.value") ?? model.RawData.GetElementValue<string>(".u_tenant");
        if (String.IsNullOrWhiteSpace(tenantKey))
        {
            this.Logger.LogWarning("Server Item does not have a Tenant: '{id}'", model.Data.Id);
            return null;
        }

        // Fetch tenant information.
        var tenantSN = await this.ServiceNowApi.GetTenantAsync(tenantKey);
        if (tenantSN == null || tenantSN.Data == null) throw new InvalidOperationException($"Tenant cannot be null: '{tenantKey}'");

        var organizations = new List<Hsb.OrganizationModel>();
        var index = _tenants.FindIndex(t => t.ServiceNowKey == tenantSN.Data.Id);

        // Tenants can be related to more than one organization.
        if (!String.IsNullOrWhiteSpace(tenantSN.Data.ClientOrganizations))
        {
            var values = tenantSN.Data.ClientOrganizations.Split(",");
            foreach (var organizationKey in values)
            {
                // If HSB has the organization already, use it, otherwise fetch it from Service Now.
                var organization = _organizations.FirstOrDefault(o => o.ServiceNowKey == organizationKey);
                if (organization == null)
                {
                    var organizationSN = await this.ServiceNowApi.GetClientOrganizationAsync(organizationKey);
                    if (organizationSN == null || organizationSN.Data == null) throw new InvalidOperationException($"Organization cannot be null: '{organizationKey}'");
                    organization = await this.HsbApi.AddOrganizationAsync(new Hsb.OrganizationModel(organizationSN)) ?? throw new InvalidOperationException($"Failed to return a new Organization: '{organizationKey}'");
                    _organizations.Add(organization);
                }
                organizations.Add(organization);
            }
        }

        var tenant = index > -1 ? _tenants[index] : new Hsb.TenantModel(tenantSN, organizations);

        // If HSB doesn't have the tenant, add it, otherwise update
        if (tenant.Id == 0)
        {
            this.Logger.LogInformation("Adding tenant '{id}'", tenant.ServiceNowKey);
            tenant = await this.HsbApi.AddTenantAsync(tenant) ?? throw new InvalidOperationException($"Failed to return a new Tenant: '{tenant.ServiceNowKey}'");
            _tenants.Add(tenant);
        }
        else if (tenantSN.Data.UpdatedOn != tenant.RawData?.GetElementValue<string>(".sys_updated_on"))
        {
            this.Logger.LogInformation("Updating tenant '{id}'", tenant.ServiceNowKey);
            var update = new Hsb.TenantModel(tenantSN, organizations)
            {
                Id = tenant.Id,
                Version = tenant.Version
            };
            tenant = await this.HsbApi.UpdateTenantAsync(update) ?? throw new InvalidOperationException($"Failed to return an updated Tenant: '{tenant.ServiceNowKey}'");
            _tenants[index] = tenant;
        }
        else
            this.Logger.LogDebug("Tenant data has not changed '{id}", tenant.ServiceNowKey);

        return tenant;
    }

    /// <summary>
    /// Add or update an organization in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.OrganizationModel?> ProcessOrganizationAsync(ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> model)
    {
        this.Logger.LogDebug("Processing Organization for Configuration Item: '{id}'", model.Data?.Id);

        if (model.Data == null) throw new InvalidOperationException("Configuration Item information cannot be null");

        var organizationKey = model.RawData.GetElementValue<string>(".u_client_organization.value") ?? model.RawData.GetElementValue<string>(".u_client_organization");
        if (String.IsNullOrWhiteSpace(organizationKey))
        {
            this.Logger.LogWarning("Configuration Item does not have an Organization: '{id}'", model.Data.Id);
            return null;
        }

        // Fetch organization information.
        var organizationSN = await this.ServiceNowApi.GetClientOrganizationAsync(organizationKey);
        if (organizationSN == null || organizationSN.Data == null) throw new InvalidOperationException($"Organization cannot be null: '{organizationKey}'");

        var index = _organizations.FindIndex(t => t.ServiceNowKey == organizationSN.Data.Id);
        var organization = index > -1 ? _organizations[index] : new Hsb.OrganizationModel(organizationSN);

        // If HSB doesn't have the organization, add it, otherwise update
        if (organization.Id == 0)
        {
            this.Logger.LogInformation("Adding organization '{id}'", organization.ServiceNowKey);
            organization = await this.HsbApi.AddOrganizationAsync(organization) ?? throw new InvalidOperationException($"Failed to return a new organization: '{organization.ServiceNowKey}'");
            _organizations.Add(organization);
        }
        else if (organizationSN.Data.UpdatedOn != organization.RawData?.GetElementValue<string?>(".sys_updated_on"))
        {
            this.Logger.LogInformation("Updating organization '{id}'", organization.ServiceNowKey);
            var update = new Hsb.OrganizationModel(organizationSN)
            {
                Id = organization.Id,
                Version = organization.Version,
            };
            organization = await this.HsbApi.UpdateOrganizationAsync(update) ?? throw new InvalidOperationException($"Failed to return an updated organization: '{organization.ServiceNowKey}'");
            _organizations[index] = organization;
        }
        else
            this.Logger.LogDebug("Organization data has not changed '{id}", organization.ServiceNowKey);

        return organization;
    }

    /// <summary>
    /// Add or update an organization in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.OrganizationModel?> ProcessOrganizationAsync(ServiceNow.ResultModel<ServiceNow.ServerModel> model)
    {
        this.Logger.LogDebug("Processing Organization for Configuration Item: '{id}'", model.Data?.Id);

        if (model.Data == null) throw new InvalidOperationException("Configuration Item information cannot be null");

        var organizationKey = model.RawData.GetElementValue<string>(".u_client_organization.value") ?? model.RawData.GetElementValue<string>(".u_client_organization");
        if (String.IsNullOrWhiteSpace(organizationKey))
        {
            this.Logger.LogWarning("Configuration Item does not have an Organization: '{id}'", model.Data.Id);
            return null;
        }

        // Fetch organization information.
        var organizationSN = await this.ServiceNowApi.GetClientOrganizationAsync(organizationKey);
        if (organizationSN == null || organizationSN.Data == null) throw new InvalidOperationException($"Organization cannot be null: '{organizationKey}'");

        var index = _organizations.FindIndex(t => t.ServiceNowKey == organizationSN.Data.Id);
        var organization = index > -1 ? _organizations[index] : new Hsb.OrganizationModel(organizationSN);

        // If HSB doesn't have the organization, add it, otherwise update
        if (organization.Id == 0)
        {
            this.Logger.LogInformation("Adding organization '{id}'", organization.ServiceNowKey);
            organization = await this.HsbApi.AddOrganizationAsync(organization) ?? throw new InvalidOperationException($"Failed to return a new organization: '{organization.ServiceNowKey}'");
            _organizations.Add(organization);
        }
        else if (organizationSN.Data.UpdatedOn != organization.RawData?.GetElementValue<string?>(".sys_updated_on"))
        {
            this.Logger.LogInformation("Updating organization '{id}'", organization.ServiceNowKey);
            var update = new Hsb.OrganizationModel(organizationSN)
            {
                Id = organization.Id,
                Version = organization.Version,
            };
            organization = await this.HsbApi.UpdateOrganizationAsync(update) ?? throw new InvalidOperationException($"Failed to return an updated organization: '{organization.ServiceNowKey}'");
            _organizations[index] = organization;
        }
        else
            this.Logger.LogDebug("Organization data has not changed '{id}", organization.ServiceNowKey);

        return organization;
    }

    /// <summary>
    /// Add a new configuration item in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="tenant"></param>
    /// <param name="organization"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.ConfigurationItemModel> AddConfigurationItemAsync(ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> model, Hsb.TenantModel? tenant, Hsb.OrganizationModel? organization)
    {
        this.Logger.LogDebug("Adding Configuration Item '{id}'", model.Data?.Id);
        var configurationItem = new Hsb.ConfigurationItemModel(model, tenant?.Id, organization?.Id);
        return await this.HsbApi.AddConfigurationItemAsync(configurationItem) ?? throw new InvalidOperationException($"Failed to return a new configuration item: '{configurationItem.ServiceNowKey}'");
    }

    /// <summary>
    /// Add a new file system record in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.FileSystemItemModel> AddFileSystemAsync(Hsb.ConfigurationItemModel model)
    {
        this.Logger.LogDebug("Adding File System Item for Configuration Item: '{id}'", model.ServiceNowKey);

        // Fetch file system item information.
        var filesSystemItemSN = await this.ServiceNowApi.GetFileSystemAsync(model.ServiceNowKey);
        if (filesSystemItemSN == null || filesSystemItemSN.Data == null) throw new InvalidOperationException($"File System Item cannot be null: '{model.ServiceNowKey}'");

        var fileSystemItem = new Hsb.FileSystemItemModel(filesSystemItemSN, model.Id);
        return await this.HsbApi.AddFileSystemItemAsync(fileSystemItem) ?? throw new InvalidOperationException($"Failed to return a new File System Item: '{fileSystemItem.ServiceNowKey}'");
    }

    /// <summary>
    /// Add a new server record in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.ServerItemModel> AddServerAsync(Hsb.ConfigurationItemModel model)
    {
        this.Logger.LogDebug("Adding Server for Configuration Item: '{id}'", model.ServiceNowKey);

        // Fetch server item information.
        var serverSN = await this.ServiceNowApi.GetServerAsync(model.ServiceNowKey);
        if (serverSN == null || serverSN.Data == null) throw new InvalidOperationException($"Server cannot be null: '{model.ServiceNowKey}'");

        // A server may be associated with an Operating System Item.
        Hsb.OperatingSystemItemModel? operatingSystemItem = null;
        var operatingSystemItemKey = serverSN.RawData.GetElementValue<string>(".u_operating_system.value");
        if (!String.IsNullOrWhiteSpace(operatingSystemItemKey))
        {
            operatingSystemItem = _operatingSystemItems.FirstOrDefault(osi => osi.ServiceNowKey == operatingSystemItemKey);

            if (operatingSystemItem == null)
            {
                var operatingSystemSN = await this.ServiceNowApi.GetOperatingSystemAsync(operatingSystemItemKey);
                if (operatingSystemSN == null || operatingSystemSN.Data == null) throw new InvalidOperationException($"Operating System Item cannot be null: '{operatingSystemItemKey}'");

                operatingSystemItem = new Hsb.OperatingSystemItemModel(operatingSystemSN);
                this.Logger.LogInformation("Adding Operating System '{id}'", operatingSystemItem.ServiceNowKey);
                operatingSystemItem = await this.HsbApi.AddOperatingSystemItemAsync(operatingSystemItem) ?? throw new InvalidOperationException($"Failed to return a new Operating System: '{operatingSystemItem.ServiceNowKey}'");
                _operatingSystemItems.Add(operatingSystemItem);
            }
        }

        var serverItem = new Hsb.ServerItemModel(serverSN, model.Id, operatingSystemItem?.Id);
        return await this.HsbApi.AddServerItemAsync(serverItem) ?? throw new InvalidOperationException($"Failed to return a new Server: '{serverItem.ServiceNowKey}'");
    }

    /// <summary>
    /// Add a new server record in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="configurationItemId"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.ServerItemModel> AddServerAsync(ServiceNow.ResultModel<ServiceNow.ServerModel> model, long? configurationItemId)
    {
        this.Logger.LogDebug("Adding Server: '{id}'", model.Data?.Id);

        // A server may be associated with an Operating System Item.
        Hsb.OperatingSystemItemModel? operatingSystemItem = null;
        var operatingSystemItemKey = model.RawData.GetElementValue<string>(".u_operating_system.value");
        if (!String.IsNullOrWhiteSpace(operatingSystemItemKey))
        {
            operatingSystemItem = _operatingSystemItems.FirstOrDefault(osi => osi.ServiceNowKey == operatingSystemItemKey);

            if (operatingSystemItem == null)
            {
                var operatingSystemSN = await this.ServiceNowApi.GetOperatingSystemAsync(operatingSystemItemKey);
                if (operatingSystemSN == null || operatingSystemSN.Data == null) throw new InvalidOperationException($"Operating System Item cannot be null: '{operatingSystemItemKey}'");

                operatingSystemItem = new Hsb.OperatingSystemItemModel(operatingSystemSN);
                this.Logger.LogInformation("Adding Operating System '{id}'", operatingSystemItem.ServiceNowKey);
                operatingSystemItem = await this.HsbApi.AddOperatingSystemItemAsync(operatingSystemItem) ?? throw new InvalidOperationException($"Failed to return a new Operating System: '{operatingSystemItem.ServiceNowKey}'");
                _operatingSystemItems.Add(operatingSystemItem);
            }
        }

        var serverItem = new Hsb.ServerItemModel(model, configurationItemId, operatingSystemItem?.Id);
        return await this.HsbApi.AddServerItemAsync(serverItem) ?? throw new InvalidOperationException($"Failed to return a new Server: '{serverItem.ServiceNowKey}'");
    }

    /// <summary>
    /// Add or update the operating system in HSB.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<Hsb.OperatingSystemItemModel> ProcessOperatingSystemAsync(Hsb.ServerItemModel model)
    {
        this.Logger.LogDebug("Processing Operating System for Server: '{id}'", model.ServiceNowKey);

        // Fetch Operating System information.
        var operatingSystemSN = await this.ServiceNowApi.GetOperatingSystemAsync(model.OperatingSystemKey);
        if (operatingSystemSN == null || operatingSystemSN.Data == null) throw new InvalidOperationException($"Operating System cannot be null: '{model.OperatingSystemKey}'");

        var index = _operatingSystemItems.FindIndex(t => t.ServiceNowKey == operatingSystemSN.Data.Id);
        var operatingSystem = index > -1 ? _operatingSystemItems[index] : new Hsb.OperatingSystemItemModel(operatingSystemSN);

        // If HSB doesn't have the Operating System, add it, otherwise update
        if (operatingSystem.Id == 0)
        {
            this.Logger.LogInformation("Adding Operating System '{id}'", operatingSystem.ServiceNowKey);
            operatingSystem = await this.HsbApi.AddOperatingSystemItemAsync(operatingSystem) ?? throw new InvalidOperationException($"Failed to return a new Operating System: '{operatingSystem.ServiceNowKey}'");
            _operatingSystemItems.Add(operatingSystem);
        }
        else if (operatingSystemSN.Data.UpdatedOn != operatingSystem.RawData?.GetElementValue<string?>(".sys_updated_on"))
        {
            this.Logger.LogInformation("Updating Operating System '{id}'", operatingSystem.ServiceNowKey);
            var update = new Hsb.OperatingSystemItemModel(operatingSystemSN)
            {
                Id = operatingSystem.Id,
                Version = operatingSystem.Version,
            };
            operatingSystem = await this.HsbApi.UpdateOperatingSystemItemAsync(update) ?? throw new InvalidOperationException($"Failed to return an updated Operating System: '{operatingSystem.ServiceNowKey}'");
            _operatingSystemItems[index] = operatingSystem;
        }
        else
            this.Logger.LogDebug("Operating System data has not changed '{id}", operatingSystem.ServiceNowKey);

        return operatingSystem;
    }
    #endregion
}

