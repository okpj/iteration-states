using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tasks.API.Model;
using Tasks.API.Services;

namespace Tasks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IterationsController : ControllerBase
{
  private readonly TfsApiOptions _options;
  public IterationsController(IOptions<TfsApiOptions> options)
  {
    _options = options.Value;
  }

  [HttpGet("get-iterations")]
  public async Task<IActionResult> GetIterationsWorkItems([FromQuery] string team)
  {
    var baseUrl = new Uri(_options.Host);
    var workItemService = new WorkItemService(new Uri(baseUrl, _options.Organization), _options.Pat, _options.Project, team);
    var iterationService = new IterationService(new Uri(baseUrl, _options.Organization), _options.Pat,
      new Microsoft.TeamFoundation.Core.WebApi.Types.TeamContext(_options.Project, team), workItemService);

    var result = await iterationService.GetIterationsAsync();
    return Ok(result);
  }

  [HttpGet("get-stat")]
  public async Task<IActionResult> GetStat([FromQuery] Guid iterationId, [FromQuery] string team)
  {
    var baseUrl = new Uri(_options.Host);
    var workItemService = new WorkItemService(new Uri(baseUrl, _options.Organization), _options.Pat, _options.Project, team);
    var iterationService = new IterationService(new Uri(baseUrl, _options.Organization), _options.Pat,
      new Microsoft.TeamFoundation.Core.WebApi.Types.TeamContext(_options.Project, team), workItemService);
    var result = await iterationService.GetStatAsync(iterationId);
    return Ok(result);
  }
}
