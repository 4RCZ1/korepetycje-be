using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class BasicResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "resource",
                columns: table => new
                {
                    resource_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    resource_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    file_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resource", x => x.resource_id);
                });

            migrationBuilder.CreateTable(
                name: "resource_group",
                columns: table => new
                {
                    resource_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    resource_group_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    is_single = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resource_group", x => x.resource_group_id);
                });

            migrationBuilder.CreateTable(
                name: "student_group",
                columns: table => new
                {
                    student_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    student_group_guid = table.Column<Guid>(type: "uuid", nullable: false),
                    is_single = table.Column<bool>(type: "boolean", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_group", x => x.student_group_id);
                });

            migrationBuilder.CreateTable(
                name: "tutor",
                columns: table => new
                {
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    resource_path_prefix = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutor", x => x.tenant_id);
                });

            migrationBuilder.CreateTable(
                name: "resource_membership",
                columns: table => new
                {
                    resource_id = table.Column<int>(type: "integer", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_resource_membership", x => new { x.resource_id, x.group_id });
                    table.ForeignKey(
                        name: "FK_resource_membership_resource_group_group_id",
                        column: x => x.group_id,
                        principalTable: "resource_group",
                        principalColumn: "resource_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_resource_membership_resource_resource_id",
                        column: x => x.resource_id,
                        principalTable: "resource",
                        principalColumn: "resource_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "access_policy",
                columns: table => new
                {
                    resource_group_id = table.Column<int>(type: "integer", nullable: false),
                    student_group_id = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_policy", x => new { x.resource_group_id, x.student_group_id });
                    table.ForeignKey(
                        name: "FK_access_policy_resource_group_resource_group_id",
                        column: x => x.resource_group_id,
                        principalTable: "resource_group",
                        principalColumn: "resource_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_access_policy_student_group_student_group_id",
                        column: x => x.student_group_id,
                        principalTable: "student_group",
                        principalColumn: "student_group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_membership",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    group_id = table.Column<int>(type: "integer", nullable: false),
                    tenant_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_membership", x => new { x.student_id, x.group_id });
                    table.ForeignKey(
                        name: "FK_student_membership_student_group_group_id",
                        column: x => x.group_id,
                        principalTable: "student_group",
                        principalColumn: "student_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_membership_student_student_id",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_policy_student_group_id",
                table: "access_policy",
                column: "student_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_resource_membership_group_id",
                table: "resource_membership",
                column: "group_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_membership_group_id",
                table: "student_membership",
                column: "group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_policy");

            migrationBuilder.DropTable(
                name: "resource_membership");

            migrationBuilder.DropTable(
                name: "student_membership");

            migrationBuilder.DropTable(
                name: "tutor");

            migrationBuilder.DropTable(
                name: "resource_group");

            migrationBuilder.DropTable(
                name: "resource");

            migrationBuilder.DropTable(
                name: "student_group");
        }
    }
}
