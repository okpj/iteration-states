using Microsoft.AspNetCore.Mvc;
using Tasks.API.Services;

namespace Tasks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IterationsController : ControllerBase
{
  private readonly TfsServiceFactory _serviceFactory;

  public IterationsController(TfsServiceFactory serviceFactory)
  {
    _serviceFactory = serviceFactory;
  }

  [HttpGet("get-iterations")]
  public async Task<IActionResult> GetIterations([FromQuery] string team)
  {
    var iterationService = _serviceFactory.CreateIterationService(team);
    var result = await iterationService.GetIterationsAsync();
    return Ok(result);
  }

  [HttpGet("get-stat")]
  public async Task<IActionResult> GetStat([FromQuery] Guid iterationId, [FromQuery] string team)
  {
    var iterationService = _serviceFactory.CreateIterationService(team);
    var result = await iterationService.GetStatAsync(iterationId);
    return Ok(result);
  }
}
