using GenTaskScheduler.Core.Abstractions.Providers;
using Microsoft.EntityFrameworkCore;

namespace GenTaskScheduler.SqlServer.Internal;

public class SqlServerSchemeProvider(GenSqlServerContext context): ISchemeProvider {
  ///<inheritdoc/>
  public string GenerateSchemeScript() => context.Database.GenerateCreateScript();
}

