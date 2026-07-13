using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace Tasks.API.Model;

public static class TfsWorkItemsExtensions
{
  public static ApiWorkItem ToApiWorkItem(this WorkItem workItem, bool withOutParentId = false) => new ApiWorkItem()
  {
    Id = workItem.Id!.Value,
    ParentId = withOutParentId ? null : workItem.Fields.TryGetValue("System.Parent", out var parentId) ? int.Parse(parentId.ToString()!) : null,
    WorkItemType = workItem.Fields.TryGetValue("System.WorkItemType", out var type) ? type.ToString() : null,
    Title = workItem.Fields.TryGetValue("System.Title", out var title) ? title.ToString() : null,
    IterationPath = workItem.Fields.TryGetValue("System.IterationPath", out var iteration) ? iteration.ToString() : null,
    State = workItem.Fields.TryGetValue("System.State", out var state) ? state.ToString() : null,
    StoryPoints = workItem.Fields.TryGetValue("Microsoft.VSTS.Scheduling.StoryPoints", out var sp) ? int.Parse(sp.ToString()!) : null,
    ActivatedDate = workItem.Fields.TryGetValue("Microsoft.VSTS.Common.ActivatedDate", out var activetedDate) ? DateTime.Parse(activetedDate.ToString()!) : null,
    ClosedDate = workItem.Fields.TryGetValue("Microsoft.VSTS.Common.ClosedDate", out var closedDate) ? DateTime.Parse(closedDate.ToString()!) : null,
    ResolvedDate = workItem.Fields.TryGetValue("Microsoft.VSTS.Common.ResolvedDate", out var resolvedDate) ? DateTime.Parse(resolvedDate.ToString()!) : null
  };
}
