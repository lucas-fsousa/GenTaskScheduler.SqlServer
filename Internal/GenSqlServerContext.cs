using GenTaskScheduler.Core.Data.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GenTaskScheduler.SqlServer.Internal;
public class GenSqlServerContext: GenTaskSchedulerDbContext {
  public GenSqlServerContext() { }
  public GenSqlServerContext(DbContextOptions<GenSqlServerContext> options) : base(options) { }

}

public class GenSqlServerContextFactory: IDesignTimeDbContextFactory<GenSqlServerContext> {
  public GenSqlServerContext CreateDbContext(string[] args) {
    var connectionArg = args.FirstOrDefault(a => a.StartsWith("--connection=")) ?? throw new ArgumentException("Missing --connection argument");
    var connString = connectionArg.Split("=", 2).Last();
    var optionsBuilder = new DbContextOptionsBuilder<GenSqlServerContext>();
    optionsBuilder.UseSqlServer(connString);

    return new GenSqlServerContext(optionsBuilder.Options);
  }
}