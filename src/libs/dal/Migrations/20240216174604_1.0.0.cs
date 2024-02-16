using Microsoft.EntityFrameworkCore.Migrations;
using HSB.DAL;

#nullable disable

namespace HSB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _100 : PostgresSeedMigration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.DropIndex(
                name: "IX_FileSystemItem_Name",
                table: "FileSystemItem");

            migrationBuilder.DropIndex(
                name: "IX_FileSystemHistoryItem_ServiceNowKey",
                table: "FileSystemHistoryItem");

            migrationBuilder.AddColumn<string>(
                name: "ServerItemServiceNowKey",
                table: "FileSystemHistoryItem",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemItem_Name_ServerItemServiceNowKey",
                table: "FileSystemItem",
                columns: new[] { "Name", "ServerItemServiceNowKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemHistoryItem_ServiceNowKey_ServerItemServiceNowKey",
                table: "FileSystemHistoryItem",
                columns: new[] { "ServiceNowKey", "ServerItemServiceNowKey" });
            PostUp(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropIndex(
                name: "IX_FileSystemItem_Name_ServerItemServiceNowKey",
                table: "FileSystemItem");

            migrationBuilder.DropIndex(
                name: "IX_FileSystemHistoryItem_ServiceNowKey_ServerItemServiceNowKey",
                table: "FileSystemHistoryItem");

            migrationBuilder.DropColumn(
                name: "ServerItemServiceNowKey",
                table: "FileSystemHistoryItem");

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemItem_Name",
                table: "FileSystemItem",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemHistoryItem_ServiceNowKey",
                table: "FileSystemHistoryItem",
                column: "ServiceNowKey");
            PostDown(migrationBuilder);
        }
    }
}
