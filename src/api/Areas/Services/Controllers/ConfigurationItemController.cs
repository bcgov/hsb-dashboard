using System.Net.Mime;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HSB.Core.Models;
using System.Net;
using HSB.DAL.Services;

namespace HSB.API.Areas.Services.Controllers;

/// <summary>
/// ConfigurationItemController class, provides endpoints for configuration items.
/// </summary>
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
    [HttpGet(Name = "GetConfigurationItems")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ConfigurationItemModel>), 200)]
    [SwaggerOperation(Tags = ["Configuration Item"])]
    public IActionResult Get()
    {
        var configurationItems = _service.Find(ci => true);
        return new JsonResult(configurationItems.Select(ci => new ConfigurationItemModel(ci)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost(Name = "AddConfigurationItems")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ConfigurationItemModel), 201)]
    [SwaggerOperation(Tags = ["Configuration Item"])]
    public IActionResult Add(ConfigurationItemModel model)
    {
        var entity = model.ToEntity();
        _service.Add(entity);
        _service.CommitTransaction();
        return new JsonResult(new ConfigurationItemModel(entity));
    }
    #endregion
}
