using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addReservationSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "ReservationSourceId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "ReservationSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationSources", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ReservationSources",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Walk In" },
                    { 2, "Hotel website" },
                    { 3, "Admin panel" },
                    { 4, "Government" },
                    { 5, "Mobile App" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationSourceId",
                table: "Reservations",
                column: "ReservationSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ReservationSources_ReservationSourceId",
                table: "Reservations",
                column: "ReservationSourceId",
                principalTable: "ReservationSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ReservationSources_ReservationSourceId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "ReservationSources");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ReservationSourceId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReservationSourceId",
                table: "Reservations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
