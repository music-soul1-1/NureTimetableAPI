using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Controllers.V2;


[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class GroupsController(ILogger<GroupsController> logger, ISQLRepository postgreRepository) : ControllerBase
{
    private readonly ILogger<GroupsController> _logger = logger;
    private readonly ISQLRepository _postgreRepository = postgreRepository;

    [HttpGet]
    [Route("GetAll")]
    [ProducesResponseType<List<GroupDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllGroups()
    {
        var groups = await _postgreRepository.GetGroupsAsync();

        if (groups == null)
        {
            return NoContent();
        }

        return Ok(groups);
    }

    [HttpGet]
    [Route("Get/{id}")]
    [ProducesResponseType<GroupDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetGroup(int id)
    {
        var group = await _postgreRepository.GetGroupAsync(id);

        if (group == null)
        {
            return NoContent();
        }

        return Ok(group);
    }

    [HttpGet]
    [Route("GetByName")]
    [ProducesResponseType<List<GroupDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetGroupsByName([FromQuery(Name = "name")] string name)
    {
        var groups = await _postgreRepository.GetGroupsAsync();

        if (groups == null)
        {
            return NoContent();
        }

        return Ok(groups.Where(g => g.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));
    }

    #region Faculties

    [HttpGet]
    [Route("Faculties/GetAll")]
    [ProducesResponseType<List<GroupsFacultyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllFaculties()
    {
        var faculties = await _postgreRepository.GetGroupsFacultiesAsync();

        if (faculties == null)
        {
            return NoContent();
        }

        return Ok(faculties);
    }

    [HttpGet]
    [Route("Faculties/Get/{id}")]
    [ProducesResponseType<GroupsFacultyDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetFacultyById(int id)
    {
        var faculties = await _postgreRepository.GetGroupsFacultiesAsync();

        if (faculties == null)
        {
            return NoContent();
        }

        return Ok(faculties.Find(f => f.Id == id));
    }

    [HttpGet]
    [Route("Faculties/GetByName")]
    [ProducesResponseType<List<GroupsFacultyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetFacultiesByName([FromQuery(Name = "name")] string name)
    {
        var faculties = await _postgreRepository.GetGroupsFacultiesAsync();

        if (faculties == null)
        {
            return NoContent();
        }

        return Ok(faculties.Where(f => f.FullName.Contains(name, StringComparison.OrdinalIgnoreCase) || 
            f.ShortName.Contains(name, StringComparison.OrdinalIgnoreCase)));
    }

    #endregion
}
