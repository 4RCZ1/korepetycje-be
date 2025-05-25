using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HelloWorld.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    BeginTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Offset = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndOrdinal = table.Column<int>(type: "integer", nullable: false),
                    LessonDuration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Student_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesId);
                    table.ForeignKey(
                        name: "FK_Series_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Series_id = table.Column<int>(type: "integer", nullable: false),
                    Ordinal = table.Column<int>(type: "integer", nullable: false),
                    Custom_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Custom_duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Tutor_info = table.Column<string>(type: "text", nullable: false),
                    Is_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    Has_occured = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => new { x.Series_id, x.Ordinal });
                    table.ForeignKey(
                        name: "FK_Lessons_Series_Series_id",
                        column: x => x.Series_id,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Series_StudentId",
                table: "Series",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Series");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
