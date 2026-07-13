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

  public async Task<IEnumerable<IterationApi>> GetIterationsAsync()
  {
    using (var httpClient = new WorkHttpClient(this._uri, _credentials))
    {
      var iterations = await httpClient.GetTeamIterationsAsync(_teamContext);

      return iterations.Select(x => new IterationApi
      {
        Id = x.Id,
        Name = x.Name,
        Path = x.Path,
        StartDate = x.Attributes.StartDate,
        FinishDate = x.Attributes.FinishDate,
        TimeFrame = x.Attributes.TimeFrame,
      });
    }
  }

  public async Task<IterationApi> GetIterationAsync(Guid iterationId)
  {
    using (var httpClient = new WorkHttpClient(this._uri, _credentials))
    {
      var iteration = await httpClient.GetTeamIterationAsync(_teamContext, iterationId);

      return new IterationApi
      {
        Id = iteration.Id,
        Name = iteration.Name,
        Path = iteration.Path,
        StartDate = iteration.Attributes.StartDate,
        FinishDate = iteration.Attributes.FinishDate,
        TimeFrame = iteration.Attributes.TimeFrame,
      };
    }
  }

  public async Task<IterationStatApi> GetStatAsync(Guid iterationId)
  {
    var iteration = await GetIterationAsync(iterationId);
    var items = (await _workItemService.GetAsync(iteration.Path));

    //Если таска открылась в предыдущем спринте, а закрылась в следующем?
    // кол-во закрытых тасок в US на текущем спринте

    var allTasks = items.Where(x => x.WorkItemType != "User Story").ToList();
    var allUS = items.Where(x => x.WorkItemType == "User Story").ToList();
    var closedTask = allTasks.Where(x => x.State == "Closed"
      && (x.ClosedDate <= iteration.FinishDate || x.ResolvedDate <= iteration.FinishDate)).ToList();
    var closedUS = allUS.Where(x => x.State == "Closed"
      && (x.ClosedDate <= iteration.FinishDate || x.ResolvedDate <= iteration.FinishDate)).ToList();

    int countSP = allTasks.Sum(x => x.StoryPoints ?? 0);
    int closedSP = closedTask.Sum(x => x.StoryPoints ?? 0);
    var percentClosedSP = countSP == 0 ? 0 : Math.Round(((double)closedSP / (double)countSP) * 100);

    int countUS = allUS.Count;
    int countClosedUS = closedUS.Count;
    var percentClosedUS = countUS == 0 ? 0 : Math.Round(((double)countClosedUS / (double)countUS) * 100);

    return new IterationStatApi
    {
      Iteration = iteration.Name,
      CountSP = countSP,
      CountClosedSP = closedSP,
      PercentClosedSP = percentClosedSP,
      CountUS = countUS,
      CountClosedUS = countClosedUS,
      PercentClosedUS = percentClosedUS
    };
  }
}
