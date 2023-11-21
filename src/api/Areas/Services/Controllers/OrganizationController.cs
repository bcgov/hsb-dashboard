using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;

namespace HSB.API.Areas.Services.Controllers;

/// <summary>
/// OrganizationController class, provides endpoints for organizations.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Area("services")]
[Route("v{version:apiVersion}/[area]/organizations")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class OrganizationController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IOrganizationService _service;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a OrganizationController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public OrganizationController(IOrganizationService service, ILogger<OrganizationController> logger)
    {
        _service = service;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetOrganizations")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationModel>), 200)]
    [SwaggerOperation(Tags = ["Organization Item"])]
    public IActionResult Get()
    {
        var organizations = _service.Find(o => true);
        return new JsonResult(organizations.Select(ci => new OrganizationModel(ci)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddOrganizations")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), 201)]
    [SwaggerOperation(Tags = ["Organization Item"])]
    public IActionResult Add(OrganizationModel model)
    {
        var entity = model.ToEntity();
        _service.Add(entity);
        _service.CommitTransaction();
        return new JsonResult(new OrganizationModel(entity));
    }
    #endregion
}
