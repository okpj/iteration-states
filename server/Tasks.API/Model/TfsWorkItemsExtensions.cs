using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace Tasks.API.Model;

public static class TfsWorkItemsExtensions
{
  public static ApiWorkItem ToApiWorkItem(this WorkItem workItem, bool withOutParentId = false)
  {
    var fields = workItem.Fields;

    return new ApiWorkItem
    {
      Id = workItem.Id!.Value,
      ParentId = withOutParentId ? null : GetInt(fields, "System.Parent"),
      WorkItemType = GetString(fields, "System.WorkItemType"),
      Title = GetString(fields, "System.Title"),
      IterationPath = GetString(fields, "System.IterationPath"),
      State = GetString(fields, "System.State"),
      StoryPoints = GetInt(fields, "Microsoft.VSTS.Scheduling.StoryPoints"),
      ActivatedDate = GetDate(fields, "Microsoft.VSTS.Common.ActivatedDate"),
      ClosedDate = GetDate(fields, "Microsoft.VSTS.Common.ClosedDate"),
      ResolvedDate = GetDate(fields, "Microsoft.VSTS.Common.ResolvedDate")
    };
  }

  private static string? GetString(IDictionary<string, object> fields, string key) =>
    fields.TryGetValue(key, out var value) ? value.ToString() : null;

  private static int? GetInt(IDictionary<string, object> fields, string key) =>
    fields.TryGetValue(key, out var value) ? int.Parse(value.ToString()!) : null;

  private static DateTime? GetDate(IDictionary<string, object> fields, string key) =>
    fields.TryGetValue(key, out var value) ? DateTime.Parse(value.ToString()!) : null;
}
