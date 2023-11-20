using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;
public class ConfigurationItem : Auditable
{
    #region Properties
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

    #region ServiceNow Properties
    public string ServiceNowKey { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string SubCategory { get; set; } = "";
    public string UPlatform { get; set; } = "";
    public string DnsDomain { get; set; } = "";
    public string SysClassName { get; set; } = "";
    public string FQDN { get; set; } = "";
    public string IPAddress { get; set; } = "";
    #endregion

    /// <summary>
    ///
    /// </summary>
    public ICollection<ServerItem> ServerItems { get; } = new List<ServerItem>();

    /// <summary>
    ///
    /// </summary>
    public ICollection<FileSystemItem> FileSystemItems { get; } = new List<FileSystemItem>();
    #endregion

    #region Constructors
    public ConfigurationItem() { }

    public ConfigurationItem(Organization organization, JsonDocument serviceNowJson)
        : this(organization?.Id ?? 0, serviceNowJson)
    {
        this.Organization = organization ?? throw new ArgumentNullException(nameof(organization));
    }

    public ConfigurationItem(int organizationId, JsonDocument serviceNowJson)
    {
        this.OrganizationId = organizationId;
        this.RawData = serviceNowJson;

        var values = JsonSerializer.Deserialize<Dictionary<string, object>>(serviceNowJson);
        InitServiceNowProperties(values);
    }
    #endregion

    #region Methods
    private void InitServiceNowProperties(Dictionary<string, object>? values)
    {
        this.ServiceNowKey = values?.GetDictionaryJsonValue<string>("sys_id") ?? "";
        this.Name = values?.GetDictionaryJsonValue<string>("name") ?? "";
        this.Category = values?.GetDictionaryJsonValue<string>("category") ?? "";
        this.SubCategory = values?.GetDictionaryJsonValue<string>("subcategory") ?? "";
        this.UPlatform = values?.GetDictionaryJsonValue<string>("u_platform") ?? "";
        this.DnsDomain = values?.GetDictionaryJsonValue<string>("dns_domain") ?? "";
        this.SysClassName = values?.GetDictionaryJsonValue<string>("sys_class_name") ?? "";
        this.FQDN = values?.GetDictionaryJsonValue<string>("fqdn") ?? "";
        this.IPAddress = values?.GetDictionaryJsonValue<string>("ip_address") ?? "";
    }
    #endregion
}
