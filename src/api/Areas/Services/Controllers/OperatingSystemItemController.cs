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
/// OperatingSystemItemController class, provides endpoints for operating-system-items.
/// </summary>
[ClientRoleAuthorize(ClientRole.ServiceNow)]
[ApiController]
[ApiVersion("1.0")]
[Area("services")]
[Route("v{version:apiVersion}/[area]/operating-system-items")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class OperatingSystemItemController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IOperatingSystemItemService _service;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a OperatingSystemItemController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public OperatingSystemItemController(IOperatingSystemItemService service, ILogger<OperatingSystemItemController> logger)
    {
        _service = service;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    /// Find all operating system items.
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "FindOperatingSystemItems-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OperatingSystemItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = ["Operating System Item"])]
    public IActionResult Find()
    {
        var operatingSystemItems = _service.Find(o => true);
        return new JsonResult(operatingSystemItems.Select(ci => new OperatingSystemItemModel(ci)));
    }

    /// <summary>
    /// Get the operating system item for the specified 'id'.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetOperatingSystemItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OperatingSystemItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = ["Operating System Item"])]
    public IActionResult GetForId(int id)
    {
        var entity = _service.FindForId(id);

        if (entity == null) return new NoContentResult();

        return new JsonResult(new OperatingSystemItemModel(entity));
    }

    /// <summary>
    /// Add or update the provided operating system item.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddOrUpdateOperatingSystemItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OperatingSystemItemModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(OperatingSystemItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Operating System Item"])]
    public IActionResult AddOrUpdate(OperatingSystemItemModel model)
    {
        var entity = model.ToEntity();
        var existing = _service.FindForId(model.Id);
        if (existing == null)
        {
            _service.Add(entity);
            _service.CommitTransaction();
            return CreatedAtAction(nameof(GetForId), new { id = entity.Id }, new OperatingSystemItemModel(entity));
        }
        else
        {
            _service.ClearChangeTracker(); // Remove existing from context.
            _service.Update(entity);
            _service.CommitTransaction();
            return new JsonResult(new OperatingSystemItemModel(entity));
        }
    }

    /// <summary>
    /// Update the operating system item.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateOperatingSystemItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OperatingSystemItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = ["Operating System Item"])]
    public IActionResult Update(OperatingSystemItemModel model)
    {
        var entity = model.ToEntity();
        _service.Update(entity);
        _service.CommitTransaction();
        return new JsonResult(new OperatingSystemItemModel(entity));
    }
    #endregion
}
