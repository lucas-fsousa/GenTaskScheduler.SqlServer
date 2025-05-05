using GenTaskScheduler.Core.Abstractions.Repository;
using GenTaskScheduler.Core.Enums;
using GenTaskScheduler.Core.Infra.Builder.TaskBuilder;
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

var task = GenScheduleTaskBuilder.Create("TesteRecorrente")
  .WithJob(new TesteText() {
    Name = "Teste",
    Description = $"solicitação de execução as {DateTimeOffset.UtcNow:s}",
    Group = "Pertence ao grupo de dependencia. Só executa em caso de erro.",
    TesteClass = new TesteClass() {
      Testes = [
        "Teste1", "Teste2", "Teste3"
      ]
    }
  }).ConfigureTriggers(triggerBuilder => {
    triggerBuilder.CreateOnceTrigger()
      .SetExecutionDateTime(DateTimeOffset.UtcNow.AddMinutes(2))
      .SetDescription("Once Trigger para executar em 2 minutos")
      .SetAutoDelete(true)
      .Build();

    triggerBuilder.CreateIntervalTrigger(DateTimeOffset.UtcNow.AddMinutes(3))
      .SetRepeatIntervalMinutes(1)
      .SetDescription("Recorrente de intervalo que será deletado após executar. Execução em 3 minutos")
      .SetAutoDelete(true)
      .Build();

    //triggerBuilder.CreateCronTrigger(DateTimeOffset.UtcNow.AddMinutes(15))
    //  .SetCronExpression("*/1 * * * *")
    //  .Build();

    //triggerBuilder.CreateWeeklyTrigger(DateTimeOffset.UtcNow.AddMinutes(15))
    //  .SetDaysOfWeek(DayOfWeek.Monday, DayOfWeek.Friday)
    //  .SetTimeOfDay(default)
    //  .Build();

    //triggerBuilder.CreateIntervalTrigger(DateTimeOffset.UtcNow.AddMinutes(15))
    //.SetRepeatIntervalMinutes(1)
    //.Build();
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
  //.WithStatus(ExecutionStatus.Failed)
  .SetAutoDelete(false)
  .SetIsActive(true)
  .Build();

await repo.AddAsync(task);

await host.RunAsync();

