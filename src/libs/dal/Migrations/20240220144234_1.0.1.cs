using Microsoft.EntityFrameworkCore.Migrations;
using HSB.DAL;

#nullable disable

namespace HSB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _101 : PostgresSeedMigration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.DropIndex(
                name: "IX_ServerItem_Name",
                table: "ServerItem");

            migrationBuilder.DropIndex(
                name: "IX_FileSystemItem_Name_ServerItemServiceNowKey",
                table: "FileSystemItem");

            migrationBuilder.DropIndex(
                name: "IX_FileSystemHistoryItem_ServiceNowKey_ServerItemServiceNowKey",
                table: "FileSystemHistoryItem");

            migrationBuilder.AddColumn<int>(
                name: "InstallStatus",
                table: "ServerItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstallStatus",
                table: "ServerHistoryItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstallStatus",
                table: "FileSystemItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstallStatus",
                table: "FileSystemHistoryItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ServerItem_InstallStatus_UpdatedOn",
                table: "ServerItem",
                columns: new[] { "InstallStatus", "UpdatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_ServerHistoryItem_InstallStatus_ServiceNowKey",
                table: "ServerHistoryItem",
                columns: new[] { "InstallStatus", "ServiceNowKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemItem_InstallStatus_ServerItemServiceNowKey",
                table: "FileSystemItem",
                columns: new[] { "InstallStatus", "ServerItemServiceNowKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemHistoryItem_InstallStatus_ServiceNowKey_ServerItemServiceNowKey",
                table: "FileSystemHistoryItem",
                columns: new[] { "InstallStatus", "ServiceNowKey", "ServerItemServiceNowKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemHistoryItem_ServiceNowKey",
                table: "FileSystemHistoryItem",
                column: "ServiceNowKey");
            PostUp(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropIndex(
                name: "IX_ServerItem_InstallStatus_UpdatedOn",
                table: "ServerItem");

            migrationBuilder.DropIndex(
                name: "IX_ServerHistoryItem_InstallStatus_ServiceNowKey",
                table: "ServerHistoryItem");

            migrationBuilder.DropIndex(
                name: "IX_FileSystemItem_InstallStatus_ServerItemServiceNowKey",
                table: "FileSystemItem");

            migrationBuilder.DropIndex(
                name: "IX_FileSystemHistoryItem_InstallStatus_ServiceNowKey_ServerItemServiceNowKey",
                table: "FileSystemHistoryItem");

            migrationBuilder.DropIndex(
                name: "IX_FileSystemHistoryItem_ServiceNowKey",
                table: "FileSystemHistoryItem");

            migrationBuilder.DropColumn(
                name: "InstallStatus",
                table: "ServerItem");

            migrationBuilder.DropColumn(
                name: "InstallStatus",
                table: "ServerHistoryItem");

            migrationBuilder.DropColumn(
                name: "InstallStatus",
                table: "FileSystemItem");

            migrationBuilder.DropColumn(
                name: "InstallStatus",
                table: "FileSystemHistoryItem");

            migrationBuilder.CreateIndex(
                name: "IX_ServerItem_Name",
                table: "ServerItem",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemItem_Name_ServerItemServiceNowKey",
                table: "FileSystemItem",
                columns: new[] { "Name", "ServerItemServiceNowKey" });

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemHistoryItem_ServiceNowKey_ServerItemServiceNowKey",
                table: "FileSystemHistoryItem",
                columns: new[] { "ServiceNowKey", "ServerItemServiceNowKey" });
            PostDown(migrationBuilder);
        }
    }
}
