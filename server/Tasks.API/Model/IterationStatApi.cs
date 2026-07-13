namespace Tasks.API.Model;

public class IterationStatApi
{
  public string Iteration { get; set; }

  public int CountSP { get; set; }
  public int CountClosedSP { get; set; }

  public double PercentClosedSP { get; set; }

  public int CountUS { get; set; }

  public int CountClosedUS { get; set; }

  public double PercentClosedUS { get; set; }
}
