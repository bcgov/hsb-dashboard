using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.API.Models.Health;
using HSB.DAL.Services;

namespace HSB.API.Controllers;

/// <summary>
/// HealthController class, provides health endpoints for the api.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
public class HealthController : ControllerBase
{
    #region Variables
    private readonly IWebHostEnvironment _environment;
    private readonly IUserService _userService;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a HealthController object, initializes with specified parameters.
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="userService"></param>
    public HealthController(
        IWebHostEnvironment environment,
        IUserService userService)
    {
        _environment = environment;
        _userService = userService;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Return api status
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(StatusModel), 200)]
    [SwaggerOperation(Tags = new[] { "health" })]
    public IActionResult Status(string status = "live")
    {
        if (status == "ready")
        {
            _userService.Find(u => true, u => u.Id, 1);
            return new JsonResult(new StatusModel(status));
        }

        return new JsonResult(new StatusModel(status));
    }

    /// <summary>
    /// Return environment information.
    /// </summary>
    /// <returns></returns>
    [HttpGet("env")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(EnvModel), 200)]
    [SwaggerOperation(Tags = new[] { "health" })]
    public IActionResult Environment()
    {
        return new JsonResult(new EnvModel(_environment));
    }
    #endregion
}
