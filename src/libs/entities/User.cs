using System.Text.Json;

namespace HSB.Entities;

/// <summary>
/// User class, provides table configuration for a user.
/// </summary>
public class User : Auditable
{
    #region Properties
    /// <summary>
    /// get/set - Primary key (Identity seed).
    /// </summary>
    public long Id { get; set; }

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
    /// get/set - Number of failed login attempts.
    /// </summary>
    public int FailedLogins { get; set; }

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
    public List<Group> Groups { get; } = new List<Group>();

    /// <summary>
    /// get/set - A collection of groups this user belongs to (many-to-many).
    /// </summary>
    public List<UserGroup> GroupsManyToMany { get; } = new List<UserGroup>();

    /// <summary>
    /// get/set - A collection of tenants this user belongs to.
    /// </summary>
    public List<Tenant> Tenants { get; } = new List<Tenant>();

    /// <summary>
    /// get/set - A collection of tenants this user belongs to (many-to-many).
    /// </summary>
    public List<UserTenant> TenantsManyToMany { get; } = new List<UserTenant>();
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of an User object.
    /// </summary>
    protected User() { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="key"></param>
    public User(string username, string email, string key)
    {
        this.Username = username;
        this.Email = email;
        this.Key = key;
        this.DisplayName = username;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    public User(string username, string email) : this(username, email, Guid.NewGuid().ToString())
    {
    }
    #endregion
}
