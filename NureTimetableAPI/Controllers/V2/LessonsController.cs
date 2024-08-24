using Microsoft.AspNetCore.Mvc;
using Hangfire.Storage;
using Hangfire;
using Asp.Versioning;
using NureTimetableAPI.Repositories;
using NureTimetableAPI.Types;
using NureTimetableAPI.Exceptions;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Jobs;
using NureTimetableAPI.Controllers.Responses;

namespace NureTimetableAPI.Controllers.V2;


[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class LessonsController(ILogger<LessonsController> logger, ISQLRepository sqlRepository, ICistRepository cistRepository) : ControllerBase
{
    private readonly ILogger<LessonsController> _logger = logger;
    private readonly ISQLRepository _sqlRepository = sqlRepository;
    private readonly ICistRepository _cistRepository = cistRepository;

    [HttpGet]
    [Route("GetById")]
    [ProducesResponseType<List<LessonDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
            var lessons = await _sqlRepository.GetLessonsAsync(id, type, startTime, endTime);

            var fetchJob = JobStorage.Current.GetConnection().GetRecurringJobs().ToList()
                .FirstOrDefault(j => j.Id == $"update-{type.ToString().ToLower()}-with-id-{id}");
            var lastJob = JobStorage.Current.GetConnection().GetRecurringJobs().ToList()
                .OrderByDescending(j => j.Cron).FirstOrDefault(j => !j.Id.Contains("keep-alive"));

            if (fetchJob == null || lessons == null)
            {
                if (lastJob != null && lastJob.LastExecution != null && lastJob.LastExecution > DateTime.Now.AddSeconds(30))
                {
                    throw new Exception($"Please wait {lastJob.LastExecution.Value.Second} seconds, and then try again");
                }
                var cistLessons = await _cistRepository.GetCistScheduleAsync(id, type);

                if (cistLessons == null)
                {
                    throw new NotFoundException($"Cist lessons not found for {type} with id {id}");
                }

                lessons = await _sqlRepository.FetchSchedule(id, cistLessons, type, startTime, endTime);

                RecurringJob.AddOrUpdate<ScheduleFetch>(
                    $"update-{type.ToString().ToLower()}-with-id-{id}",
                    job => job.Execute(id, type),
                    (lastJob != null && lastJob.NextExecution != null) ?
                    Cron.Daily(lastJob.NextExecution.Value.Hour, lastJob.NextExecution.Value.Minute + 1) :
                    Cron.Daily(DateTime.Now.Hour, DateTime.Now.Minute)
                );

                return Ok(lessons);
            }

            return Ok(lessons);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResponseMessage($"An error occupied while getting lessons for {type} with id {id}. Exception: {ex.Message}", 500));
        }
    }

    [HttpGet]
    [Route("GetByName")]
    [ProducesResponseType<List<LessonDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLessonsByName(
        [FromQuery(Name = "name")] string name,
        [FromQuery(Name = "type")] EntityType type = EntityType.Group,
        [FromQuery(Name = "startTime")] int? startTime = null,
        [FromQuery(Name = "endTime")] int? endTime = null
        )
    {
        if (name.Length < 1)
        {
            return BadRequest("Please provide a valid name");
        }

        var id = type switch
        {
            EntityType.Group => (await _sqlRepository.GetGroupAsync(name))?.Id,
            EntityType.Teacher => (await _sqlRepository.GetTeacherAsync(name))?.Id,
            EntityType.Auditory => (await _sqlRepository.GetAuditoryAsync(name))?.Id,
            _ => throw new ArgumentException($"Invalid entity type: {type}"),
        } ?? 0;

        if (id == 0)
        {
            return NotFound($"No {type} found with name {name}");
        }

        return await GetLessons(id, type, startTime, endTime);
    }
}
