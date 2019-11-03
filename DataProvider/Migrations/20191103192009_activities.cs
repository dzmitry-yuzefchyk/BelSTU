using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class activities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropIndex(
                name: "IX_ProjectSecurityPolicies_ProjectSecuritySettingsId",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Activities");

            migrationBuilder.AddColumn<string>(
                name: "SecretKey",
                table: "ProjectSecuritySettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "ProjectSecurityPolicies",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAllowed",
                table: "ProjectSecurityPolicies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseSecretKey",
                table: "ProjectSecurityPolicies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Activities",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Activities",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies",
                columns: new[] { "ProjectSecuritySettingsId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                columns: new[] { "UserId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ProjectId",
                table: "Activities",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_UserId",
                table: "Activities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Projects_ProjectId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_UserId",
                table: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ProjectId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "SecretKey",
                table: "ProjectSecuritySettings");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropColumn(
                name: "IsAllowed",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropColumn(
                name: "UseSecretKey",
                table: "ProjectSecurityPolicies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Activities");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProjectSecurityPolicies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectSecurityPolicies",
                table: "ProjectSecurityPolicies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSecurityPolicies_ProjectSecuritySettingsId",
                table: "ProjectSecurityPolicies",
                column: "ProjectSecuritySettingsId");
        }
    }
}
