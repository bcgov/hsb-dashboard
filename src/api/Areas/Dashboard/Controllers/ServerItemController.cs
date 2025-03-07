using System.Net;
using System.Net.Mime;

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

using HSB.Core.Models;
using HSB.DAL.Services;
using HSB.Keycloak;
using HSB.Keycloak.Extensions;

using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Extensions.Caching.Memory;
using HSB.Models.Dashboard;
using HSB.Models.Lists;

namespace HSB.API.Areas.Dashboard.Controllers;

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
    private readonly IServerItemService _serverItemService;
    private readonly IServerHistoryItemService _serverHistoryItemService;
    private readonly IAuthorizationHelper _authorization;
    private readonly IXlsExporter _exporter;
    private readonly IMemoryCache _memoryCache;
    private const string HSB_CACHE_KEY = "serverItems-hsb";
    private const string HSB_LIST_CACHE_KEY = "serverItemsList-hsb";
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a ServerItemController.
    /// </summary>
    /// <param name="serverItemService"></param>
    /// <param name="serverHistoryItemService"></param>
    /// <param name="memoryCache"></param>
    /// <param name="authorization"></param>
    /// <param name="exporter"></param>
    public ServerItemController(
        IServerItemService serverItemService,
        IServerHistoryItemService serverHistoryItemService,
        IMemoryCache memoryCache,
        IAuthorizationHelper authorization,
        IXlsExporter exporter)
    {
        _serverItemService = serverItemService;
        _serverHistoryItemService = serverHistoryItemService;
        _memoryCache = memoryCache;
        _authorization = authorization;
        _exporter = exporter;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find all server items for the specified 'filter'.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetServerItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ServerItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            if (_memoryCache.TryGetValue(HSB_CACHE_KEY, out IEnumerable<ServerItemModel>? cachedItems))
            {
                return new JsonResult(cachedItems);
            }
            var result = _serverItemService.Find(filter);
            var model = result.Select(ci => new ServerItemModel(ci));
            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            };
            _memoryCache.Set(HSB_CACHE_KEY, model, cacheOptions);
            return new JsonResult(model);
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
    /// Find all server items for the specified 'filter'.
    /// Return the simple details for each server item.
    /// </summary>
    /// <returns></returns>
    [HttpGet("list", Name = "GetServerItemLists-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ServerItemListModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult FindList()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            if (_memoryCache.TryGetValue(HSB_LIST_CACHE_KEY, out IEnumerable<ServerItemListModel>? cachedItems))
            {
                return new JsonResult(cachedItems);
            }
            var result = _serverItemService.FindList(filter);
            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            };
            _memoryCache.Set(HSB_LIST_CACHE_KEY, result, cacheOptions);
            return new JsonResult(result);
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _serverItemService.FindListForUser(user.Id, filter);
            return new JsonResult(result);
        }
    }

    /// <summary>
    /// Get the server item for the specified 'serviceNowKey'.
    /// </summary>
    /// <param name="serviceNowKey"></param>
    /// <param name="includeFileSystemItems"></param>
    /// <returns></returns>
    [HttpGet("{serviceNowKey}", Name = "GetServerItem-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = ["Server Item"])]
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

    /// <summary>
    /// Find all history for the specified query string parameter filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet("history", Name = "GetServerHistoryItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ServerItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Server Item"])]
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

    /// <summary>
    /// Find all compacthistory for the specified query string parameter filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet("history-compact", Name = "GetCompactServerHistoryItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<CompactServerHistoryItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult FindCompactHistory()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerHistoryItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var result = _serverHistoryItemService.FindCompactHistoryByMonth(filter.StartDate ?? DateTime.UtcNow.AddYears(-1), filter.EndDate, filter.TenantId, filter.OrganizationId, filter.OperatingSystemItemId, filter.ServiceNowKey);
            return new JsonResult(result.Select(si => new CompactServerHistoryItemModel(si)));
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            // TODO: Return compact history here too?
            var result = _serverHistoryItemService.FindHistoryByMonthForUser(user.Id, filter.StartDate ?? DateTime.UtcNow.AddYears(-1), filter.EndDate, filter.TenantId, filter.OrganizationId, filter.OperatingSystemItemId, filter.ServiceNowKey);
            return new JsonResult(result.Select(si => new ServerHistoryItemModel(si)));
        }
    }

    /// <summary>
    /// Export the server items to Excel.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("export")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult Export(string format = "excel", string name = "service-now")
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerItemFilter(query);

        if (format == "excel")
        {
            IEnumerable<Entities.ServerItem> items;
            var isHSB = this.User.HasClientRole(ClientRole.HSB);
            if (isHSB)
            {
                items = _serverItemService.Find(filter, true);
            }
            else
            {
                var user = _authorization.GetUser();
                if (user == null) return Forbid();
                items = _serverItemService.FindForUser(user.Id, filter, true);
            }

            var workbook = _exporter.GenerateExcel(name, items);

            using var stream = new MemoryStream();
            workbook.Write(stream);
            var bytes = stream.ToArray();

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        throw new NotImplementedException("Format 'csv' not implemented yet");
    }

    /// <summary>
    /// Export the server history items to Excel.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("history/export")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult ExportHistory(string format = "excel", string name = "service-now")
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerHistoryItemFilter(query);

        if (format == "excel")
        {
            IEnumerable<Entities.ServerHistoryItemSmall> items;
            var isHSB = this.User.HasClientRole(ClientRole.HSB);
            if (isHSB)
            {
                items = _serverHistoryItemService.FindHistoryByMonth(filter.StartDate ?? DateTime.UtcNow, filter.EndDate, filter.TenantId, filter.OrganizationId, filter.OperatingSystemItemId, filter.ServiceNowKey, true);
            }
            else
            {
                var user = _authorization.GetUser();
                if (user == null) return Forbid();
                items = _serverHistoryItemService.FindHistoryByMonthForUser(user.Id, filter.StartDate ?? DateTime.UtcNow, filter.EndDate, filter.TenantId, filter.OrganizationId, filter.OperatingSystemItemId, filter.ServiceNowKey, true);
            }

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
