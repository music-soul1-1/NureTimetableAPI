using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Controllers.V2;


[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class AuditoriesController(ILogger<AuditoriesController> logger, ISQLRepository postgreRepository) : ControllerBase
{
    private readonly ILogger<AuditoriesController> _logger = logger;
    private readonly ISQLRepository _postgreRepository = postgreRepository;

    [HttpGet]
    [Route("GetAll")]
    [ProducesResponseType<List<AuditoryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllAuditories()
    {
        var auditories = await _postgreRepository.GetAuditoriesAsync();

        if (auditories == null)
        {
            return NoContent();
        }

        return Ok(auditories);
    }

    [HttpGet]
    [Route("Get/{id}")]
    [ProducesResponseType<AuditoryDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAuditory(int id)
    {
        var auditory = await _postgreRepository.GetAuditoryAsync(id);

        if (auditory == null)
        {
            return NoContent();
        }

        return Ok(auditory);
    }

    [HttpGet]
    [Route("GetByName")]
    [ProducesResponseType<List<AuditoryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAuditoriesByName([FromQuery(Name = "name")] string name)
    {
        var auditories = await _postgreRepository.GetAuditoriesAsync();

        if (auditories == null)
        {
            return NoContent();
        }
        
        return Ok(auditories.Where(a => a.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));
    }

    #region Buildings

    [HttpGet]
    [Route("Buildings/GetAll")]
    [ProducesResponseType<List<BuildingDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllBuildings()
    {
        var buildings = await _postgreRepository.GetBuildingsAsync();

        if (buildings == null)
        {
            return NoContent();
        }

        return Ok(buildings);
    }

    [HttpGet]
    [Route("Buildings/Get/{id}")]
    [ProducesResponseType<BuildingDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetBuildingById(string id)
    {
        var buildings = await _postgreRepository.GetBuildingsAsync();

        if (buildings == null)
        {
            return NoContent();
        }

        return Ok(buildings.Find(b => b.Id == id));
    }

    [HttpGet]
    [Route("Buildings/GetByName")]
    [ProducesResponseType<List<BuildingDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetBuildingsByName([FromQuery(Name = "name")] string name)
    {
        var buildings = await _postgreRepository.GetBuildingsAsync();

        if (buildings == null)
        {
            return NoContent();
        }

        return Ok(buildings.Where(b => b.FullName.Contains(name, StringComparison.OrdinalIgnoreCase) || 
            b.ShortName.Contains(name, StringComparison.OrdinalIgnoreCase)));
    }

    #endregion
}
