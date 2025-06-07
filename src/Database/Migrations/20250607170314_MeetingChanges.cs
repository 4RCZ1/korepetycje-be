using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class MeetingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_timeslots_timeslot_id",
                table: "lesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_timeslots",
                table: "timeslots");

            migrationBuilder.DropColumn(
                name: "address",
                table: "student");

            migrationBuilder.DropColumn(
                name: "lesson_duration",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "custom_duration",
                table: "lesson");

            migrationBuilder.DropColumn(
                name: "has_occurred",
                table: "lesson");

            migrationBuilder.DropColumn(
                name: "is_confirmed",
                table: "lesson");

            migrationBuilder.DropColumn(
                name: "is_free",
                table: "timeslots");

            migrationBuilder.RenameTable(
                name: "timeslots",
                newName: "timeslot");

            migrationBuilder.AddColumn<int>(
                name: "address_id",
                table: "student",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "timeslot_id",
                table: "lesson_suggestion",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_timeslot",
                table: "timeslot",
                column: "timeslot_id");

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    address_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    address_data = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "attendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lesson_id = table.Column<int>(type: "integer", nullable: false),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    is_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    has_occurred = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_attendance_lesson_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
                        principalColumn: "lesson_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attendance_student_student_id",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_address_id",
                table: "student",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_timeslot_id",
                table: "lesson_suggestion",
                column: "timeslot_id");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_lesson_id",
                table: "attendance",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_student_id",
                table: "attendance",
                column: "student_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_timeslot_timeslot_id",
                table: "lesson",
                column: "timeslot_id",
                principalTable: "timeslot",
                principalColumn: "timeslot_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_timeslot_timeslot_id",
                table: "lesson_suggestion",
                column: "timeslot_id",
                principalTable: "timeslot",
                principalColumn: "timeslot_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_address_address_id",
                table: "student",
                column: "address_id",
                principalTable: "address",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_timeslot_timeslot_id",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_timeslot_timeslot_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_student_address_address_id",
                table: "student");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "attendance");

            migrationBuilder.DropIndex(
                name: "IX_student_address_id",
                table: "student");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_timeslot_id",
                table: "lesson_suggestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_timeslot",
                table: "timeslot");

            migrationBuilder.DropColumn(
                name: "address_id",
                table: "student");

            migrationBuilder.DropColumn(
                name: "timeslot_id",
                table: "lesson_suggestion");

            migrationBuilder.RenameTable(
                name: "timeslot",
                newName: "timeslots");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "student",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "lesson_duration",
                table: "schedule",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "custom_duration",
                table: "lesson",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "has_occurred",
                table: "lesson",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_confirmed",
                table: "lesson",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_free",
                table: "timeslots",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_timeslots",
                table: "timeslots",
                column: "timeslot_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_timeslots_timeslot_id",
                table: "lesson",
                column: "timeslot_id",
                principalTable: "timeslots",
                principalColumn: "timeslot_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
