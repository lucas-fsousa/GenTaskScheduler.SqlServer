using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenTaskScheduler.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class IncLastHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LastExecutionHistoryId",
                table: "ScheduledTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_NextExecution",
                table: "Triggers",
                column: "NextExecution");

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_NextExecution_LastTriggeredStatus",
                table: "Triggers",
                columns: new[] { "NextExecution", "LastTriggeredStatus" });

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

            migrationBuilder.DropIndex(
                name: "IX_Triggers_NextExecution",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Triggers_NextExecution_LastTriggeredStatus",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledTasks_ExecutionStatus",
                table: "ScheduledTasks");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledTasks_IsActive",
                table: "ScheduledTasks");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledTasks_LastExecutionHistoryId",
                table: "ScheduledTasks");

            migrationBuilder.DropColumn(
                name: "LastExecutionHistoryId",
                table: "ScheduledTasks");
        }
    }
}
