using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrookApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_player_and_jobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "player_jobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    player_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    is_quick = table.Column<bool>(type: "INTEGER", nullable: false),
                    started_at = table.Column<int>(type: "INTEGER", nullable: false),
                    finished_at = table.Column<int>(type: "INTEGER", nullable: false),
                    source_city = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    source_company = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    dest_city = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    dest_company = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    cargo_type = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    cargo_size = table.Column<int>(type: "INTEGER", nullable: false),
                    cargo_weight = table.Column<float>(type: "REAL", nullable: false),
                    base_distance = table.Column<int>(type: "INTEGER", nullable: false),
                    base_revenue = table.Column<int>(type: "INTEGER", nullable: false),
                    real_revenue = table.Column<int>(type: "INTEGER", nullable: false),
                    real_distance = table.Column<int>(type: "INTEGER", nullable: false),
                    real_xp = table.Column<int>(type: "INTEGER", nullable: false),
                    parking_level = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_jobs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    updated_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    profile_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    total_distance = table.Column<long>(type: "INTEGER", nullable: false),
                    hq_city = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "player_jobs");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
