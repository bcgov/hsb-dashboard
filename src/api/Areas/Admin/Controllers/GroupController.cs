using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;
using HSB.Keycloak;
using HSB.Core.Exceptions;
using Microsoft.AspNetCore.Http.Extensions;

namespace HSB.API.Areas.SystemAdmin.Controllers;

/// <summary>
/// GroupController class, provides endpoints for groups.
/// </summary>
[ClientRoleAuthorize(ClientRole.SystemAdministrator, ClientRole.OrganizationAdministrator)]
[ApiController]
[ApiVersion("1.0")]
[Area("admin")]
[Route("v{version:apiVersion}/[area]/groups")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class GroupController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IGroupService _service;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a GroupController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public GroupController(IGroupService service, ILogger<GroupController> logger)
    {
        _service = service;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find groups for the specified query filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetGroups-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<GroupModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Group" })]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.GroupFilter(query);
        var result = _service.Find(filter.GeneratePredicate(), filter.Sort);
        return new JsonResult(result.Select(ci => new GroupModel(ci)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetGroup-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GroupModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "Group" })]
    public IActionResult GetForId(int id)
    {
        var group = _service.FindForId(id);

        if (group == null) return new NoContentResult();

        return new JsonResult(new GroupModel(group));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddGroup-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GroupModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Group" })]
    public IActionResult Add(GroupModel model)
    {
        var entity = model.ToEntity();
        _service.Add(entity);
        _service.CommitTransaction();
        return CreatedAtAction(nameof(GetForId), new { id = entity.Id }, new GroupModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateGroup-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GroupModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Group" })]
    public IActionResult Update(GroupModel model)
    {
        var entity = model.ToEntity();
        _service.Update(entity);
        _service.CommitTransaction();
        return new JsonResult(new GroupModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpDelete("{id}", Name = "RemoveGroup-SystemAdmin")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GroupModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Group" })]
    public IActionResult Remove(GroupModel model)
    {
        var entity = model.ToEntity() ?? throw new NoContentException();
        _service.Remove(entity);
        _service.CommitTransaction();
        return new JsonResult(new GroupModel(entity));
    }
    #endregion
}
