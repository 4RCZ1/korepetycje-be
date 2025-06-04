using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonSuggestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_timeslots_timeslot_id",
                table: "lesson");

            migrationBuilder.AlterColumn<int>(
                name: "timeslot_id",
                table: "lesson",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_timeslots_timeslot_id",
                table: "lesson",
                column: "timeslot_id",
                principalTable: "timeslots",
                principalColumn: "timeslot_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_timeslots_timeslot_id",
                table: "lesson");

            migrationBuilder.AlterColumn<int>(
                name: "timeslot_id",
                table: "lesson",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_timeslots_timeslot_id",
                table: "lesson",
                column: "timeslot_id",
                principalTable: "timeslots",
                principalColumn: "timeslot_id");
        }
    }
}
