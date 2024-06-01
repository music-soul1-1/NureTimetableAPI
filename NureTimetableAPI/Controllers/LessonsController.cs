using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Repositories;
using NureTimetableAPI.Models;


namespace NureTimetableAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonsController : ControllerBase
{
    private readonly ILogger<LessonsController> _logger;
    private readonly ICistRepository cistRepository;

    public LessonsController(ILogger<LessonsController> logger, ICistRepository cistRepository)
    {
        _logger = logger;
        this.cistRepository = cistRepository;
    }

    [HttpGet]
    [Route("GetAllGroups")]
    public async Task<List<Group>?> GetAllGroups()
    {
        return await cistRepository.GetGroups();
    }
}
