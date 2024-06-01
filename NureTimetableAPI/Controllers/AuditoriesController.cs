using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuditoriesController(ILogger<AuditoriesController> logger, IPostgreSQLRepository postgreRepository) : ControllerBase
{
    private readonly ILogger<AuditoriesController> _logger = logger;
    private readonly IPostgreSQLRepository _postgreRepository = postgreRepository;

    [HttpGet]
    [Route("All")]
    public async Task<IActionResult> GetAuditories()
    {
        var auditories = await _postgreRepository.GetAuditoriesAsync();

        if (auditories == null)
        {
            return NotFound();
        }

        return Ok(auditories);
    }

    [HttpGet]
    [Route("Buildings")]
    public async Task<IActionResult> GetBuildings()
    {
        var buildings = await _postgreRepository.GetBuildingsAsync();

        if (buildings == null)
        {
            return NotFound();
        }

        return Ok(buildings);
    }
}
