using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.Work.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Tasks.API.Model;

namespace Tasks.API.Services;

public class IterationService
{
  #region Поля

  private readonly Uri _uri;

  private readonly TeamContext _teamContext;

  private readonly VssBasicCredential _credentials;

  private readonly WorkItemService _workItemService;

  #endregion

  public IterationService(Uri url, string pat, TeamContext teamContext, WorkItemService workItemService)
  {
    _uri = url;
    _teamContext = teamContext;
    _credentials = new VssBasicCredential(string.Empty, pat);
    _workItemService = workItemService;
  }

  private const string UserStoryType = "User Story";
  private const string ClosedState = "Closed";

  public async Task<IEnumerable<IterationApi>> GetIterationsAsync()
  {
    using var httpClient = new WorkHttpClient(_uri, _credentials);
    var iterations = await httpClient.GetTeamIterationsAsync(_teamContext);

    return iterations.Select(ToIterationApi);
  }

  public async Task<IterationApi> GetIterationAsync(Guid iterationId)
  {
    using var httpClient = new WorkHttpClient(_uri, _credentials);
    var iteration = await httpClient.GetTeamIterationAsync(_teamContext, iterationId);

    return ToIterationApi(iteration);
  }

  public async Task<IterationStatApi> GetStatAsync(Guid iterationId)
  {
    var iteration = await GetIterationAsync(iterationId);
    var items = await _workItemService.GetAsync(iteration.Path);

    // Не учитывает случай, когда таска открылась в предыдущем спринте, а закрылась в следующем.
    var tasks = items.Where(x => x.WorkItemType != UserStoryType).ToList();
    var userStories = items.Where(x => x.WorkItemType == UserStoryType).ToList();

    var closedTasks = tasks.Where(x => WasClosedBy(x, iteration.FinishDate)).ToList();
    var closedUserStories = userStories.Where(x => WasClosedBy(x, iteration.FinishDate)).ToList();

    var countSP = tasks.Sum(x => x.StoryPoints ?? 0);
    var closedSP = closedTasks.Sum(x => x.StoryPoints ?? 0);

    return new IterationStatApi
    {
      Iteration = iteration.Name,
      CountSP = countSP,
      CountClosedSP = closedSP,
      PercentClosedSP = PercentOf(closedSP, countSP),
      CountUS = userStories.Count,
      CountClosedUS = closedUserStories.Count,
      PercentClosedUS = PercentOf(closedUserStories.Count, userStories.Count)
    };
  }

  private static bool WasClosedBy(ApiWorkItem item, DateTime? finishDate) =>
    item.State == ClosedState && (item.ClosedDate <= finishDate || item.ResolvedDate <= finishDate);

  private static double PercentOf(int part, int total) =>
    total == 0 ? 0 : Math.Round((double)part / total * 100);

  private static IterationApi ToIterationApi(TeamSettingsIteration iteration) => new()
  {
    Id = iteration.Id,
    Name = iteration.Name,
    Path = iteration.Path,
    StartDate = iteration.Attributes.StartDate,
    FinishDate = iteration.Attributes.FinishDate,
    TimeFrame = iteration.Attributes.TimeFrame,
  };
}
