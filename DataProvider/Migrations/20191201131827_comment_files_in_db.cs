using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataProvider.Migrations
{
    public partial class comment_files_in_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentAttachments_Comments_CommentId",
                table: "CommentAttachments");

            migrationBuilder.DropColumn(
                name: "AttachedFilePath",
                table: "CommentAttachments");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "CommentAttachments");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "CommentAttachments");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "CommentAttachments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "CommentAttachments",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentAttachments_Comments_CommentId",
                table: "CommentAttachments",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentAttachments_Comments_CommentId",
                table: "CommentAttachments");

            migrationBuilder.DropColumn(
                name: "File",
                table: "CommentAttachments");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "CommentAttachments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AttachedFilePath",
                table: "CommentAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "CommentAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "CommentAttachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentAttachments_Comments_CommentId",
                table: "CommentAttachments",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
