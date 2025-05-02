using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenTaskScheduler.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTriggerStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Triggers",
                newName: "LastExecution");

            migrationBuilder.AddColumn<int>(
                name: "Executions",
                table: "Triggers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Triggers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Executions",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Triggers");

            migrationBuilder.RenameColumn(
                name: "LastExecution",
                table: "Triggers",
                newName: "StartTime");
        }
    }
}
