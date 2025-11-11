using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class MultiTenant_SimplifyPrimaryKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendance_lesson_lesson_id_tenant_id",
                table: "attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_attendance_student_student_id_tenant_id",
                table: "attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_schedule_schedule_id_tenant_id",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_timeslot_timeslot_id_tenant_id",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_address_address_id_tenant_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_lesson_lesson_id_tenant_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_student_student_id_tenant_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_timeslot_timeslot_id_tenant_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_schedule_address_address_id_tenant_id",
                table: "schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_student_address_address_id_tenant_id",
                table: "student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_timeslot",
                table: "timeslot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student",
                table: "student");

            migrationBuilder.DropIndex(
                name: "IX_student_address_id_tenant_id",
                table: "student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_schedule",
                table: "schedule");

            migrationBuilder.DropIndex(
                name: "IX_schedule_address_id_tenant_id",
                table: "schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lesson_suggestion",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_address_id_tenant_id",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_lesson_id_tenant_id",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_student_id_tenant_id",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_timeslot_id_tenant_id",
                table: "lesson_suggestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lesson",
                table: "lesson");

            migrationBuilder.DropIndex(
                name: "IX_lesson_schedule_id_tenant_id",
                table: "lesson");

            migrationBuilder.DropIndex(
                name: "IX_lesson_timeslot_id_tenant_id",
                table: "lesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_attendance",
                table: "attendance");

            migrationBuilder.DropIndex(
                name: "IX_attendance_lesson_id_tenant_id",
                table: "attendance");

            migrationBuilder.DropIndex(
                name: "IX_attendance_student_id_tenant_id",
                table: "attendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_address",
                table: "address");

            migrationBuilder.AlterColumn<int>(
                name: "timeslot_id",
                table: "timeslot",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "student_id",
                table: "student",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "schedule_id",
                table: "schedule",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "lesson_suggestion_id",
                table: "lesson_suggestion",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "lesson_id",
                table: "lesson",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "address_id",
                table: "address",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_timeslot",
                table: "timeslot",
                column: "timeslot_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_student",
                table: "student",
                column: "student_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_schedule",
                table: "schedule",
                column: "schedule_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lesson_suggestion",
                table: "lesson_suggestion",
                column: "lesson_suggestion_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lesson",
                table: "lesson",
                column: "lesson_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_attendance",
                table: "attendance",
                columns: new[] { "lesson_id", "student_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_address",
                table: "address",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_address_id",
                table: "student",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_schedule_address_id",
                table: "schedule",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_address_id",
                table: "lesson_suggestion",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_lesson_id",
                table: "lesson_suggestion",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_student_id",
                table: "lesson_suggestion",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_timeslot_id",
                table: "lesson_suggestion",
                column: "timeslot_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_schedule_id",
                table: "lesson",
                column: "schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_timeslot_id",
                table: "lesson",
                column: "timeslot_id");

            migrationBuilder.CreateIndex(
                name: "IX_attendance_student_id",
                table: "attendance",
                column: "student_id");

            migrationBuilder.AddForeignKey(
                name: "FK_attendance_lesson_lesson_id",
                table: "attendance",
                column: "lesson_id",
                principalTable: "lesson",
                principalColumn: "lesson_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_attendance_student_student_id",
                table: "attendance",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_schedule_schedule_id",
                table: "lesson",
                column: "schedule_id",
                principalTable: "schedule",
                principalColumn: "schedule_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_timeslot_timeslot_id",
                table: "lesson",
                column: "timeslot_id",
                principalTable: "timeslot",
                principalColumn: "timeslot_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_address_address_id",
                table: "lesson_suggestion",
                column: "address_id",
                principalTable: "address",
                principalColumn: "address_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_lesson_lesson_id",
                table: "lesson_suggestion",
                column: "lesson_id",
                principalTable: "lesson",
                principalColumn: "lesson_id");

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_student_student_id",
                table: "lesson_suggestion",
                column: "student_id",
                principalTable: "student",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_timeslot_timeslot_id",
                table: "lesson_suggestion",
                column: "timeslot_id",
                principalTable: "timeslot",
                principalColumn: "timeslot_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_schedule_address_address_id",
                table: "schedule",
                column: "address_id",
                principalTable: "address",
                principalColumn: "address_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_address_address_id",
                table: "student",
                column: "address_id",
                principalTable: "address",
                principalColumn: "address_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendance_lesson_lesson_id",
                table: "attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_attendance_student_student_id",
                table: "attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_schedule_schedule_id",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_timeslot_timeslot_id",
                table: "lesson");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_address_address_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_lesson_lesson_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_student_student_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_lesson_suggestion_timeslot_timeslot_id",
                table: "lesson_suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_schedule_address_address_id",
                table: "schedule");

            migrationBuilder.DropForeignKey(
                name: "FK_student_address_address_id",
                table: "student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_timeslot",
                table: "timeslot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_student",
                table: "student");

            migrationBuilder.DropIndex(
                name: "IX_student_address_id",
                table: "student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_schedule",
                table: "schedule");

            migrationBuilder.DropIndex(
                name: "IX_schedule_address_id",
                table: "schedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lesson_suggestion",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_address_id",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_lesson_id",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_student_id",
                table: "lesson_suggestion");

            migrationBuilder.DropIndex(
                name: "IX_lesson_suggestion_timeslot_id",
                table: "lesson_suggestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lesson",
                table: "lesson");

            migrationBuilder.DropIndex(
                name: "IX_lesson_schedule_id",
                table: "lesson");

            migrationBuilder.DropIndex(
                name: "IX_lesson_timeslot_id",
                table: "lesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_attendance",
                table: "attendance");

            migrationBuilder.DropIndex(
                name: "IX_attendance_student_id",
                table: "attendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_address",
                table: "address");

            migrationBuilder.AlterColumn<int>(
                name: "timeslot_id",
                table: "timeslot",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "student_id",
                table: "student",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "schedule_id",
                table: "schedule",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "lesson_suggestion_id",
                table: "lesson_suggestion",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "lesson_id",
                table: "lesson",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "address_id",
                table: "address",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_timeslot",
                table: "timeslot",
                columns: new[] { "timeslot_id", "tenant_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_student",
                table: "student",
                columns: new[] { "student_id", "tenant_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_schedule",
                table: "schedule",
                columns: new[] { "schedule_id", "tenant_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_lesson_suggestion",
                table: "lesson_suggestion",
                columns: new[] { "lesson_suggestion_id", "tenant_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_lesson",
                table: "lesson",
                columns: new[] { "lesson_id", "tenant_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_attendance",
                table: "attendance",
                columns: new[] { "lesson_id", "student_id", "tenant_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_address",
                table: "address",
                columns: new[] { "address_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_student_address_id_tenant_id",
                table: "student",
                columns: new[] { "address_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_schedule_address_id_tenant_id",
                table: "schedule",
                columns: new[] { "address_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_address_id_tenant_id",
                table: "lesson_suggestion",
                columns: new[] { "address_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_lesson_id_tenant_id",
                table: "lesson_suggestion",
                columns: new[] { "lesson_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_student_id_tenant_id",
                table: "lesson_suggestion",
                columns: new[] { "student_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_lesson_suggestion_timeslot_id_tenant_id",
                table: "lesson_suggestion",
                columns: new[] { "timeslot_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_lesson_schedule_id_tenant_id",
                table: "lesson",
                columns: new[] { "schedule_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_lesson_timeslot_id_tenant_id",
                table: "lesson",
                columns: new[] { "timeslot_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_lesson_id_tenant_id",
                table: "attendance",
                columns: new[] { "lesson_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_attendance_student_id_tenant_id",
                table: "attendance",
                columns: new[] { "student_id", "tenant_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_attendance_lesson_lesson_id_tenant_id",
                table: "attendance",
                columns: new[] { "lesson_id", "tenant_id" },
                principalTable: "lesson",
                principalColumns: new[] { "lesson_id", "tenant_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_attendance_student_student_id_tenant_id",
                table: "attendance",
                columns: new[] { "student_id", "tenant_id" },
                principalTable: "student",
                principalColumns: new[] { "student_id", "tenant_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_schedule_schedule_id_tenant_id",
                table: "lesson",
                columns: new[] { "schedule_id", "tenant_id" },
                principalTable: "schedule",
                principalColumns: new[] { "schedule_id", "tenant_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_timeslot_timeslot_id_tenant_id",
                table: "lesson",
                columns: new[] { "timeslot_id", "tenant_id" },
                principalTable: "timeslot",
                principalColumns: new[] { "timeslot_id", "tenant_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_address_address_id_tenant_id",
                table: "lesson_suggestion",
                columns: new[] { "address_id", "tenant_id" },
                principalTable: "address",
                principalColumns: new[] { "address_id", "tenant_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_lesson_lesson_id_tenant_id",
                table: "lesson_suggestion",
                columns: new[] { "lesson_id", "tenant_id" },
                principalTable: "lesson",
                principalColumns: new[] { "lesson_id", "tenant_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_student_student_id_tenant_id",
                table: "lesson_suggestion",
                columns: new[] { "student_id", "tenant_id" },
                principalTable: "student",
                principalColumns: new[] { "student_id", "tenant_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lesson_suggestion_timeslot_timeslot_id_tenant_id",
                table: "lesson_suggestion",
                columns: new[] { "timeslot_id", "tenant_id" },
                principalTable: "timeslot",
                principalColumns: new[] { "timeslot_id", "tenant_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_schedule_address_address_id_tenant_id",
                table: "schedule",
                columns: new[] { "address_id", "tenant_id" },
                principalTable: "address",
                principalColumns: new[] { "address_id", "tenant_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_student_address_address_id_tenant_id",
                table: "student",
                columns: new[] { "address_id", "tenant_id" },
                principalTable: "address",
                principalColumns: new[] { "address_id", "tenant_id" });
        }
    }
}
