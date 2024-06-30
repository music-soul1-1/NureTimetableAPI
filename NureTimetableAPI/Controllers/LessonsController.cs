using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Repositories;
using NureTimetableAPI.Types;
using NureTimetableAPI.Exceptions;
using Hangfire.Storage;
using Hangfire;


namespace NureTimetableAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonsController : ControllerBase
{
    private readonly ILogger<LessonsController> _logger;
    private readonly ISQLRepository repository;
    private readonly ICistRepository cist;

    public LessonsController(ILogger<LessonsController> logger, ISQLRepository repository, ICistRepository cist)
    {
        _logger = logger;
        this.repository = repository;
        this.cist = cist;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> GetLessons(
        [FromQuery(Name = "id")] int id,
        [FromQuery(Name = "type")] EntityType type = EntityType.Group,
        [FromQuery(Name = "startTime")] int? startTime = null,
        [FromQuery(Name = "endTime")] int? endTime = null)
    {
        try
        {
            if (id == 0)
            {
                return BadRequest("Please provide a valid Id");
            }
            var lessons = await repository.GetLessonsAsync(id, type, startTime, endTime);
            var fetchJob = Hangfire.JobStorage.Current.GetConnection().GetRecurringJobs().ToList()
                .FirstOrDefault(j => j.Id == $"update-{type.ToString().ToLower()}-with-id-{id}");

            if (fetchJob == null || lessons == null || lessons.Count < 1)
            {
                var lastJob = JobStorage.Current.GetConnection().GetRecurringJobs().ToList()
                        .OrderByDescending(j => j.LastExecution).FirstOrDefault(j => !j.Id.Contains("keep-alive"));

                if (lastJob != null && lastJob.LastExecution != null && lastJob.LastExecution > DateTime.Now.AddSeconds(30))
                {
                    throw new Exception($"Please wait {lastJob.LastExecution.Value.Second} seconds, and then try again");
                }
                var cistLessons = await cist.GetCistScheduleAsync(id, type);

                if (cistLessons == null)
                {
                    throw new NotFoundException($"Cist lessons not found for {type} with id {id}");
                }

                return Ok(await repository.FetchSchedule(id, cistLessons, type, startTime, endTime));
            }

            return Ok(lessons);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch(Exception ex)
        {
            return StatusCode(500, $"An error occupied while getting lessons for {type} with id {id}. Exception: {ex.Message}");
        }        
    }

    [HttpGet]
    [Route("GetByName")]
    public async Task<IActionResult> GetLessonsByName(
        [FromQuery(Name = "name")] string name,
        [FromQuery(Name = "type")] EntityType type = EntityType.Group,
        [FromQuery(Name = "startTime")] int? startTime = null,
        [FromQuery(Name = "endTime")] int? endTime = null
        )
    {
        try
        {
            if (name.Length < 1)
            {
                return BadRequest("Please provide a valid name");
            }
            var lessons = await repository.GetLessonsAsync(name, type, startTime, endTime);

            if (lessons == null || lessons.Count < 1)
            {
                var lastJob = Hangfire.JobStorage.Current.GetConnection().GetRecurringJobs().ToList()
                    .OrderByDescending(j => j.LastExecution).FirstOrDefault();

                if (lastJob != null && lastJob.LastExecution != null && lastJob.LastExecution > DateTime.Now.AddSeconds(30))
                {
                    throw new Exception($"Please wait {lastJob.LastExecution.Value.Second} seconds, and then try again");
                }
                var cistLessons = await cist.GetCistScheduleAsync(name, type);

                if (cistLessons == null)
                {
                    throw new NotFoundException($"Cist lessons not found for {type} with name {name}");
                }

                return Ok(await repository.FetchSchedule(name, cistLessons, type, startTime, endTime));
            }

            return Ok(lessons);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occupied while getting lessons for {type} with name {name}. Exception: {ex.Message}");
        }
    }
}
