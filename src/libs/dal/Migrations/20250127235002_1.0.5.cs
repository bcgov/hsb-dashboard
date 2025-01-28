using Microsoft.EntityFrameworkCore.Migrations;
using HSB.DAL;

#nullable disable

namespace HSB.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _105 : PostgresSeedMigration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            PreUp(migrationBuilder);

            PostUp(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            PreDown(migrationBuilder);

            PostDown(migrationBuilder);
        }
    }
}
