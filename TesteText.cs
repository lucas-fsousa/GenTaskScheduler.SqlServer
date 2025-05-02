using GenTaskScheduler.Core.Abstractions.Common;

namespace GenTaskScheduler.SqlServer {
  public class TesteText: IJob {
    public TesteClass TesteClass { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Group { get; set; } = null!;
    public async Task<object?> ExecuteJobAsync(CancellationToken cancellationToken = default) {
      Console.WriteLine($"Executing job: {Name}");
      Console.WriteLine($"Description: {Description}");
      Console.WriteLine($"Group: {Group}");

      await Task.Delay(1000 * 10, cancellationToken);
      Console.WriteLine($"Terminou");
      return "zigrigdun";
    }
  }
}
