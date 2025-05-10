using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenTaskScheduler.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class SetupInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalendarEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Executed = table.Column<bool>(type: "bit", nullable: false),
                    CalendarTriggerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NextExecution = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastExecution = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ExecutionStatus = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    AutoDelete = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BlobArgs = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    MaxExecutionTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    LastExecutionHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DependsOnStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DependsOnTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledTasks_ScheduledTasks_DependsOnTaskId",
                        column: x => x.DependsOnTaskId,
                        principalTable: "ScheduledTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskExecutionsHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TriggerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskExecutionsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskExecutionsHistory_ScheduledTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "ScheduledTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Triggers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartsAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndsAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ShouldAutoDelete = table.Column<bool>(type: "bit", nullable: false),
                    ExecutionInterval = table.Column<TimeSpan>(type: "time", nullable: true),
                    TriggerDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    LastTriggeredStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastExecution = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NextExecution = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MaxExecutions = table.Column<int>(type: "int", nullable: true),
                    Executions = table.Column<int>(type: "int", nullable: false),
                    TimeOfDay = table.Column<TimeOnly>(type: "time", nullable: false),
                    TriggerType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    CronExpression = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DaysOfMonth = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MonthsOfYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Triggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Triggers_ScheduledTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "ScheduledTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntries_CalendarTriggerId",
                table: "CalendarEntries",
                column: "CalendarTriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTasks_DependsOnTaskId",
                table: "ScheduledTasks",
                column: "DependsOnTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTasks_ExecutionStatus",
                table: "ScheduledTasks",
                column: "ExecutionStatus");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTasks_IsActive",
                table: "ScheduledTasks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTasks_LastExecutionHistoryId",
                table: "ScheduledTasks",
                column: "LastExecutionHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskExecutionsHistory_TaskId",
                table: "TaskExecutionsHistory",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_NextExecution",
                table: "Triggers",
                column: "NextExecution");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_NextExecution_LastTriggeredStatus",
                table: "Triggers",
                columns: new[] { "NextExecution", "LastTriggeredStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_TaskId",
                table: "Triggers",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEntries_Triggers_CalendarTriggerId",
                table: "CalendarEntries",
                column: "CalendarTriggerId",
                principalTable: "Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledTasks_TaskExecutionsHistory_LastExecutionHistoryId",
                table: "ScheduledTasks",
                column: "LastExecutionHistoryId",
                principalTable: "TaskExecutionsHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledTasks_TaskExecutionsHistory_LastExecutionHistoryId",
                table: "ScheduledTasks");

            migrationBuilder.DropTable(
                name: "CalendarEntries");

            migrationBuilder.DropTable(
                name: "Triggers");

            migrationBuilder.DropTable(
                name: "TaskExecutionsHistory");

            migrationBuilder.DropTable(
                name: "ScheduledTasks");
        }
    }
}
