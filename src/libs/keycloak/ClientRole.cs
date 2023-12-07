using System.ComponentModel.DataAnnotations;

namespace HSB.Keycloak;

/// <summary>
/// ClientRole enum, provides the possible Keycloak client roles.
/// </summary>
public enum ClientRole
{
    /// <summary>
    /// System Administrator role.
    /// </summary>
    [Display(Name = "system-admin")]
    SystemAdministrator,
    /// <summary>
    /// Editor role.
    /// </summary>
    [Display(Name = "organization-admin")]
    OrganizationAdministrator,
    /// <summary>
    /// Client role.
    /// </summary>
    [Display(Name = "client")]
    Client,
    /// <summary>
    /// HSB role.
    /// </summary>
    [Display(Name = "hsb")]
    HSB,
    /// <summary>
    /// Service Now role.
    /// </summary>
    [Display(Name = "service-now")]
    ServiceNow,
}
