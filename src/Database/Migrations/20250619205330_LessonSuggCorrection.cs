using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class LessonSuggCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_student_address_id",
                table: "lesson_suggestion");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_address_address_id",
                table: "lesson_suggestion",
                column: "address_id",
                principalTable: "address",
                principalColumn: "address_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_address_address_id",
                table: "lesson_suggestion");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_student_address_id",
                table: "lesson_suggestion",
                column: "address_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
