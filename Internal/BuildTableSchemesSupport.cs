using GenTaskScheduler.Core.Abstractions.Common;
using GenTaskScheduler.Core.Data.Internal;
using Microsoft.EntityFrameworkCore;

namespace GenTaskScheduler.SqlServer.Internal;
public class BuildTableSchemesSupport(GenSqlServerContext context): IBuildTableSchemesSupport {
  public string GetCreateQueryForTableSchemes() => context.Database.GenerateCreateScript();
}

