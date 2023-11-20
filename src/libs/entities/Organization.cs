using System.Text.Json;

namespace HSB.Entities;

public class Organization : SortableCodeAuditable<int>
{
    #region Properties
    public OrganizationType OrganizationType { get; set; }

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
    ///
    /// </summary>
    public ICollection<User> Users { get; } = new List<User>();

    /// <summary>
    ///
    /// </summary>
    public ICollection<UserOrganization> UsersManyToMany { get; } = new List<UserOrganization>();

    /// <summary>
    /// get - Child organizations.
    /// </summary>
    public ICollection<Organization> Children { get; } = new List<Organization>();
    #endregion

    #region Constructors
    protected Organization()
    {

    }

    public Organization(string name, OrganizationType type)
    {
        this.Name = name;
        this.OrganizationType = type;
    }
    #endregion
}
