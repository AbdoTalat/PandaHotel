using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRateIdToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RateId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RateId",
                table: "Reservations",
                column: "RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Rates_RateId",
                table: "Reservations",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Rates_RateId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_RateId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "Reservations");
        }
    }
}
