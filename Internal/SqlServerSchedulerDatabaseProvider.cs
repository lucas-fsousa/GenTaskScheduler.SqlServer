using GenTaskScheduler.Core.Abstractions.Providers;
using GenTaskScheduler.Core.Data.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenTaskScheduler.SqlServer.Internal;

public class SqlServerSchedulerDatabaseProvider: IGenTaskSchedulerDatabaseProvider {
  /// <inheritdoc/>
  public string Name => "SqlServer";

  /// <inheritdoc/>
  public void ConfigureDbContext(IServiceCollection services, string connectionString) {
    services.AddDbContext<GenSqlServerContext>(options => {
      options.EnableSensitiveDataLogging(false);
      options.UseSqlServer(connectionString, sqlOptions => {
        sqlOptions.CommandTimeout(30);
        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
      });
    });

    services.AddScoped<GenTaskSchedulerDbContext, GenSqlServerContext>();
  }

  /// <inheritdoc/>
  public void RegisterInfrastructure(IServiceCollection services) {
    services.AddScoped<ISchemeProvider, SqlServerSchemeProvider>();
  }

  /// <inheritdoc/>
  public void ApplyMigrations(IServiceProvider provider) {
    using var dbContext = provider.GetRequiredService<GenSqlServerContext>();
    dbContext.Database.Migrate();
  }
}
