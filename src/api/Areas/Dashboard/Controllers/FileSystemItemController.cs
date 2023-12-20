using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;
using HSB.Keycloak;
using Microsoft.AspNetCore.Http.Extensions;

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
    private readonly IXlsExporter _exporter;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a FileSystemItemController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="exporter"></param>
    /// <param name="logger"></param>
    public FileSystemItemController(IFileSystemItemService service, IXlsExporter exporter, ILogger<FileSystemItemController> logger)
    {
        _service = service;
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
    [SwaggerOperation(Tags = new[] { "FileSystemItem" })]
    public IActionResult Get()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.FileSystemItemFilter(query);
        var result = _service.Find(filter.GeneratePredicate(), filter.Sort);
        return new JsonResult(result.Select(ci => new FileSystemItemModel(ci)));
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
    [SwaggerOperation(Tags = new[] { "FileSystemItem" })]
    public IActionResult GetForId(int id)
    {
        var tenant = _service.FindForId(id);

        if (tenant == null) return new NoContentResult();

        return new JsonResult(new FileSystemItemModel(tenant));
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
    [SwaggerOperation(Tags = new[] { "ConfigurationItem" })]
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
