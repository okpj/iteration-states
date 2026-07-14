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

  // {0} = project,
  // {1} = корень команды в дереве итераций (Project\Team),
  // {2} = путь итерации,
  // {3} = дата окончания спринта,
  // {4} = дата начала спринта.
  // UNDER '{1}' ограничивает выборку деревом итераций конкретной команды (иначе "запасная" ветка
  // по датам подхватывала бы закрытые задачи вообще всех команд проекта).
  // Помимо задач с текущим IterationPath, забирает те, что реально закрылись в рамках дат спринта,
  // даже если сейчас числятся за другой итерацией той же команды (например, открылись в одном
  // спринте, закрылись в следующем).
  internal static string StatQuery = @"SELECT
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
AND [Microsoft.VSTS.Common.Activity] <> 'Testing'
AND [System.IterationPath] UNDER '{1}'
AND (
  [System.IterationPath] = '{2}'
  OR (
    [System.State] = 'Closed'
    AND [Microsoft.VSTS.Common.ActivatedDate] <= '{3}'
    AND ([Microsoft.VSTS.Common.ClosedDate] >= '{4}' OR [Microsoft.VSTS.Common.ResolvedDate] >= '{4}')
  )
)";

}