using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Repositories;


namespace NureTimetableAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupsController : ControllerBase
{
    private readonly ILogger<GroupsController> _logger;
    private readonly IPostgreSQLRepository _postgreRepository;

    public GroupsController(ILogger<GroupsController> logger, IPostgreSQLRepository postgreRepository)
    {
        _postgreRepository = postgreRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route("All")]
    public async Task<IActionResult> GetGroups()
    {
       var groups = await _postgreRepository.GetGroupsAsync();

        if (groups == null)
        {
            return NotFound();
        }

        return Ok(groups);
    }

    [HttpGet]
    [Route("Faculties")]
    public async Task<IActionResult> GetFaculties()
    {
        var faculties = await _postgreRepository.GetGroupsFacultiesAsync();

        if (faculties == null)
        {
            return NotFound();
        }

        return Ok(faculties);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetGroup(int id)
    {
        var group = await _postgreRepository.GetGroupAsync(id);

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }
}
