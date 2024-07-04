using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class TeachersController : ControllerBase
{
    private readonly ILogger<TeachersController> _logger;
    private readonly ISQLRepository _postgreRepository;

    public TeachersController(ILogger<TeachersController> logger, ISQLRepository postgreRepository)
    {
        _postgreRepository = postgreRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route("All")]
    [ProducesResponseType<List<TeacherDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTeachers()
    {
        var teachers = await _postgreRepository.GetTeachersAsync();

        if (teachers == null)
        {
            return NoContent();
        }

        return Ok(teachers);
    }

    [HttpGet]
    [Route("Faculties")]
    [ProducesResponseType<List<TeachersFacultyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTeachersFaculties()
    {
        var faculties = await _postgreRepository.GetTeachersFacultiesAsync();

        if (faculties == null)
        {
            return NoContent();
        }

        return Ok(faculties);
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType<TeacherDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTeacher(int id)
    {
        var teacher = await _postgreRepository.GetTeacherAsync(id);

        if (teacher == null)
        {
            return NoContent();
        }

        return Ok(teacher);
    }
}
