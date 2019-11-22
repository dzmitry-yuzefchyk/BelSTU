using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class removed_secret_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "ProjectSecuritySettings");

            migrationBuilder.DropColumn(
                name: "UseSecretKey",
                table: "ProjectSecurityPolicies");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "ProjectUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "ProjectUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "ProjectSecuritySettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseSecretKey",
                table: "ProjectSecurityPolicies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
