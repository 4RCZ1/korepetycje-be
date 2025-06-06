using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class LessonSuggestionFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "lesson_suggestion",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    suggested_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    suggested_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lesson_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson_suggestion", x => x.id);
                    table.ForeignKey(
                        name: "FK_lesson_suggestion_lesson_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
                        principalColumn: "lesson_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_lesson_id",
                table: "lesson_suggestion",
                column: "lesson_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lesson_suggestion");
        }
    }
}
