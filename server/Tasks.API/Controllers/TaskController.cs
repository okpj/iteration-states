using Microsoft.AspNetCore.Mvc;
using Tasks.API.Services;

namespace Tasks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
  private readonly TfsServiceFactory _serviceFactory;

  public TaskController(TfsServiceFactory serviceFactory)
  {
    _serviceFactory = serviceFactory;
  }

  [HttpGet("get-iterations-work-items")]
  public async Task<IActionResult> GetIterationsWorkItems([FromQuery] string iteration, [FromQuery] string team)
  {
    var workItemService = _serviceFactory.CreateWorkItemService();
    var result = await workItemService.GetAsync(iteration);
    return Ok(result);
  }
}
