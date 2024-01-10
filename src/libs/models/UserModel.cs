using System.Text.Json;
using HSB.Entities;

namespace HSB.Models;
public class UserModel : AuditableModel
{
    #region Properties
    /// <summary>
    /// get/set - Primary key (Identity seed).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// get/set - Unique username to identify the user.
    /// </summary>
    public string Username { get; set; } = "";

    /// <summary>
    /// get/set - An email address for the user.
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// get/set - Whether the email has been verified.
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// get/set - When the email was verified.
    /// </summary>
    public DateTime? EmailVerifiedOn { get; set; }

    /// <summary>
    /// get/set - Unique key to identify this user.
    /// </summary>
    public string Key { get; set; } = "";

    /// <summary>
    /// get/set - A unique display name for this user.
    /// </summary>
    public string DisplayName { get; set; } = "";

    /// <summary>
    /// get/set - User's first name.
    /// </summary>
    public string FirstName { get; set; } = "";

    /// <summary>
    /// get/set - User's middle name.
    /// </summary>
    public string MiddleName { get; set; } = "";

    /// <summary>
    /// get/set - User's last name.
    /// </summary>
    public string LastName { get; set; } = "";

    /// <summary>
    /// get/set - User's phone number.
    /// </summary>
    public string Phone { get; set; } = "";

    /// <summary>
    /// get/set - Whether this user is enabled.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// get/set - Last time user logged in.
    /// </summary>
    public DateTime? LastLoginOn { get; set; }

    /// <summary>
    /// get/set - A note related to this user.
    /// </summary>
    public string Note { get; set; } = "";

    /// <summary>
    /// get/set - A JSON object containing user preferences.
    /// </summary>
    public JsonDocument Preferences { get; set; } = JsonDocument.Parse("{}");

    /// <summary>
    /// get/set - A collection of groups this user belongs to.
    /// </summary>
    public IEnumerable<GroupModel> Groups { get; set; } = Array.Empty<GroupModel>();

    /// <summary>
    /// get/set - A collection of tenants this user belongs to.
    /// </summary>
    public IEnumerable<TenantModel> Tenants { get; set; } = Array.Empty<TenantModel>();

    /// <summary>
    /// get/set - A collection of organizations this user belongs to.
    /// </summary>
    public IEnumerable<OrganizationModel> Organizations { get; set; } = Array.Empty<OrganizationModel>();
    #endregion

    #region Constructors
    public UserModel() { }

    public UserModel(User user) : base(user)
    {
        this.Id = user.Id;
        this.Key = user.Key;
        this.Username = user.Username;
        this.DisplayName = user.DisplayName;
        this.Email = user.Email;
        this.EmailVerified = user.EmailVerified;
        this.EmailVerifiedOn = user.EmailVerifiedOn;
        this.FirstName = user.FirstName;
        this.MiddleName = user.MiddleName;
        this.LastName = user.LastName;
        this.LastLoginOn = user.LastLoginOn;
        this.IsEnabled = user.IsEnabled;
        this.Note = user.Note;
        this.Phone = user.Phone;
        this.Preferences = user.Preferences;
        this.Groups = user.GroupsManyToMany.Any() ? user.GroupsManyToMany.Where(g => g.Group != null).Select(g => new GroupModel(g.Group!)) : this.Groups;
        this.Groups = user.Groups.Any() ? user.Groups.Select(g => new GroupModel(g)) : this.Groups;
        this.Tenants = user.TenantsManyToMany.Any() ? user.TenantsManyToMany.Where(t => t.Tenant != null).Select(t => new TenantModel(t.Tenant!)) : this.Tenants;
        this.Tenants = user.Tenants.Any() ? user.Tenants.Select(t => new TenantModel(t)) : this.Tenants;
        this.Organizations = user.OrganizationsManyToMany.Any() ? user.OrganizationsManyToMany.Where(o => o.Organization != null).Select(o => new OrganizationModel(o.Organization!)) : this.Organizations;
        this.Organizations = user.Organizations.Any() ? user.Organizations.Select(o => new OrganizationModel(o)) : this.Organizations;
    }
    #endregion

    #region Methods
    public User ToEntity()
    {
        return (User)this;
    }

    public static explicit operator User(UserModel model)
    {
        var user = new User(model.Username, model.Email, model.Key)
        {
            Id = model.Id,
            DisplayName = model.DisplayName,
            EmailVerified = model.EmailVerified,
            EmailVerifiedOn = model.EmailVerifiedOn,
            FirstName = model.FirstName,
            MiddleName = model.MiddleName,
            LastName = model.LastName,
            LastLoginOn = model.LastLoginOn,
            IsEnabled = model.IsEnabled,
            Note = model.Note,
            Phone = model.Phone,
            Preferences = model.Preferences,
            Version = model.Version
        };
        user.GroupsManyToMany.AddRange(model.Groups.Select(g => new UserGroup(user.Id, g.Id)));
        user.TenantsManyToMany.AddRange(model.Tenants.Select(t => new UserTenant(user.Id, t.Id)));
        user.OrganizationsManyToMany.AddRange(model.Organizations.Select(o => new UserOrganization(user.Id, o.Id)));

        return user;
    }
    #endregion
}
