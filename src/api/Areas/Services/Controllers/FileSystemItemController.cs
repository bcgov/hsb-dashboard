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
/// FileSystemItemController class, provides endpoints for file-system-items.
/// </summary>
[ClientRoleAuthorize(ClientRole.ServiceNow)]
[ApiController]
[ApiVersion("1.0")]
[Area("services")]
[Route("v{version:apiVersion}/[area]/file-system-items")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class FileSystemItemController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IFileSystemItemService _service;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a FileSystemItemController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public FileSystemItemController(IFileSystemItemService service, ILogger<FileSystemItemController> logger)
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
    [HttpGet(Name = "GetFileSystemItems-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<FileSystemItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult Get()
    {
        var fileSystemItems = _service.Find(o => true);
        return new JsonResult(fileSystemItems.Select(ci => new FileSystemItemModel(ci)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetFileSystemItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(FileSystemItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult GetForId(int id)
    {
        var entity = _service.FindForId(id);

        if (entity == null) return new NoContentResult();

        return new JsonResult(new FileSystemItemModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddFileSystemItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(FileSystemItemModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult Add(FileSystemItemModel model)
    {
        var entity = model.ToEntity();
        _service.Add(entity);
        _service.CommitTransaction();
        return CreatedAtAction(nameof(GetForId), new { id = entity.Id }, new FileSystemItemModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateFileSystemItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(FileSystemItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult Update(FileSystemItemModel model)
    {
        var entity = model.ToEntity();
        _service.Update(entity);
        _service.CommitTransaction();
        return new JsonResult(new FileSystemItemModel(entity));
    }
    #endregion
}
