using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Controllers.V2;


[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class TeachersController(ILogger<TeachersController> logger, ISQLRepository sqlRepository) : ControllerBase
{
    private readonly ILogger<TeachersController> _logger = logger;
    private readonly ISQLRepository _sqlRepository = sqlRepository;

    [HttpGet]
    [Route("GetAll")]
    [ProducesResponseType<List<TeacherDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await _sqlRepository.GetTeachersAsync();

        if (teachers == null)
        {
            return NoContent();
        }

        return Ok(teachers);
    }

    [HttpGet]
    [Route("Get/{id}")]
    [ProducesResponseType<TeacherDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTeacherById(int id)
    {
        var teacher = await _sqlRepository.GetTeacherAsync(id);

        if (teacher == null)
        {
            return NoContent();
        }

        return Ok(teacher);
    }

    [HttpGet]
    [Route("GetByName")]
    [ProducesResponseType<List<TeacherDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTeachersByName([FromQuery(Name = "name")] string name)
    {
        var teachers = await _sqlRepository.GetTeachersAsync();

        if (teachers == null)
        {
            return NoContent();
        }

        return Ok(teachers.Where(t => t.FullName.Contains(name, StringComparison.OrdinalIgnoreCase) || 
            t.ShortName.Contains(name, StringComparison.OrdinalIgnoreCase)));
    }

    #region Faculties

    [HttpGet]
    [Route("Faculties/GetAll")]
    [ProducesResponseType<List<TeachersFacultyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTeachersFaculties()
    {
        var faculties = await _sqlRepository.GetTeachersFacultiesAsync();

        if (faculties == null)
        {
            return NoContent();
        }

        return Ok(faculties);
    }

    [HttpGet]
    [Route("Faculties/Get/{id}")]
    [ProducesResponseType<TeachersFacultyDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTeachersFacultyById(int id)
    {
        var faculties = await _sqlRepository.GetTeachersFacultiesAsync();

        if (faculties == null)
        {
            return NoContent();
        }

        return Ok(faculties.Find(f => f.Id == id));
    }

    [HttpGet]
    [Route("Faculties/GetByName")]
    [ProducesResponseType<List<TeachersFacultyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetTeachersFacultiesByName([FromQuery(Name = "name")] string name)
    {
        var faculties = await _sqlRepository.GetTeachersFacultiesAsync();

        if (faculties == null)
        {
            return NoContent();
        }

        return Ok(faculties.Where(f => f.FullName.Contains(name, StringComparison.OrdinalIgnoreCase) || 
            f.ShortName.Contains(name, StringComparison.OrdinalIgnoreCase)));
    }

    #endregion
}
