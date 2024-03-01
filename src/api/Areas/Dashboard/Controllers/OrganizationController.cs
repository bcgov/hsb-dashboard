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
using Microsoft.Extensions.Caching.Memory;

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
    private readonly IMemoryCache _memoryCache;
    private const string HSB_CACHE_KEY = "organizations-hsb";
    private const string HSB_LIST_CACHE_KEY = "organizationsList-hsb";
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a OrganizationController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="memoryCache"></param>
    /// <param name="authorization"></param>
    /// <param name="logger"></param>
    public OrganizationController(
        IOrganizationService service,
        IMemoryCache memoryCache,
        IAuthorizationHelper authorization,
        ILogger<OrganizationController> logger)
    {
        _service = service;
        _memoryCache = memoryCache;
        _authorization = authorization;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find all organizations that match the specified query filter.
    /// Only returns organizations the current user has access to.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetOrganizations-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, Location = ResponseCacheLocation.Client, Duration = 60)]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.OrganizationFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            if (_memoryCache.TryGetValue(HSB_CACHE_KEY, out IEnumerable<OrganizationModel>? cachedItems))
            {
                return new JsonResult(cachedItems);
            }
            var result = _service.Find(filter);
            var model = result.Select(ci => new OrganizationModel(ci, true));
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
            return new JsonResult(result.Select(o => new OrganizationModel(o, true)));
        }
    }

    /// <summary>
    /// Find all organizations for the specified 'filter'.
    /// Return the simple details for each organizations.
    /// </summary>
    /// <returns></returns>
    [HttpGet("list", Name = "GetOrganizationLists-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationListModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Organization" })]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, Location = ResponseCacheLocation.Client, Duration = 60)]
    public IActionResult FindList()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.OrganizationFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            if (_memoryCache.TryGetValue(HSB_LIST_CACHE_KEY, out IEnumerable<OrganizationListModel>? cachedItems))
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
            // Only return organizations this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _service.FindListForUser(user.Id, filter);
            return new JsonResult(result);
        }
    }

    /// <summary>
    /// Get the organization for the specified 'id'.
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
            return new JsonResult(new OrganizationModel(entity, true));
        }
        else
        {
            // Only return tenants this user belongs to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var entity = _service.FindForUser(user.Id, new HSB.Models.Filters.OrganizationFilter() { Id = id }).FirstOrDefault();
            if (entity == null) return Forbid();
            return new JsonResult(new OrganizationModel(entity, true));
        }
    }
    #endregion
}
