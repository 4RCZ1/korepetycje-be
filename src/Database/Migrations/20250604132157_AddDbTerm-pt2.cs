using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDbTermpt2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_terms_term_id",
                table: "lesson");

            migrationBuilder.DropColumn(
                name: "start_time",
                table: "lesson");

            migrationBuilder.AlterColumn<int>(
                name: "term_id",
                table: "lesson",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_terms_term_id",
                table: "lesson",
                column: "term_id",
                principalTable: "terms",
                principalColumn: "term_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lesson_terms_term_id",
                table: "lesson");

            migrationBuilder.AlterColumn<int>(
                name: "term_id",
                table: "lesson",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTime>(
                name: "start_time",
                table: "lesson",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_terms_term_id",
                table: "lesson",
                column: "term_id",
                principalTable: "terms",
                principalColumn: "term_id");
        }
    }
}
