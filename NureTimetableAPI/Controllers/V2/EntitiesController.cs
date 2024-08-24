using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Models.Dto;
using NureTimetableAPI.Repositories;

namespace NureTimetableAPI.Controllers.V2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class EntitiesController(ISQLRepository sqlRepository) : ControllerBase
{
    private readonly ISQLRepository _sqlRepository = sqlRepository;


    /// <summary>
    /// Gets a <see cref="CombinedEntitiesDto"/> with all the entities(groups, teachers, auditories) from the database.
    /// </summary>
    /// <param name="useMinimalModels">
    /// When set to true, response model changes to <see cref="MinimalCombinedEntitiesDto"/>.
    /// </param>
    /// <returns>
    /// <see cref="CombinedEntitiesDto"/> or <see cref="MinimalCombinedEntitiesDto"/>.
    /// </returns>
    [HttpGet("GetAll")]
    [ProducesResponseType<CombinedEntitiesDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] bool useMinimalModels = false)
    {
        try
        {
            if (useMinimalModels)
            {
                var minimalEntities = await _sqlRepository.GetMinimalCombinedEntitiesAsync();

                return Ok(minimalEntities);
            }

            var entities = await _sqlRepository.GetCombinedEntitiesAsync();

            return Ok(entities);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
