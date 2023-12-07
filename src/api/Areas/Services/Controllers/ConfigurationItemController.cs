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
/// ConfigurationItemController class, provides endpoints for configuration items.
/// </summary>
[ClientRoleAuthorize(ClientRole.ServiceNow)]
[ApiController]
[ApiVersion("1.0")]
[Area("services")]
[Route("v{version:apiVersion}/[area]/configuration-items")]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Unauthorized)]
[ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.Forbidden)]
public class ConfigurationItemController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly IConfigurationItemService _service;
    #endregion

    #region Constructors
    /// <summary>
    /// Creates a new instance of a ConfigurationItemController object.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public ConfigurationItemController(IConfigurationItemService service, ILogger<ConfigurationItemController> logger)
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
    [HttpGet(Name = "GetConfigurationItems-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ConfigurationItemModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Configuration Item" })]
    public IActionResult Get()
    {
        var configurationItems = _service.Find(ci => true);
        return new JsonResult(configurationItems.Select(ci => new ConfigurationItemModel(ci)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetConfigurationItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ConfigurationItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [SwaggerOperation(Tags = new[] { "Configuration Item" })]
    public IActionResult GetForId(int id)
    {
        var entity = _service.FindForId(id);

        if (entity == null) return new NoContentResult();

        return new JsonResult(new ConfigurationItemModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddConfigurationItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ConfigurationItemModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Configuration Item" })]
    public IActionResult Add(ConfigurationItemModel model)
    {
        var entity = model.ToEntity();
        _service.Add(entity);
        _service.CommitTransaction();
        return CreatedAtAction(nameof(GetForId), new { id = entity.Id }, new ConfigurationItemModel(entity));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut("{id}", Name = "UpdateConfigurationItem-Services")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ConfigurationItemModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponseModel), (int)HttpStatusCode.BadRequest)]
    [SwaggerOperation(Tags = new[] { "Configuration Item" })]
    public IActionResult Update(ConfigurationItemModel model)
    {
        var entity = model.ToEntity();
        _service.Update(entity);
        _service.CommitTransaction();
        return new JsonResult(new ConfigurationItemModel(entity));
    }
    #endregion
}
