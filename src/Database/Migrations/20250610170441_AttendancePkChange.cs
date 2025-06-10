using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AttendancePkChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_attendance",
                table: "attendance");

            migrationBuilder.DropIndex(
                name: "IX_attendance_lesson_id",
                table: "attendance");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "attendance");

            migrationBuilder.AlterColumn<string>(
                name: "address_name",
                table: "address",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_attendance",
                table: "attendance",
                columns: new[] { "lesson_id", "student_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_attendance",
                table: "attendance");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "attendance",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "address_name",
                table: "address",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_attendance",
                table: "attendance",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_lesson_id",
                table: "attendance",
                column: "lesson_id");
        }
    }
}
