using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;
using HSB.Keycloak;
using Microsoft.AspNetCore.Http.Extensions;
using HSB.API.Models.Health;

namespace HSB.API.Areas.Services.Controllers;

/// <summary>
/// RefreshMaterializedViewsController class, provides endpoints for refreshing materialized views.
/// </summary>
[ClientRoleAuthorize(ClientRole.ServiceNow)]
[ApiController]
[ApiVersion("1.0")]
[Area("services")]
[Route("v{version:apiVersion}/[area]/refresh-materialized-views")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class RefreshMaterializedViewsController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IRefreshMaterializedViewsService _refreshMaterializedViewsService;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a RefreshMaterializedViewsController.
    /// </summary>
    /// <param name="refreshMaterializedViewsService"></param>
    /// <param name="logger"></param>
    public RefreshMaterializedViewsController(
        IRefreshMaterializedViewsService refreshMaterializedViewsService,
        ILogger<RefreshMaterializedViewsController> logger)
    {
        _refreshMaterializedViewsService = refreshMaterializedViewsService;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Refresh all materialized views.
    /// </summary>
    /// <returns></returns>
    [HttpPost(Name = "RefreshAllMaterializedViews-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(StatusModel), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Refresh Materialized Views"])]
    public IActionResult RefreshAll()
    {
        _refreshMaterializedViewsService.RefreshAll();
        return new JsonResult(new StatusModel("refreshed"));
    }
    #endregion
}
