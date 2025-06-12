using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStudentIdFromSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_schedule_student_student_id",
                table: "schedule");

            migrationBuilder.DropIndex(
                name: "IX_schedule_student_id",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "student_id",
                table: "schedule");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "student_id",
                table: "schedule",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_schedule_student_id",
                table: "schedule",
                column: "student_id");

            migrationBuilder.AddForeignKey(
                name: "FK_schedule_student_student_id",
                table: "schedule",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
