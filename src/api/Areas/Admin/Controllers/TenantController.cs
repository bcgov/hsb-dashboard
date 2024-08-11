using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;
using HSB.Keycloak;
using HSB.Core.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;
using HSB.Models.Admin;

namespace HSB.API.Areas.Admin.Controllers;

/// <summary>
/// TenantController class, provides endpoints for tenants.
/// </summary>
[ClientRoleAuthorize(ClientRole.SystemAdministrator, ClientRole.OrganizationAdministrator)]
[ApiController]
[ApiVersion("1.0")]
[Area("admin")]
[Route("v{version:apiVersion}/[area]/tenants")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class TenantController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly ITenantService _service;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a TenantController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public TenantController(ITenantService service, ILogger<TenantController> logger)
    {
        _service = service;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find tenants for the provided query filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetTenants-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<TenantModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Tenant"])]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.TenantFilter(query);
        var result = _service.Find(filter.GeneratePredicate(), filter.Sort);
        return new JsonResult(result.Select(ci => new TenantModel(ci, true)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetTenant-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TenantModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = ["Tenant"])]
    public IActionResult GetForId(int id)
    {
        var tenant = _service.FindForId(id);

        if (tenant == null) return new NoContentResult();

        return new JsonResult(new TenantModel(tenant, true));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddTenant-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TenantModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Tenant"])]
    public IActionResult Add(TenantModel model)
    {
        var entity = model.ToEntity();
        _service.Add(entity);
        _service.CommitTransaction();

        var result = _service.FindForId(entity.Id, true);
        if (result == null) return new BadRequestObjectResult(new ErrorResponseModel("Tenant does not exist"));
        return CreatedAtAction(nameof(GetForId), new { id = result.Id }, new TenantModel(result, true));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateTenant-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TenantModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Tenant"])]
    public IActionResult Update(TenantModel model)
    {
        var original = _service.FindForIdAsNoTracking(model.Id) ?? throw new NoContentException();
        var entity = model.ToEntity();
        entity.RawData = original.RawData;
        _service.Update(entity);
        _service.CommitTransaction();

        var result = _service.FindForId(model.Id, true);
        if (result == null) return new BadRequestObjectResult(new ErrorResponseModel("Tenant does not exist"));
        return new JsonResult(new TenantModel(result, true));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpDelete("{id}", Name = "RemoveTenant-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TenantModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Tenant"])]
    public IActionResult Remove(TenantModel model)
    {
        var entity = model.ToEntity() ?? throw new NoContentException();
        _service.Remove(entity);
        _service.CommitTransaction();
        return new JsonResult(new TenantModel(entity, true));
    }
    #endregion
}
