using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using HSB.DAL;
using HSB.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HSB.API.Areas.Services.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Area("services")]
[Route("v{version:apiVersion}/{area}/configuration-items")]
public class ConfigurationItemController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly HSBContext _context;
    #endregion

    #region Constructors
    public ConfigurationItemController(HSBContext context, ILogger<ConfigurationItemController> logger)
    {
        _context = context;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    [HttpGet(Name = "GetConfigurationItems")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<ConfigurationItemModel>), 200)]
    [SwaggerOperation(Tags = ["Configuration Item"])]
    public IActionResult Get()
    {
        var configurationItems = _context.ConfigurationItems
            .AsNoTracking()
            .Include(ci => ci.Organization)
                .ThenInclude(o => o!.Parent)
                    .ThenInclude(o => o!.Parent)
            .Include(ci => ci.FileSystemItems)
            .Include(ci => ci.ServerItems)
                .ThenInclude(si => si.OperatingSystemItem)
            .ToArray();
        return new JsonResult(configurationItems.Select(ci => new ConfigurationItemModel(ci)));
    }

    [HttpPost(Name = "AddConfigurationItems")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ConfigurationItemModel), 201)]
    [SwaggerOperation(Tags = ["Configuration Item"])]
    public IActionResult Add(ConfigurationItemModel model)
    {
        var entity = model.ToEntity();
        _context.Add(entity);
        _context.SaveChanges();
        return new JsonResult(new ConfigurationItemModel(entity));
    }
    #endregion
}
