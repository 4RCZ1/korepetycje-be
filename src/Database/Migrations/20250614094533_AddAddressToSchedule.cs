using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressToSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "period",
                table: "schedule",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "interval",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "address_id",
                table: "schedule",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_schedule_address_id",
                table: "schedule",
                column: "address_id");

            migrationBuilder.AddForeignKey(
                name: "FK_schedule_address_address_id",
                table: "schedule",
                column: "address_id",
                principalTable: "address",
                principalColumn: "address_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_schedule_address_address_id",
                table: "schedule");

            migrationBuilder.DropIndex(
                name: "IX_schedule_address_id",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "address_id",
                table: "schedule");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "period",
                table: "schedule",
                type: "interval",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");
        }
    }
}
