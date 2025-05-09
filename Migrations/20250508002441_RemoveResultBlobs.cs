using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenTaskScheduler.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveResultBlobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultBlob",
                table: "TaskExecutionsHistory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ResultBlob",
                table: "TaskExecutionsHistory",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
