using System.Text.Json;
using HSB.Core.Extensions;

namespace HSB.Entities;

public class Organization : SortableCodeAuditable<int>
{
    #region Properties

    public int? ParentId { get; set; }

    public Organization? Parent { get; set; }

    public JsonDocument RawData { get; set; } = JsonDocument.Parse("{}");

    #region ServiceNow Properties
    /// <summary>
    /// get/set - The ServiceNow organization key.
    /// </summary>
    public string ServiceNowKey { get; set; } = "";
    #endregion

    /// <summary>
    ///
    /// </summary>
    public List<ServerItem> ServerItems { get; } = new List<ServerItem>();

    /// <summary>
    /// get - Child organizations.
    /// </summary>
    public List<Organization> Children { get; } = new List<Organization>();

    /// <summary>
    /// get - Tenants that belong to this organization.
    /// </summary>
    public List<Tenant> Tenants { get; } = new List<Tenant>();

    /// <summary>
    /// get - Tenants that belong to this organization. (many-to-many).
    /// </summary>
    public List<TenantOrganization> TenantsManyToMany { get; } = new List<TenantOrganization>();

    /// <summary>
    /// get - Users that belong to this organization.
    /// </summary>
    public List<User> Users { get; } = new List<User>();

    /// <summary>
    /// get - Users that belong to this organization. (many-to-many).
    /// </summary>
    public List<UserOrganization> UsersManyToMany { get; } = new List<UserOrganization>();
    #endregion

    #region Constructors
    protected Organization()
    {

    }

    public Organization(string name)
    {
        this.Name = name;
    }

    public Organization(JsonDocument data)
    {
        this.RawData = data;

        this.ServiceNowKey = data.GetElementValue<string>(".sys_id") ?? "";
        this.Name = data.GetElementValue<string>(".u_name") ?? "";
        this.Code = data.GetElementValue<string>(".u_org_code") ?? "";
    }
    #endregion
}
