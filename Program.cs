using GenTaskScheduler.Core.Abstractions.Repository;
using GenTaskScheduler.Core.Enums;
using GenTaskScheduler.Core.Infra.Builder.TaskBuilder;
using GenTaskScheduler.Core.Models.Triggers;
using GenTaskScheduler.SqlServer;
using GenTaskScheduler.SqlServer.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) => {
      config.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((context, services) => {
      var connString = context.Configuration.GetConnectionString("DefaultConnection")!;
      services.AddGenTaskSchedulerWithSqlServer<GenSqlServerContext>(connString, options => {
        options.MaxRetry = 3;
        options.RetryWaitDelay = TimeSpan.FromSeconds(5);
        options.RetryOnFailure = true;
        options.MarginOfError = TimeSpan.FromSeconds(10);
        options.DatabaseCheckInterval = TimeSpan.FromSeconds(10);
      });
    })
    .Build();


using var scope = host.Services.CreateScope();
var repo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

var task = ScheduleTaskBuilder.Create("Recurring EQ")
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
    triggerBuilder.CreateOnceTrigger().SetExecutionTime(DateTimeOffset.UtcNow.AddMinutes(1.4));
    triggerBuilder.CreateCronTrigger().SetCronExpression("0 0/1 * * * ?");
    triggerBuilder.CreateWeeklyTrigger()
      .SetTimeOfDay()
      .SetDaysOfWeek(DayOfWeek.Monday, DayOfWeek.Friday);
  })
  .DependsOn(ScheduleTaskBuilder.Create("Master Execution")
    .WithJob(new JobExec() { Descricao = "executa um job"})
    .ConfigureTriggers(triggerBuilder => triggerBuilder.CreateOnceTrigger().SetExecutionTime(DateTimeOffset.UtcNow.AddMinutes(1)))
    .NotDepends()
    .Build()
  )
  .WithStatus(ExecutionStatus.Failed)
  .SetAutoDelete(false)
  .SetIsActive(true)
  .Build();

await repo.AddAsync(task);

await host.RunAsync();

