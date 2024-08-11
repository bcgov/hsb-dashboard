using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;
using HSB.Keycloak;
using Microsoft.AspNetCore.Http.Extensions;

namespace HSB.API.Areas.Services.Controllers;

/// <summary>
/// ServerItemController class, provides endpoints for server-items.
/// </summary>
[ClientRoleAuthorize(ClientRole.ServiceNow)]
[ApiController]
[ApiVersion("1.0")]
[Area("services")]
[Route("v{version:apiVersion}/[area]/server-items")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class ServerItemController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IServerItemService _serverItemService;
    private readonly IServerHistoryItemService _serverHistoryItemService;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a ServerItemController.
    /// </summary>
    /// <param name="serverItemService"></param>
    /// <param name="serverHistoryItemService"></param>
    /// <param name="logger"></param>
    public ServerItemController(
        IServerItemService serverItemService,
        IServerHistoryItemService serverHistoryItemService,
        ILogger<ServerItemController> logger)
    {
        _serverItemService = serverItemService;
        _serverHistoryItemService = serverHistoryItemService;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Get all the server items for the specified query filter.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "FindServerItems-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ServerItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerItemFilter(query);

        var serverItems = _serverItemService.Find(filter);
        return new JsonResult(serverItems.Select(ci => new ServerItemModel(ci)));
    }

    /// <summary>
    /// Get the server item for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetServerItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult GetForId(string id)
    {
        var entity = _serverItemService.FindForId(id);

        if (entity == null) return new NoContentResult();

        return new JsonResult(new ServerItemModel(entity));
    }

    /// <summary>
    /// Add a new server item to the database.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddServerItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult Add(ServerItemModel model)
    {
        var entity = model.ToEntity();
        _serverItemService.Add(entity);
        _serverItemService.CommitTransaction();
        return CreatedAtAction(nameof(GetForId), new { id = entity.ServiceNowKey }, new ServerItemModel(entity));
    }

    /// <summary>
    /// Update the provided server item.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="updateTotals"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateServerItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult Update(ServerItemModel model, bool updateTotals = false)
    {
        var entity = model.ToEntity();
        _serverItemService.Update(entity, updateTotals);
        _serverItemService.CommitTransaction();
        return new JsonResult(new ServerItemModel(entity));
    }

    /// <summary>
    /// Delete the specified server item.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpDelete("{id}", Name = "DeleteServerItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult Delete(ServerItemModel model)
    {
        _serverItemService.Remove(model.ToEntity());
        _serverItemService.CommitTransaction();
        return new JsonResult(model);
    }

    /// <summary>
    /// Add the provided server item to the database.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("history", Name = "AddServerHistoryItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerHistoryItemModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Server Item"])]
    public IActionResult Add(ServerHistoryItemModel model)
    {
        var entity = model.ToEntity();
        _serverHistoryItemService.Add(entity);
        _serverHistoryItemService.CommitTransaction();
        return CreatedAtAction(nameof(GetForId), new { id = entity.ServiceNowKey }, new ServerHistoryItemModel(entity));
    }
    #endregion
}
