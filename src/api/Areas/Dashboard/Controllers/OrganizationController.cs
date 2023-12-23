using System.Net;
using System.Net.Mime;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

using HSB.Core.Models;
using HSB.DAL.Services;
using HSB.Keycloak;
using HSB.Keycloak.Extensions;
using HSB.Models;

using Swashbuckle.AspNetCore.Annotations;

namespace HSB.API.Areas.Hsb.Controllers;

/// <summary>
/// OrganizationController class, provides endpoints for organizations.
/// </summary>
[ClientRoleAuthorize(ClientRole.HSB, ClientRole.Client)]
[ApiController]
[ApiVersion("1.0")]
[Area("dashboard")]
[Route("v{version:apiVersion}/[area]/organizations")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class OrganizationController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IOrganizationService _service;
    private readonly IAuthorizationHelper _authorization;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a OrganizationController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="authorization"></param>
    /// <param name="logger"></param>
    public OrganizationController(
        IOrganizationService service,
        IAuthorizationHelper authorization,
        ILogger<OrganizationController> logger)
    {
        _service = service;
        _authorization = authorization;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    // TODO: Limit based on role and tenant.
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetOrganizations-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.OrganizationFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var result = _service.Find(filter.GeneratePredicate(), filter.Sort);
            return new JsonResult(result.Select(o => new OrganizationModel(o)));
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _service.FindForUser(user.Id, filter.GeneratePredicate(), filter.Sort);
            return new JsonResult(result.Select(o => new OrganizationModel(o)));
        }
    }

    // TODO: Limit based on role and tenant.
    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetOrganization-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    public IActionResult GetForId(int id)
    {
        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var entity = _service.FindForId(id);
            if (entity == null) return new NoContentResult();
            return new JsonResult(new OrganizationModel(entity));
        }
        else
        {
            // Only return tenants this user belongs to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var entity = _service.FindForUser(user.Id, (o) => o.Id == id, o => o.Id).FirstOrDefault();
            if (entity == null) return Forbid();
            return new JsonResult(new OrganizationModel(entity));
        }
    }
    #endregion
}
