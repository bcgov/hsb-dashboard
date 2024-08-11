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
/// OperatingSystemItemController class, provides endpoints for operating system items.
/// </summary>
[ClientRoleAuthorize(ClientRole.HSB, ClientRole.Client)]
[ApiController]
[ApiVersion("1.0")]
[Area("dashboard")]
[Route("v{version:apiVersion}/[area]/operating-system-items")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class OperatingSystemItemController : ControllerBase
{
    #region Variables
    private readonly IOperatingSystemItemService _service;
    private readonly IAuthorizationHelper _authorization;
    private readonly IXlsExporter _exporter;
    private readonly IMemoryCache _memoryCache;
    private const string HSB_CACHE_KEY = "operatingSystemItems-hsb";
    private const string HSB_LIST_CACHE_KEY = "operatingSystemItemsList-hsb";
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a OperatingSystemItemController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="memoryCache"></param>
    /// <param name="authorization"></param>
    /// <param name="exporter"></param>
    public OperatingSystemItemController(
        IOperatingSystemItemService service,
        IMemoryCache memoryCache,
        IAuthorizationHelper authorization,
        IXlsExporter exporter)
    {
        _service = service;
        _memoryCache = memoryCache;
        _authorization = authorization;
        _exporter = exporter;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find all the operating system items for the specified query string parameter filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetOperatingSystemItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OperatingSystemItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Operating System Item"])]
    // [ResponseCache(VaryByQueryKeys = new[] { "*" }, Location = ResponseCacheLocation.Client, Duration = 60)]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.OperatingSystemItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            if (_memoryCache.TryGetValue(HSB_CACHE_KEY, out IEnumerable<OperatingSystemItemModel>? cachedItems))
            {
                return new JsonResult(cachedItems);
            }
            var result = _service.Find(filter.GeneratePredicate(), filter.Sort);
            var model = result.Select(ci => new OperatingSystemItemModel(ci));
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

            var result = _service.FindForUser(user.Id, filter);
            return new JsonResult(result.Select(fsi => new OperatingSystemItemModel(fsi)));
        }
    }

    /// <summary>
    /// Find all operating system items for the specified 'filter'.
    /// Return the simple details for each operating system item.
    /// </summary>
    /// <returns></returns>
    [HttpGet("list", Name = "GetOperatingSystemItemLists-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OperatingSystemItemListModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Operating System Item"])]
    // [ResponseCache(VaryByQueryKeys = new[] { "*" }, Location = ResponseCacheLocation.Client, Duration = 60)]
    public IActionResult FindList()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.OperatingSystemItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            if (_memoryCache.TryGetValue(HSB_LIST_CACHE_KEY, out IEnumerable<OperatingSystemItemListModel>? cachedItems))
            {
                return new JsonResult(cachedItems);
            }
            var result = _service.FindList(filter);
            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            };
            _memoryCache.Set(HSB_LIST_CACHE_KEY, result, cacheOptions);
            return new JsonResult(result);
        }
        else
        {
            // Only return operating system items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _service.FindListForUser(user.Id, filter);
            return new JsonResult(result);
        }
    }

    /// <summary>
    /// Get the operating system item for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetOperatingSystemItem-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OperatingSystemItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = ["Operating System Item"])]
    public IActionResult GetForId(int id)
    {
        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var entity = _service.FindForId(id);
            if (entity == null) return new NoContentResult();
            return new JsonResult(new OperatingSystemItemModel(entity));
        }
        else
        {
            // Only return tenants this user belongs to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var entity = _service.FindForUser(user.Id, new HSB.Models.Filters.OperatingSystemItemFilter() { Id = id }).FirstOrDefault();
            if (entity == null) return Forbid();
            return new JsonResult(new OperatingSystemItemModel(entity));
        }
    }

    // TODO: Complete functionality
    // TODO: Limit based on role and tenant.
    /// <summary>
    /// Export the operating system items to Excel.
    /// </summary>
    /// <param name="format"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("export")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Operating System Item"])]
    public IActionResult Export(string format, string name = "service-now")
    {
        if (format == "excel")
        {
            var items = _service.Find(a => true);
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
