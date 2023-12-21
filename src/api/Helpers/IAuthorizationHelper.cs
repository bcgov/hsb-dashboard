
using System.Security.Claims;
using HSB.DAL.Services;

namespace HSB.API;

/// <summary>
/// IAuthorizationHelper interface, provides methods to test user authorization.
/// </summary>
public interface IAuthorizationHelper
{
    #region Properties
    /// <summary>
    /// get - The user in the current HTTP context.
    /// </summary>
    public ClaimsPrincipal? User { get; }

    /// <summary>
    /// get - The user service to connect to the database.
    /// </summary>
    public IUserService UserService { get; }
    #endregion

    #region Methods
    /// <summary>
    /// Get the database user information for the currently authenticated user.
    /// </summary>
    /// <returns></returns>
    public Entities.User? GetUser();
    #endregion
}
