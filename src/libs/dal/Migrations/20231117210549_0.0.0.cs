using System;
using HSB.DAL;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HSB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _000 : PostgresSeedMigration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);
            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v1()"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperatingSystemItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RawData = table.Column<JsonDocument>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    ServiceNowKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingSystemItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationType = table.Column<int>(type: "integer", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    RawData = table.Column<JsonDocument>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    ServiceNowKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organization_Organization_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v1()"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    EmailVerifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Key = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v1()"),
                    DisplayName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    FailedLogins = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "0"),
                    LastLoginOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    RawData = table.Column<JsonDocument>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    ServiceNowKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    SubCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    UPlatform = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    DnsDomain = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    SysClassName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    FQDN = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    IPAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigurationItem_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupRole",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRole", x => new { x.GroupId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_GroupRole_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => new { x.UserId, x.GroupId });
                    table.ForeignKey(
                        name: "FK_UserGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOrganization",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrganization", x => new { x.UserId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_UserOrganization_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrganization_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileSystemItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConfigurationItemId = table.Column<int>(type: "integer", nullable: false),
                    RawData = table.Column<JsonDocument>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    ServiceNowKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    SubCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    StorageType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    MediaType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    VolumeId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    SysClassName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    Capacity = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    DiskSpace = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    Size = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    SizeBytes = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    UsedSizeBytes = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    AvailableSpace = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    FreeSpace = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    FreeSpaceBytes = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileSystemItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileSystemItem_ConfigurationItem_ConfigurationItemId",
                        column: x => x.ConfigurationItemId,
                        principalTable: "ConfigurationItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConfigurationItemId = table.Column<int>(type: "integer", nullable: false),
                    OperatingSystemItemId = table.Column<int>(type: "integer", nullable: false),
                    RawData = table.Column<JsonDocument>(type: "jsonb", nullable: false, defaultValueSql: "'{}'::jsonb"),
                    ServiceNowKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    SubCategory = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    DiskSpace = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    DnsDomain = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    SysClassName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    Platform = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValueSql: "''"),
                    IPAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValueSql: "''"),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServerItem_ConfigurationItem_ConfigurationItemId",
                        column: x => x.ConfigurationItemId,
                        principalTable: "ConfigurationItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerItem_OperatingSystemItem_OperatingSystemItemId",
                        column: x => x.OperatingSystemItemId,
                        principalTable: "OperatingSystemItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationItem_OrganizationId",
                table: "ConfigurationItem",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationItem_ServiceNowKey_Name_Category_SubCategory",
                table: "ConfigurationItem",
                columns: new[] { "ServiceNowKey", "Name", "Category", "SubCategory" });

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemItem_ConfigurationItemId",
                table: "FileSystemItem",
                column: "ConfigurationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FileSystemItem_ServiceNowKey",
                table: "FileSystemItem",
                column: "ServiceNowKey");

            migrationBuilder.CreateIndex(
                name: "IX_group",
                table: "Group",
                column: "IsEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_group_key",
                table: "Group",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_group_name",
                table: "Group",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupRole_RoleId",
                table: "GroupRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatingSystemItem_ServiceNowKey_UName",
                table: "OperatingSystemItem",
                columns: new[] { "ServiceNowKey", "UName" });

            migrationBuilder.CreateIndex(
                name: "IX_Organization_ParentId",
                table: "Organization",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_organize_code",
                table: "Organization",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_organize_name",
                table: "Organization",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_organize_serviceNowKey",
                table: "Organization",
                column: "ServiceNowKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role",
                table: "Role",
                column: "IsEnabled");

            migrationBuilder.CreateIndex(
                name: "IX_role_key",
                table: "Role",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerItem_ConfigurationItemId",
                table: "ServerItem",
                column: "ConfigurationItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerItem_OperatingSystemItemId",
                table: "ServerItem",
                column: "OperatingSystemItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerItem_ServiceNowKey",
                table: "ServerItem",
                column: "ServiceNowKey");

            migrationBuilder.CreateIndex(
                name: "IX_user_display_name",
                table: "User",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email_Phone_LastName_FirstName",
                table: "User",
                columns: new[] { "Email", "Phone", "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_user_key",
                table: "User",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_username",
                table: "User",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_GroupId",
                table: "UserGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganization_OrganizationId",
                table: "UserOrganization",
                column: "OrganizationId");
            PostUp(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);
            migrationBuilder.DropTable(
                name: "FileSystemItem");

            migrationBuilder.DropTable(
                name: "GroupRole");

            migrationBuilder.DropTable(
                name: "ServerItem");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "UserOrganization");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "ConfigurationItem");

            migrationBuilder.DropTable(
                name: "OperatingSystemItem");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Organization");
            PostDown(migrationBuilder);
        }
    }
}
