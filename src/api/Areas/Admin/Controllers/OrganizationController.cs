using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;
using HSB.Keycloak;
using HSB.Core.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace HSB.API.Areas.SystemAdmin.Controllers;

/// <summary>
/// OrganizationController class, provides endpoints for organizations.
/// </summary>
[ClientRoleAuthorize(ClientRole.SystemAdministrator, ClientRole.OrganizationAdministrator)]
[ApiController]
[ApiVersion("1.0")]
[Area("admin")]
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
    public OrganizationController(
        IOrganizationService service,
        ILogger<OrganizationController> logger)
    {
        _service = service;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find organizations for the specified query filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetOrganizations-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.OrganizationFilter(query);
        var result = _service.Find(filter);
        return new JsonResult(result.Select(ci => new OrganizationModel(ci, true)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetOrganization-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult GetForId(int id)
    {
        var organization = _service.FindForId(id);

        if (organization == null) return new NoContentResult();

        return new JsonResult(new OrganizationModel(organization, true));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddOrganization-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult Add(OrganizationModel model)
    {
        var entity = model.ToEntity();
        _service.Add(entity);
        _service.CommitTransaction();

        var result = _service.FindForId(model.Id, true);
        if (result == null) return new BadRequestObjectResult(new ErrorResponseModel("Organization does not exist"));

        return CreatedAtAction(nameof(GetForId), new { id = result.Id }, new OrganizationModel(result, true));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateOrganization-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult Update(OrganizationModel model)
    {
        var entity = model.ToEntity();
        _service.Update(entity);
        _service.CommitTransaction();

        var result = _service.FindForId(model.Id, true);
        if (result == null) return new BadRequestObjectResult(new ErrorResponseModel("Organization does not exist"));
        return new JsonResult(new OrganizationModel(result, true));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpDelete("{id}", Name = "RemoveOrganization-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult Remove(OrganizationModel model)
    {
        var entity = model.ToEntity() ?? throw new NoContentException();
        _service.Remove(entity);
        _service.CommitTransaction();
        return new JsonResult(new OrganizationModel(entity, false));
    }
    #endregion
}
