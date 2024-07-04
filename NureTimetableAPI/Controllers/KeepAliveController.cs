using Microsoft.AspNetCore.Mvc;
using NureTimetableAPI.Controllers.Responses;

namespace NureTimetableAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class KeepAliveController : ControllerBase
{
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Get()
    {
        return Ok(new ResponseMessage("I'm alive!", 200));
    }
}
