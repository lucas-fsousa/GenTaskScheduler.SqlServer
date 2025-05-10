using GenTaskScheduler.Core.Data.Internal;
using GenTaskScheduler.Core.Infra.Configurations;
using GenTaskScheduler.Core.Models.Common;
using GenTaskScheduler.Core.Models.Triggers;
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
      sqlOptions.MigrationsHistoryTable("__GenSchedulerMigrationHistory__");
    });

    base.OnConfiguring(options);
  }

  /// <inheritdoc />
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);

    // setup indexes for performance on SQL SERVER
    modelBuilder.Entity<ScheduledTask>().HasIndex(t => t.ExecutionStatus);
    modelBuilder.Entity<ScheduledTask>().HasIndex(t => t.IsActive);
    modelBuilder.Entity<BaseTrigger>().HasIndex(t => t.NextExecution);
    modelBuilder.Entity<BaseTrigger>().HasIndex(t => new { t.NextExecution, t.LastTriggeredStatus });
  }
}