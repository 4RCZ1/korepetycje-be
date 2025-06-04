using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class DbTermAddedpt1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "term_id",
                table: "lesson",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "terms",
                columns: table => new
                {
                    term_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_free = table.Column<bool>(type: "boolean", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_terms", x => x.term_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_lesson_term_id",
                table: "lesson",
                column: "term_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_terms_term_id",
                table: "lesson",
                column: "term_id",
                principalTable: "terms",
                principalColumn: "term_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_terms_term_id",
                table: "lesson");

            migrationBuilder.DropTable(
                name: "terms");

            migrationBuilder.DropIndex(
                name: "IX_lesson_term_id",
                table: "lesson");

            migrationBuilder.DropColumn(
                name: "term_id",
                table: "lesson");
        }
    }
}
