using System;
using System.Linq;
using System.Security.Claims;

using HSB.Core.Extensions;

namespace HSB.Keycloak.Extensions;

/// <summary>
/// IdentityExtensions static class, provides extension methods for ClaimsPrincipal
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    /// Determine if the user any of the specified roles.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="role"></param>
    /// <returns>True if the user has any of the roles.</returns>
    public static bool HasClientRole(this ClaimsPrincipal user, params string[] role)
    {
        if (role == null) throw new ArgumentNullException(nameof(role));
        if (role.Length == 0) throw new ArgumentOutOfRangeException(nameof(role));

        return user.Claims.Any(c => c.Type == "client_roles" && role.Contains(c.Value));
    }

    /// <summary>
    /// Determine if the user all of the specified roles.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="role"></param>
    /// <returns>True if the user has all of the roles.</returns>
    public static bool HasClientRoles(this ClaimsPrincipal user, params string[] role)
    {
        if (role == null) throw new ArgumentNullException(nameof(role));
        if (role.Length == 0) throw new ArgumentOutOfRangeException(nameof(role));

        var count = user.Claims.Count(c => c.Type == "client_roles" && role.Contains(c.Value));

        return count == role.Length;
    }

    /// <summary>
    /// Determine if the user any of the specified roles.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="role"></param>
    /// <returns>True if the user has any of the roles.</returns>
    public static bool HasClientRole(this ClaimsPrincipal user, params ClientRole[] role)
    {
        if (role == null) throw new ArgumentNullException(nameof(role));

        return user.HasClientRole(role.Select(r => r.GetName()!).ToArray());
    }

    /// <summary>
    /// Determine if the user all of the specified roles.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="role"></param>
    /// <returns>True if the user has all of the roles.</returns>
    public static bool HasClientRoles(this ClaimsPrincipal user, params ClientRole[] role)
    {
        if (role == null) throw new ArgumentNullException(nameof(role));

        return user.HasClientRoles(role.Select(r => r.GetName()!).ToArray());
    }
}
