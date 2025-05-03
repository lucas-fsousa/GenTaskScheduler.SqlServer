using GenTaskScheduler.Core.Data.Internal;
using GenTaskScheduler.Core.Infra.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GenTaskScheduler.SqlServer.Internal;
public class GenSqlServerContext: GenTaskSchedulerDbContext {
  public GenSqlServerContext(DbContextOptions<GenSqlServerContext> options) : base(options) { }
  public GenSqlServerContext() { }
  protected override void OnConfiguring(DbContextOptionsBuilder options) {
    if(string.IsNullOrEmpty(GenSchedulerEnvironment.DatabaseConnectionString))
      throw new InvalidOperationException($"{nameof(GenSchedulerEnvironment.DatabaseConnectionString)} was not initialized");

    options.EnableSensitiveDataLogging(false);
    options.UseSqlServer(GenSchedulerEnvironment.DatabaseConnectionString, sqlOptions => {
      sqlOptions.CommandTimeout(30);
      sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
      sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });

    base.OnConfiguring(options);
  }
}