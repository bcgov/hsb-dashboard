using System.Security.Claims;
using System.Text.Json;
using HSB.Core.Extensions;
using HSB.Entities;

namespace HSB.API.Models.Auth;

/// <summary>
/// PrincipalModel class, provides a model to represent a user.
/// </summary>
public class PrincipalModel
{
    #region Properties
    /// <summary>
    /// get/set - Primary key to identify user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// get/set - Unique key to link to Keycloak.
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// get/set - Unique username to identify user.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// get/set - User's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// get/set - Friendly name to display.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// get/set - User's first name.
    /// </summary>
    public string? FirstName { get; set; }

    /// <summary>
    /// get/set - User's last name.
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// get/set - The last time the user logged in.
    /// </summary>
    public DateTime? LastLoginOn { get; set; }

    /// <summary>
    /// get/set - Whether the user is enabled.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// get/set - A note.
    /// </summary>
    public string Note { get; set; } = "";

    /// <summary>
    /// get/set - Groups this user belongs to.
    /// </summary>
    public IEnumerable<TenantModel> Tenants { get; set; } = Array.Empty<TenantModel>();

    /// <summary>
    /// get/set - Groups this user belongs to.
    /// </summary>
    public IEnumerable<OrganizationModel> Organizations { get; set; } = Array.Empty<OrganizationModel>();

    /// <summary>
    /// get/set - Groups this user belongs to.
    /// </summary>
    public IEnumerable<string> Groups { get; set; } = Array.Empty<string>();

    /// <summary>
    /// get/set - Keycloak roles.
    /// </summary>
    public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();

    /// <summary>
    /// get/set - User preferences.
    /// </summary>
    public JsonDocument Preferences { get; set; } = JsonDocument.Parse("{}");

    /// <summary>
    /// get/set - The current row version.
    /// </summary>
    public long? Version { get; set; }
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a PrincipalModel object.
    /// </summary>
    public PrincipalModel() { }

    /// <summary>
    /// Creates a new instance of a PrincipalModel object, initializes it with specified arguments.
    /// </summary>
    /// <param name="principal"></param>
    /// <param name="user"></param>

    public PrincipalModel(ClaimsPrincipal principal, User? user)
    {
        this.Id = user?.Id ?? 0;
        this.Key = principal.GetKey();
        this.Username = principal.GetUsername();
        this.DisplayName = principal.GetDisplayName();
        this.Email = principal.GetEmail();
        this.FirstName = principal.GetFirstName();
        this.LastName = principal.GetLastName();
        this.IsEnabled = user?.IsEnabled ?? false;
        this.Note = user?.Note ?? "";
        this.Preferences = user?.Preferences ?? JsonDocument.Parse("{}");
        this.Tenants = user?.TenantsManyToMany.Any() == true ? user.TenantsManyToMany.Where(t => t.Tenant != null).Select(t => new TenantModel(t.Tenant!)) : this.Tenants;
        this.Tenants = user?.Tenants.Any() == true ? user.Tenants.Select(t => new TenantModel(t)) : this.Tenants;
        this.Organizations = user?.OrganizationsManyToMany.Any() == true ? user.OrganizationsManyToMany.Where(t => t.Organization != null).Select(t => new OrganizationModel(t.Organization!)) : this.Organizations;
        this.Organizations = user?.Organizations.Any() == true ? user.Organizations.Select(t => new OrganizationModel(t)) : this.Organizations;
        this.Groups = user?.Groups.Select(g => g.Name).Distinct() ?? Array.Empty<string>();
        this.Roles = user?.Groups.SelectMany(g => g.Roles).Select(r => r.Name).Distinct() ?? principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        this.Version = user?.Version;
    }
    #endregion
}
