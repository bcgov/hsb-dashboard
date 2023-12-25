using System.Text.Json;
using HSB.Core.Extensions;

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
    /// get - An array of server items that belong to this tenant.
    /// </summary>
    public List<ServerItem> ServerItems { get; } = new List<ServerItem>();

    /// <summary>
    /// get - Users that belong to this tenant.
    /// </summary>
    public List<User> Users { get; } = new List<User>();

    /// <summary>
    /// get - Users that belong to this tenant (many-to-many).
    /// </summary>
    public List<UserTenant> UsersManyToMany { get; } = new List<UserTenant>();

    /// <summary>
    /// get - Organizations that belong to this tenant.
    /// </summary>
    public List<Organization> Organizations { get; } = new List<Organization>();

    /// <summary>
    /// get - Organizations that belong to this tenant (many-to-many).
    /// </summary>
    public List<TenantOrganization> OrganizationsManyToMany { get; } = new List<TenantOrganization>();
    #endregion

    #region Constructors
    protected Tenant()
    {

    }

    public Tenant(string name)
    {
        this.Name = name;
    }

    public Tenant(JsonDocument data)
    {
        this.RawData = data;

        this.ServiceNowKey = data.GetElementValue<string>(".sys_id") ?? "";
        this.Name = data.GetElementValue<string>(".u_name") ?? data.GetElementValue<string>(".sys_name") ?? "";
        this.Code = data.GetElementValue<string>(".sys_name") ?? Guid.NewGuid().ToString();
    }
    #endregion
}
