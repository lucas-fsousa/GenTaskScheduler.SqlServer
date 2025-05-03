using GenTaskScheduler.Core.Data.Internal;
using GenTaskScheduler.Core.DependencyInjection;
using GenTaskScheduler.Core.Infra.Configurations;
using GenTaskScheduler.SqlServer.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenTaskScheduler.SqlServer;
public static class GenSchedulerExtension {
  public static IServiceCollection AddGenTaskSchedulerWithSqlServer(this IServiceCollection services, string connectionString, Action<SchedulerConfiguration>? setup = null) {
    var config = new SchedulerConfiguration();
    setup?.Invoke(config);

    GenSchedulerEnvironment.Initialize(connectionString, config);
    services.AddGenTaskScheduler(SchedulerRegistrationToken.Create());
    services.AddDbContext<GenSqlServerContext>(options => {
      options.EnableSensitiveDataLogging(false);
      options.UseSqlServer(GenSchedulerEnvironment.DatabaseConnectionString, sqlOptions => {
        sqlOptions.CommandTimeout(30);
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
      });
    });

    services.AddScoped<GenTaskSchedulerDbContext>(provider => provider.GetRequiredService<GenSqlServerContext>());
    return services;
  }
}

