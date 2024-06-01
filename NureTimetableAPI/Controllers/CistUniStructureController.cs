using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Models;
using NureTimetableAPI.Models.Cist;
using NureTimetableAPI.Repositories;


namespace NureTimetableAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CistUniStructureController : ControllerBase
{
    private readonly ILogger<CistUniStructureController> _logger;
    private readonly ICistRepository cistRepository;

    public CistUniStructureController(ILogger<CistUniStructureController> logger, ICistRepository cistRepository)
    {
        _logger = logger;
        this.cistRepository = cistRepository;
    }

    [HttpGet]
    [Route("GetGroupsFaculties")]
    public async Task<List<CistGroupsFaculty>?> GetGroupsFaculties()
    {
        return await cistRepository.GetGroupsFacultiesAsync();
    }

    [HttpGet]
    [Route("GetGroups")]
    public async Task<List<Group>?> GetGroups([FromQuery] int? facultyId, [FromQuery] int? directionId)
    {
        if (facultyId.HasValue && directionId.HasValue)
            return await cistRepository.GetGroups(facultyId.Value, directionId.Value);

        return await cistRepository.GetGroups();
    }

    [HttpGet]
    [Route("GetTeachers")]
    public async Task<List<Teacher>?> GetTeachers([FromQuery] int? departmentId)
    {
        if (departmentId.HasValue)
            return await cistRepository.GetTeachers(departmentId.Value);

        return await cistRepository.GetTeachers();
    }

    [HttpGet]
    [Route("GetTeachersDepartments")]
    public async Task<List<Department>?> GetTeachersDepartments()
    {
        return await cistRepository.GetTeachersDepartments();
    }

    [HttpGet]
    [Route("GetAuditories")]
    public async Task<List<CistAuditory>?> GetAuditories()
    {
        return await cistRepository.GetAuditories();
    }

    [HttpGet]
    [Route("GetBuildings")]
    public async Task<List<CistBuilding>?> GetBuildings()
    {
        return await cistRepository.GetBuildings();
    }
}
