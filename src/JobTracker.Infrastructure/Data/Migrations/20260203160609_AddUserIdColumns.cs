using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JobPostings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Applications");
        }
    }
}
