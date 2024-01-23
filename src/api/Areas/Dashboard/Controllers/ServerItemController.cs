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
/// ServerItemController class, provides endpoints for server items.
/// </summary>
[ClientRoleAuthorize(ClientRole.HSB, ClientRole.Client)]
[ApiController]
[ApiVersion("1.0")]
[Area("dashboard")]
[Route("v{version:apiVersion}/[area]/server-items")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class ServerItemController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IServerItemService _serverItemService;
    private readonly IServerHistoryItemService _serverHistoryItemService;
    private readonly IAuthorizationHelper _authorization;
    private readonly IXlsExporter _exporter;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a ServerItemController.
    /// </summary>
    /// <param name="serverItemService"></param>
    /// <param name="serverHistoryItemService"></param>
    /// <param name="authorization"></param>
    /// <param name="exporter"></param>
    /// <param name="logger"></param>
    public ServerItemController(
        IServerItemService serverItemService,
        IServerHistoryItemService serverHistoryItemService,
        IAuthorizationHelper authorization,
        IXlsExporter exporter,
        ILogger<ServerItemController> logger)
    {
        _serverItemService = serverItemService;
        _serverHistoryItemService = serverHistoryItemService;
        _authorization = authorization;
        _exporter = exporter;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetServerItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ServerItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Server Item" })]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, Location = ResponseCacheLocation.Client, Duration = 60)]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var result = _serverItemService.Find(filter);
            return new JsonResult(result.Select(si => new ServerItemModel(si)));
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _serverItemService.FindForUser(user.Id, filter);
            return new JsonResult(result.Select(si => new ServerItemModel(si)));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet("simple", Name = "GetSimpleServerItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ServerItemSmallModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Server Item" })]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, Location = ResponseCacheLocation.Client, Duration = 60)]
    public IActionResult FindSimple()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var result = _serverItemService.FindSimple(filter);
            return new JsonResult(result);
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _serverItemService.FindSimpleForUser(user.Id, filter);
            return new JsonResult(result);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="serviceNowKey"></param>
    /// <param name="includeFileSystemItems"></param>
    /// <returns></returns>
    [HttpGet("{serviceNowKey}", Name = "GetServerItem-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "Server Item" })]
    public IActionResult GetForId(string serviceNowKey, bool includeFileSystemItems = false)
    {
        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var entity = _serverItemService.FindForId(serviceNowKey, includeFileSystemItems);
            if (entity == null) return new NoContentResult();
            return new JsonResult(new ServerItemModel(entity));
        }
        else
        {
            // Only return tenants this user belongs to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var entity = _serverItemService.FindForId(serviceNowKey, user.Id, includeFileSystemItems);
            if (entity == null) return Forbid();
            return new JsonResult(new ServerItemModel(entity));
        }
    }

    // TODO: Limit based on role and tenant.
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet("history", Name = "GetServerHistoryItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ServerItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Server Item" })]
    public IActionResult FindHistory()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerHistoryItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var result = _serverHistoryItemService.FindHistoryByMonth(filter.StartDate ?? DateTime.UtcNow.AddYears(-1), filter.EndDate, filter.TenantId, filter.OrganizationId, filter.OperatingSystemItemId, filter.ServiceNowKey);
            return new JsonResult(result.Select(si => new ServerHistoryItemModel(si)));
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _serverHistoryItemService.FindHistoryByMonthForUser(user.Id, filter.StartDate ?? DateTime.UtcNow.AddYears(-1), filter.EndDate, filter.TenantId, filter.OrganizationId, filter.OperatingSystemItemId, filter.ServiceNowKey);
            return new JsonResult(result.Select(si => new ServerHistoryItemModel(si)));
        }
    }

    // TODO: Complete functionality
    // TODO: Limit based on role and tenant.
    /// <summary>
    ///
    /// </summary>
    /// <param name="format"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("export")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Server Item" })]
    public IActionResult Export(string format, string name = "service-now")
    {
        if (format == "excel")
        {
            var items = _serverItemService.Find(a => true);
            var workbook = _exporter.GenerateExcel(name, items);

            using var stream = new MemoryStream();
            workbook.Write(stream);
            var bytes = stream.ToArray();

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        throw new NotImplementedException("Format 'csv' not implemented yet");
    }
    #endregion
}
