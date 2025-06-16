using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class LessonSuggChangespt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_student_address_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_student_student_id",
                table: "lesson_suggestion");

            migrationBuilder.AlterColumn<int>(
                name: "student_id",
                table: "lesson_suggestion",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "address_id",
                table: "lesson_suggestion",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_student_address_id",
                table: "lesson_suggestion",
                column: "address_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_student_student_id",
                table: "lesson_suggestion",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AlterColumn<int>(
                name: "student_id",
                table: "lesson_suggestion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "address_id",
                table: "lesson_suggestion",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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
    }
}
