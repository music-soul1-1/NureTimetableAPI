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

    [HttpGet]
    [Route("Faculties")]
    public async Task<IActionResult> GetTeachersFaculties()
    {
        var faculties = await _postgreRepository.GetTeachersFacultiesAsync();

        if (faculties == null)
        {
            return NotFound();
        }

        return Ok(faculties);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetTeacher(int id)
    {
        var teacher = await _postgreRepository.GetTeacherAsync(id);

        if (teacher == null)
        {
            return NotFound();
        }

        return Ok(teacher);
    }
}
