using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;
using HSB.Keycloak;

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
    private readonly IServerItemService _service;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a ServerItemController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public ServerItemController(IServerItemService service, ILogger<ServerItemController> logger)
    {
        _service = service;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetServerItems-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ServerItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Server Item" })]
    public IActionResult Get()
    {
        var serverItems = _service.Find(o => true);
        return new JsonResult(serverItems.Select(ci => new ServerItemModel(ci)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetServerItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "Server Item" })]
    public IActionResult GetForId(int id)
    {
        var entity = _service.FindForId(id);

        if (entity == null) return new NoContentResult();

        return new JsonResult(new ServerItemModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddServerItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Server Item" })]
    public IActionResult Add(ServerItemModel model)
    {
        var entity = model.ToEntity();
        _service.Add(entity);
        _service.CommitTransaction();
        return CreatedAtAction(nameof(GetForId), new { id = entity.Id }, new ServerItemModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateServerItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Server Item" })]
    public IActionResult Update(ServerItemModel model)
    {
        var entity = model.ToEntity();
        _service.Update(entity);
        _service.CommitTransaction();
        return new JsonResult(new ServerItemModel(entity));
    }
    #endregion
}
