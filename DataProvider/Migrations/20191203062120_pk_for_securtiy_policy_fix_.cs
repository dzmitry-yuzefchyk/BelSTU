using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class pk_for_securtiy_policy_fix_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ProjectSecurityPolicies",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies",
                columns: new[] { "Id", "ProjectSettingsId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSecurityPolicies_ProjectSettingsId",
                table: "ProjectSecurityPolicies",
                column: "ProjectSettingsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropIndex(
                name: "IX_ProjectSecurityPolicies_ProjectSettingsId",
                table: "ProjectSecurityPolicies");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ProjectSecurityPolicies",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies",
                columns: new[] { "ProjectSettingsId", "UserId", "Id" });
        }
    }
}
