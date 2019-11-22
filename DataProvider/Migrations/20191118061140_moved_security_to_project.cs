using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class moved_security_to_project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardSettings");

            migrationBuilder.AddColumn<int>(
                name: "AccessToChangeBoard",
                table: "ProjectSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessToChangeTask",
                table: "ProjectSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessToCreateTask",
                table: "ProjectSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessToDeleteTask",
                table: "ProjectSettings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessToChangeBoard",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "AccessToChangeTask",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "AccessToCreateTask",
                table: "ProjectSettings");

            migrationBuilder.DropColumn(
                name: "AccessToDeleteTask",
                table: "ProjectSettings");

            migrationBuilder.CreateTable(
                name: "BoardSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    AccessToChangeBoard = table.Column<int>(type: "int", nullable: false),
                    AccessToChangeTask = table.Column<int>(type: "int", nullable: false),
                    AccessToCreateTask = table.Column<int>(type: "int", nullable: false),
                    AccessToDeleteTask = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardSettings_Boards_Id",
                        column: x => x.Id,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
