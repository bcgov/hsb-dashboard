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
[ClientRoleAuthorize(ClientRole.SystemAdministrator)]
[ApiController]
[Area("system/admin")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[area]/users")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class UserController : ControllerBase
{
    #region Variables
    private readonly IUserService _userService;
    private readonly ICssHelper _cssHelper;
    private readonly JsonSerializerOptions _serializerOptions;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a UserController object, initializes with specified parameters.
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="cssHelper"></param>
    /// <param name="serializerOptions"></param>
    public UserController(
        IUserService userService,
        ICssHelper cssHelper,
        IOptions<JsonSerializerOptions> serializerOptions)
    {
        _userService = userService;
        _cssHelper = cssHelper;
        _serializerOptions = serializerOptions.Value;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find a page of user for the specified query filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetUsers-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "User" })]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.UserFilter(query);
        var result = _userService.Find(filter.GeneratePredicate(), filter.Sort);
        return new JsonResult(result.Select(u => new UserModel(u)));
    }

    /// <summary>
    /// Get user for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetUser-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "User" })]
    public IActionResult GetForId(int id)
    {
        var result = _userService.FindForId(id);

        if (result == null) return new NoContentResult();

        return new JsonResult(new UserModel(result));
    }

    /// <summary>
    /// Add user for the specified 'id'.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddUser-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "User" })]
    public IActionResult Add(UserModel model)
    {
        var user = (Entities.User)model;
        if (String.IsNullOrWhiteSpace(user.Key) || user.Key == Guid.Empty.ToString()) user.Key = Guid.NewGuid().ToString();
        var result = _userService.Add(user);
        _userService.CommitTransaction();
        var entity = result.Entity;
        return CreatedAtAction(nameof(GetForId), new { id = entity.Id }, new UserModel(entity));
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
        var roles = entry.Entity.Groups.SelectMany(g => g.Roles.Select(r => r.Name)).Distinct().ToArray();

        await _cssHelper.UpdateUserRolesAsync(model.Key.ToString(), roles);
        _userService.CommitTransaction();
        return new JsonResult(new UserModel(entry.Entity));
    }

    /// <summary>
    /// Delete user for the specified 'id'.
    /// Delete the user from Keycloak if the 'Key' is linked.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
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
    #endregion
}
