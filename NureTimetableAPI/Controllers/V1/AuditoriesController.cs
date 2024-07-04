using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Repositories;
using System.Collections.Generic;

namespace NureTimetableAPI.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class AuditoriesController(ILogger<AuditoriesController> logger, ISQLRepository postgreRepository) : ControllerBase
{
    private readonly ILogger<AuditoriesController> _logger = logger;
    private readonly ISQLRepository _postgreRepository = postgreRepository;

    [HttpGet]
    [Route("All")]
    [ProducesResponseType<List<AuditoryDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAuditories()
    {
        var auditories = await _postgreRepository.GetAuditoriesAsync();

        if (auditories == null)
        {
            return NoContent();
        }

        return Ok(auditories);
    }

    [HttpGet]
    [Route("Buildings")]
    [ProducesResponseType<List<BuildingDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetBuildings()
    {
        var buildings = await _postgreRepository.GetBuildingsAsync();

        if (buildings == null)
        {
            return NoContent();
        }

        return Ok(buildings);
    }

    [HttpGet]
    [Route("{id}")]
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
}
