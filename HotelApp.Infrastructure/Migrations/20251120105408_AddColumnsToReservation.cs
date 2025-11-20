using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsToReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemSettings_CalculationTypes_CalculationTypeId",
                table: "SystemSettings");

            migrationBuilder.DropTable(
                name: "CalculationTypes");

            migrationBuilder.RenameColumn(
                name: "PricePerNight",
                table: "Reservations",
                newName: "TotalPayments");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Reservations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "MasterDataTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "CalculationType" });

            migrationBuilder.InsertData(
                table: "MasterDataItems",
                columns: new[] { "Id", "IsActive", "MasterDataTypeId", "Name", "Value" },
                values: new object[] { 9, true, 3, "ByAlert", (short)1 });

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSettings_MasterDataItems_CalculationTypeId",
                table: "SystemSettings",
                column: "CalculationTypeId",
                principalTable: "MasterDataItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemSettings_MasterDataItems_CalculationTypeId",
                table: "SystemSettings");

            migrationBuilder.DeleteData(
                table: "MasterDataItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MasterDataTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "TotalPayments",
                table: "Reservations",
                newName: "PricePerNight");

            migrationBuilder.CreateTable(
                name: "CalculationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SystemSettings_CalculationTypes_CalculationTypeId",
                table: "SystemSettings",
                column: "CalculationTypeId",
                principalTable: "CalculationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
