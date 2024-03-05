using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;
using HSB.Keycloak;

namespace HSB.API.Areas.Services.Controllers;

/// <summary>
/// OrganizationController class, provides endpoints for organizations.
/// </summary>
[ClientRoleAuthorize(ClientRole.ServiceNow)]
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
    [HttpGet(Name = "FindOrganizations-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult Find()
    {
        var organizations = _service.Find(o => true);
        return new JsonResult(organizations.Select(ci => new OrganizationModel(ci, true)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetOrganization-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult GetForId(int id)
    {
        var entity = _service.FindForId(id);

        if (entity == null) return new NoContentResult();

        return new JsonResult(new OrganizationModel(entity, true));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddOrUpdateOrganization-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(OrganizationModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult AddOrUpdate(OrganizationModel model)
    {
        var entity = model.ToEntity();
        var existing = _service.FindForId(model.Id);
        if (existing == null)
        {
            _service.Add(entity);
            _service.CommitTransaction();
            return CreatedAtAction(nameof(GetForId), new { id = entity.Id }, new OrganizationModel(entity, true));
        }
        else
        {
            _service.ClearChangeTracker(); // Remove existing from context.
            _service.Update(entity);
            _service.CommitTransaction();
            return new JsonResult(new OrganizationModel(entity, true));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateOrganization-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult Update(OrganizationModel model)
    {
        var entity = model.ToEntity();
        _service.Update(entity);
        _service.CommitTransaction();
        return new JsonResult(new OrganizationModel(entity, true));
    }

    /// <summary>
    /// Cleanup organizations by deleting any that do not have servers.
    /// </summary>
    /// <returns></returns>
    [HttpDelete("clean", Name = "CleanOrganizations-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult Cleanup()
    {
        var organizations = _service.Cleanup();
        return new JsonResult(organizations.Select(o => new OrganizationModel(o, false)));
    }
    #endregion
}
