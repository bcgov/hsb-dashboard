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
    private readonly IFileSystemItemService _service;
    private readonly IAuthorizationHelper _authorization;
    private readonly IXlsExporter _exporter;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a FileSystemItemController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="authorization"></param>
    /// <param name="exporter"></param>
    /// <param name="logger"></param>
    public FileSystemItemController(
        IFileSystemItemService service,
        IAuthorizationHelper authorization,
        IXlsExporter exporter,
        ILogger<FileSystemItemController> logger)
    {
        _service = service;
        _authorization = authorization;
        _exporter = exporter;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    // TODO: Limit based on role and tenant.
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetFileSystemItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<FileSystemItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "File System Item" })]
    public IActionResult Get()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.FileSystemItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var result = _service.Find(filter.GeneratePredicate(), filter.Sort);
            return new JsonResult(result.Select(fsi => new FileSystemItemModel(fsi)));
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _service.FindForUser(user.Id, filter.GeneratePredicate(), filter.Sort);
            return new JsonResult(result.Select(fsi => new FileSystemItemModel(fsi)));
        }
    }

    // TODO: Limit based on role and tenant.
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
    public IActionResult GetForId(int id)
    {
        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var entity = _service.FindForId(id);
            if (entity == null) return new NoContentResult();
            return new JsonResult(new FileSystemItemModel(entity));
        }
        else
        {
            // Only return tenants this user belongs to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var entity = _service.FindForUser(user.Id, (t) => t.Id == id).FirstOrDefault();
            if (entity == null) return Forbid();
            return new JsonResult(new FileSystemItemModel(entity));
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
            var items = _service.Find(a => true);
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
