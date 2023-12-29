using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using HSB.API.CSS;
using HSB.API.Models.Auth;
using HSB.Core.Models;
using System.Net;

namespace HSB.API.Controllers;

/// <summary>
/// AuthController class, provides health endpoints for the api.
/// </summary>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class AuthController : ControllerBase
{
    #region Variables
    private readonly ICssHelper _cssHelper;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a AuthController object, initializes with specified parameters.
    /// </summary>
    /// <param name="cssHelper"></param>
    public AuthController(ICssHelper cssHelper)
    {
        _cssHelper = cssHelper;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Get user information.
    /// If the user doesn't currently exist in HSB, activate a new user by adding them to HSB.
    /// If the user exists in HSB, activate user by linking to Keycloak and updating Keycloak.
    /// </summary>
    /// <returns></returns>
    [HttpPost("userinfo")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PrincipalModel), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Auth" })]
    public async Task<IActionResult> UserInfoAsync()
    {
        var user = await _cssHelper.ActivateAsync(this.User);
        return new JsonResult(new PrincipalModel(this.User, user));
    }
    #endregion
}
