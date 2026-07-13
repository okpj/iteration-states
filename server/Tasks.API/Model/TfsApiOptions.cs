namespace Tasks.API.Model;

public class TfsApiOptions
{
  public const string Position = "TfsAPI";

  public string Organization { get; set; }

  public string Pat { get; set; }

  public string Project { get; set; }

  public string Host { get; set; }
}
