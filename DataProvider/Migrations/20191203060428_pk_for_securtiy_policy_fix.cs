using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class pk_for_securtiy_policy_fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies",
                columns: new[] { "ProjectSettingsId", "UserId", "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies",
                columns: new[] { "ProjectSettingsId", "UserId" });
        }
    }
}
