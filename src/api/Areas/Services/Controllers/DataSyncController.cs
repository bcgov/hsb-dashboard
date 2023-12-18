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
/// DataSyncController class, provides endpoints for tenants.
/// </summary>
[ClientRoleAuthorize(ClientRole.ServiceNow)]
[ApiController]
[ApiVersion("1.0")]
[Area("services")]
[Route("v{version:apiVersion}/[area]/data-syncs")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class DataSyncController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IDataSyncService _service;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a DataSyncController.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public DataSyncController(IDataSyncService service, ILogger<DataSyncController> logger)
    {
        _service = service;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [HttpGet("{name:alpha}", Name = "GetDataSync-Services-Name")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(DataSyncModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "DataSync Item" })]
    public IActionResult GetForName(string name)
    {
        var entity = _service.Find((ds) => ds.Name == name).FirstOrDefault();

        if (entity == null) return new NoContentResult();

        return new JsonResult(new DataSyncModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateDataSync-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(DataSyncModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "DataSync Item" })]
    public IActionResult Update(DataSyncModel model)
    {
        var entity = model.ToEntity();
        _service.Update(entity);
        _service.CommitTransaction();
        return new JsonResult(new DataSyncModel(entity));
    }
    #endregion
}
