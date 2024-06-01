using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeachersController : ControllerBase
{
    private readonly ILogger<TeachersController> _logger;
    private readonly IPostgreSQLRepository _postgreRepository;

    public TeachersController(ILogger<TeachersController> logger, IPostgreSQLRepository postgreRepository)
    {
        _postgreRepository = postgreRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route("All")]
    public async Task<IActionResult> GetTeachers()
    {
        var teachers = await _postgreRepository.GetTeachersAsync();

        if (teachers == null)
        {
            return NotFound();
        }

        return Ok(teachers);
    }
}
