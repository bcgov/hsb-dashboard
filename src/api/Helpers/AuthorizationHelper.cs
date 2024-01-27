using System.Security.Claims;
using HSB.Core.Extensions;
using HSB.DAL.Services;

namespace HSB.API;

/// <summary>
/// AuthorizationHelper class, provides a simple way to test user authorization.
/// </summary>
public class AuthorizationHelper : IAuthorizationHelper
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

    #region Constructors
    /// <summary>
    /// Creates a new instance of a AuthorizationHelper object, initializes with specified parameters.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="userService"></param>
    public AuthorizationHelper(ClaimsPrincipal user, IUserService userService)
    {
        this.User = user;
        this.UserService = userService;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Get the database user information for the currently authenticated user.
    /// </summary>
    /// <returns></returns>
    public Entities.User? GetUser()
    {
        // Only return tenants this user belongs to.
        var username = this.User?.GetUsername();
        if (!String.IsNullOrWhiteSpace(username))
            return this.UserService.FindByUsername(username, true);

        return null;
    }
    #endregion
}
