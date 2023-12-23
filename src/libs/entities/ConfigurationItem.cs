using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;
public class ConfigurationItem : Auditable
{
    #region Properties
    public long Id { get; set; }
    public int? TenantId { get; set; }
    public Tenant? Tenant { get; set; }
    public int? OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

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

    /// <summary>
    ///
    /// </summary>
    public List<ServerItem> ServerItems { get; } = new List<ServerItem>();

    /// <summary>
    ///
    /// </summary>
    public List<FileSystemItem> FileSystemItems { get; } = new List<FileSystemItem>();
    #endregion

    #region Constructors
    public ConfigurationItem() { }

    public ConfigurationItem(Tenant? tenant, Organization? organization, JsonDocument serviceNowJson)
        : this(tenant?.Id, organization?.Id, serviceNowJson)
    {
        this.Tenant = tenant;
        this.Organization = organization;
    }

    public ConfigurationItem(int? tenantId, int? organizationId, JsonDocument data)
    {
        this.TenantId = tenantId;
        this.OrganizationId = organizationId;
        this.RawData = data;

        this.ServiceNowKey = data.GetElementValue<string>(".sys_id") ?? "";
        this.Name = data.GetElementValue<string>(".name") ?? "";
        this.Category = data.GetElementValue<string>(".category") ?? "";
        this.SubCategory = data.GetElementValue<string>(".subcategory") ?? "";
        this.Platform = data.GetElementValue<string>(".u_platform") ?? "";
        this.DnsDomain = data.GetElementValue<string>(".dns_domain") ?? "";
        this.ClassName = data.GetElementValue<string>(".sys_class_name") ?? "";
        this.FQDN = data.GetElementValue<string>(".fqdn") ?? "";
        this.IPAddress = data.GetElementValue<string>(".ip_address") ?? "";
    }
    #endregion
}
