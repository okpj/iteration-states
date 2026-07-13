using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tasks.API.Model;
using Tasks.API.Services;

namespace Tasks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
  private readonly TfsApiOptions _options;
  public TaskController(IOptions<TfsApiOptions> options)
  {
    _options = options.Value;
  }

  [HttpGet("get-iterations-work-items")]
  public async Task<IActionResult> GetIterationsWorkItems([FromQuery] string iteration, [FromQuery] string team)
  {
    var baseUrl = new Uri(_options.Host);
    var workItemService = new WorkItemService(new Uri(baseUrl, _options.Organization), _options.Pat, _options.Project, team);
    var result = await workItemService.GetAsync(iteration);
    return Ok(result);
  }
}
