using Microsoft.Extensions.Options;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Tasks.API.Model;

namespace Tasks.API.Services;

public class TfsServiceFactory
{
  private readonly TfsApiOptions _options;

  public TfsServiceFactory(IOptions<TfsApiOptions> options)
  {
    _options = options.Value;
  }

  public WorkItemService CreateWorkItemService()
  {
    return new WorkItemService(BuildOrganizationUri(), _options.Pat, _options.Project);
  }

  public IterationService CreateIterationService(string team)
  {
    var teamContext = new TeamContext(_options.Project, team);
    return new IterationService(BuildOrganizationUri(), _options.Pat, teamContext, CreateWorkItemService());
  }

  private Uri BuildOrganizationUri() => new(new Uri(_options.Host), _options.Organization);
}
