using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class ConfigurationItemModel : AuditableModel
{
    #region Properties
    public int Id { get; set; }
    public int? TenantId { get; set; }
    public TenantModel? Tenant { get; set; }
    public int? OrganizationId { get; set; }
    public OrganizationModel? Organization { get; set; }
    public IEnumerable<ServerItemModel> Servers { get; set; } = Array.Empty<ServerItemModel>();
    public IEnumerable<FileSystemItemModel> FileSystems { get; set; } = Array.Empty<FileSystemItemModel>();
    public JsonDocument? RawData { get; set; }

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string SubCategory { get; set; } = "";
    public string Platform { get; set; } = "";
    public string DnsDomain { get; set; } = "";
    public string ClassName { get; set; } = "";
    public string FQDN { get; set; } = "";
    public string IPAddress { get; set; } = "";
    #endregion
    #endregion

    #region Constructors
    public ConfigurationItemModel() { }

    public ConfigurationItemModel(ConfigurationItem entity) : base(entity)
    {
        this.Id = entity.Id;
        this.TenantId = entity.TenantId;
        this.Tenant = entity.Tenant != null ? new TenantModel(entity.Tenant) : null;
        this.OrganizationId = entity.OrganizationId;
        this.Organization = entity.Organization != null ? new OrganizationModel(entity.Organization) : null;
        this.RawData = entity.RawData;
        this.ServiceNowKey = entity.ServiceNowKey;

        this.Name = entity.Name;
        this.Category = entity.Category;
        this.SubCategory = entity.SubCategory;
        this.Platform = entity.Platform;
        this.DnsDomain = entity.DnsDomain;
        this.ClassName = entity.ClassName;
        this.FQDN = entity.FQDN;
        this.IPAddress = entity.IPAddress;

        this.Servers = entity.ServerItems.Select(si => new ServerItemModel(si));
        this.FileSystems = entity.FileSystemItems.Select(fsi => new FileSystemItemModel(fsi));
    }

    public ConfigurationItemModel(ServiceNow.ResultModel<ServiceNow.ConfigurationItemModel> model, int? tenantId, int? organizationId)
    {
        if (model.Data == null) throw new InvalidOperationException("Configuration Item data cannot be null");
        if (tenantId == null && organizationId == null) throw new InvalidOperationException("Configuration Item must belong to either a tenant and/or an organization");

        this.TenantId = tenantId;
        this.OrganizationId = organizationId;
        this.RawData = model.RawData;
        this.ServiceNowKey = model.Data.Id;
        this.Name = model.Data.Name ?? "";
        this.Category = model.Data.Category ?? "";
        this.SubCategory = model.Data.SubCategory ?? "";
        this.Platform = model.Data.Platform ?? "";
        this.DnsDomain = model.Data.DnsDomain ?? "";
        this.ClassName = model.Data.ClassName ?? "";
        this.FQDN = model.Data.FQDN ?? "";
        this.IPAddress = model.Data.IPAddress ?? "";
    }
    #endregion

    #region Methods
    public ConfigurationItem ToEntity()
    {
        if (this.RawData == null) throw new InvalidOperationException("Property 'RawData' is required.");

        return new ConfigurationItem(this.TenantId, this.OrganizationId, this.RawData)
        {
            Id = this.Id,
            CreatedOn = this.CreatedOn,
            CreatedBy = this.CreatedBy,
            UpdatedOn = this.UpdatedOn,
            UpdatedBy = this.UpdatedBy,
            Version = this.Version,
        };
    }
    #endregion
}
