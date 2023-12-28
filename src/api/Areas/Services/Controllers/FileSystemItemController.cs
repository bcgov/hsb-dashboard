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
    private readonly IFileSystemItemService _fileSystemItemService;
    private readonly IFileSystemHistoryItemService _fileSystemHistoryItemService;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a FileSystemItemController.
    /// </summary>
    /// <param name="fileSystemItemService"></param>
    /// <param name="fileSystemHistoryItemService"></param>
    /// <param name="logger"></param>
    public FileSystemItemController(
        IFileSystemItemService fileSystemItemService,
        IFileSystemHistoryItemService fileSystemHistoryItemService,
        ILogger<FileSystemItemController> logger)
    {
        _fileSystemItemService = fileSystemItemService;
        _fileSystemHistoryItemService = fileSystemHistoryItemService;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "FindFileSystemItems-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<FileSystemItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult Find()
    {
        var fileSystemItems = _fileSystemItemService.Find(o => true);
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
    public IActionResult GetForId(string id)
    {
        var entity = _fileSystemItemService.FindForId(id);

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
        _fileSystemItemService.Add(entity);
        _fileSystemItemService.CommitTransaction();
        return CreatedAtAction(nameof(GetForId), new { id = entity.ServiceNowKey }, new FileSystemItemModel(entity));
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
        _fileSystemItemService.Update(entity);
        _fileSystemItemService.CommitTransaction();
        return new JsonResult(new FileSystemItemModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("history", Name = "AddFileSystemHistoryItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(FileSystemHistoryItemModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult Add(FileSystemHistoryItemModel model)
    {
        var entity = model.ToEntity();
        _fileSystemHistoryItemService.Add(entity);
        _fileSystemHistoryItemService.CommitTransaction();
        return CreatedAtAction(nameof(GetForId), new { id = entity.ServiceNowKey }, new FileSystemHistoryItemModel(entity));
    }
    #endregion
}
