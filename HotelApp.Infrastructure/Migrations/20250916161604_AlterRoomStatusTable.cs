using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterRoomStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomNumber_BranchId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rates_Code_BranchId",
                table: "Rates");

            migrationBuilder.AlterColumn<int>(
                name: "CalculationTypeId",
                table: "SystemSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "RoomTypes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "RoomStatuses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "RoomStatuses",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "RoomStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Rooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Reservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Rates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Options",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Guests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Floors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Companies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Code", "IsSystem" },
                values: new object[] { "Available", true });

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Code", "IsSystem" },
                values: new object[] { "Reserved", true });

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Code", "IsSystem" },
                values: new object[] { "Occupied", true });

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Code", "IsSystem" },
                values: new object[] { "Maintenance", true });

            migrationBuilder.UpdateData(
                table: "RoomStatuses",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Code", "IsSystem" },
                values: new object[] { "Cleaning", true });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomNumber_BranchId",
                table: "Rooms",
                columns: new[] { "RoomNumber", "BranchId" },
                unique: true,
                filter: "[BranchId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_Code_BranchId",
                table: "Rates",
                columns: new[] { "Code", "BranchId" },
                unique: true,
                filter: "[BranchId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomNumber_BranchId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rates_Code_BranchId",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "RoomStatuses");

            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "RoomStatuses");

            migrationBuilder.AlterColumn<int>(
                name: "CalculationTypeId",
                table: "SystemSettings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "RoomStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Rates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Options",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Guests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Floors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomNumber_BranchId",
                table: "Rooms",
                columns: new[] { "RoomNumber", "BranchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rates_Code_BranchId",
                table: "Rates",
                columns: new[] { "Code", "BranchId" },
                unique: true);
        }
    }
}
