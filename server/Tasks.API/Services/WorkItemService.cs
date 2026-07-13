using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Tasks.API.Model;

namespace Tasks.API.Services;

public class WorkItemService
{
  #region Поля

  private readonly Uri _uri;

  private readonly string _project;

  private readonly VssBasicCredential _credentials;

  #endregion


  public WorkItemService(Uri url, string pat, string project, string team)
  {
    _project = project;
    _uri = url;
    _credentials = new VssBasicCredential(string.Empty, pat);
  }


  public async Task<IList<ApiWorkItem>> GetAsync(string iteration)
  {
    var wiql = new Wiql()
    {
      Query = string.Format(WorkItemConstants.Query, iteration)
    };

    using (var httpClient = new WorkItemTrackingHttpClient(_uri, _credentials))
    {
      try
      {
        var result = await httpClient.QueryByWiqlAsync(wiql, _project);
        var ids = result.WorkItems.Select(item => item.Id).ToArray();

        if (ids.Length == 0)
          return Array.Empty<ApiWorkItem>();

        var tfsResultItems = await httpClient.GetWorkItemsAsync(ids, fields: WorkItemConstants.Fields, result.AsOf);
        var resultItems = tfsResultItems!.Select(x => x.ToApiWorkItem()).ToList();

        var parentIds = resultItems!.Where(x => x.ParentId.HasValue)
          .Select(x => x.ParentId!.Value)
          .Distinct().ToArray();

        if (parentIds?.Length > 0)
        {
          var tfsParents = await httpClient.GetWorkItemsAsync(parentIds, fields: WorkItemConstants.Fields);
          var parents = tfsParents!.Select(x => x.ToApiWorkItem(withOutParentId: true));
          resultItems.AddRange(parents);
          foreach (var item in resultItems)
          {
            var parent = parents.FirstOrDefault(x => x.Id == item.ParentId);
            item.Parent = parent;
          }
        }

        return resultItems ?? [];
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return [];
      }
    }
  }
}
