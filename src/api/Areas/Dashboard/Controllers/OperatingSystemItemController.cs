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
/// OperatingSystemItemController class, provides endpoints for operating system items.
/// </summary>
[ClientRoleAuthorize(ClientRole.HSB, ClientRole.Client)]
[ApiController]
[ApiVersion("1.0")]
[Area("dashboard")]
[Route("v{version:apiVersion}/[area]/operating-system-items")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class OperatingSystemItemController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IOperatingSystemItemService _service;
    private readonly IAuthorizationHelper _authorization;
    private readonly IXlsExporter _exporter;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a OperatingSystemItemController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="authorization"></param>
    /// <param name="exporter"></param>
    /// <param name="logger"></param>
    public OperatingSystemItemController(
        IOperatingSystemItemService service,
        IAuthorizationHelper authorization,
        IXlsExporter exporter,
        ILogger<OperatingSystemItemController> logger)
    {
        _service = service;
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
    [HttpGet(Name = "GetOperatingSystemItems-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OperatingSystemItemModel>), (int)HttpStatusCode.OK)]
    [SwaggerOperation(Tags = new[] { "Operating System Item" })]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, Location = ResponseCacheLocation.Client, Duration = 60)]
    public IActionResult Find()
    {
        var uri = new Uri(this.Request.GetDisplayUrl());
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        var filter = new HSB.Models.Filters.OperatingSystemItemFilter(query);

        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var result = _service.Find(filter.GeneratePredicate(), filter.Sort);
            return new JsonResult(result.Select(fsi => new OperatingSystemItemModel(fsi)));
        }
        else
        {
            // Only return server items this user has access to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var result = _service.FindForUser(user.Id, filter.GeneratePredicate(), filter.Sort);
            return new JsonResult(result.Select(fsi => new OperatingSystemItemModel(fsi)));
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetOperatingSystemItem-Dashboard")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OperatingSystemItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "Operating System Item" })]
    public IActionResult GetForId(int id)
    {
        var isHSB = this.User.HasClientRole(ClientRole.HSB);
        if (isHSB)
        {
            var entity = _service.FindForId(id);
            if (entity == null) return new NoContentResult();
            return new JsonResult(new OperatingSystemItemModel(entity));
        }
        else
        {
            // Only return tenants this user belongs to.
            var user = _authorization.GetUser();
            if (user == null) return Forbid();

            var entity = _service.FindForUser(user.Id, (t) => t.Id == id, osi => osi.Id).FirstOrDefault();
            if (entity == null) return Forbid();
            return new JsonResult(new OperatingSystemItemModel(entity));
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
    [SwaggerOperation(Tags = new[] { "Operating System Item" })]
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
