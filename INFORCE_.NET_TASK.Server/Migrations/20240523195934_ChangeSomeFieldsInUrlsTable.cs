using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFORCE_.NET_TASK.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSomeFieldsInUrlsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Urls",
                table: "Urls");

            migrationBuilder.AlterColumn<string>(
                name: "OriginalUrl",
                table: "Urls",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ShortUrl",
                table: "Urls",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Urls",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Urls",
                table: "Urls",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_OriginalUrl",
                table: "Urls",
                column: "OriginalUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Urls_ShortUrl",
                table: "Urls",
                column: "ShortUrl",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Urls",
                table: "Urls");

            migrationBuilder.DropIndex(
                name: "IX_Urls_OriginalUrl",
                table: "Urls");

            migrationBuilder.DropIndex(
                name: "IX_Urls_ShortUrl",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Urls");

            migrationBuilder.AlterColumn<string>(
                name: "ShortUrl",
                table: "Urls",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "OriginalUrl",
                table: "Urls",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Urls",
                table: "Urls",
                column: "ShortUrl");
        }
    }
}
