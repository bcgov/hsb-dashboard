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
[Route("v{version:apiVersion}/{area}/organizations")]
public class OrganizationController : ControllerBase
{
    #region Variables
    private readonly ILogger _logger;
    private readonly HSBContext _context;
    #endregion

    #region Constructors
    public OrganizationController(HSBContext context, ILogger<OrganizationController> logger)
    {
        _context = context;
        _logger = logger;
    }
    #endregion

    #region Endpoints
    [HttpGet(Name = "GetOrganizations")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationModel>), 200)]
    [SwaggerOperation(Tags = ["Organization Item"])]
    public IActionResult Get()
    {
        var configurationItems = _context.Organizations
            .AsNoTracking()
            .ToArray();
        return new JsonResult(configurationItems.Select(ci => new OrganizationModel(ci)));
    }

    [HttpPost(Name = "AddOrganizations")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrganizationModel), 201)]
    [SwaggerOperation(Tags = ["Organization Item"])]
    public IActionResult Add(OrganizationModel model)
    {
        var entity = model.ToEntity();
        _context.Add(entity);
        _context.SaveChanges();
        return new JsonResult(new OrganizationModel(entity));
    }
    #endregion
}
