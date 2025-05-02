using GenTaskScheduler.Core.Abstractions.Common;
using GenTaskScheduler.Core.Data.Internal;
using GenTaskScheduler.Core.DependencyInjection;
using GenTaskScheduler.Core.Infra;
using GenTaskScheduler.SqlServer.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenTaskScheduler.SqlServer;
public static class GenSchedulerExtension {
  public static IServiceCollection AddGenTaskSchedulerWithSqlServer<DbContext>(this IServiceCollection services, string connectionString, Action<SchedulerConfiguration>? setup = null) where DbContext : GenSqlServerContext {
    var config = new SchedulerConfiguration();
    setup?.Invoke(config);
    services.AddSingleton(config);

    services.AddDbContext<DbContext>(options => {
      options.UseSqlServer(connectionString, sqlOptions => {
        sqlOptions.CommandTimeout(30);
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
      });
      options.EnableSensitiveDataLogging(false);
    });
    services.AddScoped<GenTaskSchedulerDbContext>(provider => provider.GetRequiredService<DbContext>());

    services.AddGenTaskScheduler(SchedulerRegistrationToken.Create());
    services.AddSingleton<IBuildTableSchemesSupport, BuildTableSchemesSupport>();

    return services;
  }

}

