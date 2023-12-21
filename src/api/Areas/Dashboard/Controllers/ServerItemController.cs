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
/// ServerItemController class, provides endpoints for server items.
/// </summary>
[ClientRoleAuthorize(ClientRole.HSB, ClientRole.Client)]
[ApiController]
[ApiVersion("1.0")]
[Area("dashboard")]
[Route("v{version:apiVersion}/[area]/server-items")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class ServerItemController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IServerItemService _service;
    private readonly IXlsExporter _exporter;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a ServerItemController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="exporter"></param>
    /// <param name="logger"></param>
    public ServerItemController(IServerItemService service, IXlsExporter exporter, ILogger<ServerItemController> logger)
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
    [HttpGet(Name = "GetServerItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ServerItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "ServerItem" })]
    public IActionResult Get()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.ServerItemFilter(query);
        var result = _service.Find(filter.GeneratePredicate(), filter.Sort);
        return new JsonResult(result.Select(ci => new ServerItemModel(ci)));
    }

    // TODO: Limit based on role and tenant.
    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetServerItem-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ServerItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "ServerItem" })]
    public IActionResult GetForId(int id)
    {
        var role = _service.FindForId(id);

        if (role == null) return new NoContentResult();

        return new JsonResult(new ServerItemModel(role));
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