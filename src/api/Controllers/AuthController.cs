using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using HSB.API.CSS;
using HSB.API.Models.Auth;
using HSB.DAL.Services;

namespace HSB.API.Controllers;

/// <summary>
/// AuthController class, provides health endpoints for the api.
/// </summary>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    #region Variables
    private readonly ICssHelper _cssHelper;
    private readonly IUserService _userService;
    private readonly JsonSerializerOptions _serializerOptions;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a AuthController object, initializes with specified parameters.
    /// </summary>
    /// <param name="cssHelper"></param>
    /// <param name="serializerOptions"></param>
    /// <param name="userService"></param>
    public AuthController(ICssHelper cssHelper, IUserService userService, IOptions<JsonSerializerOptions> serializerOptions)
    {
        _cssHelper = cssHelper;
        _userService = userService;
        _serializerOptions = serializerOptions.Value;

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
    [ProducesResponseType(typeof(PrincipalModel), 200)]
    [SwaggerOperation(Tags = ["Auth"])]
    public async Task<IActionResult> UserInfoAsync()
    {
        var user = await _cssHelper.ActivateAsync(this.User);
        return new JsonResult(new PrincipalModel(this.User, user, _serializerOptions));
    }
    #endregion
}
