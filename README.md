# GenTaskScheduler.SqlServer

## 📌 Objective

**GenTaskScheduler.SqlServer** is an implementation of the [GenTaskScheduler](https://github.com/lucas-fsousa/GenTaskScheduler) framework tailored for use with **SQL Server** and **Entity Framework Core**. It provides robust persistence and execution support for scheduled tasks using a relational database backend.

This package extends the core task scheduling capabilities provided by `GenTaskScheduler`, enabling SQL Server integration with minimal setup.

---

## 🔗 Useful Links

- [GenTaskScheduler (Core Repository)](https://github.com/lucas-fsousa/GenTaskScheduler) – Task scheduling engine core.
- [Entity Framework Core Documentation](https://learn.microsoft.com/en-us/ef/core/) – ORM used for SQL Server persistence.
- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/) – To understand schema migration handling.
- [SQL Server Overview](https://learn.microsoft.com/en-us/sql/sql-server/) – To get started with SQL Server.

---

## ⚙️ How It Works

This package integrates SQL Server with the GenTaskScheduler runtime through the use of EF Core. It includes:

- Repositories for managing tasks, triggers, and execution history.
- Automatic or manual schema management via EF Core migrations.
- A background hosted service that regularly checks for eligible tasks and triggers them at the appropriate time.

The task execution flow looks like this:

1. Tasks and triggers are stored in the SQL Server database.
2. A background service periodically checks the database for triggers ready to execute.
3. When a trigger is due, the corresponding job is launched via the registered scheduler infrastructure.

You don’t manually execute the tasks — the scheduler system takes care of that for you.

---

## 🚀 How to Integrate

### 1. Install the NuGet Package

```bash
dotnet add package GenTaskScheduler.SqlServer
```

---

### 2. Configure the Scheduler

Example using `HostBuilder`:

```csharp
var host = Host.CreateDefaultBuilder(args)
  .ConfigureAppConfiguration((context, config) => 
      config.AddJsonFile("appsettings.json", optional: false))
  .ConfigureServices((context, services) => {
    var connString = context.Configuration.GetConnectionString("DefaultConnection")!;

    // the schedule service configuration
    services.AddGenTaskSchedulerWithSqlServer(connString, options => {
      options.MaxRetry = 3;
      options.RetryWaitDelay = TimeSpan.FromSeconds(5);
      options.RetryOnFailure = true;
      options.LateExecutionTolerance = TimeSpan.FromSeconds(10);
      options.DatabaseCheckInterval = TimeSpan.FromSeconds(10);
    });
  }).Build();
```

---

### 3. Scheduling Tasks with the Fluent API

```csharp
using var scope = host.Services.CreateScope();
var repo = scope.ServiceProvider.GetRequiredService<ITaskRepository>();

var task = GenScheduleTaskBuilder.Create($"A test calendar task")
  .WithJob(new MyClassJobExec() {
    Description = "any job"
  }).ConfigureTriggers(triggerBuilder => {
    /* Creates a calendar-based trigger that will be valid in 1 minute. 
     * This trigger will have only one execution that should happen in 2 minutes (1 minute after valid activation)
     */
    triggerBuilder.CreateCalendarTrigger(DateTimeOffset.UtcNow.AddMinutes(1))
      .AddCalendarEntries([
        new () { ScheduledDateTime = DateTimeOffset.UtcNow.AddMinutes(2) }
      ])
      .Build();
  }).NotDepends()
  .SetAutoDelete(false)
  .SetIsActive(true)
  .SetTimeout(TimeSpan.FromSeconds(20))
  .Build();

await repo.AddAsync(task);
```

---

### 4. Manual Schema Generation (Optional)

If you do **not** want to apply EF Core migrations automatically at runtime (via `applyMigrations: true`), you can use the built-in **SchemeExporter** to export the database creation script manually:

```csharp
var schemeProvider = provider.GetRequiredService<ISchemeProvider>();
var scripts = schemeProvider.GenerateSchemeScript();
File.AppendAllText("/your/path/for/file.sql", scripts);
```

This is useful for teams that prefer to manage database schemas explicitly via migration pipelines or DevOps.

---

## 🧑‍💻 Contribution

We welcome contributions! Feel free to open issues, suggest features, or submit PRs.

---

## 🛠️ Technologies Used

- .NET 8+
- SQL Server
- Entity Framework Core
- Hosted Services
- Dependency Injection

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).