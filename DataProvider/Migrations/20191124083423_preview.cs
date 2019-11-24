using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class preview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PathToBackground",
                table: "ProjectSettings");

            migrationBuilder.AddColumn<string>(
                name: "Preview",
                table: "ProjectSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preview",
                table: "ProjectSettings");

            migrationBuilder.AddColumn<string>(
                name: "PathToBackground",
                table: "ProjectSettings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
