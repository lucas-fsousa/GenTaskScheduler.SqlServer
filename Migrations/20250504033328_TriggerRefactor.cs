using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenTaskScheduler.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class TriggerRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Executed",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "InitialExecutionTime",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "RepeatIntervalMinutes",
                table: "Triggers");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "NextExecution",
                table: "ScheduledTasks",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<bool>(
                name: "Executed",
                table: "CalendarEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextExecution",
                table: "ScheduledTasks");

            migrationBuilder.DropColumn(
                name: "Executed",
                table: "CalendarEntries");

            migrationBuilder.AddColumn<bool>(
                name: "Executed",
                table: "Triggers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "InitialExecutionTime",
                table: "Triggers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepeatIntervalMinutes",
                table: "Triggers",
                type: "int",
                nullable: true);
        }
    }
}
