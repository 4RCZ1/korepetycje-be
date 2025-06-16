using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class LessonSuggChangestpt1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "address_id",
                table: "lesson_suggestion",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "student_id",
                table: "lesson_suggestion",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_address_id",
                table: "lesson_suggestion",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_student_id",
                table: "lesson_suggestion",
                column: "student_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_student_address_id",
                table: "lesson_suggestion",
                column: "address_id",
                principalTable: "student",
                principalColumn: "student_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_student_student_id",
                table: "lesson_suggestion",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_student_address_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_student_student_id",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_address_id",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_student_id",
                table: "lesson_suggestion");

            migrationBuilder.DropColumn(
                name: "address_id",
                table: "lesson_suggestion");

            migrationBuilder.DropColumn(
                name: "student_id",
                table: "lesson_suggestion");
        }
    }
}
