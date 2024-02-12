using System.Net;
using System.Net.Mime;
using System.Text.Json;
using HSB.API.CSS;
using HSB.Core.Models;
using HSB.DAL.Services;
using HSB.Keycloak;
using HSB.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace HSB.API.Areas.SystemAdmin.Controllers;

/// <summary>
/// UserController class, provides User endpoints for the admin api.
/// </summary>
[ClientRoleAuthorize(ClientRole.SystemAdministrator, ClientRole.OrganizationAdministrator)]
[ApiController]
[Area("admin")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[area]/users")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class UserController : ControllerBase
{
    #region Variables
    private readonly IUserService _userService;
    private readonly IGroupService _groupService;
    private readonly ICssHelper _cssHelper;
    private readonly JsonSerializerOptions _serializerOptions;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a UserController object, initializes with specified parameters.
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="groupService"></param>
    /// <param name="cssHelper"></param>
    /// <param name="serializerOptions"></param>
    public UserController(
        IUserService userService,
        IGroupService groupService,
        ICssHelper cssHelper,
        IOptions<JsonSerializerOptions> serializerOptions)
    {
        _userService = userService;
        _groupService = groupService;
        _cssHelper = cssHelper;
        _serializerOptions = serializerOptions.Value;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find users for the specified query filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetUsers-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserModel[]), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "User" })]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.UserFilter(query);
        var result = _userService.Find(filter);
        return new JsonResult(result.Select(u => new UserModel(u)).ToArray());
    }

    /// <summary>
    /// Get user for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includePermissions"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetUser-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "User" })]
    public IActionResult GetForId(int id, bool includePermissions)
    {
        var result = _userService.FindForId(id, includePermissions);
        if (result == null) return new NoContentResult();
        return new JsonResult(new UserModel(result));
    }

    /// <summary>
    /// Add user for the specified 'id'.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [ClientRoleAuthorize(ClientRole.SystemAdministrator)]
    [HttpPost(Name = "AddUser-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "User" })]
    public IActionResult Add(UserModel model)
    {
        var user = (Entities.User)model;
        if (String.IsNullOrWhiteSpace(user.Key) || user.Key == Guid.Empty.ToString()) user.Key = Guid.NewGuid().ToString();
        var entry = _userService.Add(user);
        _userService.CommitTransaction();

        var result = _userService.FindForId(model.Id, true);
        if (result == null) return new BadRequestObjectResult(new ErrorResponseModel("User does not exist"));
        return CreatedAtAction(nameof(GetForId), new { id = result.Id }, new UserModel(result));
    }

    /// <summary>
    /// Update user for the specified 'id'.
    /// Update the user in Keycloak if the 'Key' is linked.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateUser-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "User" })]
    public async Task<IActionResult> UpdateAsync(UserModel model)
    {
        var entry = _userService.Update((Entities.User)model);

        // Fetch groups from database to get roles associated with them.
        var roles = new List<string>();
        foreach (var group in entry.Entity.GroupsManyToMany)
        {
            var entity = _groupService.FindForId(group.GroupId);
            if (entity != null)
                roles.AddRange(entity.RolesManyToMany.Select(r => r.Role!.Name));
        }
        roles = roles.Distinct().ToList();

        // TODO: Only update if roles changed
        await _cssHelper.UpdateUserRolesAsync(model.Key.ToString(), roles.ToArray());
        _userService.CommitTransaction();

        var result = _userService.FindForId(model.Id, true);
        if (result == null) return new BadRequestObjectResult(new ErrorResponseModel("User does not exist"));
        return new JsonResult(new UserModel(result));
    }

    /// <summary>
    /// Delete user for the specified 'id'.
    /// Delete the user from Keycloak if the 'Key' is linked.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [ClientRoleAuthorize(ClientRole.SystemAdministrator)]
    [HttpDelete("{id}", Name = "RemoveUser-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "User" })]
    public async Task<IActionResult> DeleteAsync(UserModel model)
    {
        await _cssHelper.DeleteUserAsync((Entities.User)model);
        return new JsonResult(model);
    }

    /// <summary>
    /// Sync users, roles, and claims with Keycloak.
    /// This ensures users, roles, and claims within TNO have their 'Key' linked to Keycloak.
    /// </summary>
    /// <returns></returns>
    [HttpPost("sync")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Keycloak" })]
    public async Task<IActionResult> SyncAsync()
    {
        await _cssHelper.SyncAsync();
        return new OkResult();
    }
    #endregion
}
