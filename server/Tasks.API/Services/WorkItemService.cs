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

  public WorkItemService(Uri url, string pat, string project)
  {
    _project = project;
    _uri = url;
    _credentials = new VssBasicCredential(string.Empty, pat);
  }

  public async Task<IList<ApiWorkItem>> GetAsync(string iteration)
  {
    var wiql = new Wiql()
    {
      Query = string.Format(WorkItemConstants.Query, _project, iteration)
    };

    using var httpClient = new WorkItemTrackingHttpClient(_uri, _credentials);
    try
    {
      var result = await httpClient.QueryByWiqlAsync(wiql, _project);
      var ids = result.WorkItems.Select(item => item.Id).ToArray();

      if (ids.Length == 0)
        return Array.Empty<ApiWorkItem>();

      var workItems = await GetApiWorkItemsAsync(httpClient, ids, result.AsOf);
      await AttachParentsAsync(httpClient, workItems);

      return workItems;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
      return [];
    }
  }

  private static async Task<List<ApiWorkItem>> GetApiWorkItemsAsync(WorkItemTrackingHttpClient httpClient, int[] ids, DateTime? asOf)
  {
    var tfsWorkItems = await httpClient.GetWorkItemsAsync(ids, fields: WorkItemConstants.Fields, asOf);
    return tfsWorkItems.Select(x => x.ToApiWorkItem()).ToList();
  }

  private static async Task AttachParentsAsync(WorkItemTrackingHttpClient httpClient, List<ApiWorkItem> workItems)
  {
    var parentIds = workItems.Where(x => x.ParentId.HasValue)
      .Select(x => x.ParentId!.Value)
      .Distinct().ToArray();

    if (parentIds.Length == 0)
      return;

    var tfsParents = await httpClient.GetWorkItemsAsync(parentIds, fields: WorkItemConstants.Fields);
    var parents = tfsParents.Select(x => x.ToApiWorkItem(withOutParentId: true)).ToList();
    workItems.AddRange(parents);

    foreach (var item in workItems)
      item.Parent = parents.FirstOrDefault(x => x.Id == item.ParentId);
  }
}
