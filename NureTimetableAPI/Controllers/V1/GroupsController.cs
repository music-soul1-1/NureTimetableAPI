using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Controllers.V1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class GroupsController : ControllerBase
{
    private readonly ILogger<GroupsController> _logger;
    private readonly ISQLRepository _postgreRepository;

    public GroupsController(ILogger<GroupsController> logger, ISQLRepository postgreRepository)
    {
        _postgreRepository = postgreRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route("All")]
    [ProducesResponseType<List<GroupDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetGroups()
    {
        var groups = await _postgreRepository.GetGroupsAsync();

        if (groups == null)
        {
            return NoContent();
        }

        return Ok(groups);
    }

    [HttpGet]
    [Route("Faculties")]
    [ProducesResponseType<List<GroupsFacultyDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetFaculties()
    {
        var faculties = await _postgreRepository.GetGroupsFacultiesAsync();

        if (faculties == null)
        {
            return NoContent();
        }

        return Ok(faculties);
    }

    [HttpGet]
    [Route("{id}")]
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
    [ProducesResponseType<GroupDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetGroupByName([FromQuery(Name = "name")] string name)
    {
        var group = await _postgreRepository.GetGroupAsync(name);

        if (group == null)
        {
            return NoContent();
        }

        return Ok(group);
    }
}
