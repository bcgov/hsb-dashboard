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

namespace HSB.API.Areas.SystemAdmin.Controllers;

/// <summary>
/// TenantController class, provides endpoints for tenants.
/// </summary>
[ClientRoleAuthorize(ClientRole.SystemAdministrator)]
[ApiController]
[ApiVersion("1.0")]
[Area("system/admin")]
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
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetTenants-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<TenantModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Tenant" })]
    public IActionResult Get()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.TenantFilter(query);
        var result = _service.Find(filter.GeneratePredicate(), filter.Sort);
        return new JsonResult(result.Select(ci => new TenantModel(ci)));
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
    [SwaggerOperation(Tags = new[] { "Tenant" })]
    public IActionResult GetForId(int id)
    {
        var tenant = _service.FindForId(id);

        if (tenant == null) return new NoContentResult();

        return new JsonResult(new TenantModel(tenant));
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
    [SwaggerOperation(Tags = new[] { "Tenant" })]
    public IActionResult Add(TenantModel model)
    {
        var entity = model.ToEntity();
        _service.Add(entity);
        _service.CommitTransaction();
        return CreatedAtAction(nameof(GetForId), new { id = entity.Id }, new TenantModel(entity));
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
    [SwaggerOperation(Tags = new[] { "Tenant" })]
    public IActionResult Update(TenantModel model)
    {
        var entity = model.ToEntity();
        _service.Update(entity);
        _service.CommitTransaction();
        return new JsonResult(new TenantModel(entity));
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
    [SwaggerOperation(Tags = new[] { "Tenant" })]
    public IActionResult Remove(TenantModel model)
    {
        var entity = model.ToEntity() ?? throw new NoContentException();
        _service.Remove(entity);
        _service.CommitTransaction();
        return new JsonResult(new TenantModel(entity));
    }
    #endregion
}
