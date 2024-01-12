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

namespace HSB.API.Areas.Hsb.Controllers;

/// <summary>
/// FileSystemItemController class, provides endpoints for file system items.
/// </summary>
[ClientRoleAuthorize(ClientRole.HSB, ClientRole.Client)]
[ApiController]
[ApiVersion("1.0")]
[Area("dashboard")]
[Route("v{version:apiVersion}/[area]/file-system-items")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class FileSystemItemController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IFileSystemItemService _fileSystemItemService;
    private readonly IFileSystemHistoryItemService _fileSystemHistoryItemService;
    private readonly IAuthorizationHelper _authorization;
    private readonly IXlsExporter _exporter;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a FileSystemItemController.
    /// </summary>
    /// <param name="fileSystemItemService"></param>
    /// <param name="fileSystemHistoryItemService"></param>
    /// <param name="authorization"></param>
    /// <param name="exporter"></param>
    /// <param name="logger"></param>
    public FileSystemItemController(
        IFileSystemItemService fileSystemItemService,
        IFileSystemHistoryItemService fileSystemHistoryItemService,
        IAuthorizationHelper authorization,
        IXlsExporter exporter,
        ILogger<FileSystemItemController> logger)
    {
        _fileSystemItemService = fileSystemItemService;
        _fileSystemHistoryItemService = fileSystemHistoryItemService;
        _authorization = authorization;
        _exporter = exporter;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetFileSystemItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<FileSystemItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.FileSystemItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var result = _fileSystemItemService.Find(filter.GeneratePredicate(), filter.Sort);
            return new JsonResult(result.Select(fsi => new FileSystemItemModel(fsi)));
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _fileSystemItemService.FindForUser(user.Id, filter);
            return new JsonResult(result.Select(fsi => new FileSystemItemModel(fsi)));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetFileSystemItem-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(FileSystemItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult GetForId(string id)
    {
        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var entity = _fileSystemItemService.FindForId(id);
            if (entity == null) return new NoContentResult();
            return new JsonResult(new FileSystemItemModel(entity));
        }
        else
        {
            // Only return file systems items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var entity = _fileSystemItemService.FindForId(id, user.Id);
            if (entity == null) return Forbid();
            return new JsonResult(new FileSystemItemModel(entity));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet("history", Name = "GetFileSystemHistoryItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<FileSystemHistoryItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult FindHistory()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.FileSystemHistoryItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var result = _fileSystemHistoryItemService.FindHistoryByMonth(filter.StartDate ?? DateTime.UtcNow.AddYears(-1), filter.EndDate, filter.TenantId, filter.OrganizationId, filter.OperatingSystemItemId, filter.ServerItemServiceNowKey);
            return new JsonResult(result.Select(fsi => new FileSystemHistoryItemModel(fsi)));
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _fileSystemHistoryItemService.FindHistoryByMonthForUser(user.Id, filter.StartDate ?? DateTime.UtcNow.AddYears(-1), filter.EndDate, filter.TenantId, filter.OrganizationId, filter.OperatingSystemItemId, filter.ServerItemServiceNowKey);
            return new JsonResult(result.Select(fsi => new FileSystemHistoryItemModel(fsi)));
        }
    }

    // TODO: Complete functionality
    // TODO: Limit based on role and tenant.
    /// <summary>
    ///
    /// </summary>
    /// <param name="format"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("export")]
    [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult Export(string format, string name = "service-now")
    {
        if (format == "excel")
        {
            var items = _fileSystemItemService.Find(a => true);
            var workbook = _exporter.GenerateExcel(name, items);

            using var stream = new MemoryStream();
            workbook.Write(stream);
            var bytes = stream.ToArray();

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        throw new NotImplementedException("Format 'csv' not implemented yet");
    }
    #endregion
}
