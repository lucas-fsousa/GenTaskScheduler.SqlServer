using GenTaskScheduler.Core.Abstractions.Common;

namespace GenTaskScheduler.SqlServer {
  public class JobExec: IJob {
    public string Descricao { get; set; } = null!;
    public async Task<object?> ExecuteJobAsync(CancellationToken cancellationToken = default) {
      Console.WriteLine($"Descricao: {Descricao}");
      Console.WriteLine("Job aguarda 10 segundos para terminar");
      await Task.Delay(1000 * 10, cancellationToken);
      throw new Exception("Falha intencional após 10 segundos");
    }
  }
}
