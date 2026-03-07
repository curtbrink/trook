using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrookApi.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_processed_file_hash_index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_processed_files_file_hash_is_success",
                table: "processed_files",
                columns: new[] { "file_hash", "is_success" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_processed_files_file_hash_is_success",
                table: "processed_files");
        }
    }
}
