using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class files_in_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAttachments_Tasks_TaskId",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "AttachedFilePath",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "TaskAttachments");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "TaskAttachments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "TaskAttachments",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAttachments_Tasks_TaskId",
                table: "TaskAttachments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAttachments_Tasks_TaskId",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "File",
                table: "TaskAttachments");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "TaskAttachments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AttachedFilePath",
                table: "TaskAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "TaskAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "TaskAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAttachments_Tasks_TaskId",
                table: "TaskAttachments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
