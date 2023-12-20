using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;
using HSB.Keycloak;
using Microsoft.AspNetCore.Http.Extensions;

namespace HSB.API.Areas.Hsb.Controllers;

/// <summary>
/// TenantController class, provides endpoints for tenants.
/// </summary>
[ClientRoleAuthorize(ClientRole.HSB, ClientRole.Client)]
[ApiController]
[ApiVersion("1.0")]
[Area("dashboard")]
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
    // TODO: Limit based on role and tenant.
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetTenants-Dashboard")]
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

    // TODO: Limit based on role and tenant.
    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetTenant-Dashboard")]
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
    #endregion
}
