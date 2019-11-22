using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class removed_security_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSecurityPolicies_ProjectSecuritySettings_ProjectSecuritySettingsId",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropTable(
                name: "ProjectSecuritySettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropColumn(
                name: "ProjectSecuritySettingsId",
                table: "ProjectSecurityPolicies");

            migrationBuilder.AddColumn<int>(
                name: "ProjectSettingsId",
                table: "ProjectSecurityPolicies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies",
                columns: new[] { "ProjectSettingsId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSecurityPolicies_ProjectSettings_ProjectSettingsId",
                table: "ProjectSecurityPolicies",
                column: "ProjectSettingsId",
                principalTable: "ProjectSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectSecurityPolicies_ProjectSettings_ProjectSettingsId",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropColumn(
                name: "ProjectSettingsId",
                table: "ProjectSecurityPolicies");

            migrationBuilder.AddColumn<int>(
                name: "ProjectSecuritySettingsId",
                table: "ProjectSecurityPolicies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies",
                columns: new[] { "ProjectSecuritySettingsId", "UserId" });

            migrationBuilder.CreateTable(
                name: "ProjectSecuritySettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSecuritySettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSecuritySettings_Projects_Id",
                        column: x => x.Id,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectSecurityPolicies_ProjectSecuritySettings_ProjectSecuritySettingsId",
                table: "ProjectSecurityPolicies",
                column: "ProjectSecuritySettingsId",
                principalTable: "ProjectSecuritySettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
