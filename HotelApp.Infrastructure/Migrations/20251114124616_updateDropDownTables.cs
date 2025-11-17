using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDropDownTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DropDownItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DropDownItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DropDownItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DropDownItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DropDownItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DropDownItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DropDownTypes",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DropDownTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "ReservationStatus" });

            migrationBuilder.InsertData(
                table: "DropDownItems",
                columns: new[] { "Id", "DropDownTypeId", "IsActive", "Name" },
                values: new object[,]
                {
                    { 3, 2, true, "Pending" },
                    { 4, 2, true, "Confirmed" },
                    { 5, 2, true, "CheckedIn" },
                    { 6, 2, true, "CheckedOut" },
                    { 7, 2, true, "Cancelled" },
                    { 8, 2, true, "NoShow" }
                });
        }
    }
}
