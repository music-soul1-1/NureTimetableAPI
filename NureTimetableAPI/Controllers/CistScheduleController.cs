using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Models;
using Newtonsoft.Json;
using NureTimetableAPI.Repositories;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Types;
using NureTimetableAPI.Controllers.Responses;


namespace NureTimetableAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CistScheduleController : ControllerBase
{
    private readonly ILogger<CistScheduleController> _logger;
    private readonly ICistRepository cistRepository;

    public CistScheduleController(ILogger<CistScheduleController> logger, ICistRepository cistRepository)
    {
        _logger = logger;
        this.cistRepository = cistRepository;
    }

    [HttpGet]
    [Route("GetLessons")]
    public async Task<IActionResult> Get(
        [FromQuery(Name = "id")] int? id,
        [FromQuery(Name = "name")] string? name = null,
        [FromQuery(Name = "type")] EntityType type = EntityType.Group,
        [FromQuery(Name = "startTime")] int? startTime = null,
        [FromQuery(Name = "endTime")] int? endTime = null
        )
    {
        try
        {
            if (id == null && name != null)
            {
                var result = await cistRepository.GetLessonsAsync(name, type, startTime, endTime);

                if (result == null)
                {
                    return BadRequest(new ErrorMessage($"No schedule found for '{name}' with type '{type}'", 204));
                }
                return Ok(result);
            }
            var resultWithId = await cistRepository.GetLessonsAsync(id.GetValueOrDefault(), type, startTime, endTime);

            if (resultWithId == null)
            {
                return BadRequest(new ErrorMessage($"No schedule found for '{id}' with type '{type}'", 204));
            }
            return Ok(resultWithId);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"in {nameof(CistScheduleController)} in {nameof(Get)}");
            return BadRequest(new ErrorMessage($"Error occupied in {nameof(CistScheduleController)} in {nameof(Get)}", 400));
        }
    }
}
