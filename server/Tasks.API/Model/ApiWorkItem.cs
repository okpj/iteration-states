namespace Tasks.API.Model
{
  public class ApiWorkItem
  {
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? IterationPath { get; set; }

    public DateTime? ActivatedDate { get; set; }

    public DateTime? ClosedDate { get; set; }

    public DateTime? ResolvedDate { get; set; }

    public int? StoryPoints { get; set; }

    public string? State { get; set; }

    public string? WorkItemType { get; set; }

    #region Parent

    public int? ParentId { get; set; }

    public ApiWorkItem? Parent { get; set; }

    #endregion
  }
}
