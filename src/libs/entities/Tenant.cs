using System.Text.Json;

namespace HSB.Entities;

public class Tenant : SortableCodeAuditable<int>
{
    #region Properties
    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

    #region ServiceNow Properties
    /// <summary>
    /// get/set - The ServiceNow tenant key.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";
    #endregion

    /// <summary>
    /// get - An array of configuration items that belong to this tenant.
    /// </summary>
    public ICollection<ConfigurationItem> ConfigurationItems { get; } = new List<ConfigurationItem>();

    /// <summary>
    /// get - Users that belong to this tenant.
    /// </summary>
    public ICollection<User> Users { get; } = new List<User>();

    /// <summary>
    /// get - Users that belong to this tenant (many-to-many).
    /// </summary>
    public ICollection<UserTenant> UsersManyToMany { get; } = new List<UserTenant>();

    /// <summary>
    /// get - Organizations that belong to this tenant.
    /// </summary>
    public ICollection<Organization> Organizations { get; } = new List<Organization>();

    /// <summary>
    /// get - Organizations that belong to this tenant (many-to-many).
    /// </summary>
    public ICollection<TenantOrganization> OrganizationsManyToMany { get; } = new List<TenantOrganization>();
    #endregion

    #region Constructors
    protected Tenant()
    {

    }

    public Tenant(string name)
    {
        this.Name = name;
    }
    #endregion
}
