using Microsoft.TeamFoundation.Work.WebApi;

namespace Tasks.API.Model;

public class IterationApi
{
  public Guid Id { get; set; }

  public string Name { get; set; }

  public string Path { get; set; }

  public DateTime? StartDate { get; set; }

  public DateTime? FinishDate { get; set; }

  public TimeFrame? TimeFrame { get; set; }
}
