using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrookApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_driver_jobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "driver_jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    driver_id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    day_completed = table.Column<uint>(type: "INTEGER", nullable: false),
                    is_empty = table.Column<bool>(type: "INTEGER", nullable: false),
                    revenue = table.Column<long>(type: "INTEGER", nullable: false),
                    wage = table.Column<long>(type: "INTEGER", nullable: false),
                    maintenance = table.Column<long>(type: "INTEGER", nullable: false),
                    fuel = table.Column<long>(type: "INTEGER", nullable: false),
                    distance = table.Column<uint>(type: "INTEGER", nullable: false),
                    cargo_type = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    cargo_size = table.Column<uint>(type: "INTEGER", nullable: true),
                    source_city = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    source_company = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    dest_city = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    dest_company = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_driver_jobs", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "driver_jobs");
        }
    }
}
