using System.Text.Json;

namespace HSB.Entities;

public class Organization : SortableCodeAuditable<int>
{
    #region Properties

    public int? ParentId { get; set; }

    public Organization? Parent { get; set; }

    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

    #region ServiceNow Properties
    /// <summary>
    /// get/set - The ServiceNow tenant key.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";
    #endregion

    /// <summary>
    ///
    /// </summary>
    public ICollection<ConfigurationItem> ConfigurationItems { get; } = new List<ConfigurationItem>();

    /// <summary>
    /// get - Child organizations.
    /// </summary>
    public ICollection<Organization> Children { get; } = new List<Organization>();

    /// <summary>
    /// get - Tenants that belong to this organization.
    /// </summary>
    public ICollection<Tenant> Tenants { get; } = new List<Tenant>();

    /// <summary>
    /// get - Tenants that belong to this organization. (many-to-many).
    /// </summary>
    public ICollection<TenantOrganization> TenantsManyToMany { get; } = new List<TenantOrganization>();
    #endregion

    #region Constructors
    protected Organization()
    {

    }

    public Organization(string name)
    {
        this.Name = name;
    }
    #endregion
}
