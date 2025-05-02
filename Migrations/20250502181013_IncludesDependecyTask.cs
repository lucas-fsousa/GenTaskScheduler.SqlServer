using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenTaskScheduler.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class IncludesDependecyTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DependsOnStatus",
                table: "ScheduledTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "DependsOnTaskId",
                table: "ScheduledTasks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledTasks_DependsOnTaskId",
                table: "ScheduledTasks",
                column: "DependsOnTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledTasks_ScheduledTasks_DependsOnTaskId",
                table: "ScheduledTasks",
                column: "DependsOnTaskId",
                principalTable: "ScheduledTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledTasks_ScheduledTasks_DependsOnTaskId",
                table: "ScheduledTasks");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledTasks_DependsOnTaskId",
                table: "ScheduledTasks");

            migrationBuilder.DropColumn(
                name: "DependsOnStatus",
                table: "ScheduledTasks");

            migrationBuilder.DropColumn(
                name: "DependsOnTaskId",
                table: "ScheduledTasks");
        }
    }
}
