using GenTaskScheduler.Core.DependencyInjection;
using GenTaskScheduler.Core.Infra.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace GenTaskScheduler.SqlServer.Internal;
public static class SqlServerGenSchedulerExtension {
  public static IServiceCollection AddGenTaskSchedulerWithSqlServer(this IServiceCollection services, string connectionString, Action<SchedulerConfiguration>? setup = null, bool applyMigrations = false) {
    services.AddGenTaskSchedulerWithProvider<SqlServerSchedulerDatabaseProvider>(connectionString, setup, applyMigrations);
    return services;
  }
}

