using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class changed_simple_security : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToCreateBoard",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "AccessToCreateTask",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "AccessToDeleteBoard",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "AccessToDeleteTask",
                table: "ProjectSettings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccessToCreateBoard",
                table: "ProjectSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessToCreateTask",
                table: "ProjectSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessToDeleteBoard",
                table: "ProjectSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessToDeleteTask",
                table: "ProjectSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
