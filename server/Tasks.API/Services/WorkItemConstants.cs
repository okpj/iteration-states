namespace Tasks.API.Services;

internal static class WorkItemConstants
{
  internal static string[] Fields = [
  "System.Id",
    "System.WorkItemType",
    "System.Title",
    "System.State",
    "Microsoft.VSTS.Common.ActivatedDate",
    "Microsoft.VSTS.Common.ClosedDate",
    "Microsoft.VSTS.Common.ResolvedDate",
    "System.Parent",
    "Microsoft.VSTS.Scheduling.StoryPoints",
    "System.IterationPath"
];

  internal static string Query = @"SELECT 
[System.Id],
[System.WorkItemType],
[System.Title],
[System.State],
[Microsoft.VSTS.Common.ActivatedDate],
[Microsoft.VSTS.Common.ClosedDate],
[Microsoft.VSTS.Common.ResolvedDate],
[System.Parent],
[Microsoft.VSTS.Scheduling.StoryPoints],
[System.IterationPath]
FROM WorkItems WHERE [System.TeamProject] = '{0}'
AND ([System.WorkItemType] = 'Task' OR [System.WorkItemType] = 'Bug')
AND [System.IterationPath] = '{1}'
AND [Microsoft.VSTS.Common.Activity] <> 'Testing'";

}