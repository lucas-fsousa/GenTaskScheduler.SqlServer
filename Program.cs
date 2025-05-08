using GenTaskScheduler.Core.Abstractions.Repository;
using GenTaskScheduler.Core.Enums;
using GenTaskScheduler.Core.Infra.Builder.TaskBuilder;
using GenTaskScheduler.Core.Models.Common;
using GenTaskScheduler.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder(args)
  .ConfigureAppConfiguration((context, config) => config.AddJsonFile("appsettings.json", optional: false))
  .ConfigureServices((context, services) => {
    var connString = context.Configuration.GetConnectionString("DefaultConnection")!;
    services.AddGenTaskSchedulerWithSqlServer(connString, options => {
      options.MaxRetry = 3;
      options.RetryWaitDelay = TimeSpan.FromSeconds(5);
      options.RetryOnFailure = true;
      options.LateExecutionTolerance = TimeSpan.FromSeconds(10);
      options.DatabaseCheckInterval = TimeSpan.FromSeconds(10);
    });
  }).Build();

using var scope = host.Services.CreateScope();
var repo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();


var task = GenScheduleTaskBuilder.Create($"TesteRecorrente Calendar")
  .WithJob(new JobExec() {
    Descricao = "executa um job"
  }).ConfigureTriggers(triggerBuilder => {
    //triggerBuilder.CreateOnceTrigger()
    //  .SetExecutionDateTime(DateTimeOffset.UtcNow.AddMinutes(1))
    //  .SetDescription("Once Trigger para executar em 60 segundos")
    //  .SetAutoDelete(true)
    //  .Build();

    //triggerBuilder.CreateIntervalTrigger(DateTimeOffset.UtcNow.AddMinutes(1))
    //  .SetRepeatIntervalMinutes(1)
    //  .SetExecutionLimit(6)
    //  .SetDescription("Recorrente de intervalo que será deletado após executar. Execução em 1 minutos")
    //  .SetAutoDelete(true)
    //  .Build();

    //triggerBuilder.CreateCronTrigger(DateTimeOffset.UtcNow.AddMinutes(2))
    //  .SetCronExpression("*/1 8-18 * * 1-5")
    //  .Build();

    //triggerBuilder.CreateWeeklyTrigger(DateTimeOffset.UtcNow.AddMinutes(1))
    //  .SetDaysOfWeek(DayOfWeek.Tuesday, DayOfWeek.Wednesday)
    //  .SetTimeOfDay(new TimeOnly(00, 44))
    //  .Build();

    //triggerBuilder.CreateMonthlyTrigger(DateTimeOffset.UtcNow.AddMinutes(1))
    //.SetDaysOfMonth(new IntRange(1, 5), new IntRange(6, 26))
    //.SetMonthsOfYear(MonthOfYear.January, MonthOfYear.February, MonthOfYear.March, MonthOfYear.May)
    //.SetTimeOfDay(new TimeOnly(22, 39))
    //.Build();

    triggerBuilder.CreateCalendarTrigger(DateTimeOffset.UtcNow.AddMinutes(1))
    .AddCalendarEntries([
      new () { ScheduledDateTime = DateTimeOffset.UtcNow.AddMinutes(2) }
      //new () { ScheduledDateTime = DateTimeOffset.UtcNow.AddMinutes(6) },
      //new () { ScheduledDateTime = DateTimeOffset.UtcNow.AddDays(1) },
    ])
    .Build();
  }).NotDepends()
  //.DependsOn(ScheduleTaskBuilder.Create("Master Execution")
  //  .WithJob(new JobExec() { Descricao = "executa um job" })
  //  .ConfigureTriggers(triggerBuilder => triggerBuilder.CreateOnceTrigger()
  //    .SetExecutionDateTime(DateTimeOffset.UtcNow.AddMinutes(1))
  //    .SetDescription("Main task para validar job dependente")
  //    .Build()
  //  )
  //  .NotDepends()
  //  .Build()
  //)
  //.WithStatus(GenTaskHistoryStatus.Success, GenTaskHistoryStatus.Canceled)
  .SetAutoDelete(false)
  .SetIsActive(true)
  .SetTimeout(TimeSpan.FromSeconds(20))
  .Build();

await repo.AddAsync(task);

await host.RunAsync();

