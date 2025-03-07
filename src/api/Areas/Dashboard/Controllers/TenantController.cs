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
    private readonly ITenantService _tenantService;
    private readonly IAuthorizationHelper _authorization;
    private readonly IMemoryCache _memoryCache;
    private const string HSB_CACHE_KEY = "tenants-hsb";
    private const string HSB_LIST_CACHE_KEY = "tenantsList-hsb";
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a TenantController.
    /// </summary>
    /// <param name="tenantService"></param>
    /// <param name="memoryCache"></param>
    /// <param name="authorization"></param>
    public TenantController(
        ITenantService tenantService,
        IMemoryCache memoryCache,
        IAuthorizationHelper authorization)
    {
        _tenantService = tenantService;
        _memoryCache = memoryCache;
        _authorization = authorization;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find all tenants for the specified query string filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetTenants-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<TenantModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Tenant"])]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.TenantFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            if (_memoryCache.TryGetValue(HSB_CACHE_KEY, out IEnumerable<TenantModel>? cachedItems))
            {
                return new JsonResult(cachedItems);
            }
            var result = _tenantService.Find(filter.GeneratePredicate(), filter.Sort);
            var model = result.Select(ci => new TenantModel(ci, true));
            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            };
            _memoryCache.Set(HSB_CACHE_KEY, model, cacheOptions);
            return new JsonResult(model);
        }
        else
        {
            // Only return tenants this user belongs to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _tenantService.FindForUser(user.Id, filter);
            return new JsonResult(result.Select(ci => new TenantModel(ci, true)));
        }
    }

    /// <summary>
    /// Find all tenants for the specified 'filter'.
    /// Return the simple details for each tenants.
    /// </summary>
    /// <returns></returns>
    [HttpGet("list", Name = "GetTenantLists-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<TenantListModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Tenant"])]
    public IActionResult FindList()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.TenantFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            if (_memoryCache.TryGetValue(HSB_LIST_CACHE_KEY, out IEnumerable<TenantListModel>? cachedItems))
            {
                return new JsonResult(cachedItems);
            }
            var result = _tenantService.FindList(filter);
            var cacheOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            };
            _memoryCache.Set(HSB_LIST_CACHE_KEY, result, cacheOptions);
            return new JsonResult(result);
        }
        else
        {
            // Only return tenants this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _tenantService.FindListForUser(user.Id, filter);
            return new JsonResult(result);
        }
    }

    /// <summary>
    /// Get the tenant for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetTenant-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(TenantModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = ["Tenant"])]
    public IActionResult GetForId(int id)
    {
        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var entity = _tenantService.FindForId(id);
            if (entity == null) return new NoContentResult();
            return new JsonResult(new TenantModel(entity, true));
        }
        else
        {
            // Only return tenants this user belongs to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var entity = _tenantService.FindForUser(user.Id, new HSB.Models.Filters.TenantFilter() { Id = id }).FirstOrDefault();
            if (entity == null) return Forbid();
            return new JsonResult(new TenantModel(entity, true));
        }
    }
    #endregion
}
