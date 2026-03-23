using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrookApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_localization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "localized_strings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    profile_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    key = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    localized = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_localized_strings", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "localized_strings");
        }
    }
}
