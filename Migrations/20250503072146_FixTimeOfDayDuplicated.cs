using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenTaskScheduler.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class FixTimeOfDayDuplicated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MonthlyTrigger_TimeOfDay",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "WeeklyTrigger_TimeOfDay",
                table: "Triggers");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeOfDay",
                table: "Triggers",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0),
                oldClrType: typeof(TimeOnly),
                oldType: "time",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeOfDay",
                table: "Triggers",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "MonthlyTrigger_TimeOfDay",
                table: "Triggers",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "WeeklyTrigger_TimeOfDay",
                table: "Triggers",
                type: "time",
                nullable: true);
        }
    }
}
